﻿using App.Write.Model.Accounts;
using App.Write.Model.Categories;
using App.Write.Model.Transactions;
using App.Write.Model.ValueObjects;
using App.Write.Ports;

namespace App.Write.UseCases;

public class ImportBankStatement(
    IAccountRepository accountRepository,
    ICategoryRepository categoryRepository,
    ITransactionRepository transactionRepository,
    IBankStatementParser bankStatementParser
)
{
    public async Task Execute(string fileName, Stream stream)
    {
        AccountStatement statement = await bankStatementParser.Extract(fileName, stream);

        Account account = await this.EnsureAccountExists(statement);
        account.Synchronize(statement.Balance);
        (Category[] categories, Transaction[] transactions) = await this.NewTransactions(account, statement);

        await this.Save(account, categories, transactions);
    }

    private async Task<Account> EnsureAccountExists(AccountStatement statement)
    {
        Account? account = await accountRepository.ByOrDefault(statement.AccountNumber);
        if (account != null)
            return account;

        AccountId id = await accountRepository.NextIdentity();

        return Account.StartTracking(id, statement.AccountNumber, statement.Balance);
    }

    private async Task<(Category[], Transaction[])> NewTransactions(Account account, AccountStatement statement)
    {
        TransactionStatement[] newTransactionStatements = await this.NewTransactionStatements(statement);

        Dictionary<Label, Category> newCategories = [];
        List<Transaction> newTransactions = [];
        foreach (TransactionStatement newTransactionStatement in newTransactionStatements)
        {
            TransactionId id = await transactionRepository.NextIdentity();
            if (newTransactionStatement.Category == null)
            {
                newTransactions.Add(
                    account.AttachTransaction(
                        id,
                        newTransactionStatement.Identifier,
                        newTransactionStatement.Amount,
                        newTransactionStatement.Label,
                        newTransactionStatement.Date,
                        null
                    )
                );

                continue;
            }

            if (newCategories.TryGetValue(newTransactionStatement.Category, out Category? newCategory))
            {
                newTransactions.Add(
                    account.AttachTransaction(
                        id,
                        newTransactionStatement.Identifier,
                        newTransactionStatement.Amount,
                        newTransactionStatement.Label,
                        newTransactionStatement.Date,
                        newCategory
                    )
                );

                continue;
            }

            Category? category = await categoryRepository.ByOrDefault(newTransactionStatement.Category);
            if (category == null)
            {
                category = new Category(await categoryRepository.NextIdentity(), newTransactionStatement.Category!);

                newCategories.Add(newTransactionStatement.Category, category);
            }

            newTransactions.Add(
                account.AttachTransaction(
                    id,
                    newTransactionStatement.Identifier,
                    newTransactionStatement.Amount,
                    newTransactionStatement.Label,
                    newTransactionStatement.Date,
                    category
                )
            );
        }

        return ([..newCategories.Values], [..newTransactions]);
    }

    private async Task<TransactionStatement[]> NewTransactionStatements(AccountStatement statement)
    {
        ExternalId[] unknownExternalIds = await transactionRepository.UnknownExternalIds([..statement.Transactions.Select(t => t.Identifier)]);

        return [..statement.Transactions.Where(t => unknownExternalIds.Contains(t.Identifier))];
    }

    private async Task Save(Account account, Category[] categories, Transaction[] transactions)
    {
        await accountRepository.Save(account);
        foreach (Category category in categories)
            await categoryRepository.Save(category);
        foreach (Transaction transaction in transactions)
            await transactionRepository.Save(transaction);
    }
}
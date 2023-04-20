namespace MoneyManager.Application.Write.Ports;

public interface IAccountRepository
{
    Task<Account> GetById(AccountId id);
    Task<bool> Exists(AccountId id);
    Task Save(Account account);
}
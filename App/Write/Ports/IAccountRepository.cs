using App.Write.Model.Accounts;
using App.Write.Model.ValueObjects;

namespace App.Write.Ports;

public interface IAccountRepository
{
    Task<AccountId> NextIdentity();
    Task<Account> By(AccountId id);
    Task<Account?> ByOrDefault(ExternalId externalId);
    Task Save(Account account);
}
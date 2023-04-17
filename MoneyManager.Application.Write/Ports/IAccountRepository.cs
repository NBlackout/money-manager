namespace MoneyManager.Application.Write.Ports;

public interface IAccountRepository
{
    Task<Guid> NextIdentity();
    Task<Account> GetById(Guid id);
    Task<bool> ExistsByNumber(string number);
    Task Save(Account account);
}
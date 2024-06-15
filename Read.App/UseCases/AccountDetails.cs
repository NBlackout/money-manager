namespace Read.App.UseCases;

public class AccountDetails
{
    private readonly IAccountDetailsDataSource dataSource;

    public AccountDetails(IAccountDetailsDataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public async Task<AccountDetailsPresentation> Execute(Guid id) => 
        await this.dataSource.Get(id);
}
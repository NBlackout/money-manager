using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MoneyManager.Client.Application.Read.UseCases.AccountSummaries;
using MoneyManager.Client.Application.Write.UseCases.OfxFile;

namespace MoneyManager.Client.Pages;

public partial class Accounts : ComponentBase
{
    private IReadOnlyCollection<AccountSummary>? accounts;
    private string? uploadResult;
    
    [Inject] private GetAccountSummaries GetAccountSummaries { get; set; } = null!;

    [Inject] private UploadOfxFile UploadOfxFile { get; set;} = null!;

    protected override async Task OnInitializedAsync()
    {
        this.accounts = await this.GetAccountSummaries.Execute();
    }

    private async Task UploadFile(InputFileChangeEventArgs args)
    {
        IBrowserFile file = args.File;

        Stream stream = file.OpenReadStream();
        string contentType = file.ContentType;
        string fileName = file.Name;

        try
        {
            await this.UploadOfxFile.Execute(fileName, contentType, stream);

            this.uploadResult = "Noice!";
        }
        catch (Exception e)
        {
            this.uploadResult = e.Message;
        }
    }
}
using Microsoft.AspNetCore.Components.Forms;

namespace MoneyManager.Client.Pages;

public partial class Data : ComponentBase
{
    private const int OneMegaByte = 1024000;

    private string? uploadResult;

    [Inject] private UploadBankStatement UploadBankStatement { get; set; } = null!;

    private async Task UploadBankStatementFile(InputFileChangeEventArgs args)
    {
        try
        {
            await this.Upload(args.File);

            this.uploadResult = "Bank statement successfully imported";
        }
        catch (Exception e)
        {
            this.uploadResult = e.Message;
        }
    }

    private async Task Upload(IBrowserFile file)
    {
        string fileName = file.Name;
        string contentType = file.ContentType;
        Stream stream = file.OpenReadStream(OneMegaByte);

        await this.UploadBankStatement.Execute(fileName, contentType, stream);
    }
}
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Pages;

public partial class Data : ComponentBase
{
    private const int FiveMegaByte = 5 * 1024 * 1024;

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
        string contentType = ContentTypeOf(file);
        Stream stream = file.OpenReadStream(FiveMegaByte);

        await this.UploadBankStatement.Execute(fileName, contentType, stream);
    }

    private static string ContentTypeOf(IBrowserFile file) =>
        string.IsNullOrWhiteSpace(file.ContentType) is false ? file.ContentType : "application/octet-stream";
}
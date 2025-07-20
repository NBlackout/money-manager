using Microsoft.AspNetCore.Components.Forms;
using Write.App.UseCases;

namespace Client.Pages;

public partial class Data : ComponentBase
{
    private const int FiveMegaBytes = 5 * 1024 * 1024;

    private bool? isSuccessful;
    private string? uploadResult;

    [Inject] private ImportBankStatement ImportBankStatement { get; set; } = null!;

    private async Task ImportBankStatementFile(InputFileChangeEventArgs args)
    {
        try
        {
            await this.Upload(args.GetMultipleFiles());

            this.isSuccessful = true;
            this.uploadResult = "Bank statement successfully imported";
        }
        catch (Exception e)
        {
            this.isSuccessful = false;
            this.uploadResult = e.Message;
        }
    }

    private async Task Upload(IEnumerable<IBrowserFile> files)
    {
        foreach (IBrowserFile file in files)
            await this.Upload(file);
    }

    private async Task Upload(IBrowserFile file)
    {
        string fileName = file.Name;
        Stream stream = file.OpenReadStream(FiveMegaBytes);

        await this.ImportBankStatement.Execute(fileName, stream);
    }
}
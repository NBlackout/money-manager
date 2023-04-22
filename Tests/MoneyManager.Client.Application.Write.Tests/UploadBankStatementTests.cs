using MoneyManager.Client.Application.Write.UseCases.BankStatement;
using MoneyManager.Client.Infrastructure.Write.BankStatementGateway;

namespace MoneyManager.Client.Application.Write.Tests;

public class UploadBankStatementTests
{
    private readonly SpyBankStatementGateway gateway;
    private readonly UploadBankStatement sut;

    public UploadBankStatementTests()
    {
        this.gateway = new SpyBankStatementGateway();
        this.sut = new UploadBankStatement(this.gateway);
    }

    [Fact]
    public async Task Should_upload_provided_ofx_file()
    {
        const string fileName = "The file name";
        const string contentType = "The content type";
        MemoryStream stream = new(new byte[] { 0x01, 0x02, 0x03 });

        await this.sut.Execute(fileName, contentType, stream);

        List<(string, string, Stream)> expectedCalls = new() { (fileName, contentType, stream) };
        this.gateway.Calls.Should().BeEquivalentTo(expectedCalls);
    }
}
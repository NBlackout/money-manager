using Client.Write.App.Tests.TestDoubles;

namespace Client.Write.App.Tests.UseCases;

public class UploadBankStatementTests
{
    private readonly SpyBankStatementGateway gateway = new();
    private readonly UploadBankStatement sut;

    public UploadBankStatementTests()
    {
        this.sut = new UploadBankStatement(this.gateway);
    }

    [Theory, RandomData]
    public async Task Uploads_provided_ofx_file(string fileName, string contentType, byte[] content)
    {
        MemoryStream stream = new(content);
        await this.sut.Execute(fileName, contentType, stream);
        List<(string, string, Stream)> expectedCalls = [(fileName, contentType, stream)];
        this.gateway.Calls.Should().Equal(expectedCalls);
    }
}
using MoneyManager.Client.Application.Write.UseCases.OfxFile;
using MoneyManager.Client.Infrastructure.Write.OfxFileGateway;

namespace MoneyManager.Client.Application.Write.Tests;

public class UploadOfxFileTests
{
    private readonly SpyOfxFileGateway gateway;
    private readonly UploadOfxFile sut;

    public UploadOfxFileTests()
    {
        this.gateway = new SpyOfxFileGateway();
        this.sut = new UploadOfxFile(this.gateway);
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
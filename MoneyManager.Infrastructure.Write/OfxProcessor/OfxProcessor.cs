using System.Xml;

namespace MoneyManager.Infrastructure.Write.OfxProcessor;

public class OfxProcessor : IOfxProcessor
{
    private const string AccountNumberNodeName = "ACCTID";

    public async Task<AccountIdentification> Parse(Stream stream)
    {
        using XmlReader xmlReader = XmlReader.Create(stream, new XmlReaderSettings { Async = true });

        while (await xmlReader.ReadAsync())
        {
            if (xmlReader.IsStartElement())
            {
                switch (xmlReader.Name)
                {
                    case AccountNumberNodeName:
                        return new AccountIdentification(xmlReader.ReadString());
                }
            }
        }

        throw new CannotProcessOfxContent("Cannot find account number node (ACCTID)");
    }
}
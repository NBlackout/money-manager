﻿using App.Write.Ports;

namespace Infra.Write.BankStatementParsing;

public class BankStatementParser(OfxBankStatementParser ofxBankStatementParser, CsvBankStatementParser csvBankStatementParser) : IBankStatementParser
{
    public Task<AccountStatement> Extract(string fileName, Stream stream) =>
        Path.GetExtension(fileName) switch
        {
            ".csv" => csvBankStatementParser.ExtractAccountStatement(stream),
            var _ => ofxBankStatementParser.ExtractAccountStatement(stream)
        };
}
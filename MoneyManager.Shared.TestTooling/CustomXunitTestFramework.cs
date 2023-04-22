using FluentAssertions;
using FluentAssertions.Formatting;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace MoneyManager.Shared.TestTooling;

public class CustomXunitTestFramework : XunitTestFramework
{
    public CustomXunitTestFramework(IMessageSink messageSink) : base(messageSink)
    {
        AssertionOptions.FormattingOptions.UseLineBreaks = true;
        AssertionOptions.EquivalencyPlan.Add<MemoryStreamEquivalencyStep>();
        // Formatter.AddFormatter(new NeatObjectFormatter());
    }

    private class NeatObjectFormatter : IValueFormatter
    {
        public bool CanHandle(object value) => true;

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context,
            FormatChild formatChild)
        {
            formattedGraph.AddLine(string.Empty);
            using IDisposable _ = formattedGraph.WithIndentation();
            formattedGraph.AddLine(value.ToString());
        }
    }
}
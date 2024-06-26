using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Shared.TestTooling;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Used in shared assembly TestFramework attribute")]
public class CustomXunitTestFramework : XunitTestFramework
{
    public CustomXunitTestFramework(IMessageSink messageSink) : base(messageSink)
    {
        AssertionOptions.FormattingOptions.UseLineBreaks = true;
        AssertionOptions.EquivalencyPlan.Add<MemoryStreamEquivalencyStep>();
        AssertionOptions.AssertEquivalencyUsing(options => options.WithStrictOrdering());
    }
}
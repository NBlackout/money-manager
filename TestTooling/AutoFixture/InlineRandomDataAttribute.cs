namespace TestTooling.AutoFixture;

public class InlineRandomDataAttribute(params object?[] values) : InlineAutoDataAttribute(new RandomDataAttribute(), values);
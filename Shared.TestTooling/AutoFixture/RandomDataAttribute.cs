using AutoFixture.Xunit2;

namespace Shared.TestTooling.AutoFixture;

public class RandomDataAttribute : AutoDataAttribute
{
    public RandomDataAttribute() : base(RandomData.GetFixture)
    {
    }
}
using AutoFixture.Xunit2;

namespace Shared.TestTooling.AutoFixture;

public class RandomDataAttribute() : AutoDataAttribute(RandomData.GetFixture);
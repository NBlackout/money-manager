namespace Read.App.Tests.TestDoubles;

public class StubbedSlidingBalancesDataSource : ISlidingBalancesDataSource
{
    private readonly Dictionary<(DateOnly, DateOnly), SlidingBalancesPresentation> data = [];

    public Task<SlidingBalancesPresentation> All(DateOnly baseline, DateOnly startingFrom) =>
        Task.FromResult(this.data[(baseline, startingFrom)]);

    public void Feed(DateOnly baseline, DateOnly startingFrom, SlidingBalancesPresentation expected) =>
        this.data[(baseline, startingFrom)] = expected;
}
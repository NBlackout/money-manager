namespace Read.App.Tests.UseCases;

// Plafonner les budgets? -> deborder = à répartir
// Date d'init du budget?
public class BudgetTests
{
    [Fact]
    public void Should()
    {
        this.Verify([new Budget("Trip", 30)], 0, new Budget("Trip", 30));
        this.Verify([new Budget("Trip", 30)], 20, new Budget("Trip", 10));
        this.Verify([new Budget("Trip", 30)], 30, new Budget("Trip", 0));
        this.Verify([new Budget("Trip", 30), new Budget("Unforeseen", 20)], 15, new Budget("Trip", 15),
            new Budget("Unforeseen", 20));
        this.Verify([new Budget("Trip", 30), new Budget("Unforeseen", 20)], 30, new Budget("Trip", 0),
            new Budget("Unforeseen", 20));
        this.Verify([new Budget("Trip", 30), new Budget("Unforeseen", 20)], 35, new Budget("Trip", 0),
            new Budget("Unforeseen", 15));
        this.Verify([new Budget("Trip", 30), new Budget("Unforeseen", 20)], 50, new Budget("Trip", 0),
            new Budget("Unforeseen", 0));
    }

    [Fact]
    public void Should_()
    {
        this.Verify([new Budget("Trip", 30)], 40, new Budget("Trip", -10));
        this.Verify([new Budget("Trip", 30), new Budget("Unforeseen", 20)], 55, new Budget("Trip", -5),
            new Budget("Unforeseen", 0));
    }

    private void Verify(Budget[] budget, int amountSpent, params Budget[] expected)
    {
        Budget[] actual = Act(budget, amountSpent);
        actual.Should().Equal(expected);
    }

    private static Budget[] Act(Budget[] budgets, decimal amountSpent)
    {
        decimal totalBudgetAmount = budgets.Sum(b => b.AmountPerMonth);
        if (amountSpent > totalBudgetAmount)
        {
            return
            [
                budgets.First() with { AmountPerMonth = totalBudgetAmount - amountSpent },
                ..budgets.Skip(1).Select(b => b with { AmountPerMonth = 0 })
            ];
        }

        List<Budget> newBudgets = [];
        var amountToSplit = amountSpent;

        foreach (Budget budget in budgets)
        {
            if (amountToSplit > 0)
            {
                decimal whatWillBeTaken = Math.Min(budget.AmountPerMonth, amountToSplit);
                Budget newBudget = budget with { AmountPerMonth = budget.AmountPerMonth - whatWillBeTaken };
                newBudgets.Add(newBudget);
                amountToSplit -= whatWillBeTaken;

                continue;
            }

            newBudgets.Add(budget);
        }

        return newBudgets.ToArray();
    }
}

public record Budget(string Name, decimal AmountPerMonth);

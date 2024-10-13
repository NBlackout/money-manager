namespace Shared.Dto;

public record BudgetDto(Guid Id, string Name, decimal Amount, DateOnly BeginDate);
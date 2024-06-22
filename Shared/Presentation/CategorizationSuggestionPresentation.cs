namespace Shared.Presentation;

public record CategorizationSuggestionPresentation(
    Guid TransactionId,
    string TransactionLabel,
    decimal TransactionAmount,
    Guid CategoryId,
    string CategoryLabel
);
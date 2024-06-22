namespace Client.Read.App.Ports;

public interface ICategorizationGateway
{
    Task<CategorizationSuggestionPresentation[]> Suggestions();
}
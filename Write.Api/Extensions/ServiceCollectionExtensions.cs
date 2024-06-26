using Write.App.Model.Categories;
using Write.Infra.OfxProcessing;

namespace Write.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWriteDependencies(this IServiceCollection services) =>
        services.AddUseCases().AddInfrastructureAdapters();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<ImportBankStatement>()
            .AddScoped<StopAccountTracking>()
            .AddScoped<ResumeAccountTracking>()
            .AddScoped<AssignAccountLabel>()
            .AddScoped<AssignTransactionCategory>()
            .AddScoped<CreateCategory>()
            .AddScoped<DeleteCategory>();
    }

    private static IServiceCollection AddInfrastructureAdapters(this IServiceCollection services) =>
        services.AddRepositories().AddScoped<IOfxParser, OfxParser>();

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        InMemoryBankRepository bankRepository = new() { NextId = Guid.NewGuid };
        InMemoryAccountRepository accountRepository = new() { NextId = Guid.NewGuid };
        InMemoryTransactionRepository transactionRepository = new() { NextId = Guid.NewGuid };
        InMemoryCategoryRepository categoryRepository = new();
        categoryRepository.Feed(
            [
                ..CategoryFrom("Péages", "SAPN", "AUTOROUTE"),
                ..CategoryFrom("Mutuelle", "BALOO"),
                ..CategoryFrom("Assurance", "MATMUT", "MAIF", "MULTI IMPACT"),
                ..CategoryFrom("Salaires", "AGICAP"),
                ..CategoryFrom("Salaires", "DEVOLIS"),
                ..CategoryFrom("Entretien jardin", "CHARLY CROMBEZ"),
                ..CategoryFrom("Entretien maison", "CNCESU"),
                ..CategoryFrom("Nourriture", "SERRAT", "ETABLISSEMENTS C", "C.GORJU ET FILS", "CHARCUTERIE",
                    "LA PETITE SOUR", "DU PECHEUR A", "INTERMARCHE", "BOUCHERIE"),
                ..CategoryFrom("Parking", "ROUENPALAISBS"),
                ..CategoryFrom("Electricité", "ELECTRICITE DE FRANCE"),
                ..CategoryFrom("Internet", "FIXE ADSL"),
                ..CategoryFrom("Assistante maternelle", "PAJEMPLOI"),
                ..CategoryFrom("Prêt", "Compte BP"),
                ..CategoryFrom("Shopping", "AMAZON", "PayPal"),
                ..CategoryFrom("Services numériques", "AUDIBLE"),
                ..CategoryFrom("Carburant", "INTER STATION VL")
            ]
        );

        return services
            .AddSingleton<IBankRepository>(bankRepository)
            .AddSingleton(bankRepository)
            .AddSingleton<IAccountRepository>(accountRepository)
            .AddSingleton(accountRepository)
            .AddSingleton<ITransactionRepository>(transactionRepository)
            .AddSingleton(transactionRepository)
            .AddSingleton<ICategoryRepository>(categoryRepository)
            .AddSingleton(categoryRepository);
    }

    private static Category[] CategoryFrom(string label, params string[] keywords) =>
        keywords.Select(kw => Category.From(new CategorySnapshot(Guid.NewGuid(), label, kw))).ToArray();
}
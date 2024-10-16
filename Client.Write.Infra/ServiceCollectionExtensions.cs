﻿using Client.Write.App.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Write.Infra;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientWriteInfra(this IServiceCollection services)
    {
        return services
            .AddUseCases()
            .AddGateways();
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<UploadBankStatement>()
            .AddScoped<StopAccountTracking>()
            .AddScoped<ResumeAccountTracking>()
            .AddScoped<AssignAccountLabel>()
            .AddScoped<CreateCategory>()
            .AddScoped<DeleteCategory>()
            .AddScoped<AssignTransactionCategory>()
            .AddScoped<DefineBudget>();
    }

    private static IServiceCollection AddGateways(this IServiceCollection services)
    {
        return services
            .AddScoped<IBankStatementGateway, HttpBankStatementGateway>()
            .AddScoped<IAccountGateway, HttpAccountGateway>()
            .AddScoped<ICategoryGateway, HttpCategoryGateway>()
            .AddScoped<ITransactionGateway, HttpTransactionGateway>()
            .AddScoped<IBudgetGateway, HttpBudgetGateway>();
    }
}
using App.Write.Model.CategorizationRules;
using App.Write.Ports;

namespace App.Write.UseCases;

public class DeleteCategorizationRule(ICategorizationRuleRepository repository)
{
    public async Task Execute(CategorizationRuleId id) =>
        await repository.Delete(id);
}
using App.Write.Model.CategorizationRules;

namespace App.Write.Ports;

public interface ICategorizationRuleRepository
{
    Task<CategorizationRuleId> NextIdentity();
    Task Save(CategorizationRule category);
    Task Delete(CategorizationRuleId id);
}
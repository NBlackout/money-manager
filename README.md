# TODO

## Feat
- Redirect to /accounts after successful synchronization
- Change transaction category -> dropdown list with search?
- Prevent deletion of used category
- Category deduplication
- Allow to rename budget (in its own use case though)
- Change budget amount implies providing begin date
- Refresh account list on account rename
- Indent budget fake accounts below reference account
  - Use year browsing instead of month
  - Show all fake transaction of given year
- Display account balance of given month (along with transactions)
- Clear all `EditForm` on save so that next create only has placeholders and not previously saved values
- Account empty state
- Deduplicate category on import using both `ToLower` and `Trim`

## Bug
- Nullable category label during import

## Testing
- Contract testing

## Clean
- Merge Read App & Read Infra?
- Move port param and result closer to usage (VO or just below port)
- Check naming everywhere
- Categorization table background is white
- Two transactions of month requests on account switch
- Introduce `CurrentMonth` of something like that to replace `new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1)`
- `using static Shared.TestTooling.Randomizer` in `GlobalUsings`
- Set `ApiUrl` in `StubbedHttpMessageHandler` privately to replace `new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };` with `new(httpMessageHandler)`
- `Trim` in use cases ? Maybe with a `Label` VO ?

## Tooling
- Auto formatting tool
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

## Bug

## Testing
- Contract testing

## Clean
- Merge Read App & Read Infra?
- Move port param and result closer to usage (VO or just below port)
- Check naming everywhere
- Categorization table background is white
- Multiple requests issued on transaction of month navigation
- Introduce `CurrentMonth` of something like that to replace `new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1)`
- `using static Shared.TestTooling.Randomizer` in `GlobalUsings`
- `AddUseCases` should move back in primary adapters
- Set `ApiUrl` in `StubbedHttpMessageHandler` privately to replace `new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };` with `new(httpMessageHandler)`

## Tooling
- Auto formatting tool
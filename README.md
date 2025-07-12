# TODO

## Feat

- Display EUR symbol in dashboard sliding balance tooltip (+ currency format)
- Sliding balance displays balance at the beginning of each month + estimated balance for next 3 months
- Redirect to /accounts after successful synchronization
- Change transaction category -> dropdown list with search?
- Prevent deletion of used category
- Allow to rename budget (in its own use case though)
- Change budget amount implies providing begin date
- Refresh account list on account rename
- Indent budget fake accounts below reference account
    - Use year browsing instead of month
    - Show all fake transaction of given year
- Display account balance of given month (along with transactions)
- Account empty state
- Export / import budgets
- Category export / import

## Bug

- Nullable category label during import
- Multiple import sometimes fails

## Task

- Make sure bootstrap is properly used everywhere by following their documentation examples (e.g. `NavMenu`) 
- Contract testing
- Merge Read App & Read Infra?
- Check naming everywhere
- Categorization table background is white
- Two transactions of month requests on account switch
- Introduce `CurrentMonth` of something like that to replace `new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1)`
- Set `ApiUrl` in `StubbedHttpMessageHandler` privately to replace
  `new(httpMessageHandler) { BaseAddress = new Uri(ApiUrl) };` with `new(httpMessageHandler)`
- Transaction `Debit` or `Credit` instead of positive and negative amounts
- Move UseCase DI registration in infra
- Simplify read by removing infra layer ? Merging it in use cases ?
- An*() which only indirects to Any<*> could all move to static test tooling class 
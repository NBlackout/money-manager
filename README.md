# TODO

## Feat

- Accounts
    - Empty state
    - Display balance of given month (along with transactions)
- Budget
    - Import/Export
    - Change budget amount implies providing begin date
    - Indent budget fake accounts below reference account
        - Use year browsing instead of month
        - Show all fake transaction of given year
    - Allow renaming (in its own use case though)
- Categories
    - Prevent deletion of used one
- Categorization rules
    - Sort form & table by label
- Transactions
    - Display name vs original label
    - Assign category -> dropdown list with search?
- UI
    - Light/Dark/System themes support
- Organizations
    - New concept
    - Assign organization to transaction instead of category?
    - Assign category to organization

## Bug

- Nullable category label during import
- Multiple import sometimes fails
- Account activity: next month when already on last
- Categorization rule: error when creating using default (first) category

## Task

- A single CSV adapter to read/write
- Make sure bootstrap is properly used everywhere by following their documentation examples (e.g. `NavMenu`)
- Merge Read App & Read Infra?
- Categorization table background is white
- Two transactions of month HTTP requests made when switching account
- Introduce `CurrentMonth` or something like that to replace `new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1)`
- Transaction `Debit` or `Credit` instead of positive and negative amounts
- Simplify read by removing infra layer ? Merging it in use cases ?
- Fix mutation in CI
- Remove coverlet.collector after checking coverage works and CI is ok
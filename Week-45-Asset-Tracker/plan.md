# Asset Tracker: Plan

## Goal

Console app to track company assets across offices: add, remove, search, sort, and flag assets nearing end of life.

## Requirements

- Each asset belongs to an office; the office decides the currency
- User enters the price in the office's local currency; USD is calculated from it
- Exchange rate = local currency units per 1 USD (hardcoded)

### Asset (abstract)

Holds asset data, calculates expiry date, status and USD price.

- Id: 5-digit string ("10001" - "99999")
- Brand, Model
- PurchaseDate
- PriceLocal: entered by user, in the office's currency
- PriceUSD: calculated (`PriceLocal / Office.ExchangeRate`)
- Office
- AssetType: `abstract AssetType Type { get; }`
- ExpiryDate: calculated (`PurchaseDate + 3 years`)
- Status: calculated, returns `Status` enum


### Computer / MobilePhone / Tablet : Asset

Each overrides `Type` with its own `AssetType` value.


### Office

Holds country, currency and exchange rate. Predefined instances only.

- Country, CurrencyToken, ExchangeRate (decimal, local per 1 USD)
- Offices: Sweden (SEK), USA (USD, rate 1), Turkey (TRY), optionally Germany (EUR)

### AssetManager

Owns the asset list: add, remove, search, sort, ID collision check. No console I/O.

### UI (static)

Renders text, tables, menus and input prompts. Maps `Status` to color. No business rules.

### Validate (static)

Validates user input (price, date, menu selection, asset).

### Utility (static)

Small helpers only (input normalizing, `TryParse`-style readers). Keep it small.

## Enums

- SortBy: AssetType, Office, PurchaseDate
- AssetType: Computer, MobilePhone, Tablet
- Status: None, Yellow, Red, Expired

```csharp
using System.Text.RegularExpressions;

// Splits PascalCase text into separate words,
// e.g. "MobilePhone" -> "Mobile Phone", "PurchaseDate" -> "Purchase Date".
// Inserts a space between each lowercase and uppercase letter pair.
public static string SplitWords(string text) =>
Regex.Replace(text, "(?<=[a-z])(?=[A-Z])", " ");
```

## Build Order

- [ ] L1: Asset, Computer, MobilePhone, hardcoded list, print table
- [ ] L2: Status logic, sorting by type then date
- [ ] L3: Office + currency, sorting by office then date
- [ ] L4: Abstract Asset, Tablet, IDs, menu (add / view / sort / search / remove)
- [ ] L5 (optional): JSON save/load, export

## Notes for README

- Color rules are my own choice (allowed by the instructor): closer to expiry = more severe.
- Exchange rates are hardcoded.
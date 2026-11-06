# Week 45 - Asset Tracker

A C# console application for tracking company assets (computers, mobile phones and tablets) across offices in different countries. It flags assets that are close to their 3-year end of life and shows prices in USD or the office's local currency.

Built as part of the Week 45 mini project (Asset Tracking System).

## Features

- **Add assets** with input validation (type, brand, model, office, price, purchase date). Future purchase dates are rejected. Enter `q` at any prompt to cancel.
- **View assets** in a formatted table with alternating row shading.
- **Sorting** by asset type, purchase date, office, brand or price. Pressing the same key again reverses the order.
- **Search assets** by brand or model (case-insensitive, partial match).
- **Remove assets** by ID, with a confirmation prompt.
- **End-of-life status** based on a 3-year lifespan:
  - `RED`: less than 3 months remaining
  - `YELLOW`: less than 6 months remaining
- **Multi-office currency support** (Sweden/SEK, Germany/EUR, USA/USD, Turkey/TRY). Prices are entered in the office's local currency and stored in USD. A toggle switches the table between USD and local currency.
- **Unique asset IDs**: random 5-digit IDs with a collision check. An existing ID can be restored when creating an asset (useful for loading from a file). Duplicates throw an exception.
- **Menu system** with exit confirmation.
- **Seed data** on startup, so the table and status colors can be tried right away.

### Object-oriented design

- `Asset` is an abstract base class, with `Computer`, `MobilePhone` and `Tablet` inheriting from it.
- Each subclass overrides `AssetType` (polymorphism), which is an `AssetType` enum.
- `Asset` owns its lifecycle logic: `ExpiryDate`, `Status` (`AssetStatus` enum) and `AgeInMonths`.
- `Office` handles currency conversion (`ToUsd` / `FromUsd`), and `OfficeCatalog.Find` looks up offices by name.
- Assets are stored in a `List<Asset>` owned by `AssetService`, which exposes them as a read-only list.
- UI helpers are shared: `ConsoleUi` (colored output, y/n confirm) and `AssetTable` (table rendering).

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Getting started

```````bash
git clone <repository-url>
cd Week-45-Asset-Tracker
dotnet run --project Week-45-Asset-Tracker
```````

## Controls

| Screen | Key | Action |
|---|---|---|
| Main menu | `1` / `2` / `3` / `4` | Add / View / Search / Remove asset |
| Main menu | `0` | Exit (asks for confirmation) |
| View assets | `a` `d` `o` `b` `p` | Sort by type / date / office / brand / price |
| View assets | `c` | Toggle USD / local currency |
| View assets | `q` | Back to the main menu |
| Search assets | text + `Enter` | Search by brand or model |
| Search assets | empty input or `q` | Back to the main menu |
| Remove asset | ID + `Enter` | Select an asset, then confirm with `y` / `n` |
| Remove asset | `q` | Back to the main menu |
| Add asset | `q` | Cancel |

## Exchange rates

Rates are hardcoded in `Models/OfficeCatalog.cs` (USD per 1 unit of local currency):

| Office | Currency | Rate |
|---|---|---|
| Sweden | SEK | 0.095 |
| Germany | EUR | 1.08 |
| USA | USD | 1.00 |
| Turkey | TRY | 0.02 |

## Folder structure

```````
Week-45-Asset-Tracker/
├── Week-45-Asset-Tracker.sln
├── readme.md
└── Week-45-Asset-Tracker/
    ├── Program.cs              # Entry point, main menu, seed data
    ├── Week-45-Asset-Tracker.csproj
    ├── Models/
    │   ├── Asset.cs            # Abstract Asset, Computer, MobilePhone, Tablet, AssetType, AssetStatus
    │   └── OfficeCatalog.cs    # Office record (conversion), hardcoded offices/exchange rates
    ├── Services/
    │   └── AssetService.cs     # Holds the asset list: add, remove, search
    └── UI/
        ├── AddAsset.cs         # Add asset screen and input validation
        ├── ViewAssets.cs       # View screen, sorting
        ├── SearchAssets.cs     # Search by brand/model
        ├── RemoveAsset.cs      # Remove asset by ID
        ├── AssetTable.cs       # Table rendering, status colors
        ├── ConsoleUi.cs        # Colored output helpers, y/n confirm
        └── Menu.cs             # Reusable menu component
```````

## Screenshots

```````
```````

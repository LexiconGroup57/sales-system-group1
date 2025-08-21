# Cinema Sales System

Console app for a small cinema to register ticket and snack sales, and to report monthly totals and VAT. Payment processing is out-of-scope; this only records sales.

## What it does

- Loads movie sessions from `movies.txt` and snacks from `Snacks.txt` using StreamReader
- Registers sales for tickets and snacks via a menu-driven UI
- Persists sales to `sales_log.txt` using StreamWriter (auto-created on first sale)
- Aggregates monthly totals (Gross, VAT, Net) from the sales log

## How it works (high-level)

1. Start the app, you’ll see the main menu:

   - Movies – Sell ticket
   - Snacks – Sell snack
   - Monthly Summary
   - Quit

2. Selling tickets

   - Sessions are parsed from `movies.txt` into `Models.Session`.
   - Price rule: shows at 13:00 = 105 SEK, otherwise 130 SEK.
   - VAT for tickets is assumed 6% (cinema rate).
   - A sale is written to `sales_log.txt`.

3. Selling snacks

   - Snacks are parsed from `Snacks.txt` into `Models.Snack`.
   - VAT is read per item (e.g., 12%).
   - A sale is written to `sales_log.txt`.

4. Monthly summary

   - Reads `sales_log.txt` and aggregates all sales for the selected month (Gross, VAT, Net) and by type (Ticket/Snack).

## Project structure (relevant parts)

- `SalesSystem/Program.cs` – app entry
- `SalesSystem/Menu.cs` – menu and user flow
- `SalesSystem/Services/` – file I/O and domain logic
  - `MovieService.cs` – reads `movies.txt` into sessions
  - `SnackService.cs` – reads `Snacks.txt` into snack items
  - `SalesService.cs` – writes sales to `sales_log.txt`
  - `ReportService.cs` – summarizes monthly totals from `sales_log.txt`
  - `DataPaths.cs` – resolves data file paths (from the output folder)
- `SalesSystem/Models/` – simple data classes (`Session`, `Snack`, `SaleRecord`)
- `SalesSystem/movies.txt` – input data for sessions (copied to output)
- `SalesSystem/Snacks.txt` – input data for snacks (copied to output)
- `SalesSystem/bin/Debug/net9.0/sales_log.txt` – generated sales log

## Data file formats

### movies.txt (repeated blocks)

```text
Day Time,
Title
YEAR RATING DURATION
Description line 1
Description line 2

```

Notes:

- A blank line separates entries.
- The trailing comma after time is optional.
- Example: `Saturday 13:00,` → price 105 SEK; other times → 130 SEK.

### Snacks.txt (repeated blocks)

```text
Item: NAME
Weight: 200g
Price: 43
Vat: 12%

```

### sales_log.txt (auto-generated)

- Pipe-delimited fields per sale:
  - `timestamp|type|name|qty|unitPrice|vat%|gross|vatAmount|net|notes`

Example line:

```text
2025-08-21 18:45:03|Snack|Popcorn|2|43|12|86|9.2|76.8|200g
```

## How to run

From the repository root:

1. Build

```bash
dotnet build "SalesSystem/SalesSystem.csproj" -c Debug
```

2. Run

```bash
dotnet run --project "SalesSystem/SalesSystem.csproj"
```

`movies.txt` and `Snacks.txt` are copied to the output folder automatically; `sales_log.txt` is created on first sale.

## Why this approach

- Text files + StreamReader/Writer (requirement):
  - Keeps persistence simple and transparent for a small, on-site system.
  - Easy to inspect and edit data (`movies.txt`, `Snacks.txt`) without tooling.
  - Sales log is append-only text, suitable for monthly accounting checks.
- Separation via Services and Models:
  - `Services/*` isolate file parsing/writing and reporting logic (single responsibility).
  - `Models/*` keep data contracts minimal and clear.
  - `Menu` focuses on user interaction without file I/O details.
- Simple pricing/VAT rules in code:
  - 13:00 special price rule is deterministic and centralized in `MovieService`.
  - Ticket VAT fixed at 6%; snack VAT read from the snack file.
- Extensibility:
  - New data fields can be added to the txt files with small parser changes.
  - Another report (e.g., daily summary or CSV export) can be added reusing the log format.

## Troubleshooting

- Build error about locked EXE: ensure no running `SalesSystem.exe` from a previous run; close it and rebuild.
- No sessions/snacks found: verify `movies.txt` / `Snacks.txt` exist and follow the formats above.
- Sales not showing in summary: confirm `sales_log.txt` exists in the output folder and you selected the correct year/month.

## Possible next steps

- Export monthly summary to CSV
- Input validation and clearer error messages
- Unit tests for parsing and reporting

# Sprint # - Tasks

- [ ] Create menu (#2)
- [ ] Create Tickets (#4)
- [ ] Create Seats (#5)
- [ ] Create Snacks (#6)
- [ ] Create Pay (#7)

## Project Map

```
sales-system-group1/
│
├── SalesSystem/                # Main application folder
│   ├── Program.cs              # Entry point, starts the menu
│   ├── Menu.cs                 # Main menu logic, navigation between features
│   ├── Movies.cs               # Movie selection, seat booking, reads/writes movie data
│   ├── Snacks.cs               # Snack selection, reads/writes snack data
│   ├── Summary.cs              # Shows summary of selected items (movies/snacks)
│   ├── movies.txt              # Movie data (title, time, seats, etc.)
│   ├── Snacks.txt              # Snack data (name, price, etc.)
│   ├── summary.txt             # Stores user selections for summary
│   └── SalesSystem.csproj      # Project file for .NET build
│
├── SalesSystem.sln             # Visual Studio solution file
├── README.md                   # Project documentation
└── .gitignore                  # Git ignore rules
```

**Main Flow**

- `Program.cs` → `Menu.cs` → user chooses Movies, Snacks, Summary, or Pay.
- `Movies.cs` handles movie listing, seat selection, and updates seat availability.
- `Snacks.cs` handles snack listing and selection.
- `Summary.cs` displays all selected items from `summary.txt`.

## Description

### Rules for git

- Pull alltid först innan du börjar jobba.
- Jobba inte på samma fil samtidigt, eller prata ihop er om vem som ändrar vad.
- Commita ofta med tydliga meddelanden så ni vet vad som ändrats (engelska).
- Push direkt när du är klar med något så andra får det senaste.

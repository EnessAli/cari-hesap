# cari-hesap

A C# desktop application for managing **Cari Hesap** (accounts receivable / payable) records. Supports creating, editing, and tracking debit/credit entries with persistent storage via Entity Framework Core.

## Features

- Create and manage customer / supplier accounts
- Record debit and credit transactions
- View account balances and transaction history
- Persistent storage with EF Core migrations

## Project Structure

```
src/
  Data/           DbContext and repository layer
  Models/         Entity classes
  Forms/          Windows Forms UI
  Migrations/     EF Core database migrations
  Program.cs      Application entry point
```

## Tech Stack

`C#` `.NET` `Entity Framework Core` `Windows Forms`

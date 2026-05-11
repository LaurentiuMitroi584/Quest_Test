# Quest_Test
Automation testing for QuestGlobal

# AutomationExercise Playwright Tests (C# / .NET)

Automated UI tests for [https://automationexercise.com](https://automationexercise.com) using **Microsoft Playwright** and **C# (.NET 8)** with **NUnit** as the test runner.

---

## Project Structure

```
AutomationExercise.Tests/
├── AutomationExercise.Tests.csproj   # Project file with NuGet dependencies
├── .runsettings                      # Playwright/NUnit run settings
│
├── Models/
│   └── UserData.cs                   # Test data models (UserData, PaymentData)
│
├── PageObjects/
│   ├── BasePage.cs                   # Shared base page with common helpers
│   ├── HomePage.cs                   # Home page interactions
│   ├── LoginPage.cs                  # Login + Signup page
│   ├── ProductsPage.cs               # Products list + detail + review
│   ├── CartPage.cs                   # Shopping cart page
│   └── CheckoutPage.cs               # Checkout + payment page
│
├── Helpers/
│   └── TestBase.cs                   # NUnit base class with setup/teardown
│
└── Tests/
    ├── AuthTests.cs                  # TC-01, TC-02, TC-03, TC-05 + TC-OPT-01
    ├── ProductTests.cs               # TC-08, TC-09, TC-21 + TC-OPT-02
    └── CartTests.cs                  # TC-12, TC-13, TC-17 + TC-OPT-03, TC-10
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (required by Playwright for browser installation)
- Internet access to https://automationexercise.com

---

## Setup

### 1. Restore NuGet packages

```bash
dotnet restore
```

### 2. Build the project

```bash
dotnet build
```

### 3. Install Playwright browsers

After building, run the Playwright browser install script:

```bash
pwsh bin/Debug/net8.0/playwright.ps1 install
```

If you don't have PowerShell, use:

```bash
dotnet tool install --global Microsoft.Playwright.CLI
playwright install
```

---

## Running Tests

### Run all tests

```bash
dotnet test
```

### Run a specific category

```bash
dotnet test --filter "Category=Authentication"
dotnet test --filter "Category=Products"
dotnet test --filter "Category=Cart"
```

### Run a specific test by name

```bash
dotnet test --filter "FullyQualifiedName~TC01_RegisterUser"
```

### Run tests with HTML report

```bash
dotnet test --logger "html;logfilename=TestResults.html"
```

### Run tests in non-headless mode (to watch browser)

Set the `HEADED` environment variable:

```bash
# Windows (PowerShell)
$env:HEADED = "1"; dotnet test

# Linux/macOS
HEADED=1 dotnet test
```

---

## Configuration

Test settings can be customised in `TestBase.cs`:

- **Base URL**: `BaseUrl` constant
- **Viewport**: `ContextOptions()` override
- **Ad blocking**: Routes in `SetUpPageObjects()` — blocks Google Ads and DoubleClick that can interfere with test clicks
- **Video recording**: Enabled by default in `ContextOptions()` — saved to `videos/` directory (only on failure by default)

---

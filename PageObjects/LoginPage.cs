using Microsoft.Playwright;
using AutomationExercise.Tests.Models;

namespace AutomationExercise.Tests.PageObjects;


/// Page object for the Login/Signup page (/login).
/// This page contains both the login form and the new user signup form.

public class LoginPage : BasePage
{
    public LoginPage(IPage page) : base(page) { }

    // ─── Locators ───────────────────────────────────────────────────────────────

    // Login section
    private ILocator LoginEmailInput => Page.Locator("input[data-qa='login-email']");
    private ILocator LoginPasswordInput => Page.Locator("input[data-qa='login-password']");
    private ILocator LoginButton => Page.Locator("button[data-qa='login-button']");
    private ILocator LoginErrorMessage => Page.GetByText("Your email or password is incorrect!");

    // Signup section
    private ILocator SignupNameInput => Page.Locator("input[data-qa='signup-name']");
    private ILocator SignupEmailInput => Page.Locator("input[data-qa='signup-email']");
    private ILocator SignupButton => Page.Locator("button[data-qa='signup-button']");
    private ILocator SignupExistingEmailError => Page.GetByText("Email Address already exist!");

    // Headings
    private ILocator LoginHeading => Page.GetByText("Login to your account");
    private ILocator SignupHeading => Page.GetByText("New User Signup!");

    // ─── Actions ────────────────────────────────────────────────────────────────

    public async Task LoginAsync(string email, string password)
    {
        await LoginEmailInput.FillAsync(email);
        await LoginPasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }

    public async Task FillSignupNameAndEmailAsync(string name, string email)
    {
        await SignupNameInput.FillAsync(name);
        await SignupEmailInput.FillAsync(email);
        await SignupButton.ClickAsync();
    }

    // ─── Assertions ─────────────────────────────────────────────────────────────

    public async Task<bool> IsLoginHeadingVisibleAsync() =>
        await LoginHeading.IsVisibleAsync();

    public async Task<bool> IsSignupHeadingVisibleAsync() =>
        await SignupHeading.IsVisibleAsync();

    public async Task<bool> IsLoginErrorVisibleAsync() =>
        await LoginErrorMessage.IsVisibleAsync();

    public async Task<bool> IsExistingEmailErrorVisibleAsync() =>
        await SignupExistingEmailError.IsVisibleAsync();
}


/// Page object for the account registration form (/signup).
/// Displayed after clicking Signup from the login page.

public class SignupPage : BasePage
{
    public SignupPage(IPage page) : base(page) { }

    // ─── Locators ───────────────────────────────────────────────────────────────

    private ILocator AccountInfoHeading => Page.GetByText("ENTER ACCOUNT INFORMATION");
    private ILocator TitleMr => Page.GetByLabel("Mr.");
    private ILocator PasswordInput => Page.Locator("input[data-qa='password']");
    private ILocator DaySelect => Page.Locator("select[data-qa='days']");
    private ILocator MonthSelect => Page.Locator("select[data-qa='months']");
    private ILocator YearSelect => Page.Locator("select[data-qa='years']");
    private ILocator NewsletterCheckbox => Page.GetByLabel("Sign up for our newsletter!");
    private ILocator OffersCheckbox => Page.GetByLabel("Receive special offers from our partners!");
    private ILocator FirstNameInput => Page.Locator("input[data-qa='first_name']");
    private ILocator LastNameInput => Page.Locator("input[data-qa='last_name']");
    private ILocator CompanyInput => Page.Locator("input[data-qa='company']");
    private ILocator AddressInput => Page.Locator("input[data-qa='address']");
    private ILocator Address2Input => Page.Locator("input[data-qa='address2']");
    private ILocator CountrySelect => Page.Locator("select[data-qa='country']");
    private ILocator StateInput => Page.Locator("input[data-qa='state']");
    private ILocator CityInput => Page.Locator("input[data-qa='city']");
    private ILocator ZipCodeInput => Page.Locator("input[data-qa='zipcode']");
    private ILocator MobileInput => Page.Locator("input[data-qa='mobile_number']");
    private ILocator CreateAccountButton => Page.Locator("button[data-qa='create-account']");
    private ILocator AccountCreatedHeading => Page.GetByText("ACCOUNT CREATED!");
    private ILocator ContinueButton => Page.Locator("a[data-qa='continue-button']");
    private ILocator AccountDeletedHeading => Page.GetByText("ACCOUNT DELETED!");

    // ─── Actions ────────────────────────────────────────────────────────────────

    
    /// Fills all account registration fields and submits the form.
    
    public async Task FillAccountDetailsAsync(UserData user)
    {
        await TitleMr.CheckAsync();
        await PasswordInput.FillAsync(user.Password);
        await DaySelect.SelectOptionAsync(user.DateOfBirthDay);
        await MonthSelect.SelectOptionAsync(user.DateOfBirthMonth);
        await YearSelect.SelectOptionAsync(user.DateOfBirthYear);
        await NewsletterCheckbox.CheckAsync();
        await OffersCheckbox.CheckAsync();
        await FirstNameInput.FillAsync(user.FirstName);
        await LastNameInput.FillAsync(user.LastName);
        await CompanyInput.FillAsync(user.Company);
        await AddressInput.FillAsync(user.Address);
        await Address2Input.FillAsync(user.Address2);
        await CountrySelect.SelectOptionAsync(user.Country);
        await StateInput.FillAsync(user.State);
        await CityInput.FillAsync(user.City);
        await ZipCodeInput.FillAsync(user.ZipCode);
        await MobileInput.FillAsync(user.MobileNumber);
        await CreateAccountButton.ClickAsync();
    }

    public async Task ClickContinueAsync() => await ContinueButton.ClickAsync();

    // ─── Assertions ─────────────────────────────────────────────────────────────

    public async Task<bool> IsAccountInfoHeadingVisibleAsync() =>
        await AccountInfoHeading.IsVisibleAsync();

    public async Task<bool> IsAccountCreatedVisibleAsync() =>
        await AccountCreatedHeading.IsVisibleAsync();

    public async Task<bool> IsAccountDeletedVisibleAsync() =>
        await AccountDeletedHeading.IsVisibleAsync();
}

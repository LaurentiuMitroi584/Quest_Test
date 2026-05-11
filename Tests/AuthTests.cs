using AutomationExercise.Tests.Helpers;
using AutomationExercise.Tests.Models;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.Tests;


/// Authentication tests covering:
///   TC-01: Register User
///   TC-02: Login with correct credentials
///   TC-03: Login with incorrect credentials
///   TC-05: Register with existing email
///
/// Optional additional scenarios:
///   TC-OPT-01: Login with empty credentials

[TestFixture]
[Category("Authentication")]
public class AuthTests : TestBase
{
    // ─── TC-01: Register User ─────────────────────────────────────────────────

    
    /// TC-01: Verifies the full user registration flow — from entering name/email
    /// on the signup form, through all registration fields, to account creation
    /// confirmation and cleanup via account deletion.
    
    [Test]
    [Description("TC-01: Register User - Full registration flow")]
    public async Task TC01_RegisterUser_ShouldCreateAccountSuccessfully()
    {
        // Arrange
        var user = UserData.CreateDefault();

        // Act - Navigate and open login/signup page
        await HomePage.GoToHomeAsync();
        await Expect(Page.Locator("#slider")).ToBeVisibleAsync();

        await HomePage.ClickSignupLoginAsync();
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/login");

        // Verify signup heading is visible
        Assert.That(await LoginPage.IsSignupHeadingVisibleAsync(), Is.True,
            "New User Signup! heading should be visible");

        // Fill signup form
        await LoginPage.FillSignupNameAndEmailAsync(user.Name, user.Email);

        // Verify account info heading appears
        await Expect(Page.GetByText("ENTER ACCOUNT INFORMATION")).ToBeVisibleAsync();

        // Fill all account details and submit
        await SignupPage.FillAccountDetailsAsync(user);

        // Verify account created
        await Expect(Page.GetByText("ACCOUNT CREATED!")).ToBeVisibleAsync();

        await SignupPage.ClickContinueAsync();

        // Verify logged in as the new user
        await Expect(Page.GetByText($"Logged in as {user.Name}")).ToBeVisibleAsync();

        // Cleanup: delete the account
        await DeleteAccountAsync();
    }

    // ─── TC-02: Login with correct credentials ────────────────────────────────

    
    /// TC-02: Verifies that a user with valid credentials can log in successfully
    /// and the nav bar reflects the logged-in state.
    /// Pre-condition: A fresh user account is created before the test.
    
    [Test]
    [Description("TC-02: Login User with correct email and password")]
    public async Task TC02_LoginWithCorrectCredentials_ShouldLoginSuccessfully()
    {
        // Arrange: Register a user first so we have valid credentials
        var user = await RegisterUserAsync();

        // Act: Logout and log back in with the credentials
        await HomePage.ClickLogoutAsync();
        await LoginPage.LoginAsync(user.Email, user.Password);

        // Assert: Verify login success
        await Expect(Page.GetByText($"Logged in as {user.Name}")).ToBeVisibleAsync();

        // Cleanup
        await DeleteAccountAsync();
    }

    // ─── TC-03: Login with incorrect credentials ──────────────────────────────

    
    /// TC-03: Verifies that logging in with wrong email/password shows the
    /// appropriate error message and does NOT authenticate the user.
    
    [Test]
    [Description("TC-03: Login User with incorrect email and password")]
    public async Task TC03_LoginWithIncorrectCredentials_ShouldShowError()
    {
        // Arrange
        await HomePage.GoToHomeAsync();
        await HomePage.ClickSignupLoginAsync();

        // Verify login heading
        Assert.That(await LoginPage.IsLoginHeadingVisibleAsync(), Is.True,
            "Login to your account heading should be visible");

        // Act: Attempt login with invalid credentials
        await LoginPage.LoginAsync("invalid@nonexistentdomain.xyz", "WrongPassword999");

        // Assert: Error message is shown
        await Expect(Page.GetByText("Your email or password is incorrect!")).ToBeVisibleAsync();

        // Assert: User is NOT logged in
        Assert.That(await HomePage.IsLoggedInAsync(), Is.False,
            "User should not be logged in after invalid credentials");
    }

    // ─── TC-05: Register with existing email ─────────────────────────────────

    
    /// TC-05: Verifies that attempting to register with an already-registered
    /// email shows the "Email Address already exist!" error.
    
    [Test]
    [Description("TC-05: Register User with existing email")]
    public async Task TC05_RegisterWithExistingEmail_ShouldShowError()
    {
        // Arrange: Register a user first to create a known existing email
        var user = await RegisterUserAsync();
        var existingEmail = user.Email;

        // Logout and try to sign up with the same email
        await HomePage.ClickLogoutAsync();
        await HomePage.ClickSignupLoginAsync();

        // Act: Attempt signup with existing email
        await LoginPage.FillSignupNameAndEmailAsync("Another User", existingEmail);

        // Assert: Error message is displayed
        await Expect(Page.GetByText("Email Address already exist!")).ToBeVisibleAsync();

        // Cleanup: log back in and delete the original account
        await HomePage.ClickSignupLoginAsync();
        await LoginPage.LoginAsync(user.Email, user.Password);
        await DeleteAccountAsync();
    }

    // ─── TC-OPT-01: Login with empty credentials (Additional) ────────────────

    
    /// TC-OPT-01 (Additional): Verifies that submitting the login form with empty
    /// fields does not proceed and shows browser/client-side validation.
    /// This covers an edge case not in the existing 26 test cases.
    
    [Test]
    [Description("TC-OPT-01 (Additional): Login with empty credentials should not submit")]
    public async Task TC_OPT_01_LoginWithEmptyCredentials_ShouldNotProceed()
    {
        // Arrange
        await HomePage.GoToHomeAsync();
        await HomePage.ClickSignupLoginAsync();

        // Act: Click login button without filling any fields
        await Page.Locator("button[data-qa='login-button']").ClickAsync();

        // Assert: User remains on the login page (URL should not change)
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/login");

        // Assert: No "Logged in as" text appears
        Assert.That(await HomePage.IsLoggedInAsync(), Is.False,
            "User should not be logged in after empty form submission");
    }
}

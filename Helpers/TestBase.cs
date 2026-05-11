using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using AutomationExercise.Tests.Models;
using AutomationExercise.Tests.PageObjects;

namespace AutomationExercise.Tests.Helpers;


/// Base test class for all AutomationExercise test classes.
/// Provides shared Playwright page setup, teardown, and common helper flows
/// (such as registering/logging in a user) reusable across multiple tests.

[TestFixture]
public abstract class TestBase : PageTest
{
    protected const string BaseUrl = "https://automationexercise.com";

    // Page objects — initialized in SetUp
    protected HomePage HomePage = null!;
    protected LoginPage LoginPage = null!;
    protected SignupPage SignupPage = null!;
    protected ProductsPage ProductsPage = null!;
    protected CartPage CartPage = null!;
    protected CheckoutPage CheckoutPage = null!;

    [SetUp]
    public async Task SetUpPageObjects()
    {
        Page.SetDefaultTimeout(60000);

        // Block all known ad networks and trackers on this site
        await Page.RouteAsync("**/*doubleclick*", route => route.AbortAsync());
        await Page.RouteAsync("**/*googlesyndication*", route => route.AbortAsync());
        await Page.RouteAsync("**/*adsbygoogle*", route => route.AbortAsync());
        await Page.RouteAsync("**/*amazon-adsystem*", route => route.AbortAsync());
        await Page.RouteAsync("**/*ads.pubmatic*", route => route.AbortAsync());
        await Page.RouteAsync("**/*pagead*", route => route.AbortAsync());
        await Page.RouteAsync("**/*adsafeprotected*", route => route.AbortAsync());
        await Page.RouteAsync("**/*moatads*", route => route.AbortAsync());
        await Page.RouteAsync("**/*adnxs*", route => route.AbortAsync());

        HomePage = new HomePage(Page);
        LoginPage = new LoginPage(Page);
        SignupPage = new SignupPage(Page);
        ProductsPage = new ProductsPage(Page);
        CartPage = new CartPage(Page);
        CheckoutPage = new CheckoutPage(Page);
    }

    // ─── Reusable Helper Flows ───────────────────────────────────────────────────

    
    /// Registers a new user and returns the user data used. After calling this,
    /// the browser will be on the home page logged in as the new user.
    
    protected async Task<UserData> RegisterUserAsync(UserData? user = null)
    {
        user ??= UserData.CreateDefault();
        await HomePage.GoToHomeAsync();
        await HomePage.ClickSignupLoginAsync();
        await LoginPage.FillSignupNameAndEmailAsync(user.Name, user.Email);
        await SignupPage.FillAccountDetailsAsync(user);
        await SignupPage.ClickContinueAsync();
        return user;
    }

    
    /// Logs in an existing user. Assumes the user already has an account.
    
    protected async Task LoginUserAsync(string email, string password)
    {
        await HomePage.GoToHomeAsync();
        await HomePage.ClickSignupLoginAsync();
        await LoginPage.LoginAsync(email, password);
    }

    
    /// Deletes the currently logged-in user's account and verifies deletion.
    
    protected async Task DeleteAccountAsync()
    {
        await HomePage.ClickDeleteAccountAsync();
        await Expect(Page.GetByText("ACCOUNT DELETED!")).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Continue" }).ClickAsync();
    }

    
    /// Adds the first product from the products listing to the cart
    /// and returns to the products page.
    
    protected async Task AddFirstProductToCartAsync()
    {
        await HomePage.ClickProductsAsync();
        await ProductsPage.AddProductToCartByIndexAsync(0);
        await CartPage.ClickContinueShoppingAsync();
    }

    
    /// Override Playwright's default BrowserNewContextOptions to configure
    /// viewport, locale, and video/tracing for all tests.
    
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 800 },
            Locale = "en-US",
            RecordVideoDir = "videos/",
        };
    }
}

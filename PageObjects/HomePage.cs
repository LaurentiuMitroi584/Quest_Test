using Microsoft.Playwright;

namespace AutomationExercise.Tests.PageObjects;


/// Page object for the home page (https://automationexercise.com).

public class HomePage : BasePage
{
    public HomePage(IPage page) : base(page) { }

    // ─── Locators ───────────────────────────────────────────────────────────────

    private ILocator SubscriptionInput => Page.Locator("#susbscribe_email");
    private ILocator SubscriptionButton => Page.Locator("#subscribe");
    private ILocator SubscriptionSuccessAlert => Page.Locator("#success-subscribe");
    private ILocator RecommendedItemsSection => Page.Locator(".recommended_items");
    private ILocator ScrollUpArrow => Page.Locator("#scrollUp");
    private ILocator HeroText => Page.GetByText("Full-Fledged practice website for Automation Engineers");

    // ─── Actions ────────────────────────────────────────────────────────────────

    
    /// Subscribes to the newsletter using the footer subscription form.
    
    public async Task SubscribeAsync(string email)
    {
        await ScrollToBottomAsync();
        await SubscriptionInput.FillAsync(email);
        await SubscriptionButton.ClickAsync();
    }

    
    /// Returns true if the subscription success alert is visible.
    
    public async Task<bool> IsSubscriptionSuccessVisibleAsync()
    {
        await SubscriptionSuccessAlert.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        return await SubscriptionSuccessAlert.IsVisibleAsync();
    }

    
    /// Returns true if the SUBSCRIPTION text in the footer is visible.
    
    public async Task<bool> IsSubscriptionTextVisibleAsync()
    {
        return await Page.GetByText("SUBSCRIPTION").IsVisibleAsync();
    }

    
    /// Returns true if the RECOMMENDED ITEMS section is visible on the home page.
    
    public async Task<bool> IsRecommendedItemsSectionVisibleAsync()
    {
        return await RecommendedItemsSection.IsVisibleAsync();
    }

    
    /// Clicks the "Add To Cart" button for the first recommended item.
    
    public async Task AddFirstRecommendedItemToCartAsync()
    {
        var addButtons = Page.Locator(".recommended_items .add-to-cart");
        await addButtons.First.ClickAsync();
    }

    
    /// Clicks the scroll-up arrow button (bottom-right arrow).
    
    public async Task ClickScrollUpArrowAsync()
    {
        await ScrollUpArrow.ClickAsync();
        await Page.WaitForTimeoutAsync(1000);
    }

    
    /// Returns true if the hero/banner text is visible (indicates page is at top).
    
    public async Task<bool> IsHeroTextVisibleAsync()
    {
        return await HeroText.IsVisibleAsync();
    }

    
    /// Clicks the "View Product" link for the first product on the home page.
    
    public async Task ViewFirstProductAsync()
    {
        await Page.GetByRole(AriaRole.Link, new() { Name = "View Product" }).First.ClickAsync();
    }

    
    /// Returns true if the home page carousel/slider is visible (basic home page check).
    
    public async Task<bool> IsHomePageLoadedAsync()
    {
        return await Page.Locator("#slider").IsVisibleAsync();
    }
}

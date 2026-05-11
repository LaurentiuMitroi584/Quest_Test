using Microsoft.Playwright;

namespace AutomationExercise.Tests.PageObjects;


/// Base class for all Page Objects. Holds the IPage reference and provides
/// common navigation/assertion helpers shared across all pages.
public abstract class BasePage
{
    protected readonly IPage Page;
    protected const string BaseUrl = "https://automationexercise.com";

    protected BasePage(IPage page)
    {
        Page = page;
    }

    public async Task GoToHomeAsync()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }

    public async Task ClickSignupLoginAsync()
    {
        await Page.GotoAsync($"{BaseUrl}/login");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }

    public async Task ClickCartAsync()
    {
        await Page.GotoAsync($"{BaseUrl}/view_cart");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }

    public async Task ClickProductsAsync()
    {
        await Page.GotoAsync($"{BaseUrl}/products");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }

    public async Task ClickLogoutAsync()
    {
        await Page.GotoAsync($"{BaseUrl}/logout");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }

    public async Task ClickDeleteAccountAsync()
    {
        await Page.GotoAsync($"{BaseUrl}/delete_account");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }

    public async Task<bool> IsLoggedInAsync()
    {
        return await Page.GetByText("Logged in as").IsVisibleAsync();
    }

    public async Task ScrollToBottomAsync()
    {
        await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
        await Page.WaitForTimeoutAsync(500);
    }

    public async Task ScrollToTopAsync()
    {
        await Page.EvaluateAsync("window.scrollTo(0, 0)");
        await Page.WaitForTimeoutAsync(500);
    }
}
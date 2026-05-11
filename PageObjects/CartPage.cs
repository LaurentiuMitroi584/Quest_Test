using Microsoft.Playwright;

namespace AutomationExercise.Tests.PageObjects;

/// Page object for the Shopping Cart page (/view_cart).
public class CartPage : BasePage
{
    public CartPage(IPage page) : base(page) { }

    private ILocator CartItems => Page.Locator("tr[id^='product-']");
    private ILocator ContinueShoppingButton => Page.Locator(".modal-footer .btn-success");
    private ILocator EmptyCartMessage => Page.GetByText("Cart is empty!");

    public async Task ClickContinueShoppingAsync()
    {
        // Close the "Added to cart" modal
        var modal = Page.Locator("#cartModal");
        if (await modal.IsVisibleAsync())
        {
            await Page.Locator("#cartModal .btn-success").ClickAsync();
            await modal.WaitForAsync(new() { State = WaitForSelectorState.Hidden });
        }
    }

    public async Task ClickViewCartAsync()
    {
        // Navigate directly — avoids the modal link being blocked
        await Page.GotoAsync("https://automationexercise.com/view_cart");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }

    public async Task RemoveCartItemByIndexAsync(int index)
    {
        var deleteButton = CartItems.Nth(index).Locator(".cart_delete a");
        await deleteButton.ClickAsync();
        await Page.WaitForTimeoutAsync(800);
    }

    public async Task<string> GetCartItemQuantityAsync(int index)
    {
        // The quantity cell contains a <button> with the number
        var cell = CartItems.Nth(index).Locator("td.cart_quantity");
        return (await cell.InnerTextAsync()).Trim();
    }

    public async Task<int> GetCartItemCountAsync()
    {
        await Page.WaitForTimeoutAsync(1000);
        int rowCount = await CartItems.CountAsync();

        int totalQuantity = 0;

        for (int i = 0; i < rowCount; i++)
        {
            var row = CartItems.Nth(i);

            var quantityText = await row
                .Locator("td.cart_quantity button")
                .TextContentAsync();

            totalQuantity += int.Parse(quantityText!.Trim());
        }

        return totalQuantity;
    }

    public async Task<bool> IsCartEmptyAsync() =>
        await EmptyCartMessage.IsVisibleAsync();

    public async Task<bool> IsSubscriptionTextVisibleAsync() =>
        await Page.GetByText("SUBSCRIPTION").IsVisibleAsync();
}
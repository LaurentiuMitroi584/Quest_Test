using AutomationExercise.Tests.Helpers;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.Tests;

/// Shopping cart tests covering:
///   TC-12: Add Products in Cart
///   TC-13: Verify Product quantity in Cart
///   TC-17: Remove Products From Cart
/// 
[TestFixture]
[Category("Cart")]
public class CartTests : TestBase
{
    [Test]
    [Description("TC-12: Add Products in Cart")]
    public async Task TC12_AddTwoProductsToCart_BothShouldAppearInCart()
    {
        await HomePage.GoToHomeAsync();
        await HomePage.ClickProductsAsync();
        await Expect(Page.GetByText("ALL PRODUCTS")).ToBeVisibleAsync();

        // Add first product
        await ProductsPage.AddProductToCartByIndexAsync(0);
        await Page.WaitForTimeoutAsync(1500);
        await CartPage.ClickContinueShoppingAsync();
        await Page.WaitForTimeoutAsync(500);

        // Add second product
        await ProductsPage.AddProductToCartByIndexAsync(1);
        await ProductsPage.AddProductToCartByIndexAsync(1);
        await Page.WaitForTimeoutAsync(1500);

        // Navigate to cart directly
        await Page.GotoAsync($"{BaseUrl}/view_cart");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await Page.WaitForTimeoutAsync(1000);

        var cartCount = await CartPage.GetCartItemCountAsync();
        Assert.That(cartCount, Is.EqualTo(3),
            "Cart should contain exactly 3 products after adding three items");
    }

    [Test]
    [Description("TC-13: Verify Product quantity in Cart")]
    public async Task TC13_SetProductQuantityToFour_CartShouldReflectCorrectQuantity()
    {
        await HomePage.GoToHomeAsync();
        await HomePage.ClickProductsAsync();

        // Open first product detail page
        await Page.GetByRole(AriaRole.Link, new() { Name = "View Product" }).First.ClickAsync();
        await Expect(Page.Locator(".product-information h2")).ToBeVisibleAsync();

        // Set quantity to 4
        await Page.Locator("input#quantity").ClearAsync();
        await Page.Locator("input#quantity").FillAsync("4");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Add to cart" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1500);

        // Go directly to cart
        await Page.GotoAsync($"{BaseUrl}/view_cart");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await Page.WaitForTimeoutAsync(1000);

        var quantity = await CartPage.GetCartItemQuantityAsync(0);
        Assert.That(quantity, Is.EqualTo("4"),
            "Cart quantity should be 4 as set on the product detail page");
    }

    [Test]
    [Description("TC-17: Remove Products From Cart")]
    public async Task TC17_RemoveProductFromCart_CartShouldBeEmpty()
    {
        // Add a product via direct navigation
        await HomePage.ClickProductsAsync();
        await Expect(Page.GetByText("ALL PRODUCTS")).ToBeVisibleAsync();

        await ProductsPage.AddProductToCartByIndexAsync(0);
        await Page.WaitForTimeoutAsync(1500);

        // Go directly to cart
        await Page.GotoAsync($"{BaseUrl}/view_cart");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await Page.WaitForTimeoutAsync(1000);

        Assert.That(await CartPage.GetCartItemCountAsync(), Is.GreaterThan(0),
            "Cart should have items before removal");

        // Remove first item
        await CartPage.RemoveCartItemByIndexAsync(0);
        await Page.WaitForTimeoutAsync(1000);

        await Expect(Page.GetByText("Cart is empty!")).ToBeVisibleAsync();
    }

    [Test]
    [Description("TC-OPT-03: Verify empty cart message")]
    public async Task TC_OPT_03_NavigateToEmptyCart_ShouldShowEmptyMessage()
    {
        await Page.GotoAsync($"{BaseUrl}/view_cart");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await Expect(Page.GetByText("Cart is empty!")).ToBeVisibleAsync();
    }

    [Test]
    [Description("TC-10: Verify Subscription in home page")]
    public async Task TC10_SubscribeFromHomePage_ShouldShowSuccessMessage()
    {
        await HomePage.GoToHomeAsync();
        await HomePage.ScrollToBottomAsync();

        Assert.That(await Page.GetByText("SUBSCRIPTION").IsVisibleAsync(), Is.True,
            "SUBSCRIPTION text should be visible in the footer");

        await Page.Locator("#susbscribe_email").FillAsync("newsletter_test@example.com");
        await Page.Locator("#subscribe").ClickAsync();

        await Expect(Page.Locator("#success-subscribe")).ToBeVisibleAsync();
    }
}
using Microsoft.Playwright;

namespace AutomationExercise.Tests.PageObjects;


/// Page object for the All Products page (/products) and product detail page.

public class ProductsPage : BasePage
{
    public ProductsPage(IPage page) : base(page) { }

    // ─── Locators ───────────────────────────────────────────────────────────────

    private ILocator AllProductsHeading => Page.GetByText("ALL PRODUCTS");
    private ILocator ProductsList => Page.Locator(".features_items .product-image-wrapper");
    private ILocator SearchInput => Page.Locator("#search_product");
    private ILocator SearchButton => Page.Locator("#submit_search");
    private ILocator SearchedProductsHeading => Page.GetByText("SEARCHED PRODUCTS");

    // Product detail locators
    private ILocator ProductDetailName => Page.Locator(".product-information h2");
    private ILocator ProductDetailCategory => Page.Locator(".product-information p:has-text('Category')");
    private ILocator ProductDetailPrice => Page.Locator(".product-information span span");
    private ILocator ProductDetailAvailability => Page.Locator(".product-information p:has-text('Availability')");
    private ILocator ProductDetailCondition => Page.Locator(".product-information p:has-text('Condition')");
    private ILocator ProductDetailBrand => Page.Locator(".product-information p:has-text('Brand')");
    private ILocator QuantityInput => Page.Locator("input#quantity");
    private ILocator AddToCartButton => Page.GetByRole(AriaRole.Button, new() { Name = "Add to cart" });

    // Review form locators
    private ILocator WriteReviewHeading => Page.GetByText("Write Your Review");
    private ILocator ReviewNameInput => Page.Locator("input#name");
    private ILocator ReviewEmailInput => Page.Locator("input#email");
    private ILocator ReviewTextArea => Page.Locator("textarea#review");
    private ILocator ReviewSubmitButton => Page.Locator("button#button-review");
    private ILocator ReviewSuccessMessage => Page.GetByText("Thank you for your review.");

    // ─── Actions ────────────────────────────────────────────────────────────────

    public async Task SearchProductAsync(string productName)
    {
        await SearchInput.FillAsync(productName);
        await SearchButton.ClickAsync();
    }

    
    /// Clicks "View Product" for the first product in the list.
    
    public async Task ViewFirstProductAsync()
    {
        await Page.GetByRole(AriaRole.Link, new() { Name = "View Product" }).First.ClickAsync();
    }

    
    /// Hovers over a product at the given index and clicks "Add to cart".
    
    public async Task AddProductToCartByIndexAsync(int index)
    {
        var product = ProductsList.Nth(index);
        await product.HoverAsync();
        await product.Locator(".overlay-content .btn").ClickAsync();
    }

    public async Task SetQuantityAsync(int quantity)
    {
        await QuantityInput.ClearAsync();
        await QuantityInput.FillAsync(quantity.ToString());
    }

    public async Task ClickAddToCartAsync() => await AddToCartButton.ClickAsync();

    
    /// Submits a product review with the given details.
    
    public async Task SubmitReviewAsync(string name, string email, string reviewText)
    {
        await ReviewNameInput.FillAsync(name);
        await ReviewEmailInput.FillAsync(email);
        await ReviewTextArea.FillAsync(reviewText);
        await ReviewSubmitButton.ClickAsync();
    }

    // ─── Assertions ─────────────────────────────────────────────────────────────

    public async Task<bool> IsAllProductsHeadingVisibleAsync() =>
        await AllProductsHeading.IsVisibleAsync();

    public async Task<bool> IsSearchedProductsHeadingVisibleAsync() =>
        await SearchedProductsHeading.IsVisibleAsync();

    public async Task<int> GetProductCountAsync() =>
        await ProductsList.CountAsync();

    public async Task<bool> IsProductDetailNameVisibleAsync() =>
        await ProductDetailName.IsVisibleAsync();

    public async Task<bool> IsProductDetailCategoryVisibleAsync() =>
        await ProductDetailCategory.IsVisibleAsync();

    public async Task<bool> IsProductDetailPriceVisibleAsync() =>
        await ProductDetailPrice.IsVisibleAsync();

    public async Task<bool> IsProductDetailAvailabilityVisibleAsync() =>
        await ProductDetailAvailability.IsVisibleAsync();

    public async Task<bool> IsProductDetailConditionVisibleAsync() =>
        await ProductDetailCondition.IsVisibleAsync();

    public async Task<bool> IsProductDetailBrandVisibleAsync() =>
        await ProductDetailBrand.IsVisibleAsync();

    public async Task<bool> IsWriteReviewVisibleAsync() =>
        await WriteReviewHeading.IsVisibleAsync();

    public async Task<bool> IsReviewSuccessMessageVisibleAsync() =>
        await ReviewSuccessMessage.IsVisibleAsync();
}

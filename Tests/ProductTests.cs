using AutomationExercise.Tests.Helpers;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.Tests;


/// Product browsing and interaction tests covering:
///   TC-08: Verify All Products and product detail page
///   TC-09: Search Product
///   TC-21: Add review on product

[TestFixture]
[Category("Products")]
public class ProductTests : TestBase
{
    // ─── TC-08: Verify All Products and product detail page ──────────────────

    
    /// TC-08: Navigates to the Products page, verifies the product list is visible,
    /// then opens the first product's detail page and checks all required fields.
    
    [Test]
    [Description("TC-08: Verify All Products and product detail page")]
    public async Task TC08_AllProducts_ShouldShowProductListAndDetailPage()
    {
        // Navigate to home and go to products
        await HomePage.GoToHomeAsync();
        await HomePage.ClickProductsAsync();
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/products");

        // Verify ALL PRODUCTS heading
        await Expect(Page.GetByText("ALL PRODUCTS")).ToBeVisibleAsync();

        // Verify products list is visible and not empty
        var productCount = await ProductsPage.GetProductCountAsync();
        Assert.That(productCount, Is.GreaterThan(0), "Products list should contain at least one product");

        // Open first product detail
        await ProductsPage.ViewFirstProductAsync();

        // Verify all required detail fields are present
        Assert.Multiple(async () =>
        {
            Assert.That(await ProductsPage.IsProductDetailNameVisibleAsync(), Is.True,
                "Product name should be visible on detail page");
            Assert.That(await ProductsPage.IsProductDetailCategoryVisibleAsync(), Is.True,
                "Product category should be visible on detail page");
            Assert.That(await ProductsPage.IsProductDetailPriceVisibleAsync(), Is.True,
                "Product price should be visible on detail page");
            Assert.That(await ProductsPage.IsProductDetailAvailabilityVisibleAsync(), Is.True,
                "Product availability should be visible on detail page");
            Assert.That(await ProductsPage.IsProductDetailConditionVisibleAsync(), Is.True,
                "Product condition should be visible on detail page");
            Assert.That(await ProductsPage.IsProductDetailBrandVisibleAsync(), Is.True,
                "Product brand should be visible on detail page");
        });
    }

    // ─── TC-09: Search Product ────────────────────────────────────────────────

    
    /// TC-09: Searches for a product by name and verifies that the search results
    /// heading is shown and related products appear in the results.
    
    [Test]
    [Description("TC-09: Search Product")]
    public async Task TC09_SearchProduct_ShouldShowRelatedResults()
    {
        // Navigate to products page
        await HomePage.GoToHomeAsync();
        await HomePage.ClickProductsAsync();
        await Expect(Page.GetByText("ALL PRODUCTS")).ToBeVisibleAsync();

        // Search for a term known to return results
        const string searchTerm = "Top";
        await ProductsPage.SearchProductAsync(searchTerm);

        // Verify SEARCHED PRODUCTS heading appears
        await Expect(Page.GetByText("SEARCHED PRODUCTS")).ToBeVisibleAsync();

        // Verify at least one product result is shown
        var resultCount = await ProductsPage.GetProductCountAsync();
        Assert.That(resultCount, Is.GreaterThan(0),
            $"Search for '{searchTerm}' should return at least one product");
    }

    // ─── TC-21: Add review on product ────────────────────────────────────────

    
    /// TC-21: Opens a product detail page, fills the review form with name,
    /// email, and review text, submits it, and verifies the success message.
    
    [Test]
    [Description("TC-21: Add review on product")]
    public async Task TC21_AddProductReview_ShouldShowSuccessMessage()
    {
        // Navigate to products page
        await HomePage.GoToHomeAsync();
        await HomePage.ClickProductsAsync();

        // View the first product
        await ProductsPage.ViewFirstProductAsync();

        // Verify review form is visible
        await Expect(Page.GetByText("Write Your Review")).ToBeVisibleAsync();

        // Submit a review
        await ProductsPage.SubmitReviewAsync(
            name: "QA Tester",
            email: "qatester@example.com",
            reviewText: "This is an automated test review. The product looks great!"
        );

        // Verify success message
        await Expect(Page.GetByText("Thank you for your review.")).ToBeVisibleAsync();
    }

    // ─── TC-OPT-02: Search with empty query (Additional) ─────────────────────

    
    /// TC-OPT-02 (Additional): Searches with an empty query to verify the
    /// application handles it gracefully without crashing.
    
    [Test]
    [Description("TC-OPT-02 (Additional): Search with empty string should not crash")]
    public async Task TC_OPT_02_SearchWithEmptyString_ShouldHandleGracefully()
    {
        await HomePage.GoToHomeAsync();
        await HomePage.ClickProductsAsync();

        // Submit empty search
        await ProductsPage.SearchProductAsync("");

        // Verify the page did not crash — it should still show a heading
        var pageTitle = await Page.TitleAsync();
        Assert.That(pageTitle, Is.Not.Null.And.Not.Empty,
            "Page should still render a title after empty search");
    }
}

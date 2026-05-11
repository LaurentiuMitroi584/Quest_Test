using Microsoft.Playwright;
using AutomationExercise.Tests.Models;

namespace AutomationExercise.Tests.PageObjects;


/// Page object for the Checkout flow pages (/checkout and /payment).

public class CheckoutPage : BasePage
{
    public CheckoutPage(IPage page) : base(page) { }

    // ─── Locators ───────────────────────────────────────────────────────────────

    private ILocator RegisterLoginLink => Page.GetByText("Register / Login");
    private ILocator CommentTextArea => Page.Locator("textarea[name='message']");
    private ILocator PlaceOrderButton => Page.GetByRole(AriaRole.Link, new() { Name = "Place Order" });
    private ILocator DeliveryAddressSection => Page.Locator("#address_delivery");
    private ILocator BillingAddressSection => Page.Locator("#address_invoice");

    // Payment locators
    private ILocator CardNameInput => Page.Locator("input[data-qa='name-on-card']");
    private ILocator CardNumberInput => Page.Locator("input[data-qa='card-number']");
    private ILocator CardCvcInput => Page.Locator("input[data-qa='cvc']");
    private ILocator CardExpiryMonthInput => Page.Locator("input[data-qa='expiry-month']");
    private ILocator CardExpiryYearInput => Page.Locator("input[data-qa='expiry-year']");
    private ILocator PayConfirmButton => Page.Locator("button[data-qa='pay-button']");
    private ILocator OrderSuccessMessage => Page.GetByText("Your order has been placed successfully!");
    private ILocator DownloadInvoiceButton => Page.GetByRole(AriaRole.Link, new() { Name = "Download Invoice" });
    private ILocator ContinueButton => Page.GetByRole(AriaRole.Link, new() { Name = "Continue" });

    // ─── Actions ────────────────────────────────────────────────────────────────

    public async Task ClickRegisterLoginAsync() => await RegisterLoginLink.ClickAsync();

    public async Task EnterCommentAsync(string comment) => await CommentTextArea.FillAsync(comment);

    public async Task ClickPlaceOrderAsync() => await PlaceOrderButton.ClickAsync();

    public async Task ClickDownloadInvoiceAsync() => await DownloadInvoiceButton.ClickAsync();

    public async Task ClickContinueAsync() => await ContinueButton.ClickAsync();

    
    /// Fills in payment card details and submits the payment form.
    
    public async Task FillPaymentDetailsAsync(PaymentData payment)
    {
        await CardNameInput.FillAsync(payment.NameOnCard);
        await CardNumberInput.FillAsync(payment.CardNumber);
        await CardCvcInput.FillAsync(payment.Cvc);
        await CardExpiryMonthInput.FillAsync(payment.ExpiryMonth);
        await CardExpiryYearInput.FillAsync(payment.ExpiryYear);
        await PayConfirmButton.ClickAsync();
    }

    // ─── Assertions ─────────────────────────────────────────────────────────────

    public async Task<bool> IsOrderSuccessMessageVisibleAsync() =>
        await OrderSuccessMessage.IsVisibleAsync();

    public async Task<string> GetDeliveryAddressTextAsync() =>
        await DeliveryAddressSection.InnerTextAsync();

    public async Task<string> GetBillingAddressTextAsync() =>
        await BillingAddressSection.InnerTextAsync();
}

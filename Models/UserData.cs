namespace AutomationExercise.Tests.Models;


/// Represents all data needed for user account creation during tests.

public class UserData
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Company { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Address2 { get; init; } = string.Empty;
    public string Country { get; init; } = "United States";
    public string State { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string MobileNumber { get; init; } = string.Empty;
    public string DateOfBirthDay { get; init; } = "15";
    public string DateOfBirthMonth { get; init; } = "6";
    public string DateOfBirthYear { get; init; } = "1990";

    
    /// Creates a default user with a unique email to avoid conflicts between test runs.
    
    public static UserData CreateDefault()
    {
        var uniqueId = DateTime.UtcNow.Ticks;
        return new UserData
        {
            Name = "Test User",
            Email = $"testuser_{uniqueId}@testmail.com",
            Password = "Password@123",
            FirstName = "Test",
            LastName = "User",
            Company = "Test Corp",
            Address = "123 Test Street",
            Address2 = "Suite 456",
            Country = "United States",
            State = "California",
            City = "Los Angeles",
            ZipCode = "90001",
            MobileNumber = "5551234567",
            DateOfBirthDay = "15",
            DateOfBirthMonth = "6",
            DateOfBirthYear = "1990"
        };
    }
}


/// Represents payment card details used during checkout.

public class PaymentData
{
    public string NameOnCard { get; init; } = "Test User";
    public string CardNumber { get; init; } = "4111111111111111";
    public string Cvc { get; init; } = "123";
    public string ExpiryMonth { get; init; } = "12";
    public string ExpiryYear { get; init; } = "2027";
}

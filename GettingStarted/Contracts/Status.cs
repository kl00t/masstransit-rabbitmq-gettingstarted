namespace GettingStarted.Contracts;

public record Status(string Type)
{
    public const string SHIPPED_STATUS = "shipped";
}

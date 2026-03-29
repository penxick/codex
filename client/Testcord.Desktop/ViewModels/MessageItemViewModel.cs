namespace Testcord.Desktop.ViewModels;

public sealed class MessageItemViewModel
{
    public required string Author { get; init; }
    public required string Time { get; init; }
    public required string Content { get; init; }
}

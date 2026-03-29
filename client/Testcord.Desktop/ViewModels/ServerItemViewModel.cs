namespace Testcord.Desktop.ViewModels;

public sealed class ServerItemViewModel
{
    public required string Glyph { get; init; }
    public required string Name { get; init; }
    public bool IsSelected { get; init; }
}

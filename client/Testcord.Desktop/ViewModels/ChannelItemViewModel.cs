namespace Testcord.Desktop.ViewModels;

public sealed class ChannelItemViewModel
{
    public required string Prefix { get; init; }
    public required string Name { get; init; }
    public bool IsSelected { get; init; }
}

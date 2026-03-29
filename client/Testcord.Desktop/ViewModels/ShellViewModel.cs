using System.Collections.ObjectModel;
using Testcord.Desktop.Services;

namespace Testcord.Desktop.ViewModels;

public sealed class ShellViewModel : ViewModelBase
{
    private readonly IRealtimeClient _realtimeClient;
    private string _draftMessage = string.Empty;

    public ShellViewModel(IRealtimeClient realtimeClient)
    {
        _realtimeClient = realtimeClient;
        Title = "Testcord";
        CurrentWorkspace = "Architecture";
        CurrentChannel = "foundation";
        DraftMessage = "Stage 1 foundation is ready for Auth.";

        Servers = new ObservableCollection<ServerItemViewModel>
        {
            new() { Glyph = "TC", Name = "Testcord", IsSelected = true },
            new() { Glyph = "VD", Name = "Voice Lab" },
            new() { Glyph = "DM", Name = "Direct Messages" }
        };

        Channels = new ObservableCollection<ChannelItemViewModel>
        {
            new() { Prefix = "#", Name = "foundation", IsSelected = true },
            new() { Prefix = "#", Name = "backend" },
            new() { Prefix = "#", Name = "desktop" },
            new() { Prefix = "V", Name = "call-mvp" }
        };

        Messages = new ObservableCollection<MessageItemViewModel>
        {
            new() { Author = "System", Time = "09:00", Content = "Solution, server, shared contracts, docker and EF foundation are in progress." },
            new() { Author = "Design", Time = "09:03", Content = "Desktop shell mirrors Discord layout so feature modules can attach without redesign." },
            new() { Author = "Voice", Time = "09:05", Content = "Voice stays MVP-first: signaling now, media transport next stage." }
        };
    }

    public string Title { get; }

    public string CurrentWorkspace { get; }

    public string CurrentChannel { get; }

    public ObservableCollection<ServerItemViewModel> Servers { get; }

    public ObservableCollection<ChannelItemViewModel> Channels { get; }

    public ObservableCollection<MessageItemViewModel> Messages { get; }

    public string DraftMessage
    {
        get => _draftMessage;
        set
        {
            _draftMessage = value;
            OnPropertyChanged();
        }
    }

    public Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        return _realtimeClient.InitializeAsync(cancellationToken);
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AZDO.ReleaseNotes.Infrastructure;

public class NotifyPropertyChanged : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = default)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void Set<T>(ref T property, T value, [CallerMemberName] string? propertyName = default)
    {
        if (property is null && value is null) return;
        if (property != null && property.Equals(value)) return;
        property = value;
        OnPropertyChanged(propertyName);
    }
}

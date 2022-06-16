using AZDO.ReleaseNotes.ViewModels;
using System;
using System.Windows.Input;

namespace AZDO.ReleaseNotes.Commands;

public class ExportReleaseNotesCommand : ICommand
{
    private readonly MainViewModel mainViewModel;

    public ExportReleaseNotesCommand(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;
        mainViewModel.PropertyChanged += OnMainViewModelPropertyChanged;
    }

    private void OnMainViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (!string.Equals(e.PropertyName, nameof(MainViewModel.IsExporting))) return;
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => !mainViewModel.IsExporting;

    public void Execute(object? parameter) => mainViewModel.Export();
}

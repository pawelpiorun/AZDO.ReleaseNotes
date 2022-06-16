using AZDO.ReleaseNotes.ViewModels;
using System;
using System.Windows.Input;

namespace AZDO.ReleaseNotes.Commands;

public class RunSearchCommand : ICommand
{
    private readonly MainViewModel mainViewModel;

    public RunSearchCommand(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;
        mainViewModel.PropertyChanged += OnMainViewModelPropertyChanged;
    }

    private void OnMainViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (!string.Equals(e.PropertyName, nameof(MainViewModel.IsProcessing))) return;
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => !mainViewModel.IsProcessing;

    public void Execute(object? parameter) => mainViewModel.Run();
}

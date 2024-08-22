using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using MusicStore.Models;
using MusicStore.ViewModels;
using ReactiveUI;

namespace MusicStore.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

        this.WhenActivated(action =>
            action(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));

        Interactions.Errors.RegisterHandler(
            async interaction =>
            {
                if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    Debug.WriteLine("passou");
                    var x = await desktop.MainWindow.StorageProvider.OpenFolderPickerAsync(
                        new FolderPickerOpenOptions());
                    Debug.WriteLine(x);
               
                    
                    interaction.SetOutput(ErrorRecoveryOption.Abort);
                }
                else
                {
                    Debug.WriteLine("nao passou");
                }
            });
    }

    private async Task DoShowDialogAsync(InteractionContext<MusicStoreViewModel,
        AlbumViewModel?> interaction)
    {
        var dialog = new MusicStoreWindow();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<AlbumViewModel?>(this);
        interaction.SetOutput(result);
    }
}
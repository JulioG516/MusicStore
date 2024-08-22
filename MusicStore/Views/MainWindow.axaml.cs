using System;
using System.Diagnostics;
using System.Linq;
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

                    var options =
                        new FolderPickerOpenOptions()
                        {
                            AllowMultiple = false,
                            Title = "Select the folder location."
                        };

                    var folders = await desktop.MainWindow.StorageProvider.OpenFolderPickerAsync(options);
                    Debug.WriteLine(folders.Any());
                    if (folders.Any())
                    {
                        Debug.WriteLine(folders.First().Path);
                    }

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
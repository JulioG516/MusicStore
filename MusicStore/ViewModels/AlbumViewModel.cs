using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using MusicStore.Models;
using ReactiveUI;

namespace MusicStore.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    private readonly Album _album;

    public AlbumViewModel(Album album)
    {
        _album = album;
        Command = ReactiveCommand.CreateFromTask(async () =>
        {
            var ex = new ArgumentException();

            
            
            // var recovery = await Interactions.Errors.Handle(ex);
            var recovery = await Interactions.GetFolder.Handle(Unit.Default);

            if (!string.IsNullOrEmpty(recovery))
            {
                Debug.WriteLine($"Retornou - {recovery}");

                var path = new FileSystemWatcher(recovery);
       
            }
            else
            {
                Debug.WriteLine("Não selecionou nada.");
            }

        });
    }

    public string Artist => _album.Artist;
    public string Title => _album.Title;

    public ICommand Command { get; }
    
    private Bitmap? _cover;

    public Bitmap? Cover
    {
        get => _cover;
        private set => this.RaiseAndSetIfChanged(ref _cover, value);
    }

    public async Task LoadCover()
    {
        await using (var imageStream = await _album.LoadCoverBitMapAsync())
        {
            Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
        }
    }

    public async Task SaveToDiskAsync()
    {
        await _album.SaveAsync();

        if (Cover != null)
        {
            var bitmap = Cover;

            await Task.Run(() =>
            {
                using (var fs = _album.SaveCoverBitmapStream())
                {
                    bitmap.Save(fs);
                }
            });
        }
    }
}
using System.Collections.Generic;
using LibVLCSharp.Shared;

namespace JukeCore
{
    public class PlayCommand : ICommand
    {
        private readonly string _id;
        private readonly IReadOnlyList<Media> _medias;
        private readonly IMediaPlayer _mediaPlayer;
        private readonly ICurrent _current;
        private readonly IPlaylist _playlist;
        private readonly IConsole _console;

        public PlayCommand(string id, IReadOnlyList<Media> medias, IMediaPlayer mediaPlayer, ICurrent current,
            IPlaylist playlist, IConsole console)
        {
            _id = id;
            _medias = medias;
            _mediaPlayer = mediaPlayer;
            _current = current;
            _playlist = playlist;
            _console = console;
        }

        public void Execute()
        {
            _console.WriteLine("Executing play command ...");
            _current.Id = _id;
            _playlist.Set(_medias);

            var firstMedia = _playlist.Next();
            _mediaPlayer.Play(firstMedia);
        }
    }
}
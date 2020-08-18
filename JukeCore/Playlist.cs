using System;
using System.Collections.Generic;
using System.Linq;
using LibVLCSharp.Shared;

namespace JukeCore
{
    public class Playlist : IPlaylist
    {
        private readonly IConsole _console;
        private List<Media> _medias = new List<Media>();
        private int _currentTrack;

        public Playlist(IConsole console)
        {
            _console = console;
        }

        public void Set(IEnumerable<Media> medias)
        {
            var mediaList = medias.ToList();
            _console.WriteLine($"Setting {mediaList.Count} tracks as new playlist.");
            foreach (var media in _medias)
            {
                media.Dispose();
            }

            _medias = new List<Media>(mediaList);
            _currentTrack = 0;
        }

        public bool AnyNext()
        {
            return _currentTrack < _medias.Count;
        }

        public bool AnyPrevious()
        {
            return _currentTrack > 1;
        }

        public Media Next()
        {
            if (AnyNext())
            {
                _currentTrack++;
                _console.WriteLine($"Returning next track {_currentTrack} / {_medias.Count} ...");
                return _medias[_currentTrack - 1];
            }

            throw new Exception("Last track already reached.");
        }

        public Media Previous()
        {
            if (AnyPrevious())
            {
                _currentTrack--;
                _console.WriteLine($"Returning previous track {_currentTrack} / {_medias.Count} ...");
                return _medias[_currentTrack - 1];
            }

            throw new Exception("First track already reached.");
        }
    }
}
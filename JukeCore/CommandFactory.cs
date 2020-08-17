using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using LibVLCSharp.Shared;
using Newtonsoft.Json;

namespace JukeCore
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IMediaPlayer _mediaPlayer;
        private readonly IMediaFactory _mediaFactory;
        private readonly IDirectory _directory;
        private readonly ICurrent _current;
        private readonly IPlaylist _playlist;
        private readonly IPath _path;
        private readonly IConsole _console;

        public CommandFactory(IMediaPlayer mediaPlayer, IMediaFactory mediaFactory, IDirectory directory,
            ICurrent current, IPlaylist playlist, IPath path, IConsole console)
        {
            _mediaPlayer = mediaPlayer;
            _mediaFactory = mediaFactory;
            _directory = directory;
            _current = current;
            _playlist = playlist;
            _path = path;
            _console = console;
        }

        public ICommand Create(string id, string jukeCorePath)
        {
            var mappingFilePath = Path.Combine(jukeCorePath, "mapping.json");
            var mappingJson = File.ReadAllText(mappingFilePath);
            var mapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(mappingJson);

            var value = mapping.GetValueOrDefault(id);
            if (value == null)
            {
                throw new Exception($"No mapping found for ID {id}");
            }

            _console.WriteLine($"Found mapping: {value}");

            var medias = CreateMediasFromDirectory(value, jukeCorePath);
            return new PlayCommand(value, medias, _mediaPlayer, _current, _playlist, _console);
        }

        private IReadOnlyList<Media> CreateMediasFromDirectory(string id, string path)
        {
            var directory = _path.Combine(path, id);
            var files = _directory.GetFiles(directory).OrderBy(x => x).ToList();

            if (!files.Any())
            {
                throw new Exception($"Mapped directory for ID {id} contains no files.");
            }

            _console.WriteLine($"Found {files.Count} files inside directory.");
            var medias = new List<Media>();
            foreach (var file in files)
            {
                var media = _mediaFactory.CreateFromPath(file);
                medias.Add(media);
            }

            return medias;
        }
    }
}
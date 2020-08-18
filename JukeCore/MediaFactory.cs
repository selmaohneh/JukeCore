using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using LibVLCSharp.Shared;
using Newtonsoft.Json;

namespace JukeCore
{
    public class MediaFactory : IMediaFactory
    {
        private readonly IDirectory _directory;
        private readonly IPath _path;
        private readonly IConsole _console;
        private readonly LibVLC _libVlc;

        public MediaFactory(IDirectory directory, IPath path, IConsole console, LibVLC libVlc)
        {
            _directory = directory;
            _path = path;
            _console = console;
            _libVlc = libVlc;
        }

        public IReadOnlyList<Media> Create(string id, string jukeCorePath)
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
            return medias;
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
                var media = new Media(_libVlc, file);
                medias.Add(media);
            }

            return medias;
        }
    }
}
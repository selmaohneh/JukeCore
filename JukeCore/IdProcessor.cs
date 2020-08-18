namespace JukeCore
{
    public class IdProcessor : IIdProcessor
    {
        private readonly IMediaFactory _mediaFactory;
        private readonly IConsole _console;
        private readonly IPlaylist _playlist;
        private readonly IMediaPlayer _mediaPlayer;

        public IdProcessor(IMediaFactory mediaFactory, IConsole console, IPlaylist playlist, IMediaPlayer mediaPlayer)
        {
            _mediaFactory = mediaFactory;
            _console = console;
            _playlist = playlist;
            _mediaPlayer = mediaPlayer;
        }

        public void Process(string id, string jukeCoreMediaPath)
        {
            _console.WriteLine($"Processing ID {id}");

            var medias = _mediaFactory.Create(id, jukeCoreMediaPath);

            _playlist.Set(medias);

            var firstMedia = _playlist.Next();
            _mediaPlayer.Play(firstMedia);
        }
    }
}
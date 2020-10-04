using System.Threading;

namespace JukeCore
{
    /// <summary>
    /// Topic handler for handling next/prev/pause of playback
    /// </summary>
    public class CommandTopicHandler : ITopicHandler
    {
        private readonly IMediaPlayer _mediaPlayer;
        private readonly IPlaylist _playlist;
        private readonly string _mqttPrefix;

        /// <inheritdoc />
        public string Topic => $"jukeCore/{_mqttPrefix}/command";

        public string Usage => $"Topic: {Topic} Payload: <NextTrack|PreviousTrack|PlayPause>";

        /// <summary>
        /// CTOR
        /// </summary>
        public CommandTopicHandler(IMediaPlayer mediaPlayer, IPlaylist playlist, string mqttPrefix)
        {
            _mediaPlayer = mediaPlayer;
            _playlist = playlist;
            _mqttPrefix = mqttPrefix;
        }

        /// <inheritdoc />
        public void HandlePayload(string payload)
        {
            switch (payload)
            {
                case "PlayPause":
                    _mediaPlayer.Pause();
                    break;
                case "NextTrack":
                    var nextMedia = _playlist.Next();

                    ThreadPool.QueueUserWorkItem(_ => _mediaPlayer.Play(nextMedia));
                    break;
                case "PreviousTrack":
                    var previousMedia = _playlist.Previous();

                    ThreadPool.QueueUserWorkItem(_ => _mediaPlayer.Play(previousMedia));
                    break;
            }
        }
    }
}
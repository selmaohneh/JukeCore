namespace JukeCore
{
    /// <summary>
    /// Topic handler for volume control
    /// </summary>
    public class VolumeTopicHandler : ITopicHandler
    {
        private readonly IMediaPlayer _mediaPlayer;
        private readonly string _mqttPrefix;

        /// <summary>
        /// CTOR
        /// </summary>
        public VolumeTopicHandler(IMediaPlayer mediaPlayer, string mqttPrefix)
        {
            _mediaPlayer = mediaPlayer;
            _mqttPrefix = mqttPrefix;
        }

        /// <inheritdoc />
        public string Topic => $"jukeCore/{_mqttPrefix}/volume";

        /// <inheritdoc />
        public string Usage => $"Topic: {Topic} Payload: 0 - 100";

        /// <inheritdoc />
        public void HandlePayload(string payload)
        {
            int.TryParse(payload, out int volumePercent);
            _mediaPlayer.Volume = volumePercent;
        }
    }
}
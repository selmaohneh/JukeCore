namespace JukeCore
{
    /// <summary>
    /// Topic handler for receiving play ids
    /// </summary>p
    public class PlayIdTopicHandler : ITopicHandler
    {
        private readonly IIdProcessor _idProcessor;
        private readonly string _mqttPrefix;
        private readonly string _jukeCoreMediaPath;

        /// <inheritdoc />
        public string Topic => $"jukeCore/{_mqttPrefix}/playId";

        public string Usage => $"Topic: {Topic} Payload: <rfid id>";

        /// <summary>
        /// CTOR
        /// </summary>
        public PlayIdTopicHandler(IIdProcessor idProcessor,
            string mqttPrefix,
            string jukeCoreMediaPath)
        {
            _idProcessor = idProcessor;
            _mqttPrefix = mqttPrefix;
            _jukeCoreMediaPath = jukeCoreMediaPath;
        }

        /// <inheritdoc />
        public void HandlePayload(string payload)
        {
            _idProcessor.Process(payload, _jukeCoreMediaPath);
        }
    }
}
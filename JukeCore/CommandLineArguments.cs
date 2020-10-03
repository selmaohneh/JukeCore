using PowerArgs;

namespace JukeCore
{
    /// <summary>
    /// power args ...
    /// </summary>
    public class CommandLineArguments
    {

        /// <summary>
        /// Show usage / argument help
        /// </summary>
        [HelpHook]
        [ArgShortcut("?")]
        public bool Help { get; set; }

        /// <summary>
        /// The path for the media files to be played back / handled by this juke core instance
        /// </summary>
        [ArgDescription("Absolute path to the media files")]
        [ArgRequired(PromptIfMissing = false)]
        [ArgShortcut("m")]
        public string JukeCoreMediaPath { get; set; }

        /// <summary>
        /// Ip address of mqt broker. If not set
        /// </summary>
        [ArgDescription("If set - mqtt support is enabled with the given ip address for the mqtt broker")]
        [ArgShortcut("h")]
        public string MqttBrokerIp { get; set; }

        /// <summary>
        /// Mqtt port to be used to connect to the mqtt broker
        /// </summary>
        [ArgDescription("Optional mqtt broker tcp port")]
        [ArgRange(1, 65535)]
        [ArgDefaultValue(1883)]
        [ArgShortcut("d")]
        public int MqttPort { get; set; }

        /// <summary>
        /// User name for mqtt broker
        /// </summary>
        [ArgDescription("Optional mqtt user name")]
        [ArgShortcut("u")]
        public string MqttUsername { get; set; }

        /// <summary>
        /// Password for mqtt broker
        /// </summary>
        [ArgDescription("Mqtt password")]
        [ArgRequired(If = "MqttUsername")]
        [ArgShortcut("p")]
        public string MqttPassword { get; set; }

        /// <summary>
        /// Mqtt prefix to be used
        /// </summary>
        [ArgDescription("Mqtt prefix to be used")]
        [ArgRequired(If = "MqttBrokerIp")]
        [ArgShortcut("x")]
        public string MqttPrefix { get; set; }
    }
}

namespace JukeCore
{
    /// <summary>
    /// Interface for handling a topic payload
    /// </summary>
    public interface ITopicHandler
    {
        /// <summary>
        /// Topic this handler handles
        /// </summary>
        string Topic { get; }

        /// <summary>
        /// Hint ready to display for the user (supported topic and payloads)
        /// </summary>
        string Usage { get; }

        /// <summary>
        /// Method that is called when the corresponding topic receives a message.
        /// </summary>
        void HandlePayload(string payload);
    }
}
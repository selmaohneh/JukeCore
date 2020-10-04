using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Nito.AsyncEx;

namespace JukeCore
{
    /// <summary>
    /// This service publishes changes to the mqtt datamodel to an mqtt broker and receives commands from the latter.
    /// </summary>
    public class MqttService : IMqttService, IDisposable
    {
        private readonly IMqttClient _mqttClient;
        private readonly IMqttClientOptions _mqttOptions;
        private readonly IConsole _console;
        private readonly JukeCoreDataModel _dataModel;
        private readonly int _mqttTimeoutMs;
        private readonly string _mqttPrefix;

        private readonly Dictionary<string, string> _topicsToPublish;
        private readonly List<ITopicHandler> _topicHandlers;

        /// <summary>
        /// CTOR
        /// </summary>
        public MqttService(IMqttClient mqttClient,
            IMqttClientOptions mqttOptions,
            IConsole console,
            List<ITopicHandler> topicHandlers,
            Dictionary<string, string> topicsToPublish,
            JukeCoreDataModel dataModel,
            int mqttTimeoutMs,
            string mqttPrefix)
        {
            _mqttClient = mqttClient;
            _mqttOptions = mqttOptions;
            _console = console;
            _dataModel = dataModel;
            _mqttTimeoutMs = mqttTimeoutMs;
            _mqttPrefix = mqttPrefix;
            _topicHandlers = topicHandlers;
            _topicsToPublish = topicsToPublish;
            SubscribeToDataModelEvents();
            SubscribeToMqttEvents();
        }

        private void SubscribeToMqttEvents()
        {
            _mqttClient.UseApplicationMessageReceivedHandler(OnMqttMessageReceived);
        }

        private Task OnMqttMessageReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
            string topic = arg.ApplicationMessage.Topic;
            string payload = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
            var handler = _topicHandlers.Single(x => x.Topic.Equals(topic));

            try
            {
                handler.HandlePayload(payload);
 }
            catch (Exception e)
            {
                _console.WriteLine(e.Message);
            }

            return Task.CompletedTask;
        }
        private void PrintTopicsToPublish()
        {
            foreach (var topicToPublish in _topicsToPublish)
            {
                _console.WriteLine($"Will publish dataModel.{topicToPublish.Value} to {topicToPublish.Key}");
            }
        }

        private void SubscribeToDataModelEvents()
        {
            _dataModel.OnPropertyChanged += DataModelOnPropertyChanged;
        }

        private void DataModelOnPropertyChanged(object sender, ChangedProperty e)
        {
            try
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfter(_mqttTimeoutMs);
                AsyncContext.Run(async () =>
                {
                    var topicEntry = _topicsToPublish.Single(x => x.Value.Equals(e.PropertyName));
                    await PublishMessage(topicEntry.Key, e.PropertyValue, cts.Token);
                });
            }
            catch
            {
                // nothing
            }
            
        }

        private async Task PublishMessage(string topic, string payload, CancellationToken token)
        {
            await EnsureConnected(token);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();
           await  _mqttClient.PublishAsync(message);
        }

        private async Task SubscribeMqttTopics()
        {
            foreach (var topicHandler in _topicHandlers)
            {
                var subscribeOptions =
                    new MqttTopicFilterBuilder().WithTopic(topicHandler.Topic).Build();
                await _mqttClient.SubscribeAsync(subscribeOptions);
                _console.WriteLine($"Registered handler for {topicHandler.Usage}");
            }
        }

        private async Task EnsureConnected(CancellationToken token)
        {
            bool connected = _mqttClient.IsConnected;
            if (!connected)
            {
                await _mqttClient.ConnectAsync(_mqttOptions, token);
                await SubscribeMqttTopics();
                PrintTopicsToPublish();
            }
        }

       public void Dispose()
        {
            _mqttClient?.Dispose();
            _dataModel.OnPropertyChanged -= DataModelOnPropertyChanged;
        }
    }
}

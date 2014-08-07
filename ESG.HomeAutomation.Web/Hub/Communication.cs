using System.Net;
using ESG.HomeAutomation.M2Mqtt;
using ESG.HomeAutomation.M2Mqtt.Messages;
using ESG.HomeAutomation.Micro.Common.Helpers;
using ESG.HomeAutomation.Micro.Infrastructure;
using Microsoft.AspNet.SignalR;

namespace ESG.HomeAutomation.Web
{
    public class CommunicationHub : Hub
    {
        private const string MqttBrokerIp = Constants.AzureWorkerRole.Ip;
        private readonly MqttClient _mqttClient;

        public CommunicationHub()
        {
            _mqttClient = new MqttClient(IPAddress.Parse(MqttBrokerIp));
            _mqttClient.MqttMsgPublished += mqttClient_MqttMsgPublished;
            _mqttClient.MqttMsgPublishReceived += mqttClient_MqttMsgPublishReceived;
            _mqttClient.Connect("MQTTServer");

            _mqttClient.Subscribe(new string[] { Constants.Netduino.Register }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        }

        public void ToggleLed()
        {
            this._mqttClient.Publish(Constants.Server.Led.Toggle, new byte[] {}, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE);
        }

        public void PollForDevice(string deviceId)
        {
            byte[] bytes = deviceId.StringToBytes();
            this._mqttClient.Publish(Constants.Server.Poll, bytes, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE);
        }

        private static void mqttClient_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            
        }

        private void mqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            switch (e.Topic)
            {
                case Constants.Netduino.Register:
                    Clients.All.registerDevice(e.Message.BytesToString());
                    break;
            }
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}
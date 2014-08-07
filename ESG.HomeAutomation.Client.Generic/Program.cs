using System;
using System.Net;
using System.Threading;
using ESG.HomeAutomation.M2MqttNetMf43;
using ESG.HomeAutomation.M2MqttNetMf43.Messages;
using ESG.HomeAutomation.Micro.Common.Hardware.BuiltIn;
using ESG.HomeAutomation.Micro.Common.Helpers;
using ESG.HomeAutomation.Micro.Infrastructure;
using SecretLabs.NETMF.Hardware.Netduino;
using Domain = ESG.HomeAutomation.Micro.Domain;

namespace ESG.HomeAutomation.Client.Generic
{
    public class Program
    {
        private const string MqttBrokerIp = Constants.AzureWorkerRole.Ip;

        private static MqttClient _mqttClient;
        private static Led _led;

        private static Domain.Models.Devices.Generic _device;

        public static void Main()
        {
            // initialize controls
            _led = new Led(Pins.ONBOARD_LED, false);

            // initialize device object
            InitializeDevice();

            _mqttClient = new MqttClient(IPAddress.Parse(MqttBrokerIp));
            _mqttClient.MqttMsgPublished += mqttClient_MqttMsgPublished;
            _mqttClient.MqttMsgPublishReceived += mqttClient_MqttMsgPublishReceived;
            _mqttClient.Connect("MQTTNetduino");

            _mqttClient.Subscribe(new string[] { Constants.Server.Poll }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            _mqttClient.Subscribe(new string[] { Constants.Server.Led.Toggle }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            RegisterDevice();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void mqttClient_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            
        }

        private static void mqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            switch (e.Topic)
            {
                case Constants.Server.Poll:
                    if(String.Equals(e.Message.BytesToString(), _device.DeviceId))
                    RegisterDevice();
                    break;
                case Constants.Server.Led.Toggle:
                    ToggleLed();
                    break;
            }
        }

        private static void ToggleLed()
        {
            bool state = _led.Read();
            _led.Write(!state);
            RegisterDevice();
        }

        private static void InitializeDevice()
        {
            _device = new Domain.Models.Devices.Generic("netduino1");
            _device.Led.Status = _led.Read();
        }

        private static void RegisterDevice()
        {
            _device.Led.Status = _led.Read();
            string serializedDevice = Micro.Infrastructure.Helpers.Json.Serialize(_device);

            _mqttClient.Publish(Constants.Netduino.Register, serializedDevice.StringToBytes());
        }
    }
}

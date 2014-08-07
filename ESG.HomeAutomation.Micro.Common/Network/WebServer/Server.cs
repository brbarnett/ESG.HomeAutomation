using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace ESG.HomeAutomation.Micro.Common.Network.WebServer
{
    public class WebServer : IDisposable
    {
        private readonly Socket _socket;

        //open connection to onboard led so we can blink it with every request
        private readonly OutputPort _led = new OutputPort(Pins.ONBOARD_LED, false);

        public WebServer()
        {
            //Initialize Socket class
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Request and bind to an IP from DHCP server
            _socket.Bind(new IPEndPoint(IPAddress.Any, 80));

            //Debug print our IP address
            Debug.Print(Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress);

            //Start listen for web requests
            _socket.Listen(10);
            ListenForRequest();
        }

        public static void Start()
        {
            Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].EnableStaticIP(
                "192.168.1.10", "255.255.255.0", "192.168.1.1");
            WebServer webServer = new WebServer();
            webServer.ListenForRequest();
        }

        public void ListenForRequest()
        {
            while (true)
            {
                using (Socket clientSocket = _socket.Accept())
                {
                    //Get clients IP
                    IPEndPoint clientIp = clientSocket.RemoteEndPoint as IPEndPoint;
                    EndPoint clientEndPoint = clientSocket.RemoteEndPoint;

                    //int byteCount = cSocket.Available;
                    int bytesReceived = clientSocket.Available;
                    if (bytesReceived > 0)
                    {
                        //Get request
                        byte[] buffer = new byte[bytesReceived];
                        int byteCount = clientSocket.Receive(buffer, bytesReceived, SocketFlags.None);
                        string request = new string(Encoding.UTF8.GetChars(buffer));

                        Debug.Print(request);

                        //Compose a response
                        const string response = "Hello World";
                        string header = "HTTP/1.0 200 OK\r\nContent-Type: text; charset=utf-8\r\nContent-Length: " +
                                        response.Length + "\r\nConnection: close\r\n\r\n";

                        clientSocket.Send(Encoding.UTF8.GetBytes(header), header.Length, SocketFlags.None);
                        clientSocket.Send(Encoding.UTF8.GetBytes(response), response.Length, SocketFlags.None);

                        //Blink the onboard LED
                        _led.Write(true);
                        Thread.Sleep(150);
                        _led.Write(false);
                    }
                }
            }
        }

        #region IDisposable Members

        ~WebServer()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_socket != null) _socket.Close();
        }

        #endregion

    }
}

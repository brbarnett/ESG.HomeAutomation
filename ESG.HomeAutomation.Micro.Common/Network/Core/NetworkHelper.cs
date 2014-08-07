using System;
using Toolbox.NETMF.NET;

namespace ESG.HomeAutomation.Micro.Common.Network.Core
{
    public static class NetworkHelper
    {
        public static HTTP_Client.HTTP_Response Get(string server, string method)
        {
            HTTP_Client session = new HTTP_Client(new IntegratedSocket(server, 80));

            HTTP_Client.HTTP_Response response = session.Get(method);

            if (response.ResponseCode != 200)
                throw new ApplicationException("Unexpected HTTP response code: " + response.ResponseCode);

            return response;
        }

        public static HTTP_Client.HTTP_Response Post(string server, string method)
        {
            HTTP_Client session = new HTTP_Client(new IntegratedSocket(server, 80));

            HTTP_Client.HTTP_Response response = session.Post(method);

            if (response.ResponseCode != 200)
                throw new ApplicationException("Unexpected HTTP response code: " + response.ResponseCode);

            return response;
        }
    }
}

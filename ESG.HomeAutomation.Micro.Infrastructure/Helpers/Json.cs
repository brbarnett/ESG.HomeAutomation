using System;
using Microsoft.SPOT;
using Json.NETMF;

namespace ESG.HomeAutomation.Micro.Infrastructure.Helpers
{
    public class Json
    {
        public static string Serialize(object o)
        {
            return JsonSerializer.SerializeObject(o);
        }

        public static object Deserialize(string s)
        {
            return JsonSerializer.DeserializeString(s);
        }
    }
}

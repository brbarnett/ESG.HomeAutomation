
using Toolbox.NETMF;

namespace ESG.HomeAutomation.Micro.Common.Helpers
{
    public static class StringHelper
    {
        public static byte[] StringToBytes(this string str)
        {
            char[] chars = str.ToCharArray();
            byte[] bytes = Tools.Chars2Bytes(chars);
            return bytes;
        }

        public static string BytesToString(this byte[] bytes)
        {
            char[] chars = Tools.Bytes2Chars(bytes);
            string str = new string(chars);
            return str;
        }
    }
}

namespace ESG.HomeAutomation.Micro.Infrastructure
{
    public static class Constants
    {
        public static class AzureWorkerRole
        {
            public const string Ip = "23.96.224.116";
        }
        public static class Server
        {
            private const string Prefix = "server/";

            public const string Poll = Prefix + "poll";

            public static class Led
            {
                private const string Prefix = Server.Prefix + "led/";

                public const string Toggle = Prefix + "toggle";
            }
        }

        public static class Netduino
        {
            private const string Prefix = "netduino/";

            public const string Register = Prefix + "register";
        }
    }
}

using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace ESG.HomeAutomation.WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private MqttBroker.MqttBroker _broker;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("GnatMQWorkerRole entry point called");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.TraceInformation("Working");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            this._broker = new MqttBroker.MqttBroker();
            this._broker.Start();

            return base.OnStart();
        }
    }
}

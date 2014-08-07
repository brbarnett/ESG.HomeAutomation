using System.Threading;
using ESG.HomeAutomation.Micro.Common.Core;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace ESG.HomeAutomation.Micro.Common.Hardware.BuiltIn
{
    public class Led : OutputPort, IOutputPort
    {
        public Led(Cpu.Pin portId, bool initialState) 
            : base(portId, initialState)
        {
            
        }

        public static Led Initialize()
        {
            return new Led(Pins.ONBOARD_LED, false);
        }

        public void Blink(int count)
        {
            for (int i = 0; i < count; i++)
            {
                base.Write(true);
                Thread.Sleep(250);
                base.Write(false);
                Thread.Sleep(250);
            }
        }
    }
}

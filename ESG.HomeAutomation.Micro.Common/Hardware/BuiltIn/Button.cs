using ESG.HomeAutomation.Micro.Common.Core;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace ESG.HomeAutomation.Micro.Common.Hardware.BuiltIn
{
    public class Button : InputPort, IInputPort
    {
        public Button(Cpu.Pin portId, bool glitchFilter, Port.ResistorMode resistor)
            : base(portId, glitchFilter, resistor)
        {
            
        }

        public static Button Initialize()
        {
            return new Button(Pins.ONBOARD_SW1, false, ResistorMode.Disabled);
        }
    }
}

using ESG.HomeAutomation.Micro.Domain.Models.Controls;

namespace ESG.HomeAutomation.Micro.Domain.Models.Devices
{
    public class Generic : DeviceBase
    {
        public Generic(string deviceId)
        {
            base.DeviceId = deviceId;
            Led = new Led();
        }

        public Led Led { get; set; }
    }
}

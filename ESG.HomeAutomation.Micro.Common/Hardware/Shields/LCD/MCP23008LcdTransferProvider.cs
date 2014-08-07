// Micro Liquid Crystal Library
// http://microliquidcrystal.codeplex.com
// Appache License Version 2.0 

using System.Threading;
using ESG.HomeAutomation.Micro.Common.Hardware.Shields.LCD.FusionWare.SPOT;

namespace ESG.HomeAutomation.Micro.Common.Hardware.Shields.LCD
{
    public class Mcp23008LcdTransferProvider : BaseShifterLcdTransferProvider
    {
        private readonly Mcp23008Expander _expander;

        /// <summary>
        /// This setup corresponds to connections on Adafruit's LCD backpack
        /// http://www.ladyada.net/products/i2cspilcdbackpack/
        /// </summary>
        public static ShifterSetup DefaultSetup
        {
            get
            {
                return new ShifterSetup
                           {
                               RW = ShifterPin.None, // not used
                               RS = ShifterPin.GP1,
                               Enable  = ShifterPin.GP2,
                               D4 = ShifterPin.GP3,
                               D5 = ShifterPin.GP4,
                               D6 = ShifterPin.GP5,
                               D7 = ShifterPin.GP6,
                               BL = ShifterPin.GP7,
                           };
            }
        }

        public Mcp23008LcdTransferProvider(I2CBus bus) : this(bus, 0, DefaultSetup)
        {}

        public Mcp23008LcdTransferProvider(I2CBus bus, ushort address, ShifterSetup setup)
            : base(setup)
        {
            _expander = new Mcp23008Expander(bus, address);
            Thread.Sleep(10); // make sure bus initializes

            _expander.SetPinMode(setup.Enable, Mcp23008Expander.PinMode.Output);
            _expander.SetPinMode(setup.RS, Mcp23008Expander.PinMode.Output);
            _expander.SetPinMode(setup.D4, Mcp23008Expander.PinMode.Output);
            _expander.SetPinMode(setup.D5, Mcp23008Expander.PinMode.Output);
            _expander.SetPinMode(setup.D6, Mcp23008Expander.PinMode.Output);
            _expander.SetPinMode(setup.D7, Mcp23008Expander.PinMode.Output);

            if (setup.RW != ShifterPin.None)
                _expander.SetPinMode(setup.RW, Mcp23008Expander.PinMode.Output);

            if (setup.BL != ShifterPin.None)
                _expander.SetPinMode(setup.BL, Mcp23008Expander.PinMode.Output);
        }

        protected override void SendByte(byte output)
        {
            _expander.Output(output);
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                    _expander.Dispose();

                IsDisposed = true;
            }
        }
    }
}
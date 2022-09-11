using System;

using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Units;

namespace meadow_analoginput
{
    internal class AnalogInputController
    {              
        protected bool Initialized = false;        
        public static AnalogInputController Current { get; private set; }        

        private AnalogInputController()
        {            
        }

        static AnalogInputController()
        {
            Current = new AnalogInputController();            
        }        

        public async void Initialize(
            IMeadowDevice meadowDevice, 
            IPin analogIntputPin, 
            Action<IChangeResult<Voltage>> handler,
            Predicate<IChangeResult<Voltage>> filter)
        {
            if (Initialized)
            {
                return;
            }

            var observer = IAnalogInputPort.CreateObserver(
                handler: handler,
                filter: filter
            );

            var analogIn = meadowDevice.CreateAnalogInputPort(analogIntputPin, 5, AnalogInputPort.DefaultSampleInterval, AnalogInputPort.DefaultReferenceVoltage);            

            analogIn.Subscribe(observer);

            await analogIn.Read();

            analogIn.StartUpdating(TimeSpan.FromSeconds(1));

            Initialized = true;            
        }        
    }
}

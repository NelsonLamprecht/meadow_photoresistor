using System;

using Meadow;
using Meadow.Devices;
using Meadow.Units;

using meadow_analoginput;

namespace meadow_photoresistor
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV1, MeadowApp>
    {
        public MeadowApp()
        {
            Initialize(Device);                                
        }

        private void Initialize(F7FeatherV1 device)
        {
            Console.WriteLine("Initializing hardware...");

            AnalogInputController.Current.Initialize(device, Device.Pins.A05, Handler, Filter);            

            Console.WriteLine("Hardware initialized.");
        }

        //  An `Action` that will be invoked when a change occurs.
        private static void Handler(IChangeResult<Voltage> result)
        {
            Console.WriteLine($"Analog observer triggered; new: {result.New.Volts:n2}V, old: {result.Old?.Volts:n2}V");
        }

        // An optional `Predicate` that filters out any notifications that don't satisfy
        //     (return `true`) the predicate condition. Note that the first reading will always
        //     call the handler.
        private static bool Filter(IChangeResult<Voltage> result)
        {
            // filter is optional. in this case, we're only notifying if the
            // voltage changes by at least `0.1V`.
            if (result.Old is { } oldValue)
            {
                return (result.New - oldValue).Abs().Volts > 0.1;
            }
            else { return false; }
        }        
    }
}

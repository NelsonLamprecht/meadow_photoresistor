using System;
using System.Threading.Tasks;

using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Units;

namespace meadow_photoresistor
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV1, MeadowApp>
    {
        IAnalogInputPort analogIn;


        public MeadowApp()
        {
            Initialize();

            PerformOneRead().Wait();

            //==== Start updating
            analogIn.StartUpdating(TimeSpan.FromSeconds(1));
        }

        private void Initialize()
        {
            Console.WriteLine("Initializing hardware...");            


            //==== create our analog input port
            analogIn = Device.CreateAnalogInputPort(Device.Pins.A05);

            //==== Classic .NET Events
            analogIn.Updated += (s, result) => {
                //Console.WriteLine($"Analog event, new voltage: {result.New.Volts:N2}V, old: {result.Old?.Volts:N2}V");
            };

            //==== Filterable Observable
            var observer = IAnalogInputPort.CreateObserver(
                handler: result => Console.WriteLine($"Analog observer triggered; new: {result.New.Volts:n2}V, old: {result.Old?.Volts:n2}V"),
                // filter is optional. in this case, we're only notifying if the
                // voltage changes by at least `0.1V`.
                filter: result => {
                    if (result.Old is { } oldValue)
                    {
                        return (result.New - oldValue).Abs().Volts > 0.1;
                    }
                    else { return false; }
                }
            );
            analogIn.Subscribe(observer);

            Console.WriteLine("Hardware initialized.");

        }

        protected async Task PerformOneRead()
        {
            // Analog port returns a `Voltage` unit
            Voltage voltageReading = await analogIn.Read();
            Console.WriteLine($"Voltages: {voltageReading.Volts:N3}");
        }
    }
}

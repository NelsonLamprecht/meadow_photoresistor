using System;
using System.Threading.Tasks;
using System.Threading;

using Meadow.Foundation.Leds;
using Meadow.Foundation;
using Meadow.Hardware;

namespace meadow_analoginput
{
    internal class AnalogInputController
    {
        private readonly Random _random;       
        private CancellationTokenSource _cancellationTokenSource = null;        

        protected bool Initialized = false;        

        public static AnalogInputController Current { get; private set; }

        private AnalogInputController()
        {
            _random = new Random();
        }

        static AnalogInputController()
        {
            Current = new AnalogInputController();
        }

        public void Initialize(IPwmOutputController outputController, IPin redPwmPin, IPin greenPwmPin, IPin bluePwmPin)
        {
            if (Initialized)
            {
                return;
            }          
           

            Initialized = true;

            
        }

        
    }
}

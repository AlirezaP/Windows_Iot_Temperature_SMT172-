using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace SMT172_Temperature
{
    class SMT172
    {

        private double pwmh = 0;
        private double pwml = 0;

        private int _pin = 2;
        private GpioController gpio = null;
        private GpioPin tPintObj = null;

        public SMT172(int pin)
        {
            _pin = pin;

            if (gpio == null)
            {
                gpio = GpioController.GetDefault();
            }

            if (tPintObj == null)
            {
                tPintObj = gpio.OpenPin(_pin);
                tPintObj.SetDriveMode(GpioPinDriveMode.Input);
            }

        }

        public async Task<double> ReadTemperature()
        {
            pwmh = 0;
            pwml = 0;

            double sum = 0;

            for (int avg = 0; avg <= 7; avg++)
            {
                for (int i = 0; i < 2000; i++)
                {
                    if (tPintObj.Read() == GpioPinValue.High)
                    {
                        pwmh++;
                    }
                    else
                    {
                        pwml++;
                    }

                    //delay
                    //long microseconds = 1;
                    //var ticks = (TimeSpan.TicksPerMillisecond / 1000) * microseconds;

                    await Task.Delay(TimeSpan.FromTicks(10));
                }

                sum += ((pwmh / (pwml + pwmh)));
            }

            //calc
            //return ((pwmh/(pwml+pwmh))-0.32)/ 0.0047;

            double div = (double)sum / (double)8;
            double temperature = (double)212.77 * div - (double)68.085;
            return temperature;
        }
    }
}

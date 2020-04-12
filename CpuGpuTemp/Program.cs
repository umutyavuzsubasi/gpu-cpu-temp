using OpenHardwareMonitor.Hardware;
using System;
using System.Timers;

namespace HardwareTemperature
{
    class Program
    {
        static void Main(string[] args)
        {
            // add CPU and GPU as hardware
            // note that, CPU temperature data requires 'highestAvailable' permission.
            Computer computer = new Computer() { CPUEnabled = true, GPUEnabled = true };
            computer.Open();

            Timer timer = new Timer() { Enabled = true, Interval = 1000 };
            timer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                Console.Clear();
                Console.WriteLine("{0}\n", DateTime.Now);

                foreach (IHardware hardware in computer.Hardware)
                {
                    hardware.Update();

                    Console.WriteLine("{0}: {1}", hardware.HardwareType, hardware.Name);
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        // Celsius is default unit
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            Console.WriteLine("{0}: {1}°C", sensor.Name, sensor.Value);
                            // Console.WriteLine("{0}: {1}°F", sensor.Name, sensor.Value*1.8+32);
                        }

                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Press Enter to exit");
            };

            // press enter to exit
            Console.ReadLine();
        }
    }
}
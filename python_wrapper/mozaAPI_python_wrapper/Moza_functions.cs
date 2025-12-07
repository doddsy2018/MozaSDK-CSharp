using mozaAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static mozaAPI.mozaAPI;


namespace PythonMozaAPI
{
    public class Moza_functions
    {

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        static Moza_functions() // Static constructor
        {
            Console.WriteLine("** Staring Moza API. **");
            installMozaSDK();
        }
        public static IntPtr GetHwndByWindowName(string windowName)
        {
            // Set lpClassName to null to search only by window name
            IntPtr hWnd = FindWindow(null, windowName);
            return hWnd;
        }


        public object pygetHIDData()
        {
            ERRORCODE err = ERRORCODE.NORMAL;
            var wheelangle = 0.0;
            var brake = 0.0;
            var throttle = 0.0;

            var throttlemax = 32768.0;
            var brakemax = 32768.0;

            while (true)
            {
                var HIDDATA = getHIDData(ref err);
                var Sampledwheelangle = HIDDATA.fSteeringWheelAngle;
                if (!float.IsNaN(Sampledwheelangle))
                {
                    wheelangle = Sampledwheelangle;
                    throttle = HIDDATA.throttle;
                    brake = HIDDATA.brake;

                    // Convert to percentage of travel range
                    throttle = ((throttle + throttlemax) / (throttlemax * 2)) * 100;
                    brake = ((brake + brakemax) / (brakemax * 2)) * 100;

                    break;
                }
            }

            var myData = new Dictionary<string, object>();
            myData.Add("steeringangle", wheelangle);
            myData.Add("throttle", throttle);
            myData.Add("brake", brake);

            return myData;
            //removeMozaSDK();
        }
        public void pySetSpringForce(String windowName=null, ulong duration = 0xffff)
        {
            Console.WriteLine("** Enabling Wheel Spring Centering Force. **");
            ERRORCODE err = ERRORCODE.NORMAL;
            IntPtr hWnd;

            if (windowName == null)
            {
                // In C#, we use a Form/Control to get a valid HWND (Window Handle)
                Form dummyForm = new Form();
                dummyForm.Show();
                hWnd = dummyForm.Handle;
            }
            else
                hWnd = GetHwndByWindowName(windowName);

                // Spring Force Test
                var force_mgr = createWheelbaseETSpring(hWnd, ref err);
            force_mgr.setDuration(duration);

            try
            {
                force_mgr.start();
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }


        public int pyGetSwitchValues()
        {
            List<byte> currentSwitchValues = null;

            var devices = EnumSwitchesDevices(out var error);
            if (error != ERRORCODE.NORMAL || devices.Count == 0)
            {
                Console.WriteLine($"No MOZA Switch device found, error = {error}");
                return 0;
            }

            var device = devices[0];
            if (!device.Open())
            {
                Console.WriteLine("Switch device open failed.");
                return 0;
            }

            while (device.IsConnected) {
                currentSwitchValues = device.GetStateInfo(out var switcherror);
                var numSwitches = currentSwitchValues.Count;
                for (int i = 0; i <= numSwitches - 1; i++)
                    if (currentSwitchValues[i] == 1)
                    {
                        //Console.WriteLine($"c# Switch {i} = {currentSwitchValues[i]}");
                        return i;
                    }
            }

            return 0;

        }
    }
}
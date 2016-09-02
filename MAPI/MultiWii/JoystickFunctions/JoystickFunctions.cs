using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX;
using SlimDX.DirectInput;
using System.Globalization;


namespace MultiWii
{
    public static class JoystickFunctions
    {
         public static Joystick joystick=null;
        
        public static JoystickState currentState = new JoystickState();

        public static void InitializeJoystick()
        {
            DirectInput dinput = new DirectInput();
            foreach (DeviceInstance device in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                try
                {
                    joystick = new Joystick(dinput, device.InstanceGuid);
                    break;
                }
                catch (DirectInputException)
                {
                 MessageBox.Show("Error joystick.");
                }
            }
            if (joystick == null)
            {
                MessageBox.Show("Joystick not found.");
                return;
            }
            foreach (DeviceObjectInstance deviceObject in joystick.GetObjects())
            {
                if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                    joystick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(1000, 2000);
  
            }

            joystick.Acquire();
        }
  
        
        public static void readFromJoystick()
        {
            if (joystick.Acquire().IsFailure)
                return;

            if (joystick.Poll().IsFailure)
                return;

            currentState = joystick.GetCurrentState();
            if (Result.Last.IsFailure)
                return;
                        
        }

        public static JoystickState getCurrentState()
        {
            return currentState;
        }

        public static void disconnectJoystick()
        {

            if (joystick != null)
            {
                joystick.Unacquire();
                joystick.Dispose();
            }
            joystick = null;
        }
    }
    
}

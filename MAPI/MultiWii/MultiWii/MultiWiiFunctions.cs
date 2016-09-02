using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiWii
{
    #region STRUCTURES
    /* WE NEED TO DEFINE SOME STRUCTS */
    public struct RCChannels
    {
        public double roll, pitch, yaw, throttle, elapsed, timestamp;

        public RCChannels(double param1, double param2, double param3, double param4, double param5, double param6)
        {
            roll = param1;
            pitch = param2;
            yaw = param3;
            throttle = param4;
            elapsed = param5;
            timestamp = param6;
        }
    }

    public struct RawIMU
    {
        public double ax, ay, az, gx, gy, gz, elapsed, timestamp;

        public RawIMU(double param1, double param2, double param3, double param4, double param5, double param6, double param7, double param8)
        {
            ax = param1;
            ay = param2;
            az = param3;
            gx = param4;
            gy = param5;
            gz = param6;
            elapsed = param7;
            timestamp = param8;
        }
    }

    public struct Motor
    {
        public double m1, m2, m3, m4, elapsed, timestamp;

        public Motor(double param1, double param2, double param3, double param4, double param5, double param6)
        {
            m1 = param1;
            m2 = param2;
            m3 = param3;
            m4 = param4;
            elapsed = param5;
            timestamp = param6;
        }
    }

    public struct Attitude
    {
        public double angx, angy, heading, elapsed, timestamp;

        public Attitude(double param1, double param2, double param3, double param4, double param5)
        {
            angx = param1;
            angy = param2;
            heading = param3;
            elapsed = param4;
            timestamp = param5;
        }
    }

    public struct Message
    {
        public double angx, angy, heading, roll, pitch, yaw, elapsed, timestamp;

        public Message(double param1, double param2, double param3, double param4, double param5, double param6, double param7, double param8)
        {
            angx = param1;
            angy = param2;
            heading = param3;
            roll = param4;
            pitch = param5;
            yaw = param6;
            elapsed = param7;
            timestamp = param8;
        }
    }
    #endregion

    public static class MultiWiiFunctions
    {

        #region CONSTANTS
        /* WE NEED TO DEFINE MULTIWII CODE CONSTANTS */
        public const int MSP_IDENT = 100;
        public const int MSP_STATUS = 101;
        public const int MSP_RAW_IMU = 102;
        public const int MSP_SERVO = 103;
        public const int MSP_MOTOR = 104;
        public const int MSP_RC = 105;
        public const int MSP_RAW_GPS = 106;
        public const int MSP_COMP_GPS = 107;
        public const int MSP_ATTITUDE = 108;
        public const int MSP_ALTITUDE = 109;
        public const int MSP_ANALOG = 110;
        public const int MSP_RC_TUNING = 111;
        public const int MSP_PID = 112;
        public const int MSP_BOX = 113;
        public const int MSP_MISC = 114;
        public const int MSP_MOTOR_PINS = 115;
        public const int MSP_BOXNAMES = 116;
        public const int MSP_PIDNAMES = 117;
        public const int MSP_WP = 118;
        public const int MSP_BOXIDS = 119;
        public const int MSP_SERVO_CONF = 120;
        public const int MSP_RC_RAW_IMU = 121;
        public const int MSP_SET_RAW_RC = 200;
        public const int MSP_SET_RAW_GPS = 201;
        public const int MSP_SET_PID = 202;
        public const int MSP_SET_BOX = 203;
        public const int MSP_SET_RC_TUNING = 204;
        public const int MSP_ACC_CALIBRATION = 205;
        public const int MSP_MAG_CALIBRATION = 206;
        public const int MSP_SET_MISC = 207;
        public const int MSP_RESET_CONF = 208;
        public const int MSP_SET_WP = 209;
        public const int MSP_SWITCH_RC_SERIAL = 210;
        public const int MSP_SELECT_SETTING = 210;
        public const int MSP_IS_SERIAL = 211;
        public const int MSP_SET_HEAD = 211;
        public const int MSP_SET_SERVO_CONF = 212;
        public const int MSP_SET_MOTOR = 214;
        public const int MSP_BIND = 240;
        public const int MSP_EEPROM_WRITE = 250;
        public const int MSP_DEBUG = 254;
        #endregion
        
        #region VARIABLES
        /* WE NEED TO DEFINE SOME VARIABLES */
        public static RCChannels rcChannels;
        public static RawIMU rawIMU;
        public static Motor motor;
        public static Attitude attitude;
        public static Message message;
        public 
        #endregion

        #region INITIALIZE VARIABLES
        /* INITIALIZATION */
        static void Initialize()
        {
            rcChannels = new RCChannels(0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
            rawIMU = new RawIMU(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
            motor = new Motor(0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
            attitude = new Attitude(0.0, 0.0, 0.0, 0.0, 0.0);
            message = new Message(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
        }
        #endregion

        #region MULTIWII CORE FUNCTIONS

        #region GET FUNCTIONS
      
        /***** GET FUNCTIONS *****/

        public static byte[] mspIdent()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_IDENT, data, 10);
            return result;
        }

        public static byte[] mspStatus()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_STATUS, data, 14);
            return result;
        }

        public static byte[] mspRawIMU()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_RAW_IMU, data, 21);
            return result;
        }

        public static byte[] mspServo() {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_SERVO, data, 19);
            return result;
        }

        public static byte[] mspMotor()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_MOTOR, data, 19);
            return result;
        }

        public static byte[] mspRC()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_RC, data, 19);
            return result;
        }

        public static byte[] mspRawGPS()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_RAW_GPS, data, 19);
            return result;
        }

        public static byte[] mspCompGPS()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_COMP_GPS, data, 8);
            return result;
        }

        public static byte[] mspAttitude()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_ATTITUDE, data, 9);
            return result;
        }

        public static byte[] mspAltitude()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_ALTITUDE, data, 9);
            return result;
        }

        public static byte[] mspAnalog()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_ANALOG, data, 10);
            return result;
        }

        public static byte[] mspRCTuning()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_RC_TUNING, data, 10);
            return result;
        }


        public static byte[] mspPid()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_PID, data, 4);
            return result;
        }

        public static byte[] mspBox()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_BOX, data, 5);
            return result;
        }

        public static byte[] mspMisc()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_MISC, data, 25);
            return result;
        }

        public static byte[] mspMotorPins()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_MOTOR_PINS, data, 11);
            return result;
        }

        public static byte[] mspBoxNames()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_BOXNAMES, data, 25);
            return result;
        }

        public static byte[] mspPidNames()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_PIDNAMES, data, 25);
            return result;
        }

        public static byte[] mspWP()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_WP, data, 21);
            return result;
        }

        public static byte[] mspBoxIds()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_BOXIDS, data, 25);
            return result;
        }

        public static byte[] mspServoConf()
        {
            byte[] result = null;
            int[] data = { };
            result = CoreFunctions.sendCommandAndReceive(0, MSP_SERVO_CONF, data, 59);
            return result;
        }
       
        #endregion

        #region SET FUNCTIONS
       
        /***** SET FUNCTIONS *****/

        public static byte[] mspSetMotor(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_SET_MOTOR, data);
            return result;
        }

        public static byte[] mspSetRawRC(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_SET_RAW_RC, data);
            return result;
        }

        public static byte[] mspSetRCTuning(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_SET_RC_TUNING, data);
            return result;
        }

        public static byte[] mspSetPid(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_SET_PID, data);
            return result;
        }

        public static byte[] mspSetBox(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_SET_BOX, data);
            return result;
        }

        public static byte[] mspSetMisc(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_SET_MISC, data);
            return result;
        }

        public static byte[] mspSetWP(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_SET_WP, data);
            return result;
        }

        public static byte[] mspSetServoConf(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_SET_SERVO_CONF, data);
            return result;
        }

        public static byte[] mspAccCalibration(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_ACC_CALIBRATION, data);
            return result;
        }

        public static byte[] mspMagCalibration(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_MAG_CALIBRATION, data);
            return result;
        }

        public static byte[] mspResetConf(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_RESET_CONF, data);
            return result;
        }

        public static byte[] mspSelectSetting(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_SELECT_SETTING, data);
            return result;
        }

        public static byte[] mspSetHead(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_SET_HEAD, data);
            return result;
        }

        public static byte[] mspBind(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_BIND, data);
            return result;
        }

        public static byte[] mspEepromWrite(int[] data)
        {
            byte[] result = null;
            result = CoreFunctions.sendCommand(data.Length * 2, MSP_EEPROM_WRITE, data);
            return result;
        }
        #endregion

        #endregion
    }
}

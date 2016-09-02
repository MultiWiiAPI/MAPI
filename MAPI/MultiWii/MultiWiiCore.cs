using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

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


    public class MultiWiiCore
    {
        /* 
         * This class will have the core functions for the API in C#
         */
        #region CONSTANTS
        /* WE NEED TO DEFINE MULTIWII CODE CONSTANTS */
        public const int IDENT = 100;
        public const int STATUS = 101;
        public const int RAW_IMU = 102;
        public const int SERVO = 103;
        public const int MOTOR = 104;
        public const int RC = 105;
        public const int RAW_GPS = 106;
        public const int COMP_GPS = 107;
        public const int ATTITUDE = 108;
        public const int ALTITUDE = 109;
        public const int ANALOG = 110;
        public const int RC_TUNING = 111;
        public const int PID = 112;
        public const int BOX = 113;
        public const int MISC = 114;
        public const int MOTOR_PINS = 115;
        public const int BOXNAMES = 116;
        public const int PIDNAMES = 117;
        public const int WP = 118;
        public const int BOXIDS = 119;
        public const int SERVO_CONF = 120;
        public const int RC_RAW_IMU = 121;
        public const int SET_RAW_RC = 200;
        public const int SET_RAW_GPS = 201;
        public const int SET_PID = 202;
        public const int SET_BOX = 203;
        public const int SET_RC_TUNING = 204;
        public const int ACC_CALIBRATION = 205;
        public const int MAG_CALIBRATION = 206;
        public const int SET_MISC = 207;
        public const int RESET_CONF = 208;
        public const int SET_WP = 209;
        public const int SWITCH_RC_SERIAL = 210;
        public const int SELECT_SETTING = 210;
        public const int IS_SERIAL = 211;
        public const int SET_HEAD = 211;
        public const int SET_SERVO_CONF = 212;
        public const int SET_MOTOR = 214;
        public const int BIND = 240;
        public const int EEPROM_WRITE = 250;
        public const int DEBUG = 254;
        #endregion

        #region VARIABLES
        /* WE NEED TO DEFINE SOME VARIABLES */
        public RCChannels rcChannels;
        public RawIMU rawIMU;
        public Motor motor;
        public Attitude attitude;
        public Message message;

        public double elapsed;
        public Boolean print = true;
        public SerialPort serialPort;
        #endregion

        #region CONSTRUCTORS
        /* CONSTRUCTOR */
        public MultiWiiCore(String portName)
        {
            rcChannels = new RCChannels(0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
            rawIMU = new RawIMU(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
            motor = new Motor(0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
            attitude = new Attitude(0.0, 0.0, 0.0, 0.0, 0.0);
            message = new Message(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);

            elapsed = 0.0;
            print = true;

            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = 57600;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.ReadTimeout = 0;
            serialPort.WriteTimeout = 2;
            serialPort.Handshake = Handshake.None;
            serialPort.RtsEnable = false;
            serialPort.DtrEnable = false;

            AuxFunctions.openPort(serialPort, print);
        }
        #endregion

        #region TRANSMISSION FUNCTIONS
        public byte[] sendCommand(int dataLength, int code, int[] data)
        {
            byte checkSum;
            byte preamble1 = Convert.ToByte('$');
            byte preamble2 = Convert.ToByte('M');
            byte direction = Convert.ToByte('<');
            byte dataLengthByte = Convert.ToByte(dataLength);
            byte codeByte = Convert.ToByte(code);

            byte[] dataBytes = {};
            foreach (int i in data)
            {
                byte[] dataByte = BitConverter.GetBytes(Convert.ToInt16(i)).ToArray<Byte>();
                Console.WriteLine(BitConverter.ToString(dataByte));
                dataBytes = dataBytes.Concat(dataByte).ToArray<byte>();
            }

            checkSum = dataLengthByte;
            checkSum = Convert.ToByte(checkSum ^ codeByte);
            foreach (byte b in dataBytes)
            {
                checkSum = Convert.ToByte(checkSum ^ b);
            }
            byte[] totalDataAux = { preamble1, preamble2, direction, dataLengthByte, codeByte };
            byte[] checkSumArray = { checkSum };
            byte[] totalData = totalDataAux.Concat(dataBytes).Concat(checkSumArray).ToArray<byte>();
            try
            {  
                serialPort.Write(totalData, 0, totalData.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR 101: " + e.ToString() + ".");
            }

            return totalData;
        }

        public byte[] sendCommandAndReceive(int dataLength, int code, int[] data, int bytesToRead)
        {
            byte[] result = new byte[bytesToRead];
            byte[] buffer = new byte[3];
            byte checkSum = Convert.ToByte(0);
            byte preamble1 = Convert.ToByte('$');
            byte preamble2 = Convert.ToByte('M');
            byte direction = Convert.ToByte('<');
            byte dataLengthByte = Convert.ToByte(dataLength);
            byte codeByte = Convert.ToByte(code);

            byte[] dataBytes = { };
            foreach (int i in data)
            {
                byte[] dataByte = BitConverter.GetBytes(Convert.ToInt16(i)).ToArray<Byte>();
                Console.WriteLine(BitConverter.ToString(dataByte));
                dataBytes = dataBytes.Concat(dataByte).ToArray<byte>();
            }


            checkSum = Convert.ToByte(checkSum ^ dataLengthByte);
            checkSum = Convert.ToByte(checkSum ^ codeByte);
            foreach (byte b in dataBytes)
            {
                checkSum = Convert.ToByte(checkSum ^ b);
            }
            byte[] totalDataAux = { preamble1, preamble2, direction, dataLengthByte, codeByte };
            byte[] checkSumArray = { checkSum };
            Byte zero = (Convert.ToByte(0));
            byte[] zeros = { zero };
            byte[] totalData = totalDataAux.Concat(dataBytes).Concat(checkSumArray).Concat(zeros).ToArray<byte>();

            try
            {
                String sendData = BitConverter.ToString(totalData);
                Console.WriteLine("Datos enviados " + sendData );
                serialPort.Write(totalData, 0, totalData.Length);
                System.Threading.Thread.Sleep(1000);
                serialPort.Read(buffer,0,1);
                Boolean header = false;
                while(!header){
                    serialPort.Read(buffer, 0, 1);
                    if (buffer[0] == Convert.ToByte('$'))
                    {
                        serialPort.Read(buffer, 0, 1);
                        if (buffer[0] == Convert.ToByte('M'))
                        {
                            serialPort.Read(buffer, 0, 1);
                            if (buffer[0] == Convert.ToByte('>'))
                            {
                                header = true;
                                Console.WriteLine("Cabecera encontrada: " + buffer[0].ToString() + " $:" + Convert.ToByte('$').ToString() + buffer[1].ToString() + " M:" + Convert.ToByte('M').ToString() + buffer[2].ToString() + " >:" + Convert.ToByte('>').ToString());
                            }
                        }
                    }
                }
                 serialPort.Read(result, 0, bytesToRead);
                System.Threading.Thread.Sleep(50);
                Console.WriteLine("Datos leidos " + BitConverter.ToString(result));
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR 101: " + e.ToString() + ".");
            }
            return result;
        }

        #endregion
        /*
         * int[] data = { 1500, 1500, 2000, 1000, 1000 };
           result = sendCommand(10, MultiWiiCore.SET_RAW_RC, data);
         * 
         * data envia de 1 a 8 valores entre [1000-2000] (1500 para medio)
         * Si enviamos un 0 lo obviamos dicho canal
         * Canal 1 - Roll
         * Canal 2 - Pitch
         * Canal 3 - Yaw
         * Canal 4 - Throttle (Aceleracion)
         * Canal 5 - AUX1 (Armado)
         * Canal 6 - AUX2 
         * Canal 7 - AUX3 
         * Canal 8 - AUX4 
         * 
         * Para enviar esto bien hay que indicar en el sendCommand el parametro dataLength (arriba el 10)
         * 2 por canal es decir para 5 datos 10 (como en el ejemplo)
         * 
         **/

        #region MULTIWII CORE FUNCTIONS
        public byte[] setRawRC(int[] data)
        {
            //Envia datos a la naze 32
            byte[] result = null;
            DateTime start = DateTime.Now;
            int timeForSendCommand = 100;
            result = sendCommand(data.Length*2, MultiWiiCore.SET_RAW_RC, data);
            System.Threading.Thread.Sleep(timeForSendCommand);
            return result;
        }

        public byte[] mspStatus()
        {
            //Envia datos a la naze 32
            byte[] result = null;
            DateTime start = DateTime.Now;
            int timeForSendCommand = 100;
            int[] data = { };
            result = sendCommandAndReceive(0, MultiWiiCore.STATUS, data, 9);
            System.Threading.Thread.Sleep(timeForSendCommand);
            return result;
        }

        public byte[] mspAttitude()
        {
            //Envia datos a la naze 32
            byte[] result = null;
            DateTime start = DateTime.Now;
            int timeForSendCommand = 100;
            int[] data = { };
            result = sendCommandAndReceive(0, MultiWiiCore.ATTITUDE, data, 6);
            System.Threading.Thread.Sleep(timeForSendCommand);
            return result;
        }

        public byte[] mspRawIMU()
        {
            //Envia datos a la naze 32
            byte[] result = null;
            DateTime start = DateTime.Now;
            int timeForSendCommand = 100;
            int[] data = { };
            result = sendCommandAndReceive(0, MultiWiiCore.RAW_IMU, data, 12);
            System.Threading.Thread.Sleep(timeForSendCommand);
            return result;
        }

        public byte[] mspIdent() {
            //Envia datos a la naze 32
            byte[] result = null;
            DateTime start = DateTime.Now;
            int timeForSendCommand = 100;
            int[] data = { };
            result = sendCommandAndReceive(0, MultiWiiCore.IDENT, data, 7);
            System.Threading.Thread.Sleep(timeForSendCommand);
            return result;
        }

        public byte[] mspRawRC()
        {
            //Envia datos a la naze 32
            byte[] result = null;
            DateTime start = DateTime.Now;
            int timeForSendCommand = 100;
            int[] data = { };
            result = sendCommandAndReceive(0, MultiWiiCore.RC, data, 19);
            System.Threading.Thread.Sleep(timeForSendCommand);
            return result;
        }

        #endregion

    }
}
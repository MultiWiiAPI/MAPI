using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace MultiWii
{
    public static class CoreFunctions
    {
        /* 
         * This class will have the core functions for the toolkit in C#
         */

        #region VARIABLES
        public static Boolean print = true;
        public static SerialPort serialPort;
        #endregion

        #region INITIALIZE VARIABLES
        /* CONSTRUCTOR */
        public static Boolean initializeCore(String portName)
        {
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
            //serialPort.DataReceived += new System.EventHandler(dataReceived);
            return AuxiliarFunctions.openPort(serialPort, print);
        }
        #endregion

        #region CORE FUNCTIONS
        public static byte[] sendCommand(int dataLength, int code, int[] data)
        {
            byte checkSum;
            byte preamble1 = Convert.ToByte('$');
            byte preamble2 = Convert.ToByte('M');
            byte direction = Convert.ToByte('<');
            byte dataLengthByte = Convert.ToByte(dataLength);
            byte codeByte = Convert.ToByte(code);
            byte[] dataBytes = {};
            byte[] zeros = new byte[1];
            if (dataLength != 0)
            {
                foreach (int i in data)
                {
                    byte[] dataByte = BitConverter.GetBytes(Convert.ToInt16(i)).ToArray<Byte>();
                    dataBytes = dataBytes.Concat(dataByte).ToArray<byte>();
                }
            }
            else {
                byte zero = (Convert.ToByte(0));
                zeros[0] = zero;
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
            if (dataLength == 0)
            {
                totalData = totalDataAux.Concat(dataBytes).Concat(checkSumArray).Concat(zeros).ToArray<byte>();
            }
            try
            {
                Console.WriteLine(BitConverter.ToString(totalData));
                serialPort.DiscardOutBuffer();
                serialPort.Write(totalData, 0, totalData.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR 101: " + e.ToString() + ".");
                return new byte[0];
            }
            return totalData;
        }
        public static byte[] sendCommandAndReceive(int dataLength, int code, int[] data, int bytesToRead)
        {
            byte[] result = new byte[bytesToRead];
            byte[] buffer = new byte[1];
            String preamble1;
            String preamble2;
            String direction;

            try
            {
                serialPort.DiscardInBuffer();
                sendCommand(dataLength, code, data);
                DateTime start = DateTime.Now;
                while (serialPort.BytesToRead < 1) {
                    DateTime end = DateTime.Now;
                    if((end - start).TotalMilliseconds>1000){
                        Console.WriteLine("ERROR 103: Connection Lost.");
                        return result;
                    }
                    
                }
                Boolean header = false;
                while (!header)
                {
                        serialPort.Read(buffer, 0, 1);
                    if (buffer[0] == Convert.ToByte('$'))
                    {
                        preamble1 = buffer[0].ToString("X2");
                        serialPort.Read(buffer, 0, 1);

                        if (buffer[0] == Convert.ToByte('M'))
                        {
                            preamble2 = buffer[0].ToString("X2");
                            serialPort.Read(buffer, 0, 1);

                            if (buffer[0] == Convert.ToByte('>'))
                            {
                                header = true;
                                direction = buffer[0].ToString("X2");
                                Console.WriteLine("Cabecera buscada: " + Convert.ToByte('$').ToString("X2") + "-" + Convert.ToByte('M').ToString("X2") + "-" + Convert.ToByte('>').ToString("X2") + " ($M>), encontrado: " + preamble1 + "-" + preamble2 + "-" + direction + ". ");
                            }
                        }
                    }
                }
                serialPort.Read(result, 0, bytesToRead);
                Console.WriteLine("Datos leidos " + BitConverter.ToString(result));
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR 101: " + e.ToString() + ".");
                return new byte[0];
            }
            return result;
        }
        #endregion
    }
}
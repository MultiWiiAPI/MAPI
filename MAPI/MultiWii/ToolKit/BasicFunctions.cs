using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;


namespace MultiWii
{
    public static class BasicFunctions
    {
        #region VARIABLES

        public static int numExceedError = 4;
        public static int[] channelsActualValues = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] motorsActualValues = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        #endregion

        #region ACTION FUNCTIONS

        public static Boolean throttle(int throttle) {
            Boolean result = false;
            if (throttle < 1000 || throttle > 2000) {
                return result;
            }
            if (throttle == 1000)
            {
                channelsActualValues[0] = 1500;
                channelsActualValues[1] = 1500;
                channelsActualValues[2] = 1500;
            }
            int[] data = { channelsActualValues[0], channelsActualValues[1], channelsActualValues[2], throttle };
            channelsActualValues[3] = throttle;
            Console.WriteLine(string.Join(",", data));
            if(!MultiWiiFunctions.mspSetRawRC(data).Equals(new byte[0])){
                result = true;
            }
            return result;
        }

        public static Boolean roll(int roll)
        {
            Boolean result = false;
            if (roll < 1000 || roll > 2000)
            {
                return result;
            }
            int[] data = { roll, channelsActualValues[1], channelsActualValues[2], channelsActualValues[3] };
            channelsActualValues[0] = roll;
            Console.WriteLine(string.Join(",", data));
            if (!MultiWiiFunctions.mspSetRawRC(data).Equals(new byte[0]))
            {
                result = true;
            }
            return result;
        }

        public static Boolean pitch(int pitch)
        {
            Boolean result = false;
            if (pitch < 1000 || pitch > 2000)
            {
                return result;
            }
            int[] data = { channelsActualValues[0], pitch, channelsActualValues[2], channelsActualValues[3] };
            channelsActualValues[1] = pitch;
            Console.WriteLine(string.Join(",", data));
            if (!MultiWiiFunctions.mspSetRawRC(data).Equals(new byte[0]))
            {
                result = true;
            }
            return result;
        }

        public static Boolean yaw(int yaw)
        {
            Boolean result = false;
            if (yaw < 1000 || yaw > 2000)
            {
                return result;
            }
            int[] data = { channelsActualValues[0], channelsActualValues[1], yaw, channelsActualValues[3] };
            channelsActualValues[2] = yaw;
            Console.WriteLine(string.Join(",", data));
            if (!MultiWiiFunctions.mspSetRawRC(data).Equals(new byte[0]))
            {
                result = true;
            }
            return result;
        }

        public static Boolean arm()
        {
            Boolean result = false;
            int[] data = { 1500, 1500, 1500, 1000, 2000 };
            channelsActualValues[0] = 1500;
            channelsActualValues[1] = 1500;
            channelsActualValues[2] = 1500;
            channelsActualValues[3] = 1000;
            channelsActualValues[4] = 2000;
            Console.WriteLine(string.Join(",", data));
            if (!MultiWiiFunctions.mspSetRawRC(data).Equals(new byte[0]))
            {
                result = true;
            }
            return result;
        }

        public static Boolean disarm()
        {
            Boolean result = false;
            int[] data = { 1500, 1500, 1500, 1000, 1000 };
            channelsActualValues[0] = 1500;
            channelsActualValues[1] = 1500;
            channelsActualValues[2] = 1500;
            channelsActualValues[3] = 1000;
            channelsActualValues[4] = 1000;
            Console.WriteLine(string.Join(",", data));
            if (!MultiWiiFunctions.mspSetRawRC(data).Equals(new byte[0]))
            {
                result = true;
            }
            return result;
        }

        public static Boolean setMotor(int i, int intensity) {
            Boolean result = false;
            byte[] mspSetMotor;

            if (intensity < 1000 || intensity> 2000) {
                return result;
            }

            if(motorsActualValues[0] == 0){
                getMotorsValues();
            }

            switch (i) { 
                case 1: 
                    {
                        int[] data = { intensity, motorsActualValues[1], motorsActualValues[2], motorsActualValues[3], motorsActualValues[4], motorsActualValues[5], motorsActualValues[6], motorsActualValues[7] };
                        mspSetMotor = MultiWiiFunctions.mspSetMotor(data);
                        if (mspSetMotor != new byte[0])
                        {
                            result = true;
                        }
                        break;
                    }
                case 2: 
                    {
                        int[] data = { motorsActualValues[0], intensity, motorsActualValues[2], motorsActualValues[3], motorsActualValues[4], motorsActualValues[5], motorsActualValues[6], motorsActualValues[7] };
                        mspSetMotor = MultiWiiFunctions.mspSetMotor(data);
                        if (mspSetMotor != new byte[0])
                        {
                            result = true;
                        }
                        break;
                    }

                case 3:
                    {
                        int[] data = { motorsActualValues[0], motorsActualValues[1], intensity, motorsActualValues[3], motorsActualValues[4], motorsActualValues[5], motorsActualValues[6], motorsActualValues[7] };
                        mspSetMotor = MultiWiiFunctions.mspSetMotor(data);
                        if (mspSetMotor != new byte[0])
                        {
                            result = true;
                        }
                        break;
                    }
                case 4:
                    {
                        int[] data = { motorsActualValues[0], motorsActualValues[1], motorsActualValues[2], intensity, motorsActualValues[4], motorsActualValues[5], motorsActualValues[6], motorsActualValues[7] };
                        mspSetMotor = MultiWiiFunctions.mspSetMotor(data);
                        if (mspSetMotor != new byte[0])
                        {
                            result = true;
                        }
                        break;
                    }
                case 5:
                    {
                        int[] data = { motorsActualValues[0], motorsActualValues[1], motorsActualValues[2], motorsActualValues[3], intensity, motorsActualValues[5], motorsActualValues[6], motorsActualValues[7] };
                        mspSetMotor = MultiWiiFunctions.mspSetMotor(data);
                        if (mspSetMotor != new byte[0])
                        {
                            result = true;
                        }
                        break;
                    }
                case 6:
                    {
                        int[] data = { motorsActualValues[0], motorsActualValues[1], motorsActualValues[2], motorsActualValues[3], motorsActualValues[4], intensity, motorsActualValues[6], motorsActualValues[7] };
                        mspSetMotor = MultiWiiFunctions.mspSetMotor(data);
                        if (mspSetMotor != new byte[0])
                        {
                            result = true;
                        }
                        break;
                    }
                case 7:
                    {
                        int[] data = { motorsActualValues[0], motorsActualValues[1], motorsActualValues[2], motorsActualValues[3], motorsActualValues[4], motorsActualValues[5], intensity, motorsActualValues[7] };
                        mspSetMotor = MultiWiiFunctions.mspSetMotor(data);
                        if (mspSetMotor != new byte[0])
                        {
                            result = true;
                        }
                        break;
                    }
                case 8:
                    {
                        int[] data = { motorsActualValues[0], motorsActualValues[1], motorsActualValues[2], motorsActualValues[3], motorsActualValues[4], motorsActualValues[5], motorsActualValues[6], intensity };
                        mspSetMotor = MultiWiiFunctions.mspSetMotor(data);
                        if (mspSetMotor != new byte[0])
                        {
                            result = true;
                        }
                        break;
                    }
                default: break;
            
            }
            return result;
        }

        #endregion

        #region INFORMATION FUNCTIONS

        public static int[] getRCChannels()
        {
                byte[] roll = new byte[2];
                byte[] pitch = new byte[2];
                byte[] yaw = new byte[2];
                byte[] throttle = new byte[2];
                byte[] aux1 = new byte[2];
                byte[] aux2 = new byte[2];
                byte[] aux3 = new byte[2];
                byte[] aux4 = new byte[2];
                int[] result = new int[8];
                byte[] mspRC = MultiWiiFunctions.mspRC();

                while (numExceedError>0 && mspRC.Length != 19) {
                    Console.WriteLine("Num of Error: " + numExceedError);
                    numExceedError--;
                    mspRC = MultiWiiFunctions.mspRC();
                }
                if (numExceedError <= 0)
                {
                    Console.WriteLine("ERROR 102: Couldn't read the correct values");
                    numExceedError = 4;
                    return new int[0];
                }
                roll[0] = mspRC[2];
                roll[1] = mspRC[3];
                int rollChannel = BitConverter.ToInt16(roll, 0);
                Console.WriteLine("Roll Channel: " + rollChannel);
                result[0] = rollChannel;
                Boolean rollbool = rollChannel >= 1000 && rollChannel <= 2000;

                pitch[0] = mspRC[4];
                pitch[1] = mspRC[5];
                int pitchChannel = BitConverter.ToInt16(pitch, 0);
                Console.WriteLine("Pitch Channel: " + pitchChannel);
                result[1] = pitchChannel;
                Boolean pitchbool = pitchChannel >= 1000 && pitchChannel <= 2000;

                yaw[0] = mspRC[6];
                yaw[1] = mspRC[7];
                int yawChannel = BitConverter.ToInt16(yaw, 0);
                Console.WriteLine("Yaw Channel: " + yawChannel);
                result[2] = yawChannel;
                Boolean yawbool = yawChannel >= 1000 && yawChannel <= 2000;

                throttle[0] = mspRC[8];
                throttle[1] = mspRC[9];
                int throttleChannel = BitConverter.ToInt16(throttle, 0);
                Console.WriteLine("Throttle Channel: " + throttleChannel);
                result[3] = throttleChannel;
                Boolean throttlebool = throttleChannel >= 1000 && throttleChannel <= 2000;

                aux1[0] = mspRC[10];
                aux1[1] = mspRC[11];
                int aux1Channel = BitConverter.ToInt16(aux1, 0);
                Console.WriteLine("Aux1 Channel: " + aux1Channel);
                result[4] = aux1Channel;
                Boolean aux1bool = aux1Channel >= 1000 && aux1Channel <= 2000;

                aux2[0] = mspRC[12];
                aux2[1] = mspRC[13];
                int aux2Channel = BitConverter.ToInt16(aux2, 0);
                Console.WriteLine("Aux2 Channel: " + aux2Channel);
                result[5] = aux2Channel;

                aux3[0] = mspRC[14];
                aux3[1] = mspRC[15];
                int aux3Channel = BitConverter.ToInt16(aux3, 0);
                Console.WriteLine("Aux3 Channel: " + aux3Channel);
                result[6] = aux3Channel;

                aux4[0] = mspRC[16];
                aux4[1] = mspRC[17];
                int aux4Channel = BitConverter.ToInt16(aux4, 0);
                Console.WriteLine("Aux4 Channel: " + aux4Channel);
                result[7] = aux4Channel;

                if (numExceedError > 0 && (!rollbool || !pitchbool || !yawbool || !throttlebool || !aux1bool))
                {
                    Console.WriteLine("Num of Error: " + numExceedError);
                    numExceedError--;
                    result = getRCChannels();
                }
                if (numExceedError <= 0)
                {
                    Console.WriteLine("ERROR 102: Couldn't read the correct values");
                    numExceedError = 4;
                    return new int[0];
                }
                numExceedError = 4;
                return result;
        }

        public static String getMultiWiiVersion() {

            byte[] mspIdent = MultiWiiFunctions.mspIdent();

            while (numExceedError > 0 && mspIdent.Length != 10)
            {
                Console.WriteLine("Num of Error: " + numExceedError);
                numExceedError--;
                mspIdent = MultiWiiFunctions.mspIdent();
            }
            if (numExceedError <= 0)
            {
                Console.WriteLine("ERROR 102: Couldn't read the correct values");
                numExceedError = 4;
                return "Incorrect Value";
            }
            int version = Convert.ToUInt16(mspIdent[2]);
            String versionString = version.ToString();
            versionString = versionString[0] + "."+ versionString.Substring(1);
            Console.WriteLine("MultiWii Version is: " + versionString );
            return versionString;
        }

        public static int[] getInertialMeasureUnitValues()
        {
            return getInertialMeasureUnitValues(true, true, true);
        }

        public static int[] getInertialMeasureUnitValues(Boolean accelerometer, Boolean gyroscope, Boolean magnetometer)
        {
            byte[] accX = new byte[2];
            byte[] accY = new byte[2];
            byte[] accZ = new byte[2];
            byte[] gyroX = new byte[2];
            byte[] gyroY= new byte[2];
            byte[] gyroZ= new byte[2];
            byte[] magX= new byte[2];
            byte[] magY = new byte[2];
            byte[] magZ = new byte[2];
            
            int numberOfElements = 0;
            if (accelerometer) numberOfElements += 3;
            if (gyroscope) numberOfElements += 3;
            if (magnetometer) numberOfElements += 3;
            int[] result = new int[numberOfElements];

            byte[] mspRawIMU = MultiWiiFunctions.mspRawIMU();


            while (numExceedError > 0 && mspRawIMU.Length != 21)
            {
                Console.WriteLine("Num of Error: " + numExceedError);
                numExceedError--;
                mspRawIMU = MultiWiiFunctions.mspRawIMU();
            }
            if (numExceedError <= 0)
            {
                Console.WriteLine("ERROR 102: Couldn't read the correct values");
                numExceedError = 4;
                return new int[0];
            }
            if (accelerometer)
            {
                accX[0] = mspRawIMU[2];
                accX[1] = mspRawIMU[3];
                int accXValue = BitConverter.ToInt16(accX, 0);
                Console.WriteLine("Acceloremeter, Coordinate X Value: " + accXValue);
                result[0] = accXValue;


                accY[0] = mspRawIMU[4];
                accY[1] = mspRawIMU[5];
                int accYValue = BitConverter.ToInt16(accY, 0);
                Console.WriteLine("Accelerometer, Coordinate Y Value: " + accYValue);
                result[1] = accYValue;


                accZ[0] = mspRawIMU[6];
                accZ[1] = mspRawIMU[7];
                int accZValue = BitConverter.ToInt16(accZ, 0);
                Console.WriteLine("Accelerometer, Coordinate Z Value: " + accZValue);
                result[2] = accZValue;
            }

            if (gyroscope)
            {
                gyroX[0] = mspRawIMU[8];
                gyroX[1] = mspRawIMU[9];
                int gyroXValue = BitConverter.ToInt16(gyroX, 0);
                Console.WriteLine("Gyroscope, Coordinate X Value: " + gyroXValue);
                


                gyroY[0] = mspRawIMU[10];
                gyroY[1] = mspRawIMU[11];
                int gyroYValue = BitConverter.ToInt16(gyroY, 0);
                Console.WriteLine("Gyroscope, Coordinate Y Value: " + gyroYValue);


                gyroZ[0] = mspRawIMU[12];
                gyroZ[1] = mspRawIMU[13];
                int gyroZValue = BitConverter.ToInt16(gyroZ, 0);
                Console.WriteLine("Gyroscope, Coordinate Z Value: " + gyroZValue);

                if (accelerometer)
                {
                    result[3] = gyroXValue;
                    result[4] = gyroYValue;
                    result[5] = gyroZValue;
                }
                else {
                    result[0] = gyroXValue;
                    result[1] = gyroYValue;
                    result[2] = gyroZValue;
                }
            }

            if (magnetometer)
            {
                magX[0] = mspRawIMU[14];
                magX[1] = mspRawIMU[15];
                int magXValue = BitConverter.ToInt16(magX, 0);
                Console.WriteLine("Magnetometer, Coordinate X Value: " + magXValue);
                

                magY[0] = mspRawIMU[16];
                magY[1] = mspRawIMU[17];
                int magYValue = BitConverter.ToInt16(magY, 0);
                Console.WriteLine("Magnetometer, Coordinate Y Value: " + magYValue);
                

                magZ[0] = mspRawIMU[18];
                magZ[1] = mspRawIMU[19];
                int magZValue = BitConverter.ToInt16(magZ, 0);
                Console.WriteLine("Magnetometer, Coordinate Z Value: " + magZValue);

                if (accelerometer && gyroscope)
                {
                    result[6] = magXValue;
                    result[7] = magYValue;
                    result[8] = magZValue;
                }
                else
                {
                    if (accelerometer || gyroscope)
                    {
                        result[3] = magXValue;
                        result[4] = magYValue;
                        result[5] = magZValue;
                    }
                    else
                    {
                        result[0] = magXValue;
                        result[1] = magYValue;
                        result[2] = magZValue;
                    }
                }
            }
            numExceedError = 4;
            return result;
        }

        public static int[] getAccelerometerValues() {
            return getInertialMeasureUnitValues(true, false, false);
        }

        public static int[] getGyroscopeValues()
        {
            return getInertialMeasureUnitValues(false, true, false);
        }

        public static int[] getMagnetometerValues()
        {
            return getInertialMeasureUnitValues(false, false, true);
        }

        public static int[] getMotorsValues() {

            byte[] motor1 = new byte[2];
            byte[] motor2 = new byte[2];
            byte[] motor3 = new byte[2];
            byte[] motor4 = new byte[2];
            byte[] motor5 = new byte[2];
            byte[] motor6 = new byte[2];
            byte[] motor7 = new byte[2];
            byte[] motor8 = new byte[2];
            int[] result = new int[8];
            byte[] mspMotor = MultiWiiFunctions.mspMotor();

            while (numExceedError > 0 && mspMotor.Length != 19)
            {
                Console.WriteLine("Num of Error: " + numExceedError);
                numExceedError--;
                mspMotor = MultiWiiFunctions.mspMotor();
            }
            if (numExceedError <= 0)
            {
                Console.WriteLine("ERROR 102: Couldn't read the correct values");
                numExceedError = 4;
                return new int[0];
            }
            motor1[0] = mspMotor[2];
            motor1[1] = mspMotor[3];
            int motor1Channel = BitConverter.ToInt16(motor1, 0);
            Console.WriteLine("Motor1 Channel: " + motor1Channel);
            result[0] = motor1Channel;
            motorsActualValues[0] = motor1Channel;

            motor2[0] = mspMotor[4];
            motor2[1] = mspMotor[5];
            int motor2Channel = BitConverter.ToInt16(motor2, 0);
            Console.WriteLine("Motor2 Channel: " + motor2Channel);
            result[1] = motor2Channel;
            motorsActualValues[1] = motor2Channel;

            motor3[0] = mspMotor[6];
            motor3[1] = mspMotor[7];
            int motor3Channel = BitConverter.ToInt16(motor3, 0);
            Console.WriteLine("Motor3 Channel: " + motor3Channel);
            result[2] = motor3Channel;
            motorsActualValues[2] = motor3Channel;

            motor4[0] = mspMotor[8];
            motor4[1] = mspMotor[9];
            int motor4Channel = BitConverter.ToInt16(motor4, 0);
            Console.WriteLine("Motor4 Channel: " + motor4Channel);
            result[3] = motor4Channel;
            motorsActualValues[3] = motor4Channel;

            motor5[0] = mspMotor[10];
            motor5[1] = mspMotor[11];
            int motor5Channel = BitConverter.ToInt16(motor5, 0);
            Console.WriteLine("Motor5 Channel: " + motor5Channel);
            result[4] = motor5Channel;
            motorsActualValues[4] = motor5Channel;

            motor6[0] = mspMotor[12];
            motor6[1] = mspMotor[13];
            int motor6Channel = BitConverter.ToInt16(motor6, 0);
            Console.WriteLine("Motor6 Channel: " + motor6Channel);
            result[5] = motor6Channel;
            motorsActualValues[5] = motor6Channel;

            motor7[0] = mspMotor[14];
            motor7[1] = mspMotor[15];
            int motor7Channel = BitConverter.ToInt16(motor7, 0);
            Console.WriteLine("Motor7 Channel: " + motor7Channel);
            result[6] = motor7Channel;
            motorsActualValues[6] = motor7Channel;

            motor8[0] = mspMotor[16];
            motor8[1] = mspMotor[17];
            int motor8Channel = BitConverter.ToInt16(motor8, 0);
            Console.WriteLine("Motor8 Channel: " + motor8Channel);
            result[7] = motor8Channel;
            motorsActualValues[7] = motor8Channel;
            
            numExceedError = 4;
            return result;
        
        }
        
        public static int[] getAttitude()
        {
            byte[] angxArray = new byte[2];
            byte[] angyArray = new byte[2];
            byte[] headingArray = new byte[2];
            int[] result = new int[3];

            byte[] mspAttitude = MultiWiiFunctions.mspAttitude();

            angxArray[0] = mspAttitude[2];
            angxArray[1] = mspAttitude[3];
            int angx = BitConverter.ToInt16(angxArray, 0);
            Console.WriteLine("Ang x: " + angx);
            result[0] = angx;

            angyArray[0] = mspAttitude[4];
            angyArray[1] = mspAttitude[5];
            int angy = BitConverter.ToInt16(angyArray, 0);
            Console.WriteLine("Ang y: " + angy);
            result[1] = angy;

            headingArray[0] = mspAttitude[6];
            headingArray[1] = mspAttitude[7];
            int heading = BitConverter.ToInt16(headingArray, 0);
            Console.WriteLine("Heading: " + heading);
            result[2] = heading;
            return result;
        }

        public static int[] getAltitude() {
            byte[] cmArray = new byte[4];
            byte[] cmsArray = new byte[2];
 
            int[] result = new int[2];

            byte[] mspAltitude = MultiWiiFunctions.mspAltitude();

            cmArray[0] = mspAltitude[2];
            cmArray[1] = mspAltitude[3];
            cmArray[2] = mspAltitude[4];
            cmArray[3] = mspAltitude[5];
            int cm = BitConverter.ToInt32(cmArray, 0);
            Console.WriteLine("Altitude in cm: " + cm);
            result[0] = cm;

            cmsArray[0] = mspAltitude[6];
            cmsArray[1] = mspAltitude[7];
            int cms = BitConverter.ToInt16(cmsArray, 0);
            Console.WriteLine("Velocity in cm/s: " + cms);
            result[1] = cms;
            return result;
        }

        public static double getAltitudeInMeters() {
            int cm = getAltitude()[0];
            return cm / 100;
        }

        public static double getVelocityAltitudeInMetersPerSecond()
        {
            int cms = getAltitude()[1];
            return cms / 100;
        }

#endregion
    }
}
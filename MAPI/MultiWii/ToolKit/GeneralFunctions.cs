using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiWii
{
    public static class GeneralFunctions
    {
        #region ON-OFF FUNCTIONS

        public static Boolean turnOn_turnOff(Boolean on) {
            Boolean armed = BasicFunctions.channelsActualValues[4] == 2000;
            Boolean result = false;
            if (armed && !on )
            {
                result = BasicFunctions.disarm();
            }
            
            if (!armed && on ){
                result = BasicFunctions.arm();
            }
            return result;
        }

        #endregion

        #region MOVE FUNCTIONS

        public static Boolean takeOff() {
            Boolean result = false;
            Boolean condition = false;
            Boolean armed = BasicFunctions.channelsActualValues[4] == 2000;
            int throttleValue = BasicFunctions.channelsActualValues[3];
            condition = armed && throttleValue <= 1000;

            if (condition) 
            {
                result = BasicFunctions.throttle(1200);
            }

            Thread.Sleep(300);

            if (condition)
            {
                result = BasicFunctions.throttle(1700);
            }

            Thread.Sleep(1000);

            while (condition)
            {
                throttleValue -= 50;
                if (throttleValue <= 1400)
                {
                    throttleValue = 1400;
                    result = BasicFunctions.throttle(throttleValue);
                    break;
                }
                result = BasicFunctions.throttle(throttleValue);
                Thread.Sleep(300);
            }
            return result;
        }

        public static Boolean land() {
            Boolean result = false;
            Boolean condition = false;
            Boolean armed = BasicFunctions.channelsActualValues[4] == 2000;
            int throttleValue = BasicFunctions.channelsActualValues[3];
            condition = armed && throttleValue > 1000;

            while (condition)
            {
               throttleValue -= 50;
               if (throttleValue <= 1000 ){
                  throttleValue = 1000;
                  result = BasicFunctions.throttle(throttleValue);
                  break;
               }
               result = BasicFunctions.throttle(throttleValue);
               Thread.Sleep(500);
            }
            return result;
        }

        public static Boolean rotateRight(int seconds) {
            Boolean result = false;
            Boolean condition = false;
            Boolean armed = BasicFunctions.channelsActualValues[4] == 2000;
            double executionTime = seconds*1000;
            int yawValue = BasicFunctions.channelsActualValues[2];
            int throttleValue = BasicFunctions.channelsActualValues[3];
            condition =  armed && throttleValue > 1000;
            Boolean actualYawMajor = yawValue > 1700;
            Boolean actualYawMinor = yawValue < 1700;

            DateTime start = DateTime.Now;
            while (condition && actualYawMajor)
            {
                Thread.Sleep(300);
                yawValue -= 50;
                if (yawValue <= 1700)
                {
                    yawValue = 1700;
                    actualYawMajor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.yaw(yawValue);
            }

            if (executionTime <= 0) {
                while (true)
                {
                    Thread.Sleep(300);
                    yawValue -= 50;
                    if (yawValue <= 1500)
                    {
                        yawValue = 1500;
                        result = BasicFunctions.yaw(yawValue);
                        break;
                    }
                    result = BasicFunctions.yaw(yawValue);
                }
                return result;
            }

            start = DateTime.Now;
            while (condition && actualYawMinor)
            {
                Thread.Sleep(300);
                yawValue += 50;
                if (yawValue >= 1700)
                {
                    yawValue = 1700;
                    actualYawMinor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.yaw(yawValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    yawValue -= 50;
                    if (yawValue <= 1500)
                    {
                        yawValue = 1500;
                        result = BasicFunctions.yaw(yawValue);
                        break;
                    }
                    result = BasicFunctions.yaw(yawValue);
                }
                return result;
            }

            Thread.Sleep(Convert.ToInt32(executionTime));
            while (true)
            {
                Thread.Sleep(300);
                yawValue -= 50;
                if (yawValue <= 1500)
                {
                    yawValue = 1500;
                    result = BasicFunctions.yaw(yawValue);
                    break;
                }
                result = BasicFunctions.yaw(yawValue);
            }
            return result;
        }

        public static Boolean rotateLeft(int seconds) {
            Boolean result = false;
            Boolean condition = false;
            Boolean armed = BasicFunctions.channelsActualValues[4] == 2000;
            double executionTime = seconds * 1000;
            int yawValue = BasicFunctions.channelsActualValues[2];
            int throttleValue = BasicFunctions.channelsActualValues[3];
            condition = armed && throttleValue > 1000;
            Boolean actualYawMajor = yawValue > 1300;
            Boolean actualYawMinor = yawValue < 1300;

            DateTime start = DateTime.Now;
            while (condition && actualYawMajor)
            {
                Thread.Sleep(300);
                yawValue -= 50;
                if (yawValue <= 1300)
                {
                    yawValue = 1300;
                    actualYawMajor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.yaw(yawValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    yawValue -= 50;
                    if (yawValue <= 1500)
                    {
                        yawValue = 1500;
                        result = BasicFunctions.yaw(yawValue);
                        break;
                    }
                    result = BasicFunctions.yaw(yawValue);
                }
                return result;
            }

            start = DateTime.Now;
            while (condition && actualYawMinor)
            {
                Thread.Sleep(300);
                yawValue += 50;
                if (yawValue >= 1300)
                {
                    yawValue = 1300;
                    actualYawMinor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.yaw(yawValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    yawValue += 50;
                    if (yawValue >= 1500)
                    {
                        yawValue = 1500;
                        result = BasicFunctions.yaw(yawValue);
                        break;
                    }
                    result = BasicFunctions.yaw(yawValue);
                }
                return result;
            }

            Thread.Sleep(Convert.ToInt32(executionTime));
            while (true)
            {
                Thread.Sleep(300);
                yawValue += 50;
                if (yawValue >= 1500)
                {
                    yawValue = 1500;
                    result = BasicFunctions.yaw(yawValue);
                    break;
                }
                result = BasicFunctions.yaw(yawValue);
            }
            return result;
        }

        public static Boolean up(int seconds) {
            Boolean result = false;
            Boolean condition = false;
            Boolean armed = BasicFunctions.channelsActualValues[4] == 2000;
            double executionTime = seconds * 1000;
            int throttleValue = BasicFunctions.channelsActualValues[3];
            int maxThrottleValue = throttleValue + 200;
            condition = armed && throttleValue > 1000;

            if (throttleValue == 2000) { return false; }
            if (maxThrottleValue > 2000) { maxThrottleValue = 2000; }

            DateTime start = DateTime.Now;
            while (condition)
            {
                Thread.Sleep(300);
                throttleValue += 50;
                if (throttleValue >= maxThrottleValue)
                {
                    throttleValue = maxThrottleValue;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                    result = BasicFunctions.throttle(throttleValue);
                    break;
                }
                result = BasicFunctions.throttle(throttleValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    throttleValue -= 50;
                    if (throttleValue <= maxThrottleValue - 200)
                    {
                        throttleValue = maxThrottleValue - 200;
                        result = BasicFunctions.throttle(throttleValue);
                        break;
                    }
                    result = BasicFunctions.throttle(throttleValue);
                }
                return result;
            }

            Thread.Sleep(Convert.ToInt32(executionTime));
            while (true)
            {
                Thread.Sleep(300);
                throttleValue -= 50;
                if (throttleValue <= maxThrottleValue - 200)
                {
                    throttleValue = maxThrottleValue - 200;
                    result = BasicFunctions.throttle(throttleValue);
                    break;
                }
                result = BasicFunctions.throttle(throttleValue);
            }
            return result;
        }

        public static Boolean down(int seconds) {
            Boolean result = false;
            Boolean condition = false;
            Boolean armed = BasicFunctions.channelsActualValues[4] == 2000;
            double executionTime = seconds * 1000;
            int throttleValue = BasicFunctions.channelsActualValues[3];
            int minThrottleValue = throttleValue - 200;
            condition = armed && throttleValue > 1000;

            if (throttleValue == 2000) { return false; }
            if (minThrottleValue < 1150) { minThrottleValue = 1150; }

            DateTime start = DateTime.Now;
            while (condition)
            {
                Thread.Sleep(300);
                throttleValue -= 50;
                if (throttleValue <= minThrottleValue)
                {
                    throttleValue = minThrottleValue;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                    result = BasicFunctions.throttle(throttleValue);
                    break;
                }
                result = BasicFunctions.throttle(throttleValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    throttleValue += 50;
                    if (throttleValue >= minThrottleValue + 200)
                    {
                        throttleValue = minThrottleValue + 200;
                        result = BasicFunctions.throttle(throttleValue);
                        break;
                    }
                    result = BasicFunctions.throttle(throttleValue);
                }
                return result;
            }

            Thread.Sleep(Convert.ToInt32(executionTime));
            while (true)
            {
                Thread.Sleep(300);
                throttleValue += 50;
                if (throttleValue <= minThrottleValue + 200)
                {
                    throttleValue = minThrottleValue + 200;
                    result = BasicFunctions.throttle(throttleValue);
                    break;
                }
                result = BasicFunctions.throttle(throttleValue);
            }
            return result;
        }

        public static Boolean straightRight(int seconds)
        {
            Boolean result = false;
            Boolean condition = false;
            Boolean armed = BasicFunctions.channelsActualValues[4] == 2000;
            double executionTime = seconds * 1000;
            int rollValue = BasicFunctions.channelsActualValues[0];
            int throttleValue = BasicFunctions.channelsActualValues[3];
            condition = armed && throttleValue > 1000;
            Boolean actualRollMajor = rollValue > 1700;
            Boolean actualRollMinor = rollValue < 1700;

            DateTime start = DateTime.Now;
            while (condition && actualRollMajor)
            {
                Thread.Sleep(300);
                rollValue -= 50;
                if (rollValue <= 1700)
                {
                    rollValue = 1700;
                    actualRollMajor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.roll(rollValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    rollValue -= 50;
                    if (rollValue <= 1500)
                    {
                        rollValue = 1500;
                        result = BasicFunctions.roll(rollValue);
                        break;
                    }
                    result = BasicFunctions.roll(rollValue);
                }
                return result;
            }

            start = DateTime.Now;
            while (condition && actualRollMinor)
            {
                Thread.Sleep(300);
                rollValue += 50;
                if (rollValue >= 1700)
                {
                    rollValue = 1700;
                    actualRollMinor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.roll(rollValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    rollValue -= 50;
                    if (rollValue <= 1500)
                    {
                        rollValue = 1500;
                        result = BasicFunctions.roll(rollValue);
                        break;
                    }
                    result = BasicFunctions.roll(rollValue);
                }
                return result;
            }

            Thread.Sleep(Convert.ToInt32(executionTime));
            while (true)
            {
                Thread.Sleep(300);
                rollValue -= 50;
                if (rollValue <= 1500)
                {
                    rollValue = 1500;
                    result = BasicFunctions.roll(rollValue);
                    break;
                }
                result = BasicFunctions.roll(rollValue);
            }
            return result;
        }

        public static Boolean straightLeft(int seconds)
        {
            Boolean result = false;
            Boolean condition = false;
            Boolean armed = BasicFunctions.channelsActualValues[4] == 2000;
            double executionTime = seconds * 1000;
            int rollValue = BasicFunctions.channelsActualValues[0];
            int throttleValue = BasicFunctions.channelsActualValues[3];
            condition = armed && throttleValue > 1000;
            Boolean actualRollMajor = rollValue > 1300;
            Boolean actualRollMinor = rollValue < 1300;

            DateTime start = DateTime.Now;
            while (condition && actualRollMajor)
            {
                Thread.Sleep(300);
                rollValue -= 50;
                if (rollValue <= 1300)
                {
                    rollValue = 1300;
                    actualRollMajor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.roll(rollValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    rollValue += 50;
                    if (rollValue >= 1500)
                    {
                        rollValue = 1500;
                        result = BasicFunctions.roll(rollValue);
                        break;
                    }
                    result = BasicFunctions.roll(rollValue);
                }
                return result;
            }

            start = DateTime.Now;
            while (condition && actualRollMinor)
            {
                Thread.Sleep(300);
                rollValue += 50;
                if (rollValue >= 1300)
                {
                    rollValue = 1300;
                    actualRollMinor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.roll(rollValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    rollValue += 50;
                    if (rollValue >= 1500)
                    {
                        rollValue = 1500;
                        result = BasicFunctions.roll(rollValue);
                        break;
                    }
                    result = BasicFunctions.roll(rollValue);
                }
                return result;
            }

            Thread.Sleep(Convert.ToInt32(executionTime));
            while (true)
            {
                Thread.Sleep(300);
                rollValue += 50;
                if (rollValue >= 1500)
                {
                    rollValue = 1500;
                    result = BasicFunctions.roll(rollValue);
                    break;
                }
                result = BasicFunctions.roll(rollValue);
            }
            return result;
        }

        public static Boolean forward(int seconds) { 
            Boolean result = false;
            Boolean condition = false;
            Boolean armed = BasicFunctions.channelsActualValues[4] == 2000;
            double executionTime = seconds * 1000;
            int pitchValue = BasicFunctions.channelsActualValues[1];
            int throttleValue = BasicFunctions.channelsActualValues[3];
            condition = armed && throttleValue > 1000;
            Boolean actualPitchMajor = pitchValue > 1700;
            Boolean actualPitchMinor = pitchValue < 1700;

            DateTime start = DateTime.Now;
            while (condition && actualPitchMajor)
            {
                Thread.Sleep(300);
                pitchValue -= 50;
                if (pitchValue <= 1700)
                {
                    pitchValue = 1700;
                    actualPitchMajor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.pitch(pitchValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    pitchValue -= 50;
                    if (pitchValue <= 1500)
                    {
                        pitchValue = 1500;
                        result = BasicFunctions.pitch(pitchValue);
                        break;
                    }
                    result = BasicFunctions.pitch(pitchValue);
                }
                return result;
            }

            start = DateTime.Now;
            while (condition && actualPitchMinor)
            {
                Thread.Sleep(300);
                pitchValue += 50;
                if (pitchValue >= 1700)
                {
                    pitchValue = 1700;
                    actualPitchMinor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.pitch(pitchValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    pitchValue -= 50;
                    if (pitchValue <= 1500)
                    {
                        pitchValue = 1500;
                        result = BasicFunctions.pitch(pitchValue);
                        break;
                    }
                    result = BasicFunctions.pitch(pitchValue);
                }
                return result;
            }

            Thread.Sleep(Convert.ToInt32(executionTime));
            while (true)
            {
                Thread.Sleep(300);
                pitchValue -= 50;
                if (pitchValue <= 1500)
                {
                    pitchValue = 1500;
                    result = BasicFunctions.pitch(pitchValue);
                    break;
                }
                result = BasicFunctions.pitch(pitchValue);
            }
            return result;
        }

        public static Boolean backward(int seconds) {
            Boolean result = false;
            Boolean condition = false;
            Boolean armed = BasicFunctions.channelsActualValues[4] == 2000;
            double executionTime = seconds * 1000;
            int pitchValue = BasicFunctions.channelsActualValues[1];
            int throttleValue = BasicFunctions.channelsActualValues[3];
            condition = armed && throttleValue > 1000;
            Boolean actualPitchMajor = pitchValue > 1300;
            Boolean actualPitchMinor = pitchValue < 1300;

            DateTime start = DateTime.Now;
            while (condition && actualPitchMajor)
            {
                Thread.Sleep(300);
                pitchValue -= 50;
                if (pitchValue <= 1300)
                {
                    pitchValue = 1300;
                    actualPitchMajor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.pitch(pitchValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    pitchValue += 50;
                    if (pitchValue >= 1500)
                    {
                        pitchValue = 1500;
                        result = BasicFunctions.pitch(pitchValue);
                        break;
                    }
                    result = BasicFunctions.pitch(pitchValue);
                }
                return result;
            }

            start = DateTime.Now;
            while (condition && actualPitchMinor)
            {
                Thread.Sleep(300);
                pitchValue += 50;
                if (pitchValue >= 1300)
                {
                    pitchValue = 1300;
                    actualPitchMinor = false;
                    DateTime end = DateTime.Now;
                    executionTime -= (end - start).TotalMilliseconds;
                }
                result = BasicFunctions.pitch(pitchValue);
            }

            if (executionTime <= 0)
            {
                while (true)
                {
                    Thread.Sleep(300);
                    pitchValue += 50;
                    if (pitchValue >= 1500)
                    {
                        pitchValue = 1500;
                        result = BasicFunctions.pitch(pitchValue);
                        break;
                    }
                    result = BasicFunctions.pitch(pitchValue);
                }
                return result;
            }

            Thread.Sleep(Convert.ToInt32(executionTime));
            while (true)
            {
                Thread.Sleep(300);
                pitchValue += 50;
                if (pitchValue >= 1500)
                {
                    pitchValue = 1500;
                    result = BasicFunctions.pitch(pitchValue);
                    break;
                }
                result = BasicFunctions.pitch(pitchValue);
            }
            return result;
        }

        #endregion
    }
}

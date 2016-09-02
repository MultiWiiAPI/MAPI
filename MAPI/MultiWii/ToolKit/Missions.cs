using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiWii
{
    public static class Missions
    {
        #region VARIABLES

        public static volatile Boolean locked = false;
        public static System.Threading.Thread basicTurnOnThread = new System.Threading.Thread(basicmission_turnOn);
        public static System.Threading.Thread basicTurnOffThread = new System.Threading.Thread(basicmission_turnOff);
        public static System.Threading.Thread basicTakeOffThread = new System.Threading.Thread(basicmission_takeOff);
        public static System.Threading.Thread basicLandThread = new System.Threading.Thread(basicmission_land);
        public static System.Threading.Thread basicRotateRightThread = new System.Threading.Thread(basicmission_rotateRight);
        public static System.Threading.Thread basicRotateLeftThread = new System.Threading.Thread(basicmission_rotateLeft);
        public static System.Threading.Thread basicUpThread = new System.Threading.Thread(basicmission_up);
        public static System.Threading.Thread basicDownThread = new System.Threading.Thread(basicmission_down);
        public static System.Threading.Thread basicStraightRightThread = new System.Threading.Thread(basicmission_straightRight);
        public static System.Threading.Thread basicStraightLeftThread = new System.Threading.Thread(basicmission_straightLeft);
        public static System.Threading.Thread basicForwardThread = new System.Threading.Thread(basicmission_forward);
        public static System.Threading.Thread basicBackwardThread = new System.Threading.Thread(basicmission_backward);
        public static System.Threading.Thread missionDemo1Thread = new System.Threading.Thread(mission_demo1);

        #endregion

        #region BASIC MISSIONS

        public static void basicmission_turnOn()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.turnOn_turnOff(true);
                locked = false;
            }
        }

        public static void basicmission_turnOff()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.turnOn_turnOff(false);
                locked = false;
            }
        }

        public static void basicmission_takeOff()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.takeOff();
                Thread.Sleep(3000);
                locked = false;
            }
        }

        public static void basicmission_land()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.land();
                Thread.Sleep(3000);
                locked = false;
            }
        }

        public static void basicmission_rotateRight()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.rotateRight(3);
                Thread.Sleep(3000);
                locked = false;
            }
        }

        public static void basicmission_rotateLeft()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.rotateLeft(3);
                Thread.Sleep(3000);
                locked = false;
            }
        }

        public static void basicmission_up()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.up(3);
                Thread.Sleep(3000);
                locked = false;
            }
        }

        public static void basicmission_down()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.down(3);
                Thread.Sleep(3000);
                locked = false;
            }
        }

        public static void basicmission_straightRight()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.straightRight(3);
                Thread.Sleep(3000);
                locked = false;
            }
        }

        public static void basicmission_straightLeft()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.straightLeft(3);
                Thread.Sleep(3000);
                locked = false;
            }
        }

        public static void basicmission_forward()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.forward(3);
                Thread.Sleep(3000);
                locked = false;
            }
        }

        public static void basicmission_backward()
        {
            if (!locked)
            {
                locked = true;
                Boolean result = true;
                result = result && GeneralFunctions.backward(3);
                Thread.Sleep(3000);
                locked = false;
            }
        }

        #endregion

        #region MISSIONS

        public static void mission_demo1()
        {
            basicmission_turnOn();
            basicmission_takeOff();
            basicmission_rotateRight();
            basicmission_rotateLeft();
            basicmission_land();
            basicmission_turnOff();
        }

        #endregion

        #region ABORT MISSIONS

        public static void Abort()
        {
            locked = true;
            basicTurnOnThread.Abort();
            basicTurnOffThread.Abort();
            basicTakeOffThread.Abort();
            basicLandThread.Abort();
            basicRotateRightThread.Abort();
            basicRotateLeftThread.Abort();
            basicUpThread.Abort();
            basicDownThread.Abort();
            basicStraightRightThread.Abort();
            basicStraightLeftThread.Abort();
            basicForwardThread.Abort();
            basicBackwardThread.Abort();
            BasicFunctions.disarm();
            BasicFunctions.arm();
            basicTurnOnThread = new System.Threading.Thread(basicmission_turnOn);
            basicTurnOffThread = new System.Threading.Thread(basicmission_turnOff);
            basicTakeOffThread = new System.Threading.Thread(basicmission_takeOff);
            basicLandThread = new System.Threading.Thread(basicmission_land);
            basicRotateRightThread = new System.Threading.Thread(basicmission_rotateRight);
            basicRotateLeftThread = new System.Threading.Thread(basicmission_rotateLeft);
            basicUpThread = new System.Threading.Thread(basicmission_up);
            basicDownThread = new System.Threading.Thread(basicmission_down);
            basicStraightRightThread = new System.Threading.Thread(basicmission_straightRight);
            basicStraightLeftThread = new System.Threading.Thread(basicmission_straightLeft);
            basicForwardThread = new System.Threading.Thread(basicmission_forward);
            basicBackwardThread = new System.Threading.Thread(basicmission_backward);
            missionDemo1Thread = new System.Threading.Thread(mission_demo1);
            locked = false;
        }

        #endregion

    }
}

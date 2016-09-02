using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using SlimDX;
using SlimDX.DirectInput;
using System.Management;
using System.IO;
using System.Media;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Windows.Forms.DataVisualization.Charting;

namespace MultiWii
{
    public partial class Form1 : Form
    {

        #region VARIABLES

        private SerialPort serialPort = new SerialPort();
        public Boolean armed = false, joystickEnable = false, activateControl = false;


        int speed_Counter;
        int turn_Counter;
        int straight_Counter;
        int head_Counter;

        int joy1min, joy1max, joy2min, joy2max, joy3min, joy3max, joy4min, joy4max = 0;
        int upCounter, downCounter, leftCounter, rightCounter = 0;
        Boolean upRecognized, downRecognized, leftRecognized, rightRecognized = false;
        Boolean enableImage;
        Boolean basicCamera;
        SoundPlayer snapshotSound = new SoundPlayer("..\\..\\camara_5.wav");
        // SoundPlayer errorSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
        //SoundPlayer rightSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
        //SoundPlayer encounteredSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
        Point rectStartPoint;
        Rectangle rect = new Rectangle();
        Brush selectionBrush = new SolidBrush(Color.FromArgb(128, 72, 145, 220));

        Capture cap;
        Dictionary<String, Image<Bgr, Byte>> templateMap;
        Double threshold;
        Boolean timeLapse = false;
        Bgr colorSelected = new Bgr(0, 0, 0);

        int plotCounter;

        #endregion

        #region INITIALIZATE FORM

        public Form1()
        {
            InitializeComponent();
            richTextBoxDataSend.AppendText("**************************************************\n Welcome to the User Interface for MAPI. **\n**************************************************\n\n");
            richTextBoxDataSendAutonomous.AppendText("**************************************************\n Welcome to the User Interface for MAPI. \n**************************************************\n\n");
            richTextBox1.AppendText("**************************************************\n Welcome to the User Interface for MAPI. \n**************************************************\n\n");
            Console.WriteLine(serialPortComboBox.Text);
            updatePortComboBox();
            enableImage = false;
            basicCamera = true;
            templateMap = new Dictionary<String, Image<Bgr, Byte>>();
        }

        #endregion

        #region PORT CONNECTION FUNCTIONS

        private void serialPortComboBox_Click(object sender, EventArgs e)
        {
            updatePortComboBox();
        }

        public void updatePortComboBox()
        {
            string[] arrayComPortsNames = null;
            arrayComPortsNames = SerialPort.GetPortNames().ToArray();

            for (int y = 0; y < arrayComPortsNames.Length; y++)
            {
                serialPortComboBox.Items.Clear();
                foreach (string s in arrayComPortsNames)
                {
                    if (s.Count() > 0)
                    {
                        serialPortComboBox.Items.Add(s);
                    }
                }
                if (serialPortComboBox.Items.Count > 0)
                {

                    serialPortComboBox.Text = arrayComPortsNames[0];
                }
                else
                {
                    serialPortComboBox.Items.Add("No COM Ports avaliable");
                    serialPortComboBox.Text = "No COM Ports avaliable";
                }
            }
        }

        private void openPortButton_Click(object sender, EventArgs e)
        {
            if (serialPortComboBox.Text != "No COM Ports avaliable" && serialPortComboBox.Text != "")
            {
                if (serialPort.IsOpen) serialPort.Close();
                richTextBoxDataSend.AppendText(serialPortComboBox.Text + " opened successfully.\n");
                richTextBox1.AppendText(serialPortComboBox.Text + " opened successfully.\n");
                richTextBoxDataSendAutonomous.AppendText(serialPortComboBox.Text + " opened successfully.\n");
                Console.WriteLine(serialPortComboBox.Text);
                CoreFunctions.initializeCore(serialPortComboBox.Text);
                serialPort = CoreFunctions.serialPort;
            }
            else
            {
                richTextBoxDataSend.Text = "No COM Ports selected. Nothing to do.";
                richTextBox1.Text = "No COM Ports selected. Nothing to do.";
                richTextBoxDataSendAutonomous.Text = "No COM Ports selected. Nothing to do.";
                Console.WriteLine("No COM Ports selected. Nothing to do.");
            }
        }

        #endregion

        #region CHART

        private void chart1_Click(object sender, EventArgs e)
        {
            if (timerChartPlot.Enabled == true)
            {
                timerChartPlot.Enabled = false;
            }
            else
            {
                DialogResult alert = MessageBox.Show("Arm is disabled.",
                                                    "Arm is disabled.",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Warning
                                                    );
            }

        }

        private void timerChartPlot_Tick(object sender, EventArgs e)
        {
            int[] gyroscopeValues = BasicFunctions.getGyroscopeValues();
            if (tabControl1.SelectedTab == tabPage1)
            {
                sensorsChart.Series[0].Points.AddXY(plotCounter, gyroscopeValues[0]);
                sensorsChart.Series[1].Points.AddXY(plotCounter, gyroscopeValues[1]);
                sensorsChart.Series[2].Points.AddXY(plotCounter, gyroscopeValues[2]);

                if (plotCounter != 0)
                {
                    channelsChart.Series[0].Points.RemoveAt(0);
                    channelsChart.Series[1].Points.RemoveAt(0);
                    channelsChart.Series[2].Points.RemoveAt(0);
                    channelsChart.Series[3].Points.RemoveAt(0);
                }
                channelsChart.Series[0].Points.AddXY(1, BasicFunctions.channelsActualValues[0]);
                channelsChart.Series[1].Points.AddXY(2, BasicFunctions.channelsActualValues[1]);
                channelsChart.Series[2].Points.AddXY(3, BasicFunctions.channelsActualValues[2]);
                channelsChart.Series[3].Points.AddXY(4, BasicFunctions.channelsActualValues[3]);
            }
            else
            {

                sensorsChartAutonomous.Series[0].Points.AddXY(plotCounter, gyroscopeValues[0]);
                sensorsChartAutonomous.Series[1].Points.AddXY(plotCounter, gyroscopeValues[1]);
                sensorsChartAutonomous.Series[2].Points.AddXY(plotCounter, gyroscopeValues[2]);

                if (plotCounter != 0)
                {
                    channelsChartAutonomous.Series[0].Points.RemoveAt(0);
                    channelsChartAutonomous.Series[1].Points.RemoveAt(0);
                    channelsChartAutonomous.Series[2].Points.RemoveAt(0);
                    channelsChartAutonomous.Series[3].Points.RemoveAt(0);
                }
                channelsChartAutonomous.Series[0].Points.AddXY(1, BasicFunctions.channelsActualValues[0]);
                channelsChartAutonomous.Series[1].Points.AddXY(2, BasicFunctions.channelsActualValues[1]);
                channelsChartAutonomous.Series[2].Points.AddXY(3, BasicFunctions.channelsActualValues[2]);
                channelsChartAutonomous.Series[3].Points.AddXY(4, BasicFunctions.channelsActualValues[3]);
            }

            plotCounter += 1;


        }

        private void buttonGraphics_Click(object sender, EventArgs e)
        {
            if (buttonGraphics.Text == "Plots" && tabControl1.SelectedTab == tabPage1)
            {

                if (BasicFunctions.channelsActualValues.ElementAt(4) == 2000)
                {
                    sensorsChart.Series[0].Points.Clear();
                    sensorsChart.Series[1].Points.Clear();
                    sensorsChart.Series[2].Points.Clear();
                    channelsChart.Series[0].Points.Clear();
                    channelsChart.Series[1].Points.Clear();
                    channelsChart.Series[2].Points.Clear();
                    channelsChart.Series[3].Points.Clear();
                    plotCounter = 0;
                    timerChartPlot.Enabled = true;
                    richTextBoxDataSend.AppendText("Plots started.\nPlot 1: Accelerometer values.\nPlot 2: Channels Values.\n");
                    buttonGraphics.Text = "Stop Plots";
                }
            }
            else
            {
                if (buttonGraphics.Text == "Stop Plots" && tabControl1.SelectedTab == tabPage1)
                {
                    sensorsChart.Series[0].Points.Clear();
                    sensorsChart.Series[1].Points.Clear();
                    sensorsChart.Series[2].Points.Clear();
                    channelsChart.Series[0].Points.Clear();
                    channelsChart.Series[1].Points.Clear();
                    channelsChart.Series[2].Points.Clear();
                    channelsChart.Series[3].Points.Clear();
                    plotCounter = 0;
                    timerChartPlot.Enabled = false;
                    richTextBoxDataSend.AppendText("Plots stopped.\n");
                    buttonGraphics.Text = "Plots";
                }
            }


            if (buttonGraphicsAutonomous.Text == "Plots" && tabControl1.SelectedTab == tabPage2)
            {
                if (BasicFunctions.channelsActualValues.ElementAt(4) == 2000)
                {
                    sensorsChartAutonomous.Series[0].Points.Clear();
                    sensorsChartAutonomous.Series[1].Points.Clear();
                    sensorsChartAutonomous.Series[2].Points.Clear();
                    channelsChartAutonomous.Series[0].Points.Clear();
                    channelsChartAutonomous.Series[1].Points.Clear();
                    channelsChartAutonomous.Series[2].Points.Clear();
                    channelsChartAutonomous.Series[3].Points.Clear();
                    plotCounter = 0;
                    timerChartPlot.Enabled = true;
                    richTextBoxDataSendAutonomous.AppendText("Plots started.\nPlot 1: Accelerometer values.\nPlot 2: Channels Values.\n");
                    buttonGraphicsAutonomous.Text = "Stop Plots";
                }
            }
            else
            {
                if (buttonGraphicsAutonomous.Text == "Stop Plots" && tabControl1.SelectedTab == tabPage2)
                {
                    sensorsChartAutonomous.Series[0].Points.Clear();
                    sensorsChartAutonomous.Series[1].Points.Clear();
                    sensorsChartAutonomous.Series[2].Points.Clear();
                    channelsChartAutonomous.Series[0].Points.Clear();
                    channelsChartAutonomous.Series[1].Points.Clear();
                    channelsChartAutonomous.Series[2].Points.Clear();
                    channelsChartAutonomous.Series[3].Points.Clear();
                    plotCounter = 0;
                    timerChartPlot.Enabled = false;
                    richTextBoxDataSendAutonomous.AppendText("Plots stopped.\n");
                    buttonGraphicsAutonomous.Text = "Plots";
                }
            }

        }

        #endregion

        #region ARM FUNCTIONS

        private void arm_Click(object sender, EventArgs e)
        {
            Boolean result = BasicFunctions.arm();
            if (result)
            {
                richTextBoxDataSend.AppendText("Drone armed. \n");
                armed = true;
                disarm.Enabled = true;
                speed_up.Enabled = true;
                slow_down.Enabled = false;

                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                createDeviceButton.Enabled = true;
                EnableManualControl.Enabled = true;

                speed_Counter = 0;
                turn_Counter = 2;
                straight_Counter = 2;
                head_Counter = 2;
            }

        }

        private void disarm_Click(object sender, EventArgs e)
        {
            Boolean result = BasicFunctions.disarm();
            if (result)
            {
                richTextBoxDataSend.AppendText("Drone disarmed. \n");
                armed = false;
                disarm.Enabled = false;
                speed_up.Enabled = false;
                slow_down.Enabled = false;
                straight_left.Enabled = false;
                straight_right.Enabled = false;
                right.Enabled = false;
                left.Enabled = false;
                head_up.Enabled = false;
                head_down.Enabled = false;
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                createDeviceButton.Enabled = false;
                EnableManualControl.Enabled = false;

                timerChartPlot.Enabled = false;
            }
        }

        #endregion

        #region MANUAL CONTROL FUNCTIONS

        public void temporizador_Tick(object sender, EventArgs e)
        {
            JoystickFunctions.readFromJoystick();
            UpdateManualControlUI();
        }

        public void UpdateManualControlUI()
        {
            if (JoystickFunctions.joystick == null)
            {
                createDeviceButton.Text = "Connect";
            }
            else
            {
                createDeviceButton.Text = "Disconnect";

            }
            if (joy1min == 0 || joy1max == 0 || joy2min == 0 || joy2max == 0 || joy3min == 0 || joy3max == 0 || joy4min == 0 || joy4max == 0)
            {
                //Stick 2 eje x
                string pb1 = JoystickFunctions.getCurrentState().X.ToString();
                progressBar1.Value = Convert.ToInt32(pb1);
                label_1X.Text = pb1;
                //Stick 2 eje y
                string pb2 = JoystickFunctions.getCurrentState().Y.ToString();
                progressBar2.Value = Convert.ToInt32(pb2);
                label_1Y.Text = pb2;
                //Stick 1 eje x
                string pb3 = JoystickFunctions.getCurrentState().RotationY.ToString();
                progressBar3.Value = Convert.ToInt32(pb3);
                label_2X.Text = pb3;
                //Stick 1 eje y
                string pb4 = JoystickFunctions.getCurrentState().Z.ToString();
                progressBar4.Value = Convert.ToInt32(pb4);
                label_2Y.Text = pb4;
            }
            else
            {
                //Stick 2 eje x
                string pb1 = AuxiliarFunctions.normalize(joy4min, joy4max, JoystickFunctions.getCurrentState().X).ToString();
                progressBar1.Value = Convert.ToInt32(pb1);
                progressBar1.Minimum = 1000;
                label_1X.Text = pb1;
                //Stick 2 eje y
                string pb2 = AuxiliarFunctions.normalize(joy3min, joy3max, JoystickFunctions.getCurrentState().Y).ToString();
                progressBar2.Value = Convert.ToInt32(pb2);
                progressBar2.Minimum = 1000;
                label_1Y.Text = pb2;
                //Stick 1 eje x
                string pb3 = AuxiliarFunctions.normalize(joy2min, joy2max, JoystickFunctions.getCurrentState().RotationY).ToString();
                progressBar3.Value = Convert.ToInt32(pb3);
                progressBar3.Minimum = 1000;
                label_2X.Text = pb3;
                //Stick 1 eje y
                string pb4 = AuxiliarFunctions.normalize(joy1min, joy1max, JoystickFunctions.getCurrentState().Z).ToString();
                progressBar4.Value = Convert.ToInt32(pb4);
                progressBar4.Minimum = 1000;
                label_2Y.Text = pb4;

                if (activateControl)
                {

                    int[] motors = { Convert.ToInt32(pb1), Convert.ToInt32(pb2), Convert.ToInt32(pb3), Convert.ToInt32(pb4) };
                    if (motors[3] > 1900)
                    {
                        motors[3] = 1900;
                    }
                    MultiWiiFunctions.mspSetRawRC(motors);
                }
            }


        }

        private void createDeviceButton_Click(object sender, EventArgs e)
        {
            string pathFile = "..\\..\\joystickCalibrate.conf";
            if (File.Exists(pathFile))
            {
                richTextBoxDataSend.AppendText("Joystick connected.\n");
                String calibration = File.ReadLines(pathFile).First();
                String[] calibrationValues = calibration.Split('-');
                joy1min = Convert.ToInt32(calibrationValues[0]);
                joy1max = Convert.ToInt32(calibrationValues[1]);
                joy2min = Convert.ToInt32(calibrationValues[2]);
                joy2max = Convert.ToInt32(calibrationValues[3]);
                joy3min = Convert.ToInt32(calibrationValues[4]);
                joy3max = Convert.ToInt32(calibrationValues[5]);
                joy4min = Convert.ToInt32(calibrationValues[6]);
                joy4max = Convert.ToInt32(calibrationValues[7]);

                richTextBoxDataSend.AppendText("Calibration found, " + joy1min + "-" + joy1max + "-" + joy2min + "-" + joy2max + "-" + joy3min + "-" + joy3max + "-" + joy4min + "-" + joy4max + ".\n");
            }
            else
            {
                richTextBoxDataSend.AppendText("Calibration not found, please calibrate the device.");
            }

            Console.WriteLine(joy1min + "-" + joy1max + "-" + joy2min + "-" + joy2max + "-" + joy3min + "-" + joy3max + "-" + joy4min + "-" + joy4max);

            if (JoystickFunctions.joystick == null)
            {
                CalibrateButton.Enabled = true;
                temporizador.Interval = 1000 / 12;
                temporizador.Start();

                JoystickFunctions.InitializeJoystick();

                progressBar1.Enabled = true;
                label_1X.Enabled = true;
                progressBar2.Enabled = true;
                label_1Y.Enabled = true;
                progressBar3.Enabled = true;
                label_2X.Enabled = true;
                progressBar4.Enabled = true;
                label_2Y.Enabled = true;

            }
            else
            {

                temporizador.Stop();

                JoystickFunctions.disconnectJoystick();
                richTextBoxDataSend.AppendText("Joystick disconnected.\n");
                CalibrateButton.Enabled = false;
                joystickEnable = false;
                progressBar1.Enabled = false;
                label_1X.Enabled = false;
                progressBar2.Enabled = false;
                label_1Y.Enabled = false;
                progressBar3.Enabled = false;
                label_2X.Enabled = false;
                progressBar4.Enabled = false;
                label_2Y.Enabled = false;

            }
            UpdateManualControlUI();
        }

        private void CalibrateButton_Click(object sender, EventArgs e)
        {

            richTextBoxDataSend.AppendText("Calibrating Joystick...\n");

            DialogResult result1 = MessageBox.Show("Stick 1\nVertical\nMin", "Calibrate Channel 1", MessageBoxButtons.OKCancel);
            if (result1 == DialogResult.OK)
            {
                joy1min = JoystickFunctions.getCurrentState().Z;
                DialogResult result2 = MessageBox.Show("Stick 1\nVertical\nMax", "Calibrate Channel 1", MessageBoxButtons.OKCancel);

                if (result2 == DialogResult.OK)
                {
                    joy1max = JoystickFunctions.getCurrentState().Z;
                    DialogResult result3 = MessageBox.Show("Stick 1\nHorizontal\nMin", "Calibrate Channel 2", MessageBoxButtons.OKCancel);
                    if (result3 == DialogResult.OK)
                    {
                        joy2min = JoystickFunctions.getCurrentState().RotationY;
                        DialogResult result4 = MessageBox.Show("Stick 1\nHorizontal\nMax", "Calibrate Channel 2", MessageBoxButtons.OKCancel);
                        if (result4 == DialogResult.OK)
                        {
                            joy2max = JoystickFunctions.getCurrentState().RotationY;
                            DialogResult result5 = MessageBox.Show("Stick 2\nVertical\nMin", "Calibrate Channel 3", MessageBoxButtons.OKCancel);
                            if (result5 == DialogResult.OK)
                            {
                                joy3min = JoystickFunctions.getCurrentState().Y;
                                DialogResult result6 = MessageBox.Show("Stick 2\nVertical\nMax", "Calibrate Channel 3", MessageBoxButtons.OKCancel);
                                if (result6 == DialogResult.OK)
                                {
                                    joy3max = JoystickFunctions.getCurrentState().Y;
                                    DialogResult result7 = MessageBox.Show("Stick 2\nHorizontal\nMin", "Calibrate Channel 4", MessageBoxButtons.OKCancel);
                                    if (result7 == DialogResult.OK)
                                    {
                                        joy4min = JoystickFunctions.getCurrentState().X;
                                        DialogResult result8 = MessageBox.Show("Stick 2\nHorizontal\nMax", "Calibrate Channel 4", MessageBoxButtons.OKCancel);
                                        if (result8 == DialogResult.OK)
                                        {
                                            joy4max = JoystickFunctions.getCurrentState().X;
                                            Console.WriteLine(joy1min + "-" + joy1max + "-" + joy2min + "-" + joy2max + "-" + joy3min + "-" + joy3max + "-" + joy4min + "-" + joy4max);
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter("..\\..\\joystickCalibrate.conf"))
                                            {
                                                file.WriteLine(joy1min + "-" + joy1max + "-" + joy2min + "-" + joy2max + "-" + joy3min + "-" + joy3max + "-" + joy4min + "-" + joy4max);
                                            }

                                        }
                                    }
                                }
                            }
                        }

                    }


                }

            }
            richTextBoxDataSend.AppendText("Joystick calibrated.\n");
        }

        private void enableManualControl_CheckedChanged(object sender, EventArgs e)
        {
            activateControl = !activateControl;
            if (activateControl)
            {
                richTextBoxDataSend.AppendText("Manual control enabled.\n");
            }
            else
            {
                richTextBoxDataSend.AppendText("Manual control disabled.n");
            }
        }

        private void manual_control_CheckedChanged(object sender, EventArgs e)
        {
            if (manual_control.Checked == true)
            {
                richTextBoxDataSend.AppendText("Manual control by joystick activated.\n");
                speed_up.Visible = false;
                slow_down.Visible = false;
                straight_left.Visible = false;
                straight_right.Visible = false;
                right.Visible = false;
                left.Visible = false;
                head_up.Visible = false;
                head_down.Visible = false;

                groupBox1.Visible = true;
                groupBox2.Visible = true;
                createDeviceButton.Visible = true;
                CalibrateButton.Visible = true;
                EnableManualControl.Visible = true;

            }
            else
            {
                richTextBoxDataSend.AppendText("Manual control by mouse activated.\n");
                speed_up.Visible = true;
                slow_down.Visible = true;
                straight_left.Visible = true;
                straight_right.Visible = true;
                right.Visible = true;
                left.Visible = true;
                head_up.Visible = true;
                head_down.Visible = true;

                groupBox1.Visible = false;
                groupBox2.Visible = false;
                createDeviceButton.Visible = false;
                CalibrateButton.Visible = false;
                EnableManualControl.Visible = false;
            }

        }

        #endregion

        #region UI CONTROL FUNCTIONS

        private void speed_up_Click(object sender, EventArgs e)
        {
            if (armed == true && speed_Counter < 4)
            {
                slow_down.Enabled = true;
                straight_left.Enabled = true;
                straight_right.Enabled = true;
                right.Enabled = true;
                left.Enabled = true;
                head_up.Enabled = true;
                head_down.Enabled = true;
                speed_Counter += 1;
                int data = AuxiliarFunctions.speedController(speed_Counter);
                BasicFunctions.throttle(data);
                richTextBoxDataSend.AppendText("Speed up done, actual: " + speed_Counter + " maximum: 3.\n");
            }
            else
            {
                speed_up.Enabled = false;
            }
        }

        private void slow_down_Click(object sender, EventArgs e)
        {
            speed_up.Enabled = true;
            if (armed == true && speed_Counter >= 1)
            {
                speed_Counter -= 1;
                int data = AuxiliarFunctions.speedController(speed_Counter);
                BasicFunctions.throttle(data);
                richTextBoxDataSend.AppendText("Speed down done, actual: " + speed_Counter + " minimum: 1.\n");
                if (speed_Counter == 0)
                {
                    turn_Counter = 2;
                    straight_Counter = 2;
                    head_Counter = 2;
                    slow_down.Enabled = false;
                    straight_left.Enabled = false;
                    straight_right.Enabled = false;
                    right.Enabled = false;
                    left.Enabled = false;
                    head_up.Enabled = false;
                    head_down.Enabled = false;
                }
            }
        }

        private void straight_right_Click(object sender, EventArgs e)
        {
            straight_left.Enabled = true;
            if (armed == true && speed_Counter > 0 && straight_Counter > 0)
            {
                straight_Counter -= 1;
                int data = AuxiliarFunctions.moveController(straight_Counter);
                BasicFunctions.roll(data);
                richTextBoxDataSend.AppendText("Straight right done, actual: " + straight_Counter + " minimum: 1.\n");
                if (straight_Counter == 0)
                {
                    straight_right.Enabled = false;
                }
            }
        }

        private void straight_left_Click(object sender, EventArgs e)
        {
            straight_right.Enabled = true;
            if (armed == true && speed_Counter > 0 && straight_Counter < 4)
            {
                straight_Counter += 1;
                int data = AuxiliarFunctions.moveController(straight_Counter);
                BasicFunctions.roll(data);
                richTextBoxDataSend.AppendText("Straight left done, actual: " + straight_Counter + " maximum: 3.\n");
                if (straight_Counter == 4)
                {
                    straight_left.Enabled = false;
                }
            }
        }

        private void head_up_Click(object sender, EventArgs e)
        {
            head_down.Enabled = true;
            if (armed == true && speed_Counter > 0 && head_Counter > 0)
            {
                head_Counter -= 1;
                int data = AuxiliarFunctions.moveController(head_Counter);
                BasicFunctions.pitch(data);
                richTextBoxDataSend.AppendText("Head up done, actual: " + head_Counter + " minimum: 1.\n");
                if (head_Counter == 0)
                {
                    head_up.Enabled = false;
                }
            }
        }

        private void head_down_Click(object sender, EventArgs e)
        {
            head_up.Enabled = true;
            if (armed == true && speed_Counter > 0 && head_Counter < 4)
            {
                head_Counter += 1;
                int data = AuxiliarFunctions.moveController(head_Counter);
                BasicFunctions.pitch(data);
                richTextBoxDataSend.AppendText("Head down, actual: " + head_Counter + " maximum: 3.\n");
                if (head_Counter == 4)
                {
                    head_down.Enabled = false;
                }
            }
        }

        private void right_Click(object sender, EventArgs e)
        {
            left.Enabled = true;
            if (armed == true && speed_Counter > 0 && turn_Counter > 0)
            {
                turn_Counter -= 1;
                int data = AuxiliarFunctions.moveController(turn_Counter);
                BasicFunctions.yaw(data);
                richTextBoxDataSend.AppendText("Turn right done, actual: " + turn_Counter + " minimum: 1.\n");
                if (turn_Counter == 0)
                {
                    right.Enabled = false;
                }
            }
        }

        private void left_Click(object sender, EventArgs e)
        {
            right.Enabled = true;
            if (armed == true && speed_Counter > 0 && turn_Counter < 4)
            {
                turn_Counter += 1;
                int data = AuxiliarFunctions.moveController(turn_Counter);
                BasicFunctions.yaw(data);
                richTextBoxDataSend.AppendText("Turn left done, actual: " + turn_Counter + " maximum: 1.\n");
                if (turn_Counter == 4)
                {
                    left.Enabled = false;
                }
            }
        }

        #endregion

        #region INFORMATION FUNCTIONS

        private void buttonRcChannels_Click(object sender, EventArgs e)
        {
            int[] result = BasicFunctions.getRCChannels();
            richTextBoxDataSend.AppendText("Channels Values (roll/pitch/yaw/throttle/aux1/aux2/aux3/aux4): " + String.Join(",", result.Select(p => p.ToString()).ToArray()) + ".\n");
            richTextBoxDataSendAutonomous.AppendText("Channels Values (roll/pitch/yaw/throttle/aux1/aux2/aux3/aux4): " + String.Join(",", result.Select(p => p.ToString()).ToArray()) + ".\n");
        }

        private void buttonVersion_Click(object sender, EventArgs e)
        {
            String result = BasicFunctions.getMultiWiiVersion();
            for (int i = 0; i < 4; i++)
            {
                if (result == "0")
                {
                    result = BasicFunctions.getMultiWiiVersion();
                }
                else
                {
                    break;
                }
            }
            richTextBoxDataSend.AppendText("Version of Multiwii: " + result + ".");
            richTextBoxDataSendAutonomous.AppendText("Version of Multiwii: " + result + ".");
            richTextBox1.AppendText("Version of Multiwii: " + result + ".");
        }

        private void buttonSensorsState_Click(object sender, EventArgs e)
        {
            int[] result = BasicFunctions.getInertialMeasureUnitValues();
            richTextBoxDataSend.AppendText("Sensors State (accelerometer x/y/z, gyroscope x/y/z, magnetometer x/y/z): " + String.Join(",", result.Select(p => p.ToString()).ToArray()) + "./n");
            richTextBoxDataSendAutonomous.AppendText("Sensors State (accelerometer x/y/z, gyroscope x/y/z, magnetometer x/y/z): " + String.Join(",", result.Select(p => p.ToString()).ToArray()) + "./n");
        }

        #endregion

        #region MISSION FUNCTIONS

        private void buttonStartMission_Click(object sender, EventArgs e)
        {
            if (!Missions.locked)
            {
                richTextBoxDataSendAutonomous.AppendText("Mission Demo 1 started.\n");
                Missions.missionDemo1Thread.Abort();
                Missions.missionDemo1Thread = new System.Threading.Thread(Missions.mission_demo1);
                Missions.missionDemo1Thread.Start();
            }
            else
            {
                richTextBoxDataSendAutonomous.AppendText("Couldn't start Mission Demo 1, its locked. Please abort actual mission or wait a little.\n");
            }
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            Missions.Abort();
            richTextBoxDataSendAutonomous.AppendText("Missions active aborted successfully.\n");
            buttonStartBasicMission.Enabled = false;
            buttonLand.Enabled = false;
            buttonTakeOff.Enabled = true;
            buttonStartMission.Enabled = true;
        }

        private void buttonTakeOff_Click(object sender, EventArgs e)
        {
            if (!Missions.locked)
            {
                richTextBoxDataSendAutonomous.AppendText("Basic mission Take Off started.\n");
                Missions.basicTakeOffThread.Abort();
                Missions.basicTakeOffThread = new System.Threading.Thread(Missions.basicmission_takeOff);
                Missions.basicTakeOffThread.Start();
                buttonStartBasicMission.Enabled = true;
                buttonLand.Enabled = true;
                buttonTakeOff.Enabled = false;
            }
            else
            {

                richTextBoxDataSendAutonomous.AppendText("Couldn't start basic mission, its locked. Please abort actual mission or wait a little.\n");
            }
        }

        private void buttonLand_Click(object sender, EventArgs e)
        {
            if (!Missions.locked)
            {
                richTextBoxDataSendAutonomous.AppendText("Basic mission Land started.\n");
                Missions.basicLandThread.Abort();
                Missions.basicLandThread = new System.Threading.Thread(Missions.basicmission_land);
                Missions.basicLandThread.Start();
                buttonStartBasicMission.Enabled = false;
                buttonLand.Enabled = false;
                buttonTakeOff.Enabled = true;
            }
            else
            {

                richTextBoxDataSendAutonomous.AppendText("Couldn't start basic mission, its locked. Please abort actual mission or wait a little.\n");
            }
        }

        private void buttonStartBasicMission_Click(object sender, EventArgs e)
        {
            if (!Missions.locked)
            {
                richTextBoxDataSendAutonomous.AppendText("Basic mission selected started.\n");
                if (radioButtonUp.Checked)
                {
                    richTextBoxDataSendAutonomous.AppendText("Basic Mission: up.\n");
                    Missions.basicUpThread.Abort();
                    Missions.basicUpThread = new System.Threading.Thread(Missions.basicmission_up);
                    Missions.basicUpThread.Start();
                }
                if (radioButtonDown.Checked)
                {
                    richTextBoxDataSendAutonomous.AppendText("Basic Mission: down.\n");
                    Missions.basicDownThread.Abort();
                    Missions.basicDownThread = new System.Threading.Thread(Missions.basicmission_down);
                    Missions.basicDownThread.Start();
                }
                if (radioButtonRotateLeft.Checked)
                {
                    richTextBoxDataSendAutonomous.AppendText("Basic Mission: rotate left.\n");
                    Missions.basicRotateLeftThread.Abort();
                    Missions.basicRotateLeftThread = new System.Threading.Thread(Missions.basicmission_rotateLeft);
                    Missions.basicRotateLeftThread.Start();
                }
                if (radioButtonRotateRight.Checked)
                {
                    richTextBoxDataSendAutonomous.AppendText("Basic Mission: rotate right.\n");
                    Missions.basicRotateRightThread.Abort();
                    Missions.basicRotateRightThread = new System.Threading.Thread(Missions.basicmission_rotateRight);
                    Missions.basicRotateRightThread.Start();
                }
                if (radioButtonStraightLeft.Checked)
                {
                    richTextBoxDataSendAutonomous.AppendText("Basic Mission: straight left.\n");
                    Missions.basicStraightLeftThread.Abort();
                    Missions.basicStraightLeftThread = new System.Threading.Thread(Missions.basicmission_straightLeft);
                    Missions.basicStraightLeftThread.Start();
                }
                if (radioButtonStraightRight.Checked)
                {
                    richTextBoxDataSendAutonomous.AppendText("Basic Mission: straight right.\n");
                    Missions.basicStraightRightThread.Abort();
                    Missions.basicStraightRightThread = new System.Threading.Thread(Missions.basicmission_straightRight);
                    Missions.basicStraightRightThread.Start();
                }
                if (radioButtonForward.Checked)
                {
                    richTextBoxDataSendAutonomous.AppendText("Basic Mission: forward.\n");
                    Missions.basicForwardThread.Abort();
                    Missions.basicForwardThread = new System.Threading.Thread(Missions.basicmission_forward);
                    Missions.basicForwardThread.Start();
                }
                if (radioButtonBackward.Checked)
                {
                    richTextBoxDataSendAutonomous.AppendText("Basic Mission: backward.\n");
                    Missions.basicBackwardThread.Abort();
                    Missions.basicBackwardThread = new System.Threading.Thread(Missions.basicmission_backward);
                    Missions.basicBackwardThread.Start();
                }

            }
            else
            {

                richTextBoxDataSendAutonomous.AppendText("Couldn't start basic mission, its locked. Please abort actual mission or wait a little.\n");
            }
        }

        private void buttonOn_Click(object sender, EventArgs e)
        {
            if (buttonOn.Text == "Turn On")
            {
                richTextBoxDataSendAutonomous.AppendText("Turning On the drone.\n");
                Missions.basicTurnOnThread.Abort();
                Missions.basicTurnOnThread = new System.Threading.Thread(Missions.basicmission_turnOn);
                Missions.basicTurnOnThread.Start();
                buttonOn.Text = "Turn Off";
                buttonStartBasicMission.Enabled = false;
                buttonLand.Enabled = false;
                buttonTakeOff.Enabled = true;
                buttonStartMission.Enabled = true;
                richTextBoxDataSendAutonomous.AppendText("Drone is turned on.\n");
            }
            else
            {
                richTextBoxDataSendAutonomous.AppendText("Turning Off the drone.\n");
                Missions.basicTurnOffThread.Abort();
                Missions.basicTurnOffThread = new System.Threading.Thread(Missions.basicmission_turnOff);
                Missions.basicTurnOffThread.Start();
                buttonOn.Text = "Turn On";
                buttonStartBasicMission.Enabled = false;
                buttonLand.Enabled = false;
                buttonTakeOff.Enabled = false;
                buttonStartMission.Enabled = false;
                richTextBoxDataSendAutonomous.AppendText("Drone is turned off.\n");
                timerChartPlot.Enabled = false;
            }
        }

        #endregion

        #region IMAGE FUNCTIONS

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedIndex == 3)
            {
                buttonAbortImage.Visible = false;
                buttonAbortImage.Enabled = false;
                buttonChangeCamera.Visible = true;
                buttonChangeCamera.Enabled = true;
                cap = new Capture(0);
                List<Image<Bgr, byte>> templateList = new List<Image<Bgr, byte>>();
                templateList = AuxiliarFunctions.searchTemplates("..\\..\\images", "template_", "png", 10);

                if (templateList.Count <= 0)
                {
                    String path = Path.GetFullPath("..\\..\\images");
                    MessageBox.Show("Templates not found in the next path: " + path + ". Please, create the templates for image recognition.", "Image Recognition Information", MessageBoxButtons.OK);
                }
                else
                {
                    String path = Path.GetFullPath("..\\..\\images");
                    DialogResult templateNotification = MessageBox.Show("Templates found in the next path: " + path + ". Want to use these templates?", "Image Recognition Information", MessageBoxButtons.YesNo);
                    if (templateNotification == DialogResult.No)
                    {
                        templateList.Clear();
                    }
                    if (templateNotification == DialogResult.Yes)
                    {
                        if (templateList.Count == 4)
                        {
                            templateMap.Add("up", templateList[0]);
                            templateMap.Add("down", templateList[1]);
                            templateMap.Add("right", templateList[2]);
                            templateMap.Add("left", templateList[3]);
                        }
                        buttonStart.Enabled = true;
                    }
                    Console.WriteLine("Number of templates: " + templateMap.Keys.Count);
                }
                timerVideo.Enabled = true;
            }
            else
            {
                if (cap != null)
                {
                    cap.Dispose();
                }
                timerVideo.Enabled = false;
                timerMissionRecognized.Enabled = false;
                templateMap.Clear();


            }
            timerChartPlot.Enabled = false;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (checkBoxColorFilter.Checked && colorSelected.Equals(new Bgr(0, 0, 0)))
            {
                MessageBox.Show("Color not selected, please make double click on the capture to select a color.", "Color filter parameter not initializated", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (!enableImage)
                {
                    Console.WriteLine("templates: " + templateMap.Count);
                    timerMissionRecognized.Enabled = true;
                    buttonAbortImage.Visible = true;
                    buttonAbortImage.Enabled = true;
                    buttonChangeCamera.Visible = false;
                    buttonChangeCamera.Enabled = false;
                    ImageFunctions.initializeImageRecognition(templateMap, checkBoxSaltAndPepper.Checked, checkBoxGaussian.Checked, checkBoxBlur.Checked, checkBoxSobel.Checked, checkBoxCanny.Checked, checkBoxColorFilter.Checked, colorSelected);
                    BasicFunctions.arm();
                    buttonStart.Text = "Stop processing";
                }
                else
                {
                    buttonAbortImage.Visible = false;
                    buttonAbortImage.Enabled = false;
                    buttonChangeCamera.Visible = true;
                    buttonChangeCamera.Enabled = true;
                    templateMap.Clear();
                    List<Image<Bgr, byte>> templateList = new List<Image<Bgr, byte>>();
                    templateList = AuxiliarFunctions.searchTemplates("..\\..\\images", "template_", "png", 10);
                    timerMissionRecognized.Enabled = false;
                    if (templateList.Count <= 0)
                    {
                        String path = Path.GetFullPath("..\\..\\images");
                        MessageBox.Show("Templates not found in the next path: " + path + ". Please, create the templates for image recognition.", "Image Recognition Information", MessageBoxButtons.OK);
                    }
                    else
                    {
                        String path = Path.GetFullPath("..\\..\\images");

                        if (templateList.Count == 4)
                        {
                            templateMap.Add("up", templateList[0]);
                            templateMap.Add("down", templateList[1]);
                            templateMap.Add("right", templateList[2]);
                            templateMap.Add("left", templateList[3]);
                        }
                        buttonStart.Enabled = true;
                    }
                    pictureBoxVideo1.Image = null;
                    BasicFunctions.disarm();
                    buttonStart.Text = "Start processing";
                }
                enableImage = !enableImage;
            }
        }

        private void timerVideo_Tick_1(object sender, EventArgs e)
        {
            threshold = Convert.ToDouble(hScrollBarPorcentage.Value) / 100;
            Image<Bgr, Byte> aux = cap.QueryFrame();
            if (enableImage && !timeLapse)
            {
                timeLapse = true;
                Image<Bgr, Byte> imageToShow = ImageFunctions.imageProcessing(aux, threshold, 1, checkBoxSaltAndPepper.Checked, checkBoxGaussian.Checked, checkBoxBlur.Checked, checkBoxSobel.Checked, checkBoxCanny.Checked, checkBoxColorFilter.Checked, checkBoxShowProcessing.Checked);
                if (ImageFunctions.encountered)
                {
                    imageToShow = drawSquare(imageToShow);
                }
                else
                {
                    upCounter = 0;
                    downCounter = 0;
                    leftCounter = 0;
                    rightCounter = 0;
                }
                pictureBoxVideo1.Image = imageToShow.Resize(640, 480, INTER.CV_INTER_CUBIC).ToBitmap();
                timeLapse = false;
            }
            else
            {
                pictureBoxVideo1.Image = aux.Resize(640, 480, INTER.CV_INTER_CUBIC).ToBitmap();
            }
        }

        private void buttonSnapshot_Click(object sender, EventArgs e)
        {
            if (!rect.Size.IsEmpty)
            {
                if (radioButtonUpArrow.Checked)
                {
                    snapshotSound.Play();
                    buttonStart.Enabled = true;
                    if (System.IO.File.Exists("..\\..\\images\\template_1.png"))
                        System.IO.File.Delete("..\\..\\images\\template_1.png");
                    AuxiliarFunctions.cropImageByRectangle(pictureBoxVideo1.Image, rect).Save("..\\..\\images\\template_1.png");
                    if (templateMap.ContainsKey("up"))
                    {
                        templateMap["up"] = new Image<Bgr, Byte>("..\\..\\images\\template_1.png");
                    }
                    else
                    {
                        templateMap.Add("up", new Image<Bgr, Byte>("..\\..\\images\\template_1.png"));
                    }
                    radioButtonDownArrow.Checked = true;
                }
                else
                {
                    if (radioButtonDownArrow.Checked)
                    {
                        snapshotSound.Play();
                        buttonStart.Enabled = true;
                        if (System.IO.File.Exists("..\\..\\images\\template_2.png"))
                            System.IO.File.Delete("..\\..\\images\\template_2.png");
                        AuxiliarFunctions.cropImageByRectangle(pictureBoxVideo1.Image, rect).Save("..\\..\\images\\template_2.png");
                        if (templateMap.ContainsKey("down"))
                        {
                            templateMap["down"] = new Image<Bgr, Byte>("..\\..\\images\\template_2.png");
                        }
                        else
                        {
                            templateMap.Add("down", new Image<Bgr, Byte>("..\\..\\images\\template_2.png"));
                        }
                        radioButtonLeftArrow.Checked = true;
                    }
                    else
                    {
                        if (radioButtonLeftArrow.Checked)
                        {
                            snapshotSound.Play();
                            buttonStart.Enabled = true;
                            if (System.IO.File.Exists("..\\..\\images\\template_3.png"))
                                System.IO.File.Delete("..\\..\\images\\template_3.png");
                            AuxiliarFunctions.cropImageByRectangle(pictureBoxVideo1.Image, rect).Save("..\\..\\images\\template_3.png");
                            if (templateMap.ContainsKey("left"))
                            {
                                templateMap["left"] = new Image<Bgr, Byte>("..\\..\\images\\template_3.png");
                            }
                            else
                            {
                                templateMap.Add("left", new Image<Bgr, Byte>("..\\..\\images\\template_3.png"));
                            }
                            radioButtonRightArrow.Checked = true;
                        }
                        else
                        {
                            if (radioButtonRightArrow.Checked)
                            {
                                snapshotSound.Play();
                                buttonStart.Enabled = true;
                                if (System.IO.File.Exists("..\\..\\images\\template_4.png"))
                                    System.IO.File.Delete("..\\..\\images\\template_4.png");
                                AuxiliarFunctions.cropImageByRectangle(pictureBoxVideo1.Image, rect).Save("..\\..\\images\\template_4.png");
                                if (templateMap.ContainsKey("right"))
                                {
                                    templateMap["right"] = new Image<Bgr, Byte>("..\\..\\images\\template_4.png");
                                }
                                else
                                {
                                    templateMap.Add("right", new Image<Bgr, Byte>("..\\..\\images\\template_4.png"));
                                }
                                rectStartPoint = new Point();
                                rect.Location = new Point(0, 0);
                                rect.Size = new Size(0, 0);
                                pictureBoxVideo1.Invalidate();
                                radioButtonRightArrow.Checked = false;
                            }
                        }
                    }
                }
            }
        }

        private void buttonChangeCamera_Click(object sender, EventArgs e)
        {
            if (basicCamera)
            {
                cap = new Capture(1);
                basicCamera = false;
            }
            else
            {
                cap = new Capture();
                basicCamera = true;
            }
        }

        private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rectStartPoint = e.Location;
                Invalidate();
            }
            else
            {
                rectStartPoint = new Point();
                rect.Location = new Point(0, 0);
                rect.Size = new Size(0, 0);
                pictureBoxVideo1.Invalidate();
            }

        }

        private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }
            else if (e.Button == MouseButtons.Left)
            {
                Point tempEndPoint = e.Location;
                rect.Location = new Point(
                    Math.Min(rectStartPoint.X, tempEndPoint.X),
                    Math.Min(rectStartPoint.Y, tempEndPoint.Y));
                rect.Size = new Size(
                    Math.Abs(rectStartPoint.X - tempEndPoint.X),
                    Math.Abs(rectStartPoint.Y - tempEndPoint.Y));
                pictureBoxVideo1.Invalidate();
            }
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (pictureBoxVideo1.Image != null)
            {
                if (rect != null && rect.Width > 0 && rect.Height > 0)
                {
                    e.Graphics.FillRectangle(selectionBrush, rect);
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (rect.Contains(e.Location))
                {
                    Console.WriteLine("Right click");
                }
            }
        }

        private void hScrollBarPorcentage_ValueChanged(object sender, EventArgs e)
        {
            labelPorcentage.Text = hScrollBarPorcentage.Value + "%";
        }

        private void pictureBoxVideo1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Image<Bgr, Byte> im = new Image<Bgr, Byte>(new Bitmap(pictureBoxVideo1.Image));
            colorSelected = im[e.Y, e.X];
            MessageBox.Show("Color selected  Color Blue: " + colorSelected.Blue + " Color Green: " + colorSelected.Green + " Color Red: " + colorSelected.Red, "Selection Color for Matching", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void buttonApplyChanges_Click(object sender, EventArgs e)
        {
            if (checkBoxColorFilter.Checked && colorSelected.Equals(new Bgr(0, 0, 0)))
            {
                MessageBox.Show("Color not selected, please make double click on the capture to select a color.", "Color filter parameter not initializated", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (buttonStart.Text == "Stop processing")
                {
                    buttonStart_Click(sender, e);
                    buttonStart_Click(sender, e);
                }
            }
        }

        private void checkBoxColorFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxColorFilter.Checked && colorSelected.Equals(new Bgr(0, 0, 0)))
            {
                MessageBox.Show("Color not selected, please make double click on the capture to select a color.", "Color filter parameter not initializated", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public Image<Bgr, Byte> drawSquare(Image<Bgr, Byte> imageToShow)
        {
            ImageFunctions.maxLocationFinal.X = ImageFunctions.maxLocationFinal.X * 4;
            ImageFunctions.maxLocationFinal.Y = ImageFunctions.maxLocationFinal.Y * 4;

            Rectangle match = new Rectangle(ImageFunctions.maxLocationFinal, ImageFunctions.templateFinal.Size);
            switch (ImageFunctions.templateEncountered)
            {
                case "up":
                    {
                        upCounter++;
                        downCounter = 0;
                        leftCounter = 0;
                        rightCounter = 0;
                        if (upCounter > 4)
                        {
                            upRecognized = true;
                            imageToShow.Draw(match, new Bgr(Color.Red), 3);
                        }
                        break;
                    }
                case "down":
                    {
                        upCounter = 0;
                        downCounter++;
                        leftCounter = 0;
                        rightCounter = 0;
                        if (downCounter > 4)
                        {
                            downRecognized = true;
                            imageToShow.Draw(match, new Bgr(Color.Blue), 3);
                        }
                        break;
                    }
                case "left":
                    {
                        upCounter = 0;
                        downCounter = 0;
                        leftCounter++;
                        rightCounter = 0;
                        if (leftCounter > 4)
                        {
                            leftRecognized = true;
                            imageToShow.Draw(match, new Bgr(Color.Pink), 3);
                        }
                        break;
                    }
                case "right":
                    {
                        upCounter = 0;
                        downCounter = 0;
                        leftCounter = 0;
                        rightCounter++;
                        if (rightCounter > 4)
                        {
                            rightRecognized = true;
                            imageToShow.Draw(match, new Bgr(Color.Green), 3);
                        }
                        break;
                    }
                default: break;
            }
            return imageToShow;
        }

        private void timerMissionRecognized_Tick(object sender, EventArgs e)
        {
            if (upRecognized)
            {
                upRecognized = false;
                if (!Missions.locked)
                {
                    Missions.basicTakeOffThread.Abort();
                    Missions.basicTakeOffThread = new System.Threading.Thread(Missions.basicmission_takeOff);
                    Missions.basicTakeOffThread.Start();
                }
            }
            if (downRecognized)
            {
                downRecognized = false;
                if (!Missions.locked)
                {
                    Missions.basicLandThread.Abort();
                    Missions.basicLandThread = new System.Threading.Thread(Missions.basicmission_land);
                    Missions.basicLandThread.Start();
                }
            }
            if (rightRecognized)
            {
                rightRecognized = false;
                if (!Missions.locked)
                {
                    Missions.basicRotateRightThread.Abort();
                    Missions.basicRotateRightThread = new System.Threading.Thread(Missions.basicmission_rotateRight);
                    Missions.basicRotateRightThread.Start();
                }

            }
            if (leftRecognized)
            {
                leftRecognized = false;
                if (!Missions.locked)
                {
                    Missions.basicRotateLeftThread.Abort();
                    Missions.basicRotateLeftThread = new System.Threading.Thread(Missions.basicmission_rotateLeft);
                    Missions.basicRotateLeftThread.Start();
                }
            }
        }

        private void buttonAbortImage_Click(object sender, EventArgs e)
        {
            Missions.Abort();
        }

        #endregion

        #region ABOUT

        private void tabAbout_Selected(object sender, TabControlEventArgs e)
        {
            timerAbout.Enabled = true;
        }

        private void timerAboutTick(object sender, EventArgs e)
        {

            if (panel1.Location.Y != 0)
            {
                panel1.Location = new Point(panel1.Location.X, panel1.Location.Y - 1);
            }
            else
            {
                timerAbout.Enabled = false;
            }
        }
       
        #endregion

        #region TEXT FUNCTIONS

        private void richTextBoxDataSendAutonomous_TextChanged(object sender, EventArgs e)
        {
            richTextBoxDataSendAutonomous.SelectionStart = richTextBoxDataSendAutonomous.Text.Length;
            richTextBoxDataSendAutonomous.ScrollToCaret();
        }

        private void richTextBoxDataSend_TextChanged(object sender, EventArgs e)
        {
            richTextBoxDataSend.SelectionStart = richTextBoxDataSend.Text.Length;
            richTextBoxDataSend.ScrollToCaret();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        #endregion

        #region FINALIZATE FORM

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            BasicFunctions.disarm();
        }
        
        #endregion

    }
}

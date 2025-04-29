using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


namespace BT_2
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(@"D:\Documents\Visual_studio\Pictures\led_off.png");
            pictureBox2.Image = new Bitmap(@"D:\Documents\Visual_studio\Pictures\led_off.png");

            getAvailablePorts();
            L1off.BackColor = Color.Red;
            L2off.BackColor = Color.Red;
            L3off.BackColor = Color.Red;
            button4.BackColor = Color.Red;   // BUTTON DISCONNECT
        }


        void getAvailablePorts()
        {
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);

        }
        //=========================================================================================
        // BUTTON CONNECT
        //=========================================================================================
        byte ledtt = 0b00000000;
        byte[] tx_buffer = new byte[1];
        private void button3_Click(object sender, EventArgs e)     
        {
            try
            {
                if (comboBox1.Text == "" || comboBox2.Text == "")
                {
                    textBox2.Text = "Please select port settings";
                }
                else
                {
                    button3.BackColor = Color.Lime;
                    button4.BackColor = SystemColors.Control;

                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.Open();
                    progressBar1.Value = 100;
                    button1.Enabled = true;        // BUTTON SEND
                    button2.Enabled = true;        // BUTTON CLEAR                      
                    button3.Enabled = false;       // BUTTON CONNECT
                    button4.Enabled = true;        // BUTTON DISCONNECT
                    textBox1.Enabled = true;
                    timer1.Start();
                    
                }
            }
            catch (UnauthorizedAccessException)
            {
                textBox2.Text = "Unauthorized Access";
                button3.BackColor = SystemColors.Control;
                button4.BackColor = Color.Red;
            }
        }

        //=========================================================================================
        // BUTTON DISCONNECT
        //=========================================================================================
        private void button4_Click(object sender, EventArgs e)  
        {
            
            button4.BackColor = Color.Red;
            button3.BackColor = SystemColors.Control;
            
            serialPort1.Close();
            progressBar1.Value = 0;
            button1.Enabled = false;        // BUTTON SEND
            button2.Enabled = false;        // BUTTON CLEAR    
            button3.Enabled = true;         // BUTTON CONNECT 
            button4.Enabled = false;        // BUTTON DISCONNECT
            textBox1.Enabled = false;
            L1off.BackColor = Color.Red;
            L2off.BackColor = Color.Red;
            L3off.BackColor = Color.Red;
            L1on.BackColor = SystemColors.Control;
            L2on.BackColor = SystemColors.Control;
            L3on.BackColor = SystemColors.Control;
            pictureBox1.Image = new Bitmap(@"D:\Documents\Visual_studio\Pictures\led_off.png");
            pictureBox2.Image = new Bitmap(@"D:\Documents\Visual_studio\Pictures\led_off.png");

            timer1.Stop();

        }

        //=========================================================================================
        // truyền dữ liệu bằng hộp thoại
        //=========================================================================================
        private void button1_Click(object sender, EventArgs e)    // BUTTON SEND  
        {
            serialPort1.WriteLine(textBox1.Text);
            textBox1.Text = "";
        }

        //=========================================================================================
        // xóa dữ liệu ở hộp thoại recieve
        //=========================================================================================
        private void button2_Click(object sender, EventArgs e)    // BUTTON CLEAR  
        {
            textBox2.Text = "";
        }

        //=========================================================================================
        // nhận dữ liệu 1 cách tự động bằng timer quét liên tục bằng timer 2
        //=========================================================================================
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(timer2_Tick));
        }
        
        //=========================================================================================
        // BUTTON QUIT
        //=========================================================================================
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //=========================================================================================
        // Cập nhật giá trị vào chuổi
        //=========================================================================================
        static string UpdateLedtt(string text, int x, char newchar){
            char[] chuoi = text.ToCharArray();
            chuoi[x] = newchar;

            return new string(chuoi);   
        }

        //=========================================================================================
        // LED 1
        //=========================================================================================        
        private void L1on_Click(object sender, EventArgs e)
        {
            if(button3.Enabled == false) 
            {
                //ledtt = (byte)(ledtt ^ (1 << 0));
                tx_buffer[0] |= (0x01 << 0);

                L1on.BackColor = Color.Lime;
                L1off.BackColor = SystemColors.Control;
            }
        }
        private void L1off_Click(object sender, EventArgs e)
        {
            if (button3.Enabled == false)
            {
                //ledtt = (byte)(ledtt ^ (1 << 0));
                tx_buffer[0] = (byte)(tx_buffer[0] & ~(0x01 << 0));


                L1off.BackColor = Color.Red;
                L1on.BackColor = SystemColors.Control;
            }
        }


        //=========================================================================================
        // LED 2
        //=========================================================================================       
        private void L2on_Click(object sender, EventArgs e)
        {
            if (button3.Enabled == false)
            {
                //ledtt = (byte)(ledtt ^ (1 << 1));
                tx_buffer[0] |= (0x01 << 1);

                L2on.BackColor = Color.Lime;
                L2off.BackColor = SystemColors.Control;
            }         
        }
        private void L2off_Click(object sender, EventArgs e)
        {
            if (button3.Enabled == false)
            {
                //ledtt = (byte)(ledtt ^ (1 << 1));
                tx_buffer[0] = (byte)(tx_buffer[0] & ~(0x01 << 1));

                L2off.BackColor = Color.Red;
                L2on.BackColor = SystemColors.Control;
            }          
        }
        //=========================================================================================
        // LED 3
        //=========================================================================================
        private void L3on_Click(object sender, EventArgs e)
        {
            if (button3.Enabled == false)
            {
                //ledtt = (byte)(ledtt ^ (1 << 2));
                tx_buffer[0] |= (0x01 << 2);


                L3on.BackColor = Color.Lime;
                L3off.BackColor = SystemColors.Control;
            }
        }
        private void L3off_Click(object sender, EventArgs e)
        {  
            if (button3.Enabled == false)
            {
                //ledtt = (byte)(ledtt ^ (1 << 2));
                tx_buffer[0] = (byte)(tx_buffer[0] & ~(0x01 << 2));


                L3off.BackColor = Color.Red;
                L3on.BackColor = SystemColors.Control;
            }
        }
        //=========================================================================================

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            if (serialPort1.IsOpen)
            {
                //serialPort1.Write(new byte[] { ledtt }, 0, 1);

                string datasend = Encoding.UTF8.GetString(tx_buffer);
                serialPort1.Write(datasend);
            }

            //if((serialPort1.IsOpen) & (serialPort1.BytesToRead > 0))
            //{
            //    var numbytetoread = serialPort1.BytesToRead;

            //}

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            textBox2.Text = serialPort1.ReadExisting();
            if (textBox2.Text == "1")
            {
                pictureBox1.Image = new Bitmap(@"D:\Documents\Visual_studio\Pictures\led_on.png");
            }

            else if (textBox2.Text == "2")
            {
                pictureBox2.Image = new Bitmap(@"D:\Documents\Visual_studio\Pictures\led_on.png");
            }

            else
            {
                pictureBox1.Image = new Bitmap(@"D:\Documents\Visual_studio\Pictures\led_off.png");
                pictureBox2.Image = new Bitmap(@"D:\Documents\Visual_studio\Pictures\led_off.png");
            }
        }

    }
}

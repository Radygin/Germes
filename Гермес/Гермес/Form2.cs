using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO; // это для работы с файлами
using System.Xml.Serialization; //это для сохранения классов

namespace Гермес
{       
    public partial class Form2 : Form
    {   
        string[] myPort;              
        
        public Form2()
        {
            InitializeComponent();
            this.Text = "Настройки подключения сканера";

            myPort = SerialPort.GetPortNames();         // в массив помещаем доступные порты
            comboBox1.Items.AddRange(myPort);           // теперь этот массив заносим в список(comboBox) 

            Object[] BaudRate = { 2400, 4800, 7200, 9600, 14400, 19200, 38400, 57600, 115200, 128000 };
            comboBox2.Items.AddRange(BaudRate);
        }


        private void Form2_Load_1(object sender, EventArgs e)
        {
            Form1 mainForm = this.Owner as Form1;

            if (mainForm.COM_Mode == true)
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
          

            if (radioButton1.Checked == true)
            {           
                Serial_Emulation_Mode();
            }

            if (radioButton2.Checked == true)
            {
                Keyboard_Emulation_Mode();

            }
        }

        void Serial_Emulation_Mode()
        {
            label1.Enabled = true;
            label2.Enabled = true;
            label3.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            checkBox1.Enabled = true;
            button1.Enabled = true;

            Form1 mainForm = this.Owner as Form1;

            comboBox1.Text = mainForm.PortName;
            comboBox2.Text = mainForm.BaudRate;

            checkBox1.Checked = mainForm.AutoConn;

            if (mainForm.serialPort1.IsOpen == true)    // если порт открыт
            {
                Connected();
            }

            if (mainForm.serialPort1.IsOpen == false)  // если порт закрыт
            {
                Disconnected();
            }
        }

        void Keyboard_Emulation_Mode()
        {
            Form1 mainForm = this.Owner as Form1;

            mainForm.Keyboard_Moded();

            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            checkBox1.Enabled = false;
            button1.Enabled = false;                      
        }


        void Connected()    
        {
            Form1 mainForm = this.Owner as Form1;

            mainForm.Connected();

            button1.ForeColor = Color.Red;
            button1.Text = "Отключить";

            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;

            label1.ForeColor = Color.Green;
            label1.Text = "Порт открыт";
            return;

        }

        void Disconnected()
        {
            Form1 mainForm = this.Owner as Form1;

            mainForm.Disconnected();

            button1.ForeColor = Color.Green;
            button1.Text = "Подключить";

            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;

            label1.ForeColor = Color.Red;
            label1.Text = "Порт закрыт";
            return;
        }
        
        private void button1_Click(object sender, EventArgs e)      // Подключение/Отключение
        {
            Form1 mainForm = this.Owner as Form1;

            if (mainForm.serialPort1.IsOpen == true)                // Отключение
            {
                mainForm.serialPort1.Close();
                if (mainForm.serialPort1.IsOpen == false)
                {
                    Disconnected();
                    return;
                }               
            }
            if (mainForm.serialPort1.IsOpen == false)               //Подключение
            {

                mainForm.serialPort1.PortName = comboBox1.Text;
                mainForm.serialPort1.BaudRate = Convert.ToInt32 (comboBox2.Text);

                try
                {
                    mainForm.serialPort1.Open();
                }
                catch 
                {

                MessageBox.Show( "Невозможно подключиться! Проверьте подключение сканера и параметры подключения.",
                                 "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                if (mainForm.serialPort1.IsOpen == true)
                {
                    Connected();
                    return;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)      /// Кнопка сохранения настроек
        {
                
            
            iniSettings iniSet = new iniSettings();

            iniSet.COM_ModeXML = radioButton1.Checked;
            iniSet.PortNameXML = comboBox1.Text;
            iniSet.BaudRateXML = comboBox2.Text;
            iniSet.AutoConnXML = checkBox1.Checked;
            
            using (Stream writer = new FileStream("program.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(iniSettings));
                serializer.Serialize(writer, iniSet);
            }
            MessageBox.Show("Настройки сохранены","Настройки",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Serial_Emulation_Mode();
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                Keyboard_Emulation_Mode();
            }            
        }        
    }
}

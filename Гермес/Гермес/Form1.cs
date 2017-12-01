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
      
    public partial class Form1 : Form
    {
        public Boolean COM_Mode;
        public String PortName;
        public String BaudRate;
        public Boolean AutoConn;

        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "ИНИЦИАЛИЗАЦИЯ";
        }


        void ReadXML()
        {
            using (Stream stream = new FileStream("program.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(iniSettings));

                // в тут же созданную копию класса iniSettings под именем iniSet
                iniSettings iniSet = (iniSettings)serializer.Deserialize(stream);

                COM_Mode = iniSet.COM_ModeXML;
                PortName = iniSet.PortNameXML;
                BaudRate = iniSet.BaudRateXML;
                AutoConn = iniSet.AutoConnXML;

            }
        }                                           ////Чтение файла настроек

        public void Keyboard_Moded()
        {
            toolStripStatusLabel1.ForeColor = Color.Navy;
            toolStripStatusLabel1.Text = "РЕЖИМ КЛАВИАТУРЫ";
        }                             ////Отображение режима Keyboard

        void Autoconnect_Serial()
        {
            try
            {


                if (serialPort1.IsOpen == false)
                {
                    serialPort1.PortName = PortName;
                    serialPort1.BaudRate = Convert.ToInt32(BaudRate);

                    if (AutoConn == true)
                    {
                        serialPort1.Open();                     //// Автоподключение к порту
                    }
                }
            }
            catch
            {
                MessageBox.Show("Невозможно подлючиться к устройству",
                                "Ошибка подключения сканера", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (serialPort1.IsOpen == true)
            {
                Connected();
                return;
            }

            if (serialPort1.IsOpen == false)
            {
                Disconnected();
                return;
            }
        }                                ////Автоподключение Serial

        public void Connected()
        {

            toolStripStatusLabel1.ForeColor = Color.Green;
            toolStripStatusLabel1.Text = "ПОДКЛЮЧЕН К " + PortName;
            return;
        }                                  ////Отображение режима Serial подключен

        public void Disconnected()
        {
            toolStripStatusLabel1.ForeColor = Color.Red;
            toolStripStatusLabel1.Text = "ОТКЛЮЧЕН";
            return;
        }                               ////Отображение режима Serial отключен

        private void Form1_Load(object sender, EventArgs e)         ////Event Загрузки формы
        {
            ReadXML();

            if (COM_Mode == true)
            {
                if (AutoConn == true)
                {
                    Autoconnect_Serial();
                }
                else
                {
                    Disconnected();
                }
            }
            else Keyboard_Moded();
        }

        private void button1_Click(object sender, EventArgs e)      //// Event Кнопка настройки подключения
        {
            Form2 FormConn = new Form2();
            FormConn.Owner = this;
            FormConn.ShowDialog();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Гермес
{
    // класс, который мы будем сохранять
    public class iniSettings // имя выбрано просто для читаемости кода впоследствии
    {
        public Boolean COM_ModeXML; // Режим работы
        public String PortNameXML; // Номер порта
        public String BaudRateXML; // Скорость порта
        public Boolean AutoConnXML;// Автоподключение
    }
}

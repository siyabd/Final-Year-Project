using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO.Ports;

namespace IISFINALPROJECT.Models.Extra
{
    public class Port
    {
        SerialPort port;
        Port()
        {
            port = new SerialPort("COM15", 9600); ;
            port.Open();
        }
       
        public void sendData(String Data ){
            port.Write(Data);
        }

        public void closePort()
        {
            port.Close();
        }
    }
}
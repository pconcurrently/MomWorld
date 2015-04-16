using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;

namespace MomWorld
{
    public class SMSServices
    {
        #region SMS
        static SerialPort port = new SerialPort();
        static SMS objSMS = new SMS();
        static ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        #endregion

        public static string Send(SendSMSViewModel model)
        {
            //Input numbers
            string[] inputs = new string[] { "COM3", "9600", "8", "300", "300" };
            try
            {
                port = objSMS.OpenPort(inputs[0], inputs[1], inputs[2], inputs[3], inputs[4]);
                if (port == null)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                objSMS.ClosePort(port);
                return null;
            }

            try
            {

                if (objSMS.sendMsg(port, model.PhoneNumber, model.Message))
                {
                    return "Sent SMS successfully";
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                objSMS.ClosePort(port);
            }
        }

        public static string Send(string phoneNumber, string message)
        {
            //Input numbers
            string[] inputs = new string[] { "COM3", "9600", "8", "300", "300" };
            try
            {
                port = objSMS.OpenPort(inputs[0], inputs[1], inputs[2], inputs[3], inputs[4]);
                if (port == null)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                objSMS.ClosePort(port);
                return null;
            }

            try
            {

                if (objSMS.sendMsg(port, phoneNumber, message))
                {
                    return "Sent SMS successfully";
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                objSMS.ClosePort(port);
            }
        }
    }
}
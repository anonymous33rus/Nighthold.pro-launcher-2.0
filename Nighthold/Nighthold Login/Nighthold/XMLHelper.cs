using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows;
using System.Xml;

namespace Nighthold_Login.Nighthold
{
    class XMLHelper
    {
        private const string CONFIG_FILE_PATH = @"User\Nighthold-Login.cfg";

        public static string GetSettingValue(string key)
        {

            try
            {
                if (!File.Exists(CONFIG_FILE_PATH))
                {
                    CreateFileConfiguration();
                }

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(CONFIG_FILE_PATH);
                foreach (XmlNode xmlNode in xmlDocument.SelectSingleNode("configuration/appSettings"))
                {
                    if (xmlNode.Attributes["key"].Value == key)
                    {
                        return xmlNode.Attributes["value"].Value;
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        public static void UpdateSettingValue(string key, string value)
        {
            try
            {
                if (!File.Exists(CONFIG_FILE_PATH))
                {
                    CreateFileConfiguration();
                }

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(CONFIG_FILE_PATH);
                foreach (XmlNode xmlNode in xmlDocument.SelectSingleNode("configuration/appSettings"))
                {
                    if (xmlNode.Attributes["key"].Value == key)
                    {
                        xmlNode.Attributes["value"].Value = value;
                    }
                }
                xmlDocument.Save(CONFIG_FILE_PATH);
            }
            catch
            {

            }
        }

        private static void CreateFileConfiguration()
        {
            Directory.CreateDirectory("User");

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(CONFIG_FILE_PATH, settings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("configuration");
                xmlWriter.WriteStartElement("appSettings");

                xmlWriter.WriteStartElement("add");
                xmlWriter.WriteAttributeString("key", "remember_me");
                xmlWriter.WriteAttributeString("value", "true");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("add");
                xmlWriter.WriteAttributeString("key", "login_user");
                xmlWriter.WriteAttributeString("value", "");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("add");
                xmlWriter.WriteAttributeString("key", "login_pass");
                xmlWriter.WriteAttributeString("value", "");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }
    }
}

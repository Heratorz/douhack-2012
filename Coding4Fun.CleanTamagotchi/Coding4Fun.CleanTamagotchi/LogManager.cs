using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Coding4Fun.CleanTamagotchi
{
    public class LogManager
    {
        internal static void CreateLog(string target, string filePostfix, string xslPath, bool isGreen)
        {
            ProcessStartInfo psi = new ProcessStartInfo(
                @"c:\Program Files (x86)\Microsoft Fxcop 10.0\FxCopCmd.exe",
                string.Format(@"/c /f:{0} /outputCulture:1033 /o:{1} /fo", target, target + filePostfix));

            psi.ErrorDialog = false;
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;

            var proc = new System.Diagnostics.Process();
            proc.StartInfo = psi;
            bool started = proc.Start();
            if (started)
            {
                proc.WaitForExit();
            }

            MemoryStream transformed = new MemoryStream();
            XmlDocument doc = new XmlDocument();
            XslCompiledTransform xslt = new XslCompiledTransform();

            using (XmlReader input = XmlReader.Create(target + filePostfix))
            {
                xslt.Load(xslPath);
                xslt.Transform(input, new XsltArgumentList(), transformed);

                transformed.Position = 0;

                try
                {
                    doc.Load(transformed);
                }
                catch (Exception)
                {
                    return;
                }
            }

            var info = Deserialize(doc);
            info.IsGreen = isGreen;

            XmlSerializer xs = new XmlSerializer(typeof(Info));
            using (StreamWriter xmlTextWriter = new StreamWriter(target + filePostfix))
            {
                xs.Serialize(xmlTextWriter, info);
                xmlTextWriter.Flush();
            }
        }

        internal static Info Deserialize(XmlDocument xmlDoc)
        {
            Info res;
            XmlSerializer serializer = new XmlSerializer(typeof(Info));

            string xmlString = xmlDoc.OuterXml.ToString();
            byte[] buffer = ASCIIEncoding.UTF8.GetBytes(xmlString);

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                res = ((Info)serializer.Deserialize(ms));
            }
            return res;
        }

        internal static void RemoveLog(string fullPath)
        {
            try
            {
                System.IO.File.Delete(fullPath);
            }
            catch (System.IO.IOException)
            {
                return;
            }
        }

        internal static Info GetLog(string fullPath)
        {
            Info info = null;
            XmlSerializer xs = new XmlSerializer(typeof(Info));
            using (FileStream fs = new FileStream(fullPath, FileMode.Open))
            {
                XmlReader xr = new XmlTextReader(fs);
                info = (Info)xs.Deserialize(xr);
            }
            return info;
        }

        internal static XmlDocument LoadLog(string logPath, string xslPath)
        {
            MemoryStream transformed = new MemoryStream();
            XmlDocument doc = new XmlDocument();
            XslCompiledTransform xslt = new XslCompiledTransform();

            using (XmlReader input = XmlReader.Create(logPath))
            {
                xslt.Load(xslPath);
                xslt.Transform(input, new XsltArgumentList(), transformed);

                transformed.Position = 0;

                try
                {
                    doc.Load(transformed);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return doc;
        }
    }
}

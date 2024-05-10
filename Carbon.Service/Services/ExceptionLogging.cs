using Carbon.Service.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Carbon.Service.Services
{
    public class ExceptionLogging : IExceptionLogging
    {
        const long udcMaxLogFileSize = 10485760;

        public bool blnLogError(string strRequestParam, string strSource, string strMethod, string strStatement, string strDescription)
        {
            string strAppPath = System.Reflection.Assembly.GetCallingAssembly().Location.ToString();
            //return blnLogMessage("E", strAppPath, strSource, strMethod, strStatement, strDescription, false, null, null, null, null);
            return blnLogMessage("E", "", strRequestParam, strAppPath, strSource, strMethod, strStatement, strDescription);
        }

        public bool blnLogMessage(string strMessageType, string strRequestUrl, string strRequestParam, string strAppPath, string strSource, string strMethod, string strStatement, string strDescription)
        {

            const string udcCardRegEx = @"\b4\d{15}|5[1-5]\d{14}|3[47]\d{13}|6\d{15,18}|5[0678]\d{14}\b";

            const string udcCVVRegEx = @"\b=[0-9]{3,4}[|$]";

            string strAppName = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location.ToString()).ProductName.ToString();
            string strAppVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location.ToString()).ProductVersion.ToString();
            string strIpAddress = string.Empty;
            ArrayList arrMessage = new ArrayList();

            try
            {
                strAppName = System.Diagnostics.FileVersionInfo.GetVersionInfo(strAppPath).ProductName.ToString();
                strAppVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(strAppPath).ProductVersion.ToString();
                strIpAddress = strGetIP();
            }
            catch { }

            arrMessage.Add("** Date=" + DateTime.Now.ToString("d MMM yyyy @ h:mm:ss tt"));

            switch (strMessageType.ToUpper())
            {
                case "E":
                    arrMessage.Add("Type=ERROR");
                    break;
                case "I":
                    arrMessage.Add("Type=INFO");
                    break;
                case "W":
                    arrMessage.Add("Type=WARNING");
                    break;
                case "D":
                    arrMessage.Add("Type=DEBUG");
                    break;
                default:
                    arrMessage.Add("Type=UNKNOWN");
                    break;
            }

            arrMessage.Add("Server=" + System.Environment.MachineName.ToString() + " (" + strIpAddress + ")");
            arrMessage.Add("Application=" + strAppName + " " + strAppVersion);
            if (strRequestUrl.Length > 0)
                arrMessage.Add("RequestURL=" + strRequestUrl);
            arrMessage.Add("RequestParam=" + strRequestParam);
            arrMessage.Add("Source=" + strSource + ", Method=" + strMethod);

            if (strStatement.Length > 0)
            {
                strStatement = Regex.Replace(strStatement, udcCardRegEx, strReplaceData);
                strStatement = Regex.Replace(strStatement, udcCVVRegEx, "=xxx|");

                arrMessage.Add("Statement=" + strStatement);
            }
            if (strDescription.ToString().Length > 0)
            {
                strDescription = Regex.Replace(strDescription, udcCardRegEx, strReplaceData);
                strDescription = Regex.Replace(strDescription, udcCVVRegEx, "=xxx|");

                arrMessage.Add("Description=" + strDescription);
            }

            bool blnReturn = blnWriteLog(strAppName, arrMessage);

            return blnReturn;
        }

        public string strGetIP()
        {
            string strHostName = "";
            strHostName = Dns.GetHostName();
            IPHostEntry objIPEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] objIPAddress = objIPEntry.AddressList;

            return objIPAddress[objIPAddress.Length - 1].ToString();
        }

        public bool blnWriteLog(string strAppName, ArrayList arlData)
        {

            string strLogFileName = strGetLogFileName(strAppName);

            try
            {
                StreamWriter stwLog = new StreamWriter(strLogFileName, true);

                for (int intCount = 0; intCount < arlData.Count; intCount++)
                {
                    stwLog.WriteLine(arlData[intCount].ToString());
                }
                stwLog.WriteLine();
                stwLog.Close();
                stwLog = null;
            }
            catch
            {
                // Is there anything you can do here ???
            }

            return true;

        }

        public string strReplaceData(Match objMatch)
        {
            string strMatch = objMatch.ToString();
            return strMatch.Substring(0, (strMatch.StartsWith("3") ? 5 : 6)) + "-" + strMatch.Substring((strMatch.Length - 4), 4);
        }

        public string strGetAppVersion(System.Reflection.Assembly asmApp)
        {
            string strReturn = asmApp.Location + " v" + System.Diagnostics.FileVersionInfo.GetVersionInfo(asmApp.Location).ProductVersion.ToString();
            return strReturn;
        }

        public string strGetLogFileName(string strFilePrefix)
        {

            string strFolder = System.AppDomain.CurrentDomain.BaseDirectory + "Log\\";
            string strFile = strFolder + strFilePrefix + "_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            try
            {
                if (System.IO.Directory.Exists(strFolder) == false)
                {
                    System.IO.Directory.CreateDirectory(strFolder);
                }
            }
            catch { return ""; }

            if (File.Exists(strFile))
            {
                string strNewFile = strFile;
                int intLogCount = 1;
                while ((new FileInfo(strNewFile)).Length > udcMaxLogFileSize)
                {
                    strNewFile = strFolder + strFilePrefix + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + intLogCount.ToString("0000") + ".log";
                    if (File.Exists(strNewFile) == false) { break; }
                    intLogCount++;
                }
                strFile = strNewFile;
            }

            return strFile;

        }
    }
}

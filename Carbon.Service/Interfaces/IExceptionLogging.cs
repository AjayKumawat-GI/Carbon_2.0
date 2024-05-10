using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Carbon.Service.Interfaces
{
    public interface IExceptionLogging
    {
        bool blnLogError(string strRequestParam, string strSource, string strMethod, string strStatement, string strDescription);
        bool blnLogMessage(string strMessageType, string strRequestUrl, string strRequestParam, string strAppPath, string strSource, string strMethod, string strStatement, string strDescription);
        string strGetIP();
        bool blnWriteLog(string strAppName, ArrayList arlData);
        string strReplaceData(Match objMatch);
        string strGetAppVersion(Assembly asmApp);
        string strGetLogFileName(string strFilePrefix);
    }
}

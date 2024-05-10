using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Utility.CustomExceptions
{
    [Serializable]
    public class UniversalException : Exception
    {
        public UniversalException()
        {
        }

        public UniversalException(string message) : base(message)
        {
        }

        public UniversalException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

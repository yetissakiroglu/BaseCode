using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LogAttribute : Attribute
    {
        public string Message { get; }

        public LogAttribute(string message)
        {
            Message = message;
        }
    }
}

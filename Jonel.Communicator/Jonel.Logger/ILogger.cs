using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonel.Logger
{
    public interface ILogger
    {
        void Info(string message);
        void Info(string appHandle, string message);
        void Log(string appHandle, string message);
        void Log(string appHandle, Exception ex);
    }
}

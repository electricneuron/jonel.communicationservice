using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonel.Logger
{
    public interface ILogger
    {
        void Log(string error);
        void Log(Exception ex);
    }
}

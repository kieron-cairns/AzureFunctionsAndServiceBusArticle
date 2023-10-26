using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSaverWebAPITests.ControllerTestUtils
{
    public class DuplicatePrimaryKeyException : Exception
    {
        public DuplicatePrimaryKeyException(string message) : base(message)
        {
        }
    }
}

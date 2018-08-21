using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAPortable
{
    class ChartException : Exception
    {
        public ChartException()
        {
        }

        public ChartException(string message) : base(message)
        {
        }

        public ChartException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

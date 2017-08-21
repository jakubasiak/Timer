using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Timer.Model
{
    class TimeSpan
    {
        public int Period { get; set; }
        public SpaneType SpaneType { get; set; }
        public bool IsActive { get; set; }

        public void CountDown()
        {
            Period -= 1;
        }

    }
    enum SpaneType
    {
        WORK,
        REST
    }
}


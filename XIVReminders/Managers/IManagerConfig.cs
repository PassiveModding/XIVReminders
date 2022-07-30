using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XIVReminders.Managers
{
    internal interface IManagerConfig
    {
        public bool Enabled { get; set; }
    }
}

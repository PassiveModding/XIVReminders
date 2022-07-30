using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XIVReminders.Managers.Retainers
{
    internal class RetainerAlert
    {
        public ulong RetainerId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime TimeStamp { get; set; }
        public bool IsHidden { get; set; } = false;
    }
}

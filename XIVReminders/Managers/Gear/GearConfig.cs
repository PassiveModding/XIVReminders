using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XIVReminders.Managers.Gear
{
    internal class GearConfig : IManagerConfig
    {
        public bool Enabled { get; set; }
        public bool Spiritbonds { get; set; }
        public bool ShowMateriaExtractionButton { get; set; }
        public bool Repair { get; set; }
        public bool ShowRepairButton { get; set; }
        public int LowConditionThreshold { get; set; }
        public int CriticalConditionThreshold { get; set; }

        public static GearConfig Default => new GearConfig
        {
            Enabled = true,
            Spiritbonds = true,
            ShowMateriaExtractionButton = true,
            Repair = true,
            ShowRepairButton = true,
            LowConditionThreshold = 30,
            CriticalConditionThreshold = 10
        };
    }
}

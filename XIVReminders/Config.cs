using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XIVReminders.Modules;
using static XIVReminders.Modules.CurrencyAlerts;

namespace XIVReminders
{
    internal class Config : IPluginConfiguration
    {
        public Config()
        {
        }

        public int Version { get; set; } = 1;

        public bool HideDuringCombat { get; set; }
        public bool HideDuringInstance { get; set; }
        public bool ShowUI { get; set; }
        public bool ShowSettings { get; set; }

        public CurrencyAlert[] Alerts { get; set; } = CurrencyAlerts.GetDefaults();
        

        [NonSerialized]
        private DalamudPluginInterface? pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.pluginInterface?.SavePluginConfig(this);
        }
        public void Reset()
        {
            Alerts = CurrencyAlerts.GetDefaults();
        }
    }
}

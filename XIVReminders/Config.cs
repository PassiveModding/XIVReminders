using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using XIVReminders.Managers.Gear;
using XIVReminders.Managers.Items;
using XIVReminders.Managers.Retainers;

namespace XIVReminders
{
    internal class Config : IPluginConfiguration
    {
        public int Version { get; set; } = 1;
        public bool HideDuringCombat { get; set; }
        public bool HideDuringInstance { get; set; }
        public bool ShowUI { get; set; }
        public bool ShowSettings { get; set; }

        public ItemConfig? Items { get; set; } = null;
        public RetainerConfig? Retainers { get; set; } = null;
        public GearConfig? Gear { get; set; } = null;

        [NonSerialized]
        private DalamudPluginInterface? pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Save()
        {
            pluginInterface?.SavePluginConfig(this);
        }

        public void Reset()
        {
            Items = ItemConfig.Default;
            Retainers = RetainerConfig.Default;
            Gear = GearConfig.Default;
            Save();
        }
    }
}

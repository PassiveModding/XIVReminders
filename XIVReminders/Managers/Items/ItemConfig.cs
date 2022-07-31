using System.Collections.Generic;
using static XIVReminders.Managers.Items.Data;

namespace XIVReminders.Managers.Items
{
    internal class ItemConfig : IManagerConfig
    {
        public bool Enabled { get; set; }
        public Dictionary<Item, ItemAlert>? Items { get; set; }

        private static Dictionary<Item, ItemAlert> GenerateDefaultAlerts()
        {
            var configValues = new Dictionary<Item, ItemAlert>();
            foreach (var (id, info) in GetItemEnumerator())
            {
                configValues[id] = new ItemAlert
                {
                    Enabled = true,
                    Threshold = info.DefaultThreshold
                };
            }

            return configValues;
        }

        public static ItemConfig Default => new ItemConfig
        {
            Enabled = true,
            Items = GenerateDefaultAlerts()
        };
    }
}

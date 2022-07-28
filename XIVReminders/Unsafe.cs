using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XIVReminders.Modules;

namespace XIVReminders
{
    internal class Unsafe
    {
        // timer to reduce check interval to once per 5 sec
        private static DateTime lastCurrencyCheck = DateTime.MinValue;
        private static TimeSpan checkInterval = TimeSpan.FromSeconds(5);
        public static CurrencyAlerts.CurrencyAlert[] GetActiveCurrencyAlerts(Config config)
        {
            if (lastCurrencyCheck + checkInterval > DateTime.UtcNow) return config.Alerts.Where(x => x.Enabled && x.LastKnownValue > x.Threshold).ToArray();
            if (config.Alerts.All(x => x.Enabled == false)) return Array.Empty<CurrencyAlerts.CurrencyAlert>();

            unsafe
            {
                InventoryManager* inventoryManager = InventoryManager.Instance();
                foreach (var alert in config.Alerts)
                {
                    if (!alert.Enabled) continue;

                    alert.LastKnownValue = inventoryManager->GetInventoryItemCount((uint)alert.ItemId);
                }
            }

            lastCurrencyCheck = DateTime.UtcNow;
            return config.Alerts.Where(x => x.Enabled && x.LastKnownValue > x.Threshold).ToArray();
        }
    }
}

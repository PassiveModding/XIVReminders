using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XIVReminders.Modules
{
    internal partial class CurrencyAlerts
    {
        public static CurrencyAlert[] Alerts { get; set; } = new[]
        {
            new CurrencyAlert
            {
                DisplayName = "Tomestones of Poetics",
                ItemId = 28,
                Threshold = 1400,
                DefaultThreshold = 1400,
                MaxValue = 2000,
                Enabled = true,
                Category = "Battle"
            },
            new CurrencyAlert
            {
                DisplayName = "Tomestones of Aphorism",
                ItemId = 42,
                Threshold = 1400,
                DefaultThreshold = 1400,
                MaxValue = 2000,
                Enabled = true,
                Category = "Battle"
            },
            new CurrencyAlert
            {
                DisplayName = "Tomestones of Astronomy",
                ItemId = 43,
                Threshold = 1400,
                DefaultThreshold = 1400,
                MaxValue = 2000,
                Enabled = true,
                Category = "Battle"
            },
            new CurrencyAlert
            {
                DisplayName = "Storm Seals",
                ItemId = 20,
                Threshold = 75000,
                DefaultThreshold = 75000,
                MaxValue = 90000,
                Enabled = true,
                Category = "Grand Company"
            },
            new CurrencyAlert
            {
                DisplayName = "Serpent Seals",
                ItemId = 21,
                Threshold = 75000,
                DefaultThreshold = 75000,
                MaxValue = 90000,
                Enabled = true,
                Category = "Grand Company"
            },
            new CurrencyAlert
            {
                DisplayName = "Flame Seals",
                ItemId = 22,
                Threshold = 75000,
                DefaultThreshold = 75000,
                MaxValue = 90000,
                Enabled = true,
                Category = "Grand Company"
            },
            new CurrencyAlert
            {
                DisplayName = "Wolf Marks",
                ItemId = 25,
                Threshold = 18000,
                DefaultThreshold = 18000,
                MaxValue = 20000,
                Enabled = true,
                Category = "PVP"

            },
            new CurrencyAlert
            {
                DisplayName = "Trophy Crystal",
                ItemId = 36656,
                Threshold = 18000,
                DefaultThreshold = 18000,
                MaxValue = 20000,
                Enabled = true,
                Category = "PVP"
            },
            new CurrencyAlert
            {
                DisplayName = "Allied Seals",
                ItemId = 27,
                Threshold = 3500,
                DefaultThreshold = 3500,
                MaxValue = 4000,
                Enabled = true,
                Category = "Hunt"
            },
            new CurrencyAlert
            {
                DisplayName = "Centurio Seals",
                ItemId = 10307,
                Threshold = 3500,
                DefaultThreshold = 3500,
                MaxValue = 4000,
                Enabled = true,
                Category = "Hunt"
            },
            new CurrencyAlert
            {
                DisplayName = "Sack of Nuts",
                ItemId = 26533,
                Threshold = 3500,
                DefaultThreshold = 3500,
                MaxValue = 4000,
                Enabled = true,
                Category = "Hunt"
            },
            new CurrencyAlert
            {
                DisplayName = "Bicolor Gemstone",
                ItemId = 26807,
                Threshold = 800,
                DefaultThreshold = 800,
                MaxValue = 1000,
                Enabled = true,
                Category = "Hunt"
            },
            new CurrencyAlert
            {
                DisplayName = "White Crafters' Scrip",
                ItemId = 25199,
                Threshold = 1500,
                DefaultThreshold = 1500,
                MaxValue = 2000,
                Enabled = true,
                Category = "DOH/DOL"
            },
            new CurrencyAlert
            {
                DisplayName = "Purple Crafters' Scrip",
                ItemId = 33913,
                Threshold = 1500,
                DefaultThreshold = 1500,
                MaxValue = 2000,
                Enabled = true,
                Category = "DOH/DOL"
            },
            new CurrencyAlert
            {
                DisplayName = "White Gatherers' Scrip",
                ItemId = 25200,
                Threshold = 1500,
                DefaultThreshold = 1500,
                MaxValue = 2000,
                Enabled = true,
                Category = "DOH/DOL"
            },
            new CurrencyAlert
            {
                DisplayName = "Purple Gatherers' Scrip",
                ItemId = 33914,
                Threshold = 1500,
                DefaultThreshold = 1500,
                MaxValue = 2000,
                Enabled = true,
                Category = "DOH/DOL"
            },
            new CurrencyAlert
            {
                DisplayName = "Skybuilders' Scrip",
                ItemId = 28063,
                Threshold = 7500,
                DefaultThreshold = 7500,
                MaxValue = 10000,
                Enabled = true,
                Category = "DOH/DOL"
            }
        };

        public static CurrencyAlert[] GetDefaults()
        {
            var copies = new CurrencyAlert[Alerts.Length];
            for (int i = 0; i < Alerts.Length; i++)
            {
                var alert = Alerts[i];
                var copy = new CurrencyAlert
                {
                    Category = alert.Category,
                    DisplayName = alert.DisplayName,
                    DefaultThreshold = alert.DefaultThreshold,
                    Enabled = alert.Enabled,
                    ItemId = alert.ItemId,
                    LastKnownValue = alert.LastKnownValue,
                    MaxValue = alert.MaxValue,
                    Threshold = alert.Threshold
                };
                copies[i] = copy;
            }

            return copies;
        }

        public static void DrawConfig(Config config)
        {
            if (ImGui.BeginTabBar("Currencies"))
            {
                var alertGroups = config.Alerts.GroupBy(x => x.Category);
                foreach (var alertGroup in alertGroups)
                {
                    if (ImGui.BeginTabItem(alertGroup.Key))
                    {
                        foreach (var alert in alertGroup)
                        {
                            var enabled = alert.Enabled;
                            var threshold = alert.Threshold;

                            if (ImGui.Checkbox(alert.DisplayName, ref enabled))
                            {
                                alert.Enabled = enabled;
                                config.Save();
                            }

                            // Label is used to define uniqueness of the inputs, use ## to hide the contents
                            if (ImGui.SliderInt($"##{alert.DisplayName}", ref threshold, 1, alert.MaxValue))
                            {
                                alert.Threshold = threshold;
                                config.Save();
                            }
                        }
                        ImGui.EndTabItem();
                    }
                }

                ImGui.EndTabBar();
            }
        }
    }
}

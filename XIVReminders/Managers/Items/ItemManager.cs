using Dalamud.Interface;
using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using System;
using System.Linq;
using System.Threading;

namespace XIVReminders.Managers.Items
{
    internal class ItemManager : IBaseManager
    {
        public string Name { get; init; } = "Items";
        public Config Config { get; init; }
        public Timer CheckLoop { get; init; }

        public ItemManager(Config config)
        {
            Config = config;
            if (Config.Items == null) Config.Items = ItemConfig.Default;

            CheckLoop = new Timer(e =>
            {
                try
                {
                    ReadCurrencyGameata();
                }
                finally
                {
                    CheckLoop?.Change(TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
                }
            }, null, TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
        }

        private void ReadCurrencyGameata()
        {
            if (Config?.Items == null) return;
            if (!Helpers.IsPlayerAvailable()) return;

            unsafe
            {
                InventoryManager* inventoryManager = InventoryManager.Instance();
                foreach (var category in Config.Items.Categories)
                {
                    if (category.Enabled == false) continue;
                    foreach (var (id, item) in category.Items)
                    {
                        if (!item.Enabled) continue;
                        item.LastKnownValue = inventoryManager->GetInventoryItemCount((uint)id);
                    }
                }
            }
        }

        public void DrawUiMenu()
        {
            if (Config?.Items == null) return;

            foreach (var category in Config.Items.Categories)
            {
                if (category.Enabled == false) continue;
                foreach (var (id, item) in category.Items)
                {
                    if (!item.Enabled || item.LastKnownValue < item.Threshold) continue;
                    Helpers.RenderRow(item.DisplayName, item.LastKnownValue.ToString());
                }
            }
        }

        public void DrawConfigMenu()
        {
            if (Config?.Items == null) return;

            if (ImGui.BeginTabBar("Currencies"))
            {
                foreach (var category in Config.Items.Categories)
                {
                    if (ImGui.BeginTabItem($"{category.Name}##itemcategorysettings"))
                    {
                        foreach (var (id, item) in category.Items)
                        {
                            var enabled = item.Enabled;
                            var threshold = item.Threshold;

                            if (ImGui.Checkbox(item.DisplayName, ref enabled) && enabled != item.Enabled)
                            {
                                item.Enabled = enabled;
                                Config.Save();
                            }

                            // Label is used to define uniqueness of the inputs, use ## to hide the contents
                            if (ImGui.SliderInt($"##{item.DisplayName}", ref threshold, 1, item.MaxValue) && threshold != item.Threshold)
                            {
                                item.Threshold = threshold;
                                Config.Save();
                            }

                            ImGui.SameLine();
                            if (Helpers.IconButton(FontAwesomeIcon.Stop, $"reset{item.DisplayName}"))
                            {
                                item.Threshold = item.DefaultThreshold;
                                Config.Save();
                            }
                        }
                        ImGui.EndTabItem();
                    }
                }
                ImGui.EndTabBar();
            }
        }


        public IManagerConfig GetConfig()
        {
            if (Config.Items == null)
            {
                Config.Items = ItemConfig.Default;
                Config.Save();
            }

            return Config.Items;
        }
    }
}

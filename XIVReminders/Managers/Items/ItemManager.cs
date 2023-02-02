using Dalamud.Interface;
using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using static XIVReminders.Managers.Items.Data;

namespace XIVReminders.Managers.Items
{
    internal class ItemManager : IBaseManager
    {
        public string Name { get;} = "Items";
        public Dictionary<Item, int> CurrentItemQuantities { get; } = new Dictionary<Item, int>();

        public Config Config { get; }
        public Timer CheckLoop { get; }
        public Dictionary<Item, ItemAttribute> ItemDefaults { get; }
        public Dictionary<ItemCategory, (Item, ItemAttribute)[]> ItemCategories { get; }

        public ItemManager(Config config)
        {
            Config = config;            
            
            // set defaults if they haven't been set already
            if (Config.Items == null) Config.Items = ItemConfig.Default;
            if (Config.Items.Items == null) Config.Items.Items = ItemConfig.Default.Items;

            ItemDefaults = GetItemEnumerator().ToDictionary(x => x.Item1, x => x.Item2);

            // Build category index to reduce draw times for config ui
            ItemCategories = ItemDefaults.GroupBy(x => x.Value.Category).ToDictionary(x => x.Key, x => x.Select(y => (y.Key, y.Value)).ToArray());

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
            if (Config?.Items?.Items == null) return;
            if (!Helpers.IsPlayerAvailable()) return;

            unsafe
            {
                InventoryManager* inventoryManager = InventoryManager.Instance();
                foreach (var (item, config) in Config.Items.Items)
                {
                    if (!config.Enabled) continue;
                    CurrentItemQuantities[item] = inventoryManager->GetInventoryItemCount((uint)item);
                }
            }
        }

        public void DrawUiMenu()
        {
            if (Config?.Items?.Items == null) return;

            foreach (var (id, config) in Config.Items.Items)
            {
                // disabled in config
                if (config == null || !config.Enabled) continue;
                // none recorded
                if (!CurrentItemQuantities.ContainsKey(id) || !ItemDefaults.ContainsKey(id)) continue;
                // below threshold
                if (CurrentItemQuantities[id] < config.Threshold) continue;

                Helpers.RenderRow(ItemDefaults[id].Name, CurrentItemQuantities[id].ToString());
            }
        }


        public void DrawConfigMenu()
        {
            if (Config?.Items?.Items == null) return;

            if (!ImGui.BeginTabBar("Currencies"))
            {
                return;
            }

            foreach (var category in ItemCategories)
            {
                if (!ImGui.BeginTabItem($"{category.Key}##itemcategorysettings"))
                {
                    continue;
                }

                foreach (var (id, item) in category.Value)
                {
                    if (!Config.Items.Items.ContainsKey(id))
                    {
                        Config.Items.Items[id] = new ItemAlert
                        {
                            Enabled = true,
                            Threshold = item.DefaultThreshold
                        };
                    }

                    var itemConfig = Config.Items.Items[id];
                    var enabled = itemConfig.Enabled;
                    var threshold = itemConfig.Threshold;

                    if (ImGui.Checkbox(item.Name, ref enabled) && enabled != itemConfig.Enabled)
                    {
                        itemConfig.Enabled = enabled;
                        Config.Save();
                    }

                    // Label is used to define uniqueness of the inputs, use ## to hide the contents
                    if (ImGui.SliderInt($"##{item.Name}", ref threshold, 1, item.MaxCount) && threshold != itemConfig.Threshold)
                    {
                        itemConfig.Threshold = threshold;
                        Config.Save();
                    }

                    ImGui.SameLine();
                    if (Helpers.IconButton(FontAwesomeIcon.Stop, $"reset{item.Name}"))
                    {
                        itemConfig.Threshold = item.DefaultThreshold;
                        Config.Save();
                    }
                }

                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }

        public bool TryFormatTitleContent(out string titleContent)
        {
            titleContent = string.Empty;
            return false;
        }

        public void DrawExtraWindows()
        {

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

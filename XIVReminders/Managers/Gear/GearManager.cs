﻿using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XivCommon;

namespace XIVReminders.Managers.Gear
{
    internal class GearManager : IBaseManager
    {
        public string Name => "Gear";
        internal const uint EquipmentContainerSize = 13;
        private int MaxSpiritBondPercent { get; set; } = 0;
        private int MinConditionPercent { get; set; } = 100;

        public Config Config { get; init; }
        public XivCommonBase XivCommon { get; init; }
        public Timer CheckLoop { get; init; }

        public GearManager(Config config, XivCommonBase xivCommon)
        {
            Config = config;
            XivCommon = xivCommon;
            if (Config.Gear == null) Config.Gear = GearConfig.Default;

            CheckLoop = new Timer(e =>
            {
                try
                {
                    ReadGearData();
                }
                finally
                {
                    CheckLoop?.Change(TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
                }
            }, null, TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
        }

        public void ReadGearData()
        {
            if (Config?.Gear == null) return;
            if (!Config.Gear.Enabled) return; 
            if (!Helpers.IsPlayerAvailable()) return;

            int localMaxSpirit = int.MinValue;
            int localMinCond = int.MaxValue;
            unsafe
            {
                var manager = InventoryManager.Instance();
                var container = manager->GetInventoryContainer(InventoryType.EquippedItems);
                for (var i = 0; i < EquipmentContainerSize; i++)
                {
                    var item = container->GetInventorySlot(i);
                    var spiritBond = item->Spiritbond;
                    if (spiritBond > localMaxSpirit) localMaxSpirit = spiritBond;
                    var condition = item->Condition;
                    if (condition < localMinCond) localMinCond = condition;
                }
            }
            MaxSpiritBondPercent = localMaxSpirit / 100;
            MinConditionPercent = localMinCond / 300;
        }

        public void DrawConfigMenu()
        {
            if (Config?.Gear == null) return;
            var enabled = Config.Gear.Enabled;
            if (ImGui.Checkbox("Enabled##GearTeacker", ref enabled) && enabled != Config.Gear.Enabled)
            {
                Config.Gear.Enabled = enabled;
                Config.Save();
            }

            ImGui.NewLine();
            var spiritbond = Config.Gear.Spiritbonds;
            if (ImGui.Checkbox("SpiritBond", ref spiritbond) && spiritbond != Config.Gear.Spiritbonds)
            {
                Config.Gear.Spiritbonds = spiritbond;
                Config.Save();
            }

            var extractButton = Config.Gear.ShowMateriaExtractionButton;
            if (ImGui.Checkbox("Show Materia Extraction Button", ref extractButton) && extractButton != Config.Gear.ShowMateriaExtractionButton)
            {
                Config.Gear.ShowMateriaExtractionButton = extractButton;
                Config.Save();
            }

            ImGui.NewLine();
            var condition = Config.Gear.Repair;
            if (ImGui.Checkbox("Repair", ref condition) && condition != Config.Gear.Repair)
            {
                Config.Gear.Repair = condition;
                Config.Save();
            }

            var repairButton = Config.Gear.ShowRepairButton;
            if (ImGui.Checkbox("Show Gear Repair Button", ref repairButton) && repairButton != Config.Gear.ShowRepairButton)
            {
                Config.Gear.ShowRepairButton = repairButton;
                Config.Save();
            }

            ImGui.PushItemWidth(100);
            var lowConditionThreshold = Config.Gear.LowConditionThreshold;
            if (ImGui.InputInt("Low Condition Threshold", ref lowConditionThreshold) && lowConditionThreshold != Config.Gear.LowConditionThreshold)
            {
                Config.Gear.LowConditionThreshold = lowConditionThreshold;
                Config.Save();
            }

            var criticalConditionThreshold = Config.Gear.CriticalConditionThreshold;
            if (ImGui.InputInt("Critical Condition Threshold", ref criticalConditionThreshold) && criticalConditionThreshold != Config.Gear.CriticalConditionThreshold)
            {
                Config.Gear.CriticalConditionThreshold = criticalConditionThreshold;
                Config.Save();
            }
            ImGui.PopItemWidth();
        }

        public void DrawUiMenu()
        {
            if (Config?.Gear == null) return;
            if (!Config.Gear.Enabled) return;
            if (Config.Gear.Spiritbonds && MaxSpiritBondPercent >= 100)
            { 
                Helpers.RenderRow($"Spiritbond", $"{MaxSpiritBondPercent}%");
                if (Config.Gear.ShowMateriaExtractionButton)
                {
                    ImGui.SameLine();
                    if (ImGui.Button("Extract"))
                    {
                        XivCommon.Functions.Chat.SendMessage("/gaction \"Materia Extraction\"");
                    }
                }
            }

            if (Config.Gear.Repair)
            {
                if (MinConditionPercent < Config.Gear.CriticalConditionThreshold || MinConditionPercent < Config.Gear.LowConditionThreshold)
                {
                    if (MinConditionPercent < Config.Gear.CriticalConditionThreshold)
                        Helpers.RenderRow($"Gear Condition Critical", $"{MinConditionPercent}%");
                    else if (MinConditionPercent < Config.Gear.LowConditionThreshold)
                        Helpers.RenderRow($"Gear Condition Low", $"{MinConditionPercent}%");

                    ImGui.SameLine();
                    if (Config.Gear.ShowRepairButton)
                    {
                        ImGui.SameLine();
                        if (ImGui.Button("Repair"))
                        {
                            XivCommon.Functions.Chat.SendMessage("/gaction repair");
                        }
                    }
                }
            }
        }

        public IManagerConfig GetConfig()
        {
            if (Config.Gear == null)
            {
                Config.Gear = GearConfig.Default;
                Config.Save();
            }

            return Config.Gear;
        }
    }
}

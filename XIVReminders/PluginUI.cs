﻿using Dalamud.Game.ClientState.Conditions;
using ImGuiNET;
using System;
using System.Numerics;
using XIVReminders.Managers;

namespace XIVReminders
{
    internal class PluginUI : IDisposable
    {
        public PluginUI(Config config, IBaseManager[] managers)
        {
            Config = config;
            Managers = managers;
        }

        public Config Config { get; }
        public IBaseManager[] Managers { get; }

        public void Dispose()
        {
        }

        public void Draw()
        {
            try
            {
                DrawUI();
                DrawConfig();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void DrawConfig()
        {
            var showSettings = Config.ShowSettings;
            if (!showSettings) return;

            ImGui.SetNextWindowSize(new Vector2(700, 500), ImGuiCond.FirstUseEver);
            if (ImGui.Begin("XIVReminder Settings", ref showSettings))
            {
                Config.ShowSettings = showSettings;

                if (ImGui.BeginTabBar("ConfigMenuBar"))
                {
                    if (ImGui.BeginTabItem("Config"))
                    {
                        var hideDuringCombat = Config.HideDuringCombat;
                        if (ImGui.Checkbox("Hide During Combat", ref hideDuringCombat) && hideDuringCombat != Config.HideDuringCombat)
                        {
                            Config.HideDuringCombat = hideDuringCombat;
                            Config.Save();
                        }

                        var hideDuringInstance = Config.HideDuringInstance;
                        if (ImGui.Checkbox("Hide During Instance", ref hideDuringInstance) && hideDuringInstance != Config.HideDuringInstance)
                        {
                            Config.HideDuringInstance = hideDuringInstance;
                            Config.Save();
                        }

                        var showUi = Config.ShowUI;
                        if (ImGui.Checkbox("Show Reminder Window", ref showUi) && showUi != Config.ShowUI)
                        {
                            Config.ShowUI = showUi;
                            Config.Save();
                        }

                        if (ImGui.Button("Reset All Settings"))
                        {
                            Config.Reset();
                            Config.Save();
                        }

                        ImGui.EndTabItem();
                    }

                    foreach (var manager in Managers)
                    {
                        if (ImGui.BeginTabItem(manager.Name))
                        {
                            manager.DrawConfigMenu();
                            ImGui.EndTabItem();
                        }
                    }

                    ImGui.EndTabBar();
                }
            }

            ImGui.End();
        }

        public void DrawUI()
        {
            if (!Config.ShowUI) return;
            if (Config.HideDuringCombat && Dalamud.Conditions[ConditionFlag.InCombat]) return;
            if (Config.HideDuringInstance)
            {
                // note: BoundToDuty97 will hide during duty pop
                if (Dalamud.Conditions[ConditionFlag.BoundByDuty] ||
                    Dalamud.Conditions[ConditionFlag.BoundByDuty56] ||
                    Dalamud.Conditions[ConditionFlag.BoundByDuty95])
                    return;
            }

            var showUi = Config.ShowUI;
            if (ImGui.Begin("XIVReminder", ref showUi))
            {
                Config.ShowUI = showUi;
                Config.Save();
                if (ImGui.BeginTable("Reminders", 2, ImGuiTableFlags.Borders))
                {
                    ImGui.TableSetupColumn("Name");
                    ImGui.TableSetupColumn("Value", ImGuiTableColumnFlags.WidthStretch);
                    ImGui.TableHeadersRow();

                    foreach (var manager in Managers)
                    {
                        manager.DrawUiMenu();
                    }
                }

                ImGui.EndTable();
            }

            ImGui.End();
        }
    }
}

using Dalamud.Game.ClientState.Conditions;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XIVReminders.Modules;

namespace XIVReminders
{
    internal class PluginUI : IDisposable
    {
        public PluginUI(Config config)
        {
            Config = config;
        }

        public Config Config { get; }

        public void Dispose()
        {
        }

        public void Draw()
        {
            DrawUI();
            DrawConfig();
        }

        public void DrawConfig()
        {
            var showSettings = Config.ShowSettings;
            if (!showSettings) return;

            ImGui.SetNextWindowSize(new Vector2(700, 500), ImGuiCond.FirstUseEver);
            if (ImGui.Begin("XIVReminder Settings", ref showSettings))
            {
                Config.ShowSettings = showSettings;
                Config.Save();

                if (ImGui.BeginTabBar("ConfigMenuBar"))
                {
                    if (ImGui.BeginTabItem("Config"))
                    {
                        var hideDuringCombat = Config.HideDuringCombat;
                        if (ImGui.Checkbox("Hide During Combat", ref hideDuringCombat))
                        {
                            Config.HideDuringCombat = hideDuringCombat;
                            Config.Save();
                        }

                        var hideDuringInstance = Config.HideDuringInstance;
                        if (ImGui.Checkbox("Hide During Instance", ref hideDuringInstance))
                        {
                            Config.HideDuringInstance = hideDuringInstance;
                            Config.Save();
                        }

                        var showUi = Config.ShowUI;
                        if (ImGui.Checkbox("ShowUI", ref showUi))
                        {
                            Config.ShowUI = showUi;
                            Config.Save();
                        }

                        if (ImGui.Button("Reset"))
                        {
                            Config.Reset();
                            Config.Save();
                        }

                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Currencies"))
                    {
                        CurrencyAlerts.DrawConfig(Config);
                        ImGui.EndTabItem();
                    }
                }
            }

            ImGui.End();
        }

        public static Random rnd = new Random();
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

                    var alerts = Unsafe.GetActiveCurrencyAlerts(Config);

                    foreach (var alert in alerts)
                    {
                        if (!alert.Enabled) continue;
                        RenderRow(alert.DisplayName, alert.LastKnownValue.ToString());
                    }
                }

                ImGui.EndTable();
            }

            ImGui.End();
        }

        private void RenderRow(string name, string value)
        {
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.Text(name);
            ImGui.TableNextColumn();
            ImGui.Text(value);
        }
    }
}

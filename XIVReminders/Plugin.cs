using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;

namespace XIVReminders
{
    public class Plugin : IDalamudPlugin
    {
        public string Name => "XIVReminder";
        internal Config Config { get; }
        internal PluginUI PluginUI { get; }
        internal Commands Commands { get; }

        public Plugin(DalamudPluginInterface pluginInterface)
        {
            Dalamud.Initialize(pluginInterface);

            this.Config = Dalamud.PluginInterface.GetPluginConfig() as Config ?? new Config();
            this.Config.Initialize(Dalamud.PluginInterface);
            this.PluginUI = new PluginUI(this.Config);
            Commands = Commands.GetCommands(Config);
            Commands.InitCommands();

            Dalamud.PluginInterface.UiBuilder.Draw += PluginUI.Draw;
            Dalamud.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        private void DrawConfigUI()
        {
            Config.ShowSettings = true;
        }

        public void Dispose()
        {
            Dalamud.PluginInterface.UiBuilder.Draw -= PluginUI.Draw;
            Dalamud.PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;
            Commands.DisposeCommands();
            PluginUI.Dispose();
        }
    }
}
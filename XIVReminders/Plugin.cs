using Dalamud.Plugin;
using XIVReminders.Managers.Gear;
using XIVReminders.Managers.Items;
using XIVReminders.Managers.Retainers;

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

            Config = Dalamud.PluginInterface.GetPluginConfig() as Config ?? new Config();
            Config.Initialize(Dalamud.PluginInterface);
            Commands = Commands.GetCommands(Config);
            Commands.InitCommands();

            this.PluginUI = new PluginUI(this.Config, new Managers.IBaseManager[]
            {
                new ItemManager(Config),
                new RetainerManager(Config),
                new GearManager(Config)
            });

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
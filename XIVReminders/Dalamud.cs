using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace XIVReminders
{
    public class Dalamud
    {
        public static void Initialize(DalamudPluginInterface pluginInterface) =>
            pluginInterface.Create<Dalamud>();

        [PluginService]
        [RequiredVersion("1.0")]
        public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService]
        [RequiredVersion("1.0")]
        public static CommandManager Commands { get; private set; } = null!;
        [PluginService]
        [RequiredVersion("1.0")]
        public static Condition Conditions { get; private set; } = null!;
        [PluginService]
        [RequiredVersion("1.0")]
        public static ClientState State { get; private set; } = null!;
    }
}

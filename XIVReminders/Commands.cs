using Dalamud.Game.Command;
using System.Collections.Generic;

namespace XIVReminders
{
    internal class Commands
    {
        private static Dictionary<int, Commands> InstanceDict = new Dictionary<int, Commands>();
        public static Commands GetCommands(Config config)
        {
            var hc = config.GetHashCode();
            if (InstanceDict.ContainsKey(hc))
            {
                return InstanceDict[hc];
            }

            var instance = new Commands(config);
            InstanceDict[hc] = instance;
            return instance;
        }

        private Dictionary<string, CommandInfo> _commands { get; init; }

        public Config Config { get; }

        private Commands(Config config)
        {
            Config = config;
            _commands = new Dictionary<string, CommandInfo>()
            {
                {
                    "/reminders", new CommandInfo((a,b) => Config.ShowSettings = true)
                    {
                        HelpMessage = "Kweh"
                    }
                },
                {
                    "/remind", new CommandInfo((a,b) => Config.ShowUI = true)
                    {
                        HelpMessage = "Kweh"
                    }
                }
            };
        }

        public void InitCommands()
        {
            foreach (var command in _commands)
            {
                Dalamud.Commands.AddHandler(command.Key, command.Value);
            }
        }

        public void DisposeCommands()
        {
            if (_commands == null) return;
            foreach (var command in _commands)
            {
                Dalamud.Commands.RemoveHandler(command.Key);
            }
        }
    }
}

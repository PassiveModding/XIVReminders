using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XIVReminders.Managers.Retainers
{
    internal class RetainerManager : IBaseManager
    {
        public string Name { get; } = "Retainers";
        public Timer CheckLoop { get; init; }
        public Config Config { get; init; }

        public RetainerManager(Config config)
        {
            Config = config;
            if (Config.Retainers == null) Config.Retainers = RetainerConfig.Default;

            CheckLoop = new Timer(e =>
            {
                try
                {
                    ReadRetainerGameData();
                }
                finally
                {
                    CheckLoop?.Change(TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
                }
            }, null, TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
        }

        private void ReadRetainerGameData()
        {
            if (Config?.Retainers?.PlayerRetainers == null) return;
            if (Config.Retainers.Enabled == false) return;
            if (!Helpers.IsPlayerAvailable()) return;

            unsafe
            {
                var instance = FFXIVClientStructs.FFXIV.Client.Game.RetainerManager.Instance();
                for (uint i = 0; i < 10; i++)
                {
                    var retainer = instance->GetRetainerBySortedIndex(i);
                    if (retainer->ClassJob == 0) continue;

                    var retainerId = retainer->RetainerID;
                    RetainerAlert retAlert;
                    if (Config.Retainers.PlayerRetainers.ContainsKey(retainerId))
                    {
                        retAlert = Config.Retainers.PlayerRetainers[retainerId];
                    }
                    else
                    {
                        retAlert = new RetainerAlert
                        {
                            RetainerId = retainerId,
                            IsHidden = false
                        };
                    }

                    retAlert.Name = Helpers.ReadBytesAsString(retainer->Name, 32);
                    retAlert.TimeStamp = Helpers.DateFromTimeStamp(retainer->VentureComplete);
                    Config.Retainers.PlayerRetainers[retAlert.RetainerId] = retAlert;
                }
            }
        }

        public void DrawConfigMenu()
        {
            if (Config?.Retainers == null) return;
            var enabled = Config.Retainers.Enabled;
            if (ImGui.Checkbox("Retainers", ref enabled) && enabled != Config.Retainers.Enabled)
            {
                Config.Retainers.Enabled = enabled;
                Config.Save();
            }

            foreach (var (uid, retainer) in Config.Retainers.PlayerRetainers)
            {
                var retHidden = retainer.IsHidden;
                if (ImGui.Checkbox($"Hide {retainer.Name} [{uid}]", ref retHidden) && retHidden != retainer.IsHidden)
                {
                    retainer.IsHidden = retHidden;
                    Config.Save();
                }
            }
        }

        public void DrawUiMenu()
        {
            if (Config?.Retainers == null) return;
            if (Config.Retainers.Enabled == false) return;

            foreach (var (uid, retainer) in Config.Retainers.PlayerRetainers)
            {
                if (retainer.IsHidden) continue;
                if (retainer.TimeStamp > DateTime.UtcNow)
                {
                    var diff = retainer.TimeStamp - DateTime.UtcNow;
                    if (diff < TimeSpan.FromHours(1))
                    {
                        Helpers.RenderRow($"{retainer.Name} on venture", diff.ToString(@"mm\:ss"));
                    }
                    else
                    {
                        Helpers.RenderRow($"{retainer.Name} on venture", diff.ToString(@"hh\:mm\:ss"));
                    }
                }
                else
                {
                    Helpers.RenderRow($"{retainer.Name} complete", retainer.TimeStamp.ToString("dd MMM"));
                }
            }
        }

        public IManagerConfig GetConfig()
        {
            if (Config.Retainers == null)
            {
                Config.Retainers = RetainerConfig.Default;
                Config.Save();
            }

            return Config.Retainers;
        }
    }
}

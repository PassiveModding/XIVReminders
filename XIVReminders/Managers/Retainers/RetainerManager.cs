using ImGuiNET;
using System;
using System.Threading;

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
                    if (Config.Retainers.HideRetainersUntilComplete) continue;
                    var diff = retainer.TimeStamp - DateTime.UtcNow;
                    Helpers.RenderRow($"{retainer.Name}", Helpers.FormatTimeSpan(diff));
                    continue;
                }

                if (retainer.TimeStamp.Year < 2000) Helpers.RenderRow($"{retainer.Name}", "Pending");
                else
                {
                    var since = DateTime.UtcNow - retainer.TimeStamp;
                    var sinceStr = Helpers.FormatTimeSpan(since);
                    Helpers.RenderRow($"{retainer.Name} complete", sinceStr.Length != 0 ? $"{sinceStr} ago" : "Now");
                }
            }
        }

        public void DrawExtraWindows()
        {

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

using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    bool propertyChanged = false;

                    var retainerId = retainer->RetainerID;
                    RetainerAlert retAlert;
                    if (Config.Retainers.PlayerRetainers.ContainsKey(retainerId))
                    {
                        retAlert = Config.Retainers.PlayerRetainers[retainerId];
                    }
                    else
                    {
                        propertyChanged = true;
                        retAlert = new RetainerAlert
                        {
                            RetainerId = retainerId,
                            IsHidden = false
                        };
                    }

                    var retainerName = Helpers.ReadBytesAsString(retainer->Name, 32);
                    if (retainerName != retAlert.Name)
                    {
                        propertyChanged = true;
                        retAlert.Name = retainerName;
                    }
                    var timestamp = Helpers.DateFromTimeStamp(retainer->VentureComplete);
                    if (retAlert.TimeStamp != timestamp)
                    {
                        propertyChanged = true;
                        retAlert.TimeStamp = timestamp;
                    }

                    if (propertyChanged)
                    {
                        Config.Retainers.PlayerRetainers[retAlert.RetainerId] = retAlert;
                        Config.Save();
                    }
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

        public bool TryFormatTitleContent(out string titleContent)
        {
            titleContent = "";
            if (Config?.Retainers == null) return false;
            if (Config.Retainers.Enabled == false) return false;
            var count = Config.Retainers.PlayerRetainers.Count(x => x.Value.TimeStamp < DateTime.UtcNow && !x.Value.IsHidden);
            if (count > 0)
            {
                titleContent = $"{count} Retainers";
                return true;
            }

            return false;
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

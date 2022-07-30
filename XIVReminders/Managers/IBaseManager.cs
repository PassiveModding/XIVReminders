using System.Threading;

namespace XIVReminders.Managers
{
    internal interface IBaseManager
    {
        public string Name { get; }
        void DrawConfigMenu();
        void DrawUiMenu();
        void DrawExtraWindows();
        public Config Config { get; init; }
        public Timer CheckLoop { get; init; }
        IManagerConfig GetConfig();
    }
}
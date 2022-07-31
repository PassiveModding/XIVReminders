using System.Threading;

namespace XIVReminders.Managers
{
    internal interface IBaseManager
    {
        public string Name { get; }
        void DrawConfigMenu();
        void DrawUiMenu();
        void DrawExtraWindows();
        public Timer CheckLoop { get; }
    }
}
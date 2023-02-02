using System.Threading;

namespace XIVReminders.Managers
{
    internal interface IBaseManager
    {
        public string Name { get; }
        void DrawConfigMenu();
        void DrawUiMenu();
        void DrawExtraWindows();
        bool TryFormatTitleContent(out string titleContent);
        public Timer CheckLoop { get; }
    }
}
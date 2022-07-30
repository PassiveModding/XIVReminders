using Dalamud.Interface;
using ImGuiNET;
using System;
using System.Numerics;
using System.Text;

namespace XIVReminders
{
    internal class Helpers
    {
        public static DateTime DateFromTimeStamp(uint timeStamp)
        {
            // 1970 epoch
            const long timeFromEpoch = 62135596800;
            if (timeStamp == 0u)
            {
                return DateTime.MinValue;
            }

            var ticks = (timeStamp + timeFromEpoch) * TimeSpan.TicksPerSecond;
            return new DateTime(ticks, DateTimeKind.Utc);
        }

        public static bool IsPlayerAvailable()
        {
            if (!Dalamud.State.IsLoggedIn) return false;
            if (Dalamud.State.LocalPlayer == null) return false;

            return true;
        }

        public static string FormatTimeSpan(TimeSpan ts)
        {
            var sb = new StringBuilder();
            if (ts.TotalDays >= 1) sb.Append($"{ts.Days}d ");
            if (ts.Hours > 0) sb.Append($"{ts.Hours}h ");
            if (ts.Minutes > 0) sb.Append($"{ts.Minutes}m ");
            if (ts.Seconds > 0) sb.Append($"{ts.Seconds}s ");

            return sb.ToString().TrimEnd();
        }

        public static void RenderRow(string name, string value)
        {
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.Text(name);
            ImGui.TableNextColumn();
            ImGui.Text(value);
        }
        public static bool IconButton(FontAwesomeIcon icon, string id, float? width = null)
        {
            var h = ImGui.CalcTextSize("").Y;
            ImGui.PushFont(UiBuilder.IconFont);
            var w = width ?? ImGui.CalcTextSize(icon.ToIconString()).X;
            var ret = ImGui.Button($"{icon.ToIconString()}##{id}", new Vector2(w, h) + ImGui.GetStyle().FramePadding * 2f);
            ImGui.PopFont();
            return ret;
        }

        public static unsafe string ReadBytesAsString(byte* bytes, int length)
        {
            var sb = new StringBuilder();
            // iterate chars in retainer name bytes to form string
            for (int y = 0; y < length; y++)
            {
                var cVal = (char)bytes[y];
                if (cVal == '\0') break;
                sb.Append(cVal);
            }
            return sb.ToString();
        }
    }
}

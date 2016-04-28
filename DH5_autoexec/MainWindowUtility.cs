using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoIt;

namespace DH5_autoexec
{
    static class MainWindowUtility
    {
        public const string MainWindowTitle = "[TITLE:Dungeon Hunter 5; CLASS:ApplicationFrameWindow]";

        public static bool ExistsMainWindow()
        {
            string title = MainWindowTitle;

            int handle = AutoItX.WinWait(title, "", 1);

            return handle != 0;
        }

        public static IntPtr GetMainWindowHandle()
        {
            string title = MainWindowTitle;

            int handle = AutoItX.WinWait(title, "", 3);

            if (handle == 0)
            {
                return IntPtr.Zero;
            }

            IntPtr hwnd = AutoItX.WinGetHandle(title, "");

            return hwnd;
        }

        public static void ActivateAndWait()
        {
            if (ExistsMainWindow())
            {
                AutoItX.WinActivate(MainWindowTitle);
                AutoItX.WinWaitActive(MainWindowTitle, "", 3);
            }
        }

        public static void Close()
        {
            AutoItX.WinClose(MainWindowTitle);
        }
    }
}

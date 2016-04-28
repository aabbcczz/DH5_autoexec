using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using AutoIt;

namespace DH5_autoexec
{
    static class CheatEngineUtility
    {
        public const string MainWindowTitle = "[TITLE:Cheat Engine 6.4]";

        public static bool ExistsMainWindow()
        {
            string title = MainWindowTitle;

            int handle = AutoItX.WinWait(title, "", 2);

            return handle != 0;
        }

        public static IntPtr GetMainWindowHandle()
        {
            string title = MainWindowTitle;

            int handle = AutoItX.WinWait(title, "", 10);

            if (handle == 0)
            {
                return IntPtr.Zero;
            }

            IntPtr hwnd = AutoItX.WinGetHandle(title, "");

            return hwnd;
        }

        public static bool EnableCheating()
        {
            IntPtr hwnd = GetMainWindowHandle();

            if (hwnd == IntPtr.Zero)
            {
                Console.WriteLine("failed to find cheat engine main window");
                return false;
            }

            // try to close the process list dialog firstly
            // now try to find the dialog
            string processListWindowTitle = "[Title:Process List]";

            int processListHandle = AutoItX.WinWait(processListWindowTitle, "", 10);
            IntPtr processListHwnd = IntPtr.Zero;

            if (processListHandle != 0)
            {
                processListHwnd = AutoItX.WinGetHandle(processListWindowTitle, "");
                if (processListHwnd == IntPtr.Zero)
                {
                    Console.WriteLine("failed to get handle of process list dialog");
                    return false;
                }

                AutoItX.WinSetState(processListHwnd, AutoItX.SW_FORCEMINIMIZE | AutoItX.SW_HIDE);
            }


            // move out mouse to avoid affect the menu item.
            //AutoItX.MouseMove(0, 0, 0);

            AutoItX.WinActivate(hwnd);

            AutoItX.WinWaitActive(hwnd, 10);

            AutoItX.Send("!f");
            AutoItX.Sleep(500);
            AutoItX.Send("{DOWN}");
            AutoItX.Sleep(500);
            AutoItX.Send("{DOWN}");
            AutoItX.Sleep(500);
            AutoItX.Send("{ENTER}");

            // now try to find the process list dialog
            processListHandle = AutoItX.WinWait(processListWindowTitle, "", 10);
            if (processListHandle == 0)
            {
                Console.WriteLine("failed to find process list dialog");
                return false;
            }

            processListHwnd = AutoItX.WinGetHandle(processListWindowTitle, "");
            if (processListHwnd == IntPtr.Zero)
            {
                Console.WriteLine("failed to get handle of process list dialog");
                return false;
            }

            AutoItX.WinSetState(processListHwnd, AutoItX.SW_RESTORE | AutoItX.SW_SHOW);
            AutoItX.WinActivate(processListHwnd);

            AutoItX.WinWaitActive(processListHwnd, 10);

            // get the Open button
            IntPtr openButtonHwnd = AutoItX.ControlGetHandle(processListHwnd, "[CLASSNN:Button3]");
            if (openButtonHwnd == IntPtr.Zero)
            {
                Console.WriteLine("failed to get the handle of open button");
                return false;
            }

            AutoItX.ControlClick(processListHwnd, openButtonHwnd);

            // wait for the first confirmation dialog
            string confirmationDialogTitle = "Confirmation";
            int confirmationHandle = AutoItX.WinWait(confirmationDialogTitle, "", 3);
            if (confirmationHandle == 0)
            {
                // it means the DH5_W8.exe is the same one, no change
                return true;
            }

            IntPtr confirmationHwnd = AutoItX.WinGetHandle(confirmationDialogTitle, "");
            if (confirmationHwnd == IntPtr.Zero)
            {
                Console.WriteLine("failed to get confirmation dialog handle");
                return false;
            }

            // get the No button
            IntPtr noButtonHwnd = AutoItX.ControlGetHandle(confirmationHwnd, "[CLASSNN:Button2]");
            if (noButtonHwnd == IntPtr.Zero)
            {
                Console.WriteLine("failed to get the handle of no button");
                return false;
            }

            AutoItX.ControlClick(confirmationHwnd, noButtonHwnd);

            AutoItX.Sleep(2000);

            // wait for the second confirmation dialog
            confirmationHandle = AutoItX.WinWait(confirmationDialogTitle, "", 3);
            if (confirmationHandle == 0)
            {
                Console.WriteLine("failed to wait for the second confirmation dialog");
                return false;
            }

            confirmationHwnd = AutoItX.WinGetHandle(confirmationDialogTitle, "");
            if (confirmationHwnd == IntPtr.Zero)
            {
                Console.WriteLine("failed to get the second confirmation dialog handle");
                return false;
            }

            // get the No button
            IntPtr yesButtonHwnd = AutoItX.ControlGetHandle(confirmationHwnd, "[CLASSNN:Button1]");
            if (noButtonHwnd == IntPtr.Zero)
            {
                Console.WriteLine("failed to get the handle of yes button");
                return false;
            }

            AutoItX.ControlClick(confirmationHwnd, yesButtonHwnd);
            AutoItX.Sleep(1000);

            // now go back the main window and try to enable the script
            AutoItX.WinActivate(hwnd);
            AutoItX.WinWaitActive(hwnd, 10);

            IntPtr listControlHwnd = AutoItX.ControlGetHandle(hwnd, "[CLASSNN:Window11]");
            if (listControlHwnd == IntPtr.Zero)
            {
                Console.WriteLine("failed to get the handle of list control");
                return false;
            }

            AutoItX.ControlClick(hwnd, listControlHwnd, "LEFT", 1, 10, 7);

            return true;
        }
     }
}

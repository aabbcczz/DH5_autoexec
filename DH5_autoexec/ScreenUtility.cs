using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AutoIt;

namespace DH5_autoexec
{
    static class ScreenUtility
    {
        private const int BaseResolutionX = 1680;
        private const int BaseResolutionY = 1050;

        private const int BaseWindowLeftOffset = 0;
        private const int BaseWindowTopOffset = 32;
        private const int BaseWindowRightOffset = 0;
        private const int BaseWindowBottomOffset = 40;

        private const int BaseWindowWidth = BaseResolutionX - BaseWindowLeftOffset - BaseWindowRightOffset;
        private const int BaseWindowHeight = BaseResolutionY - BaseWindowTopOffset - BaseWindowBottomOffset;

        static bool _isFullScreen;
        static Rectangle _screenRect;

        public static bool IsFullScreen
        {
            get { return _isFullScreen; }
        }

        public static void SetScreenAttribute(Rectangle screenRect, bool isFullScreen)
        {
            _isFullScreen = isFullScreen;
            _screenRect = screenRect;
        }

        public static Point Convert(Point orignal)
        {
            if (_isFullScreen)
            {
                var x = orignal.X - BaseWindowLeftOffset;
                var y = orignal.Y - BaseWindowTopOffset;

                var newX = (int)Math.Round((double)x * _screenRect.Width / BaseWindowWidth);
                var newY = (int)Math.Round((double)y * _screenRect.Height / BaseWindowHeight);

                newX += _screenRect.Left;
                newY += _screenRect.Top;

                return new Point(newX, newY);
            }
            else
            {
                return orignal;
            }
        }

        public static void Click(int x, int y)
        {
            Point newPos = Convert(new Point(x, y));
            AutoItX.MouseClick("LEFT", newPos.X, newPos.Y);
        }

        public static void Drag(int x1, int y1, int x2, int y2)
        {
            Point newPos1 = Convert(new Point(x1, y1));
            Point newPos2 = Convert(new Point(x2, y2));

            AutoItX.MouseClickDrag("LEFT", newPos1.X, newPos1.Y, newPos2.X, newPos2.Y);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(HandleRef hWnd, [In, Out] ref RECT rect);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(HandleRef hWnd, [In, Out] ref RECT rect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static bool IsForegroundFullScreen()
        {
            return IsWindowFullScreen(null, GetForegroundWindow());
        }

        public static bool IsWindowFullScreen(Screen screen, IntPtr hwnd)
        {
            return GetWindowBounds(hwnd).Contains(GetScreenBounds(screen));
        }

        public static Rectangle GetScreenBounds(Screen screen)
        {
            if (screen == null)
            {
                screen = Screen.PrimaryScreen;
            }

            return screen.Bounds;

        }
        public static Rectangle GetWindowBounds(IntPtr hwnd)
        {
            RECT rect = new RECT();
            GetWindowRect(new HandleRef(null, hwnd), ref rect);
            return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }

        public static Rectangle GetClientRect(IntPtr hwnd)
        {
            RECT rect = new RECT();
            GetClientRect(new HandleRef(null, hwnd), ref rect);
            return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }
    }


}

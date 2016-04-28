using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoIt;

namespace DH5_autoexec
{
    class AutomationDetector
    {
        private List<uint> recentColors = new List<uint>();
        private const int xCord = 753;
        private const int yCord = 970;

        private const int detectionCount = 10;

        public bool AutomationEnabled { get; private set; }

        public AutomationDetector()
        {
            AutomationEnabled = true;
        }

        public void Detect()
        {
            uint color = ((uint)AutoItX.PixelGetColor(xCord, yCord)) & 0xFFFFFF;

            Console.WriteLine("Automation detect: color = {0:X8}", color);

            recentColors.Add(color);

            if (recentColors.Count > detectionCount)
            {
                recentColors.RemoveAt(0);

                for (int i = 0; i < recentColors.Count - 1; ++i)
                {
                    if (recentColors[i] != recentColors[i + 1])
                    {
                        AutomationEnabled = true;
                        return;
                    }
                }

                AutomationEnabled = false;
            }
        }

        public void EnableAutomation()
        {
            ScreenUtility.Click(xCord, yCord);

            recentColors.Clear();

            AutomationEnabled = true;
        }
    }
}

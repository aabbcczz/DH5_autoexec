using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoIt;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace DH5_autoexec
{
    class Program
    {
        const int MaxSingleTaskTime = 120 * 1000;

        static int _dungeonSequence = 0;

        static StateWaiter _stateWaiter = new StateWaiter();

        static void Main(string[] args)
        {
            string target = "Dungeon Hunter 5.lnk";

            int runCount = 20;

            if (args.Length > 0)
            {
                runCount = int.Parse(args[0]);
            }

            JobType job = JobType.DailyDungeon;

            if (args.Length > 1)
            {
                if (string.Compare(args[1], "wanted") == 0)
                {
                    job = JobType.Wanted;
                }
                else if (string.Compare(args[1], "dungeon") == 0)
                {
                    job = JobType.DailyDungeon;
                }
                else if (string.Compare(args[1], "element") == 0)
                {
                    job = JobType.ElementChallenge;
                }
                else if (string.Compare(args[1], "chest") == 0)
                {
                    job = JobType.OpenChest;
                }
                else
                {
                    Console.WriteLine("only support wanted, dungeon, element, chest as job type");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    return;
                }
            }

            if (args.Length > 2)
            {
                target = args[2];
            }

            if (job == JobType.OpenChest)
            {
                OpenChest();
                return;
            }

            while (runCount > 0)
            {
                Run(target, job);

                runCount--;

                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void OpenChest()
        {
            bool existsMainWindow = MainWindowUtility.ExistsMainWindow();

            if (!existsMainWindow && !ScreenUtility.IsForegroundFullScreen())
            {
                Console.WriteLine("No main window exist");
                return;
            }

            if (existsMainWindow)
            {
                MainWindowUtility.ActivateAndWait();
            }

            // open chest
            for (int i = 0; i < 5000; ++i)
            {
                ScreenUtility.Click(177, 877);

                AutoItX.Sleep(1000);
            }

            return;

        }

        static void Run(string target, JobType job)
        {
            bool existsMainWindow = MainWindowUtility.ExistsMainWindow();

            if (!existsMainWindow && !ScreenUtility.IsForegroundFullScreen())
            {
                if (!Launch(target))
                {
                    return;
                }
            }
            else
            {
                Console.WriteLine("Main windows had been opened");
            }

            if (existsMainWindow)
            {
                MainWindowUtility.ActivateAndWait();
            }

            var screenBounds = ScreenUtility.GetScreenBounds(null);
            Console.WriteLine("Screen: <{0}, {1}, {2}, {3}>",
                screenBounds.Top, screenBounds.Left, screenBounds.Height, screenBounds.Width);

            IntPtr hwnd = IntPtr.Zero;
            if (!ScreenUtility.IsForegroundFullScreen())
            {
                hwnd = MainWindowUtility.GetMainWindowHandle();

                var windowBounds = ScreenUtility.GetWindowBounds(hwnd);
                var clientRect = ScreenUtility.GetClientRect(hwnd);

                Console.WriteLine("Window: <{0}, {1}, {2}, {3}>",
                    windowBounds.Top, windowBounds.Left, windowBounds.Height, windowBounds.Width);

                Console.WriteLine("Client: <{0}, {1}, {2}, {3}>",
                    clientRect.Top, clientRect.Left, clientRect.Height, clientRect.Width);

                ScreenUtility.SetScreenAttribute(screenBounds, false);
                //int left, width, top, height;
                //if (windowBounds.Left < 0)
                //{
                //    width = windowBounds.Width - 2 * Math.Abs(windowBounds.Left);
                //    left = 0;
                //}
                //else
                //{
                //    width = windowBounds.Width;
                //    left = windowBounds.Left;
                //}

                //if (windowBounds.Top < 0)
                //{
                //    height = windowBounds.Height - 2 * Math.Abs(windowBounds.Top);
                //    top = 0;
                //}
                //else
                //{
                //    height = windowBounds.Height;
                //    top = windowBounds.Top;
                //}
            }
            else
            {
                ScreenUtility.SetScreenAttribute(screenBounds, true);
            }

            if (!_stateWaiter.WaitForState(GameState.Unknown, GameState.MainWindow, null, MaxSingleTaskTime))
            { 
                Console.WriteLine("Failed to show main window although main window was opened");
                MainWindowUtility.Close();

                return;
            }

            if (job == JobType.ElementChallenge)
            {
                if (!CheatEngineUtility.EnableCheating())
                {
                    Console.WriteLine("Cheating is not enabled, can't run element challenging job");
                    return;
                }
            }

            if (!ScreenUtility.IsForegroundFullScreen())
            {
                MainWindowUtility.ActivateAndWait();
            }

            Run(job);

            if (_stateWaiter.Detector.Detect() != GameState.MainWindow)
            {
                Console.WriteLine("Failed to go back main window after run");
                MainWindowUtility.Close();

                return;
            }
        }

        static bool Launch(string shortCutLink)
        {
            if (!Path.IsPathRooted(shortCutLink))
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                shortCutLink = Path.Combine(desktopPath, shortCutLink);
            }
            
            Console.WriteLine("Try to launch {0}", shortCutLink);

            Process process = new Process();

            try
            {
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = shortCutLink;
                process.StartInfo.CreateNoWindow = false;

                process.Start();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to launch {0}: Exception {1}", shortCutLink, ex);
                return false;
            }
        }

        static void EnterDailyDungeon()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                // slide to 熔铸辅助材料
                ScreenUtility.Drag(1500, 914, 1500, 314);
                AutoItX.Sleep(1500);

                // 星期一有水晶和熔铸，需要多滑动一次
                //ScreenUtility.Drag(1500, 914, 1500, 314);
                //AutoItX.Sleep(1500);
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                if (DateTime.Now.Hour < 13)
                {
                    // do nothing
                }
                else
                {
                    // slide to 禁忌材料
                    ScreenUtility.Drag(1500, 914, 1500, 314);
                    AutoItX.Sleep(1500);
                }
            }

            // enter 熔铸辅助材料
            ScreenUtility.Click(1500, 914);
        }

        static void EnterWanted()
        {
            // slide to 通缉
            //ScreenUtility.Drag(1500, 914, 1500, 314);
            //AutoItX.Sleep(1500);
            //ScreenUtility.Drag(1500, 914, 1500, 314);
            //AutoItX.Sleep(1500);

            // enter 通缉
            ScreenUtility.Click(1500, 914);
        }

        static void EnterElementChallenge()
        {
            // enter element challenge
            ScreenUtility.Click(1500, 314);
        }

        static void SelectDungeonGrade(DayOfWeek weekday)
        {
            if (weekday == DayOfWeek.Sunday || weekday == DayOfWeek.Saturday)
            {
                throw new ArgumentOutOfRangeException();
            }

            int order;
            switch (weekday)
            {
                case DayOfWeek.Monday:
                    // 熔铸材料
                    // click 专家
                    ScreenUtility.Click(1478, 610);
                    AutoItX.Sleep(1000);
                    break;

                case DayOfWeek.Tuesday:
                    if (DateTime.Now.Hour < 13)
                    {
                        // still the 熔铸材料
                        // click 专家
                        ScreenUtility.Click(1478, 610);
                        AutoItX.Sleep(1000);
                    }
                    else
                    {
                        // click 史诗
                        ScreenUtility.Click(1478, 760);
                        AutoItX.Sleep(1000);
                    }
                    break;

                case DayOfWeek.Wednesday:
                    order = _dungeonSequence % 3;
                    if (order >= 0)
                    {
                        // click 困难
                        //ScreenUtility.Click(1478, 460);
                        //AutoItX.Sleep(1000);
                    }

                    if (order >= 0)
                    {
                        // click 专家
                        //ScreenUtility.Click(1478, 610);
                        //AutoItX.Sleep(1000);
                    }

                    if (order >= 0)
                    {
                        // click 史诗
                        ScreenUtility.Click(1478, 760);
                        AutoItX.Sleep(1000);
                    }

                    break;
                case DayOfWeek.Thursday:
                    //Thursday: 清流 320，烈焰440, 自然 560, 半影 680， 多彩810
                    order = _dungeonSequence % 5;

                    int xCord = 1478;
                    int[] yCords = new int[5] { 320, 440, 560, 680, 810 };

                    // 半影包含两个，要特殊处理
                    int trueOrder = (order == 3 || order == 4) ? 3 : order;

                    // 目前只刷多彩，刷其它水晶时候请注释这行
                    trueOrder = 4;

                    int yCord = yCords[trueOrder];

                    ScreenUtility.Click(xCord, yCord);
                    AutoItX.Sleep(1000);
                    break;
                case DayOfWeek.Friday:
                    // Friday
                    order = _dungeonSequence % 2;

                    if (order >= 0)
                    {
                        // click 史诗
                        ScreenUtility.Click(1478, 320);
                        AutoItX.Sleep(1000);
                    }

                    if (order >= 1)
                    {
                        // click 传奇
                        ScreenUtility.Click(1478, 470);
                        AutoItX.Sleep(1000);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _dungeonSequence++;

            // click 继续
            ScreenUtility.Click(1478, 920);
        }

        static void SelectWanted()
        {
            // click 开始
            ScreenUtility.Click(1260, 928);
        }

        static void SelectWantedDifficulty()
        {
            // 选择 传奇
            ScreenUtility.Click(400, 900);
            AutoItX.Sleep(1000);

            // for rush
            ScreenUtility.Click(1020, 920);
            // click 开始游戏
            //ScreenUtility.Click(1460, 920);
        }

        static void SelectElementChallenge()
        {
            // click 开始
            ScreenUtility.Click(1260, 928);
        }

        static void SelectElementChallengeLevel()
        {
            // click 开始游戏
            ScreenUtility.Click(1460, 920);
        }


        static void EnterSelectFightType()
        {
            ScreenUtility.Click(1562, 814);

            // wait for 2s to ensure all fights are populated.
            AutoItX.Sleep(2000); 
        }

        static void SelectedEquipment()
        {
            // click the 继续
            ScreenUtility.Click(1478, 920);
        }

        static void SelectPartner()
        {
            // select first partner if possible
            ScreenUtility.Click(1120, 280);
            AutoItX.Sleep(1000);

            // click 开始
            ScreenUtility.Click(1478, 920);
        }

        static void Fight()
        {
            AutomationDetector detector = new AutomationDetector();

            // 启用自动化
            detector.EnableAutomation();

            DateTime startBattleTime = DateTime.Now;

            Random rand = new Random();
            string[] keys = new string[] { "a", "s", "w", "d" };

            // wait for combat ending
            while (_stateWaiter.Detector.Detect() == GameState.Fighting)
            {
                // 判断 自动 是否被启用
                if (!detector.AutomationEnabled)
                {
                    // send some random keys before enable automation
                    for (int i = 0; i < 100; ++i)
                    {
                        AutoItX.Send(keys[rand.Next() % keys.Length]);
                    }

                    detector.EnableAutomation();
                }
                else
                {
                    detector.Detect();
                }

                AutoItX.Sleep(500);

                DateTime current = DateTime.Now;
                TimeSpan battleTime = current - startBattleTime;

                if (battleTime.TotalMinutes > 10)
                {
                    // overtime, must be stuck
                    Console.WriteLine("Battle over 10 minutes, might be stuck");
                    MainWindowUtility.Close();
                    return;
                }
            }

        }

        static void RunDungeon()
        {
            var weekday = DateTime.Now.DayOfWeek;

            if (weekday == DayOfWeek.Saturday || weekday == DayOfWeek.Sunday)
            {
                return;
            }

            if (!ScreenUtility.IsForegroundFullScreen())
            {
                MainWindowUtility.ActivateAndWait();
            }

            if (!_stateWaiter.WaitForState(GameState.Unknown, GameState.MainWindow, null, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.MainWindow, GameState.FightTypeSelection, EnterSelectFightType, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.FightTypeSelection, GameState.DailyDungeon, EnterDailyDungeon, MaxSingleTaskTime))
            {
                return;
            }

            SelectDungeonGrade(weekday);

            if (!_stateWaiter.WaitForState(GameState.Unknown, GameState.SelectEquipment, null, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.SelectEquipment, GameState.SelectPartner, SelectedEquipment, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.SelectPartner, GameState.Fighting, SelectPartner, MaxSingleTaskTime))
            {
                return;
            }

            Fight();

            if (!_stateWaiter.WaitForState(GameState.Unknown, GameState.MainWindow, null, MaxSingleTaskTime))
            {
                return;
            }
        }

        static void RunWanted()
        {
            if (!ScreenUtility.IsForegroundFullScreen())
            {
                MainWindowUtility.ActivateAndWait();
            }

            if (!_stateWaiter.WaitForState(GameState.Unknown, GameState.MainWindow, null, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.MainWindow, GameState.FightTypeSelection, EnterSelectFightType, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.FightTypeSelection, GameState.WantedChallenge, EnterWanted, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.WantedChallenge, GameState.WantedDifficulty, SelectWanted, MaxSingleTaskTime))
            {
                return;
            }

            SelectWantedDifficulty();

            //for (; ; )
            //{
            //    // rush only
            //    SelectWantedDifficulty();
            //    AutoItX.Sleep(1000);
            //    _stateWaiter.WaitForState(GameState.Unknown, GameState.WantedDifficulty, null, MaxSingleTaskTime);
            //}

            if (!_stateWaiter.WaitForState(GameState.Unknown, GameState.SelectEquipment, null, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.SelectEquipment, GameState.SelectPartner, SelectedEquipment, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.SelectPartner, GameState.Fighting, SelectPartner, MaxSingleTaskTime))
            {
                return;
            }

            Fight();

            if (!_stateWaiter.WaitForState(GameState.Unknown, GameState.WantedChallenge, null, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.WantedChallenge, GameState.MainWindow, GoBackMainWindowFromWantedOrElementChallenge, MaxSingleTaskTime))
            {
                return;
            }

            return;

        }

        static void RunElementChallenge()
        {
            if (!ScreenUtility.IsForegroundFullScreen())
            {
                MainWindowUtility.ActivateAndWait();
            }

            if (!_stateWaiter.WaitForState(GameState.Unknown, GameState.MainWindow, null, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.MainWindow, GameState.FightTypeSelection, EnterSelectFightType, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.FightTypeSelection, GameState.ElementChallenge, EnterElementChallenge, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.ElementChallenge, GameState.ElementChallengeLevel, SelectElementChallenge, MaxSingleTaskTime))
            {
                return;
            }

            SelectElementChallengeLevel();

            if (!_stateWaiter.WaitForState(GameState.Unknown, GameState.SelectEquipment, null, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.SelectEquipment, GameState.SelectPartner, SelectedEquipment, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.SelectPartner, GameState.Fighting, SelectPartner, MaxSingleTaskTime))
            {
                return;
            }

            Fight();

            if (!_stateWaiter.WaitForState(GameState.Unknown, GameState.ElementChallengeCompleted, null, MaxSingleTaskTime))
            {
                return;
            }

            AutoItX.Sleep(1000);
            AutoItX.Send("{ESC}");

            if (!_stateWaiter.WaitForState(GameState.Unknown, GameState.ElementChallenge, null, MaxSingleTaskTime))
            {
                return;
            }

            if (!_stateWaiter.WaitForState(GameState.ElementChallenge, GameState.MainWindow, GoBackMainWindowFromWantedOrElementChallenge, MaxSingleTaskTime))
            {
                return;
            }
        }

        static void Run(JobType job)
        {
            switch (job)
            {
                case JobType.DailyDungeon:
                    RunDungeon();
                    break;
                case JobType.Wanted:
                    RunWanted();
                    break;
                case JobType.ElementChallenge:
                    RunElementChallenge();
                    break;
                default:
                    Console.WriteLine("Unsupported job {0}", job);
                    return;
            }
        }
 

        static void GoBackMainWindowFromWantedOrElementChallenge()
        {
            // 点击 返回主界面箭头
            ScreenUtility.Click(76, 94);
        }
    }
}

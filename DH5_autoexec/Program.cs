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

        static StateWaiter _stateWaiter = new StateWaiter();

        static int _position;
        static int _grade;

        static void Main(string[] args)
        {
            string target = "Dungeon Hunter 5.lnk";

            if (args.Length < 4)
            {
                Console.WriteLine("Invalid number of parameter");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }

            int runCount = int.Parse(args[0]);

            JobType job = JobType.DailyDungeon;

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

            _position = int.Parse(args[2]);

            if (_position <= 0)
            {
                Console.WriteLine("position must be greater than 0");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }

            _grade = int.Parse(args[3]);
            if (_grade <= 0)
            {
                Console.WriteLine("grade must be greater than 0");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }

            if (job == JobType.OpenChest)
            {
                OpenChest(runCount);
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

        static void OpenChest(int runCount)
        {
            //bool existsMainWindow = MainWindowUtility.ExistsMainWindow();

            //if (!existsMainWindow && !ScreenUtility.IsForegroundFullScreen())
            //{
            //    Console.WriteLine("No main window exist");
            //    return;
            //}

            //if (existsMainWindow)
            //{
            //    MainWindowUtility.ActivateAndWait();
            //}

            // open chest
            for (int i = 0; i < runCount; ++i)
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
                //if (!CheatEngineUtility.EnableCheating())
                //{
                //    Console.WriteLine("Cheating is not enabled, can't run element challenging job");
                //    return;
                //}
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

        static void EnterMatch()
        {
            int position = _position;

            if (position == 0)
            {
                Console.WriteLine("Invalid position 0");
                return;
            }

            while (position > 2)
            {
                ScreenUtility.Drag(1500, 914, 1500, 314);
                position--;
            }

            if (position == 1)
            {
                ScreenUtility.Click(1500, 314);
            }
            else if (position == 2)
            {
                ScreenUtility.Click(1500, 914);
            }
        }
 
        static void SelectDungeonGrade(DayOfWeek weekday)
        {
            if (weekday == DayOfWeek.Sunday || weekday == DayOfWeek.Saturday)
            {
                throw new ArgumentOutOfRangeException();
            }

            int grade = _grade;

            switch (weekday)
            {
                case DayOfWeek.Monday:
                    {
                        int xCord = 1478;
                        // 普通，困难，专家
                        int[] yCords = new int[] { 310, 460, 610 };

                        if (grade > yCords.Length)
                        {
                            grade = yCords.Length;
                        }

                        ScreenUtility.Click(xCord, yCords[grade - 1]);
                        AutoItX.Sleep(1000);
                        break;
                    }
                case DayOfWeek.Tuesday:
                    {
                        int xCord = 1478;
                        // 普通，困难，专家，史诗
                        int[] yCords = new int[] { 310, 460, 610, 760 };

                        if (grade > yCords.Length)
                        {
                            grade = yCords.Length;
                        }

                        ScreenUtility.Click(xCord, yCords[grade - 1]);
                        AutoItX.Sleep(1000);
                        break;
                    }
                case DayOfWeek.Wednesday:
                    {
                        int xCord = 1478;
                        // 普通，困难，专家，史诗
                        int[] yCords = new int[] { 310, 460, 610, 760 };

                        if (grade > yCords.Length)
                        {
                            grade = yCords.Length;
                        }

                        ScreenUtility.Click(xCord, yCords[grade - 1]);
                        AutoItX.Sleep(1000);
                        break;
                    }
                case DayOfWeek.Thursday:
                    {
                        //Thursday: 清流 320，烈焰440, 自然 560, 半影 680， 多彩810
                        int xCord = 1478;
                        int[] yCords = new int[5] { 320, 440, 560, 680, 810 };

                        if (grade > yCords.Length)
                        {
                            grade = yCords.Length;
                        }

                        ScreenUtility.Click(xCord, yCords[grade - 1]);
                        AutoItX.Sleep(1000);
                        break;
                    }
                case DayOfWeek.Friday:
                    {
                        int xCord = 1478;
                        // 史诗, 传奇
                        int[] yCords = new int[] { 320, 470 };

                        if (grade > yCords.Length)
                        {
                            grade = yCords.Length;
                        }

                        ScreenUtility.Click(xCord, yCords[grade - 1]);
                        AutoItX.Sleep(1000);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

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
            // ScreenUtility.Click(1020, 920);
            // click 开始游戏
            ScreenUtility.Click(1460, 920);
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

        static void TouchToContinue()
        {
            ScreenUtility.Click(1168, 985);
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

            if (!_stateWaiter.WaitForState(
                GameState.FightTypeSelection, 
                GameState.DailyDungeon,
                EnterMatch,
                MaxSingleTaskTime))
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

            if (!_stateWaiter.WaitForState(GameState.SelectPartner, GameState.Fighting, SelectPartner, MaxSingleTaskTime, TouchToContinue))
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

            if (!_stateWaiter.WaitForState(
                GameState.FightTypeSelection, 
                GameState.WantedChallenge,
                EnterMatch,
                MaxSingleTaskTime))
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

            if (!_stateWaiter.WaitForState(GameState.SelectPartner, GameState.Fighting, SelectPartner, MaxSingleTaskTime, TouchToContinue))
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

            if (!_stateWaiter.WaitForState(
                GameState.FightTypeSelection, 
                GameState.ElementChallenge, 
                EnterMatch,
                MaxSingleTaskTime))
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

            if (!_stateWaiter.WaitForState(GameState.SelectPartner, GameState.Fighting, SelectPartner, MaxSingleTaskTime, TouchToContinue))
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

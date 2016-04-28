using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoIt;
using System.Drawing;

namespace DH5_autoexec
{
    class StateDetector
    {
        private List<Tuple<GameState, List<PixelColor>>> _stateDetectionPixels =
            new List<Tuple<GameState, List<PixelColor>>>()
            {
                Tuple.Create(
                    GameState.DialogWarningOfDisconnect,
                    new List<PixelColor>()
                    {
                        new PixelColor(863, 418, 0xF8F2EC),
                        new PixelColor(880, 418, 0xF8F2EC),
                        new PixelColor(880, 429, 0xF8F2EC),
                    }),

                Tuple.Create(
                    GameState.DialogSendAllyRequest,
                    new List<PixelColor>()
                    {
                        new PixelColor(1606, 94, 0x4D4D4D),
                        new PixelColor(264, 91, 0x4D4B31),
                        new PixelColor(1004, 295, 0xF9F8F2),
                    }),

                Tuple.Create(
                    GameState.DialogBuyEnergy1,
                    new List<PixelColor>()
                    {
                        new PixelColor(1606, 94, 0x666666),
                        new PixelColor(746, 354, 0xF8F2EC),
                        new PixelColor(934, 395, 0xF8F2EC),
                    }),

                Tuple.Create(
                    GameState.DialogBuyEnergy2,
                    new List<PixelColor>()
                    {
                        new PixelColor(1606, 94, 0x666666),
                        new PixelColor(746, 351, 0xF8F2EC),
                        new PixelColor(934, 391, 0xF8F2EC),
                    }),

                Tuple.Create(
                    GameState.DialogUseFreeEnergy,
                    new List<PixelColor>()
                    {
                        new PixelColor(1606, 94, 0x666666),
                        new PixelColor(715, 425, 0xF8F2EC),
                        new PixelColor(870, 420, 0xF8F2EC),
                        new PixelColor(934, 455, 0xF8F2EC),
                    }),

                Tuple.Create(
                    GameState.DialogAdvertisement,
                    new List<PixelColor>()
                    {
                        new PixelColor(1424, 128, 0xA90606),
                        new PixelColor(1442, 146, 0xF11D23),
                        new PixelColor(1407, 144, 0x9F0002),
                    }),

                Tuple.Create(
                    GameState.DialogGetAward,
                    new List<PixelColor>()
                    {
                        new PixelColor(1323, 210, 0xF20B0B),
                        new PixelColor(619, 252, 0xF9F8F2),
                        new PixelColor(692, 254, 0xF9F8F2),
                        new PixelColor(1049, 247, 0xF9F8F2),
                    }),

                Tuple.Create(
                    GameState.DialogLossConnection,
                    new List<PixelColor>()
                    {
                        new PixelColor(680, 296, 0xF9F8F2),
                        new PixelColor(745, 293, 0xF9F8F2),
                        new PixelColor(809, 295, 0xF9F8F2),
                        new PixelColor(917, 305, 0xF9F8F2),
                    }),

                Tuple.Create(
                    GameState.DialogChat,
                    new List<PixelColor>()
                    {
                        new PixelColor(1550, 64, 0x333333),
                        new PixelColor(1560, 64, 0x333333),
                        new PixelColor(1560, 74, 0x333333),
                        new PixelColor(650, 222, 0xFFFFFF),
                        new PixelColor(750, 222, 0xFFFFFF),
                        new PixelColor(850, 222, 0xFFFFFF),
                        new PixelColor(950, 222, 0xFFFFFF),
                    }),

                Tuple.Create(
                    GameState.DialogWantedCompleted,
                    new List<PixelColor>()
                    {
                        new PixelColor(648, 139, 0xF0FFA0),
                        new PixelColor(715, 125, 0xF0FFA0),
                        new PixelColor(846, 152, 0xF0FFA0),
                        new PixelColor(942, 126, 0xF0FFA0),

                    }),

                Tuple.Create(
                    GameState.DialogDungeonCompleted,
                    new List<PixelColor>()
                    {
                        new PixelColor(643, 155, 0xEEC929),
                        new PixelColor(735, 143, 0xEEC929),
                        new PixelColor(847, 160, 0xEEC929),
                        new PixelColor(961, 180, 0xEEC929),

                    }),

                Tuple.Create(
                    GameState.DialogElementChallengeCompleted,
                    new List<PixelColor>()
                    {
                        new PixelColor(627, 920, 0xEEE5DB),
                        new PixelColor(694, 920, 0xEEE5DB),
                        new PixelColor(997, 958, 0xEEE5DB),

                    }),

                Tuple.Create(
                    GameState.DialogElementChallengePaused,
                    new List<PixelColor>()
                    {
                        new PixelColor(766, 164, 0xF9F6C9),
                        new PixelColor(841, 140, 0xF9F6C9),
                    }),

                //Tuple.Create(
                //    GameState.DialogTouchToContinue,
                //    new List<PixelColor>()
                //    {
                //        new PixelColor(237, 33, PixelColor.SpecialColorForDetectingUnchange),
                //        new PixelColor(926, 928, PixelColor.SpecialColorForDetectingChange),
                //    }),

                Tuple.Create(
                    GameState.DialogGiveUp,
                    new List<PixelColor>()
                    {
                        new PixelColor(1323, 200, 0xF20B0B),
                        new PixelColor(810, 417, 0xF8F2EC),
                        new PixelColor(880, 439, 0xF8F2EC),
                    }),

                Tuple.Create(
                    GameState.MainWindow,
                    new List<PixelColor>()
                    {
                        new PixelColor(1550, 64, 0xFFFFFF),
                        new PixelColor(1560, 64, 0xFFFFFF),
                        new PixelColor(1560, 74, 0xFFFFFF),
                    }),

                Tuple.Create(
                    GameState.FightTypeSelection,
                    new List<PixelColor>()
                    {
                        new PixelColor(64, 112, 0xECECEC),
                        new PixelColor(251, 90, 0xFFF7A1),
                        new PixelColor(251, 105, 0xFFF7A1),
                    }),

                Tuple.Create(
                    GameState.WantedChallenge,
                    new List<PixelColor>()
                    {
                        new PixelColor(72, 94, 0xEFEFEF),
                        new PixelColor(1606, 94, 0xFFFFFF),
                        new PixelColor(394, 105, 0xFFF7A1),
                        new PixelColor(195, 108, 0xFFF7A1),
                        new PixelColor(485, 98, 0x000000),
                    }),

                Tuple.Create(
                    GameState.ElementChallenge,
                    new List<PixelColor>()
                    {
                        new PixelColor(72, 94, 0xEFEFEF),
                        new PixelColor(1606, 94, 0xFFFFFF),
                        new PixelColor(206, 86, 0xFFF7A1),
                        new PixelColor(337, 89, 0xFFF7A1),
                        new PixelColor(65, 193, 0xEBE2D8),
                    }),

                Tuple.Create(
                    GameState.ElementChallengeLevel,
                    new List<PixelColor>()
                    {
                        new PixelColor(72, 94, 0xEFEFEF),
                        new PixelColor(1606, 94, 0xFFFFFF),
                        new PixelColor(206, 86, 0xFFF7A1),
                        new PixelColor(337, 89, 0xFFF7A1),
                        new PixelColor(55, 195, 0xEEE5DB),
                        new PixelColor(186, 179, 0xEDE4DA),
                    }),

                Tuple.Create(
                    GameState.DailyDungeon,
                    new List<PixelColor>()
                    {
                        new PixelColor(72, 94, 0xEFEFEF),
                        new PixelColor(1606, 94, 0xFFFFFF),
                        new PixelColor(198, 95, 0xFFF7A1),
                        new PixelColor(254, 92, 0xFFF7A1),
                        new PixelColor(384, 89, 0xFFF7A1),
                        new PixelColor(352, 108, 0xFFF7A1),
                    }),

                Tuple.Create(
                    GameState.WantedDifficulty,
                    new List<PixelColor>()
                    {
                        new PixelColor(72, 94, 0xEFEFEF),
                        new PixelColor(1606, 94, 0xFFFFFF),
                        new PixelColor(394, 105, 0xFFF7A1),
                        new PixelColor(195, 108, 0xFFF7A1),
                        new PixelColor(485, 98, 0xFFF7A1),
                    }),

                Tuple.Create(
                    GameState.SelectEquipment,
                    new List<PixelColor>()
                    {
                        new PixelColor(72, 94, 0xEFEFEF),
                        new PixelColor(1606, 94, 0xFFFFFF),
                        new PixelColor(195, 111, 0xFFF7A1),
                        new PixelColor(433, 103, 0xFFF7A1),
                        new PixelColor(498, 86, 0xFFF7A1),
                    }),

                Tuple.Create(
                    GameState.SelectPartner,
                    new List<PixelColor>()
                    {
                        new PixelColor(1606, 94, 0xFFFFFF),
                        new PixelColor(160, 485, PixelColor.SpecialColorForDetectingChange),
                    }),

                Tuple.Create(
                    GameState.ElementChallengeCompleted,
                    new List<PixelColor>()
                    {
                        new PixelColor(1624, 260, 0x808080),
                        new PixelColor(1638, 260, 0x808080),
                        new PixelColor(58, 190, 0xA5FB29),
                    }),

                Tuple.Create(
                    GameState.Fighting,
                    new List<PixelColor>()
                    {
                        new PixelColor(1624, 260, 0x808080),
                        new PixelColor(1638, 260, 0x808080),
                    }),
            };

        public GameState Detect()
        {
            List<GameState> states = new List<GameState>();

            foreach (var t in _stateDetectionPixels)
            {
                bool detected = true;
                Console.WriteLine("Try to detect state {0}", t.Item1);

                foreach (var p in t.Item2)
                {
                    if (!FuzzyMatch(p.X, p.Y, p.Color))
                    {
                        Console.WriteLine("FuzzyMatch({0}, {1}, {2:X08}) failed", p.X, p.Y, p.Color);

                        detected = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("FuzzyMatch({0}, {1}, {2:X08}) succeeded", p.X, p.Y, p.Color);
                    }
                }

                if (detected)
                {
                    return t.Item1;
                }
            }

            return GameState.Unknown;
        }

        private static bool DetectChangeOrNotChange(int x, int y, uint color)
        {
            uint previousColor = GetPixelColor(x, y);

            int consistentCount = 0;

            for (int i = 0; i < 10; ++i)
            {
                AutoItX.Sleep(100);

                uint currentColor = GetPixelColor(x, y);

                if (currentColor == previousColor)
                {
                    ++consistentCount;
                }

                previousColor = currentColor;
            }

            bool detected = true;
            if ((color == PixelColor.SpecialColorForDetectingChange && consistentCount >= 5)
                || (color == PixelColor.SpecialColorForDetectingUnchange && consistentCount <= 5))
            {
                detected = false;
            }

            return detected;
        }

        private static uint GetPixelColor(int x, int y)
        {
            return ((uint)AutoItX.PixelGetColor(x, y)) & 0x00FFFFFF;
        }

        private static bool FuzzyMatch(int x, int y, uint color)
        {
            Point newPos = ScreenUtility.Convert(new Point(x, y));

            Console.WriteLine("<{0}, {1}> -> <{2}, {3}>", x, y, newPos.X, newPos.Y);
            if (ScreenUtility.IsFullScreen)
            {
                Point[] points = new Point[]
                {
                    new Point(newPos.X - 1, newPos.Y),
                    new Point(newPos.X, newPos.Y),
                    new Point(newPos.X + 1, newPos.Y),
                    new Point(newPos.X, newPos.Y - 1),
                    new Point(newPos.X, newPos.Y + 1),
                };

                foreach (var point in points)
                {
                    if (color == PixelColor.SpecialColorForDetectingChange
                        || color == PixelColor.SpecialColorForDetectingUnchange)
                    {
                        if (DetectChangeOrNotChange(point.X, point.Y, color))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        uint trueColor = GetPixelColor(point.X, point.Y);

                        if (trueColor == color)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            else
            {
                if (color == PixelColor.SpecialColorForDetectingChange
                    || color == PixelColor.SpecialColorForDetectingUnchange)
                {
                    return DetectChangeOrNotChange(x, y, color);
                }
                else
                {
                    return color == GetPixelColor(x, y);
                }
            }
        }
    }
}

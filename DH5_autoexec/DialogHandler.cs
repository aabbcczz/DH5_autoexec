using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoIt;
namespace DH5_autoexec
{
    static class DialogHandler
    {
        public static Dictionary<GameState, Tuple<int, int>> _dialogs = 
            new Dictionary<GameState,Tuple<int,int>>()
            {
                { GameState.DialogAdvertisement, Tuple.Create(1424, 128) },
                { GameState.DialogWarningOfDisconnect, Tuple.Create(853, 808) },
                { GameState.DialogSendAllyRequest, Tuple.Create(670, 857) },
                { GameState.DialogBuyEnergy1, Tuple.Create(1020, 813) },
                { GameState.DialogBuyEnergy2, Tuple.Create(1020, 813) },
                { GameState.DialogUseFreeEnergy, Tuple.Create(1020, 813) },
                { GameState.DialogGetAward, Tuple.Create(845, 820) },
                { GameState.DialogLossConnection, Tuple.Create(836, 794) },
                //{ GameState.DialogTouchToContinue, Tuple.Create(1168, 985) },
                { GameState.DialogDungeonCompleted, Tuple.Create(1061, 933) },
                { GameState.DialogWantedCompleted, Tuple.Create(1061, 933) },
                { GameState.DialogChat, Tuple.Create(101, 213) },
                { GameState.DialogElementChallengeCompleted, Tuple.Create(1246, 950) },
                { GameState.DialogElementChallengePaused, Tuple.Create(238, 944) },
                { GameState.DialogGiveUp, Tuple.Create(1030, 810) },
            };

        public static bool IsDialogState(GameState state)
        {
            return _dialogs.ContainsKey(state);
        }

        public static void Handle(GameState state)
        {
            if (_dialogs.ContainsKey(state))
            {
                var tuple = _dialogs[state];

                Console.WriteLine("{0} handled, click on <{1}, {2}>", state, tuple.Item1, tuple.Item2);

                ScreenUtility.Click(tuple.Item1, tuple.Item2);
            }
        }
    }
}

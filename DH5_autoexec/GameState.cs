using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH5_autoexec
{
    enum GameState
    {
        Unknown = 0,
        DialogWarningOfDisconnect,
        MainWindow,
        DialogAdvertisement, // 大广告
        FightTypeSelection, // 赛事
        WantedChallenge,
        WantedDifficulty,
        DailyDungeon,
        DialogBuyEnergy1,
        DialogBuyEnergy2,
        SelectEquipment,
        SelectPartner,
//        DialogTouchToContinue,
        Fighting,
        DialogWantedCompleted,
        DialogDungeonCompleted,
        DialogElementChallengeCompleted,
        DialogSendAllyRequest,
        DialogGetAward,
        DialogLossConnection,
        DialogChat,
        ElementChallenge,
        ElementChallengeLevel,
        ElementChallengeCompleted,
        DialogElementChallengePaused,
        DialogGiveUp,
        DialogUseFreeEnergy,
    }
}

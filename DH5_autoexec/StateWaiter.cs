using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH5_autoexec
{
    class StateWaiter
    {
        public StateDetector Detector { get; private set; }

        public StateWaiter()
        {
            Detector = new StateDetector();
        }

        public bool WaitForState(GameState currentState, GameState nextState, Action action, int waitTimeInMs, Action actionForUnknown = null)
        {
            DateTime startTime = DateTime.Now;

            Console.WriteLine("WaitForState: {0} -> {1}", currentState, nextState);

            do
            {
                if (!MainWindowUtility.ExistsMainWindow() && !ScreenUtility.IsForegroundFullScreen())
                {
                    Console.WriteLine("No main window and no full screen foreground window");
                    return false;
                }

                var activeState = Detector.Detect();

                Console.WriteLine("{0} State: {1}", DateTime.Now, activeState);

                if (activeState == nextState)
                {
                    return true;
                }

                if (DialogHandler.IsDialogState(activeState))
                {
                    DialogHandler.Handle(activeState);
                }

                if (activeState == currentState
                    && currentState != GameState.Unknown
                    && action != null)
                {
                    Console.WriteLine("Execute action {0}", action);
                    action();
                }

                if (activeState == GameState.Unknown && actionForUnknown != null)
                {
                    Console.WriteLine("Execute action for unknown {0}", actionForUnknown);
                    actionForUnknown();
                }

                var elapsedTime = DateTime.Now - startTime;
                if (elapsedTime.TotalMilliseconds > waitTimeInMs)
                {
                    return false;
                }

                AutoIt.AutoItX.Sleep(1000);
            } while (true);
        }
    }
}

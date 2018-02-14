using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoPrintClicker
{
    public class AutoClicker
    {
        public CancellationToken CancellationToken { get; private set; }
        public TimeSpan SleepDuration { get; set; }

        private const string WindowClassName = "CNMSTMN_ERRDLG";
        private const string ButtonClassName = "Button";
        private const string ButtonCaption = "OK";

        private const int BN_CLICKED = 245;

        public AutoClicker(CancellationToken cancellationToken, TimeSpan sleepDuration)
        {
            CancellationToken = cancellationToken;
            SleepDuration = sleepDuration;
        }

        public Task Watch()
        {
            return Task.Run(() => WatchInternal(), CancellationToken);
        }

        private async Task WatchInternal()
        {
            while (true)
            {
                if (CancellationToken.IsCancellationRequested) break;

                TryClick();

                await Task.Delay(SleepDuration);
            }
        }

        private void TryClick()
        {
            // first we find the window
            var windowHandle = NativeMethods.FindWindow(WindowClassName, null);
            if (windowHandle == default(IntPtr)) return;

            // then we find the ok button
            var buttonHandle = NativeMethods.FindWindowEx(windowHandle, IntPtr.Zero, ButtonClassName, ButtonCaption);
            if (buttonHandle == default(IntPtr)) return;

            // then we click it
            NativeMethods.SendMessage((int)buttonHandle, BN_CLICKED, 0, IntPtr.Zero);
            Console.WriteLine($"Click Was Successful At {DateTime.Now.ToShortDateString()} - ${DateTime.Now.ToShortTimeString()}");
        }
    }
}

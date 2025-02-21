using Fiddler;
using System.Diagnostics;

namespace Office365FiddlerExtension.Services
{
    public class StatusBar
    {
        private static StatusBar _instance;
        public static StatusBar Instance => _instance ?? (_instance = new StatusBar());

        /// <summary>
        /// Function to update the status bar while the extension is processing sessions.
        /// </summary>
        /// <param name="CurrentSession"></param>
        /// <param name="TotalSessions"></param>
        public void UpdateStatusBarOnSessionProgression(int CurrentSession, int TotalSessions)
        {
            double PercentageProgress = (double)CurrentSession / TotalSessions * 100;

            FiddlerObject.StatusText = $"{LangHelper.GetString("Office 365 Fiddler Extension")}: " +
                $"{LangHelper.GetString("Processing")} " +
                $"{LangHelper.GetString("sessions")} " +
                $"{CurrentSession} / " +
                $"{TotalSessions} " +
                $"({PercentageProgress.ToString("0.##")}%)";
        }

        /// <summary>
        /// Function to update the status bar when the extension has finished processing sessions.
        /// Call if no source filename is available.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="SessionsProcessed"></param>
        public void UpdateStatusBarOnSessionProcessComplete(Stopwatch sw, int SessionsProcessed)
        {
            if (sw.ElapsedMilliseconds < 1000)
            {
                FiddlerObject.StatusText = $"{LangHelper.GetString("Office 365 Fiddler Extension")}: " +
                    $"{LangHelper.GetString("Processed")} " +
                    $"{SessionsProcessed} " +
                    $"{LangHelper.GetString("sessions")} " +
                    $"{LangHelper.GetString("in")} " +
                    $"{sw.Elapsed.TotalMilliseconds.ToString("0.##")}ms.";
            }
            else
            {
                FiddlerObject.StatusText = $"{LangHelper.GetString("Office 365 Fiddler Extension")}: " +
                    $"{LangHelper.GetString("Processed")} " +
                    $"{SessionsProcessed} " +
                    $"{LangHelper.GetString("sessions")} " +
                    $"{LangHelper.GetString("in")} " +
                    $"{sw.Elapsed.TotalSeconds.ToString("0.##")} seconds.";
            }
        }

        /// <summary>
        /// Function to update the status bar when the extension has finished processing sessions.
        /// Call if a source filename is available.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="SessionsProcessed"></param>
        /// <param name="Filename"></param>
        public void UpdateStatusBarOnSessionProcessComplete(Stopwatch sw, int SessionsProcessed, string Filename)
        {
            if (sw.ElapsedMilliseconds < 1000)
            {
                FiddlerObject.StatusText = $"{LangHelper.GetString("Office 365 Fiddler Extension")}: " +
                    $"{LangHelper.GetString("Processed")} " +
                    $"{SessionsProcessed} " +
                    $"{LangHelper.GetString("sessions")} " +
                    $"{LangHelper.GetString("in")} " +
                    $"{sw.Elapsed.TotalMilliseconds.ToString("0.##")}ms " +
                    $"from " +
                    $"{Filename}";
            }
            else
            {
                FiddlerObject.StatusText = $"{LangHelper.GetString("Office 365 Fiddler Extension")}: " +
                    $"{LangHelper.GetString("Processed")} " +
                    $"{SessionsProcessed} " +
                    $"{LangHelper.GetString("sessions")} " +
                    $"{LangHelper.GetString("in")} " +
                    $"{sw.Elapsed.TotalSeconds.ToString("0.##")} seconds " +
                    $"from " +
                    $"{Filename}";
            }
        }
    }
}

using Fiddler;
using System;
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
            try
            {
                double PercentageProgress = (double)CurrentSession / TotalSessions * 100;

                FiddlerObject.StatusText = $"{LangHelper.GetString("Office 365 Fiddler Extension")}: " +
                    $"{LangHelper.GetString("Processing")} " +
                    $"{LangHelper.GetString("sessions")} " +
                    $"{CurrentSession} / " +
                    $"{TotalSessions} " +
                    $"({PercentageProgress.ToString("0")}%)";
                    // Percentage with two decimal places. Keeping this in case it's needed in the future.
                    // 3.12.2025 Changing this to just show round numbers.
                    //$"({PercentageProgress.ToString("0.##")}%)";
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
            }
        }

        /// <summary>
        /// Function to update the status bar when the extension has finished processing sessions.
        /// Call if no source filename is available.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="SessionsProcessed"></param>
        public void UpdateStatusBarOnSessionProcessComplete(Stopwatch _sw, int _SessionsProcessed)
        {
            try
            {
                if (_sw.ElapsedMilliseconds < 1000)
                {
                    FiddlerObject.StatusText = $"{LangHelper.GetString("Office 365 Fiddler Extension")}: " +
                        $"{LangHelper.GetString("Processed")} " +
                        $"{_SessionsProcessed} " +
                        $"{LangHelper.GetString("sessions")} " +
                        $"{LangHelper.GetString("in")} " +
                        $"{_sw.Elapsed.TotalMilliseconds.ToString("0")}ms.";
                        // Percentage with two decimal places. Keeping this in case it's needed in the future.
                        // 3.21.2025 Changing this to just show round numbers.
                        // $"{sw.Elapsed.TotalMilliseconds.ToString("0.##")}ms.";
                }
                else
                {
                    FiddlerObject.StatusText = $"{LangHelper.GetString("Office 365 Fiddler Extension")}: " +
                        $"{LangHelper.GetString("Processed")} " +
                        $"{_SessionsProcessed} " +
                        $"{LangHelper.GetString("sessions")} " +
                        $"{LangHelper.GetString("in")} " +
                        $"{_sw.Elapsed.TotalSeconds.ToString("0")} seconds.";
                        // Percentage with two decimal places. Keeping this in case it's needed in the future.
                        // 3.21.2025 Changing this to just show round numbers.
                        //$"{sw.Elapsed.TotalSeconds.ToString("0.##")} seconds.";
                }
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
            }
        }

        /// <summary>
        /// Function to update the status bar when the extension has finished processing sessions.
        /// Call if a source filename is available.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="SessionsProcessed"></param>
        /// <param name="Filename"></param>
        public void UpdateStatusBarOnSessionProcessComplete(Stopwatch _sw, int _SessionsProcessed, string _Filename)
        {
            try
            {
                if (_sw.ElapsedMilliseconds < 1000)
                {
                    FiddlerObject.StatusText = $"{LangHelper.GetString("Office 365 Fiddler Extension")}: " +
                        $"{LangHelper.GetString("Processed")} " +
                        $"{_SessionsProcessed} " +
                        $"{LangHelper.GetString("sessions")} " +
                        $"{LangHelper.GetString("in")} " +
                        $"{_sw.Elapsed.TotalMilliseconds.ToString("0")}ms " +
                        // Percentage with two decimal places. Keeping this in case it's needed in the future.
                        // 3.21.2025 Changing this to just show round numbers.
                        // $"{sw.Elapsed.TotalMilliseconds.ToString("0.##")}ms " +
                        $"from " +
                        $"{_Filename}";
                }
                else
                {
                    FiddlerObject.StatusText = $"{LangHelper.GetString("Office 365 Fiddler Extension")}: " +
                        $"{LangHelper.GetString("Processed")} " +
                        $"{_SessionsProcessed} " +
                        $"{LangHelper.GetString("sessions")} " +
                        $"{LangHelper.GetString("in")} " +
                        $"{_sw.Elapsed.TotalMilliseconds.ToString("0")}ms " +
                        // Percentage with two decimal places. Keeping this in case it's needed in the future.
                        // 3.21.2025 Changing this to just show round numbers.
                        // $"{sw.Elapsed.TotalMilliseconds.ToString("0.##")}ms " +
                        $"from " +
                        $"{_Filename}";
                }
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
            }
        }
    }
}

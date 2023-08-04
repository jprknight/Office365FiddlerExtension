using Fiddler;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Office365FiddlerExtension.UI;

namespace Office365FiddlerExtension
{
    /// <summary>
    /// Add context menu into Fiddler application UI.
    /// </summary>
    public class ContextMenuUI
    {
        private static ContextMenuUI _instance;

        public static ContextMenuUI Instance => _instance ?? (_instance = new ContextMenuUI());

        public ContextMenuUI() { }

        private readonly MenuItem Separator1 = new MenuItem("-");

        private readonly MenuItem Separator2 = new MenuItem("-");

        private readonly MenuItem CmiProcessSelectedSessions = new MenuItem("Process Selected Sessions");

        private readonly MenuItem CmiProcessAllSessions = new MenuItem("Process All Sessions");

        private readonly MenuItem CmiClearAllSessionProcessing = new MenuItem("Clear Selected Sessions Processing");

        private readonly MenuItem CmiSetSessionSeverity = new MenuItem("Set Session Severity");

        private readonly MenuItem CmiSessionSeverityTen = new MenuItem("10 - Gray (Uninteresting)");

        private readonly MenuItem CmiSessionSeverityTwenty = new MenuItem("20 - Blue (False Positive)");

        private readonly MenuItem CmiSessionSeverityThirty = new MenuItem("30 - Green (Normal)");

        private readonly MenuItem CmiSessionSeverityFourty = new MenuItem("40 - Orange (Warning)");

        private readonly MenuItem CmiSessionSeverityFifty = new MenuItem("50 - Black (Concerning)");

        private readonly MenuItem CmiSessionSeveritySixty = new MenuItem("60 - Red (Severe)");

        private bool IsInitialized { get; set; }

        public void initialize()
        {
            if (IsInitialized) return;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Adding context menu to UI.");

            CmiProcessSelectedSessions.Click += new EventHandler(CmiProcessSelectedSessions_Click);

            CmiProcessAllSessions.Click += new EventHandler(CmiProcessAllSessions_Click);

            CmiClearAllSessionProcessing.Click += new EventHandler(CmiClearAllSessionProcessing_Click);

            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(0, CmiProcessSelectedSessions);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(1, CmiProcessAllSessions);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(2, CmiClearAllSessionProcessing);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(3, Separator1);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(4, CmiSetSessionSeverity);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(5, Separator2);

            this.CmiSetSessionSeverity.MenuItems.AddRange(new MenuItem[] {
                this.CmiSessionSeverityTen,
                this.CmiSessionSeverityTwenty,
                this.CmiSessionSeverityThirty,
                this.CmiSessionSeverityFourty,
                this.CmiSessionSeverityFifty,
                this.CmiSessionSeveritySixty
            });

            CmiSessionSeverityTen.Click += new EventHandler(CmiSessionSeverityTen_Click);

            CmiSessionSeverityTwenty.Click += new EventHandler(CmiSessionSeverityTwenty_Click);

            CmiSessionSeverityThirty.Click += new EventHandler(CmiSessionSeverityThirty_Click);

            CmiSessionSeverityFourty.Click += new EventHandler(CmiSessionSeverityFourty_Click);

            CmiSessionSeverityFifty.Click += new EventHandler(CmiSessionSeverityFifty_Click);

            CmiSessionSeveritySixty.Click += new EventHandler(CmiSessionSeveritySixty_Click);
        }

        private void CmiSessionSeverityTen_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionUninteresting();
        }

        private void CmiSessionSeverityTwenty_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionFalsePositive();
        }

        private void CmiSessionSeverityThirty_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionNormal();
        }

        private void CmiSessionSeverityFourty_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionWarning();
        }

        private void CmiSessionSeverityFifty_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionConcerning();
        }

        private void CmiSessionSeveritySixty_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionSevere();
        }
        
        private void CmiClearAllSessionProcessing_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.ClearAllSessionProcessing();
        }

        private void CmiProcessAllSessions_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.ProcessAllSessions();
        }

        private void CmiProcessSelectedSessions_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.ProcessSelectedSessions();
        }
    }
}

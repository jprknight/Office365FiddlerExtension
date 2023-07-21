using Fiddler;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

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

        private readonly MenuItem Separator = new MenuItem("-");

        private readonly MenuItem CmiProcessSelectedSessions = new MenuItem("Process Selected Session(s)");

        private readonly MenuItem CmiProcessAllSessions = new MenuItem("Process All Sessions");

        private readonly MenuItem CmiClearAllSessionProcessing = new MenuItem("Clear All Session Processing");

        private readonly MenuItem CmiSetSessionSeverity = new MenuItem("Set Session Severity");

        private readonly MenuItem CmiSessionSeverityZero = new MenuItem(" 0 - Gray (Uninteresting)");

        private readonly MenuItem CmiSessionSeverityTen = new MenuItem("10 - Blue (False Positive)");

        private readonly MenuItem CmiSessionSeverityTwenty = new MenuItem("20 - Green (Normal)");

        private readonly MenuItem CmiSessionSeverityThirty = new MenuItem("30 - Orange (Warning)");

        private readonly MenuItem CmiSessionSeverityFourty = new MenuItem("40 - Black (Concerning)");

        private readonly MenuItem CmiSessionSeverityFifty = new MenuItem("50 - Red (Severe)");

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
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(3, Separator);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(4, CmiSetSessionSeverity);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(5, Separator);

            this.CmiSetSessionSeverity.MenuItems.AddRange(new MenuItem[] { 
                this.CmiSessionSeverityZero, 
                this.CmiSessionSeverityTen,
                this.CmiSessionSeverityTwenty,
                this.CmiSessionSeverityThirty,
                this.CmiSessionSeverityFourty,
                this.CmiSessionSeverityFifty
            });

            CmiSessionSeverityZero.Click += new EventHandler(CmiSessionSeverityZero_Click);

            CmiSessionSeverityTen.Click += new EventHandler(CmiSessionSeverityTen_Click);

            CmiSessionSeverityTwenty.Click += new EventHandler(CmiSessionSeverityTwenty_Click);

            CmiSessionSeverityThirty.Click += new EventHandler(CmiSessionSeverityThirty_Click);

            CmiSessionSeverityFourty.Click += new EventHandler(CmiSessionSeverityFourty_Click);

            CmiSessionSeverityFifty.Click += new EventHandler(CmiSessionSeverityFifty_Click);
        }

        private void CmiSessionSeverityZero_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmiSessionSeverityTen_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmiSessionSeverityTwenty_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmiSessionSeverityThirty_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmiSessionSeverityFourty_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmiSessionSeverityFifty_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

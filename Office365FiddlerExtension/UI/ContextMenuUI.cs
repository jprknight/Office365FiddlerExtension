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

        private readonly MenuItem SubMenuSeparator = new MenuItem("-");

        private readonly MenuItem CmiAnalyzeAllSessions = new MenuItem(LangHelper.GetString("Analyze All Sessions"));

        private readonly MenuItem CmiAnalyzeSelectedSessions = new MenuItem(LangHelper.GetString("Analyze Selected Sessions"));

        private readonly MenuItem CmiClearAnalysisSelectedSessions = new MenuItem(LangHelper.GetString("Clear Selected Sessions"));

        private readonly MenuItem CmiSetSessionSeverity = new MenuItem(LangHelper.GetString("Set Session Severity"));

        private readonly MenuItem CmiRecalculateAnalysisSelectedSessions = new MenuItem(LangHelper.GetString("Recalculate Selected Sessions"));

        private readonly MenuItem CmiSessionSeverityTen = new MenuItem($"10 - {LangHelper.GetString("Grey")} ({LangHelper.GetString("Uninteresting")})");

        private readonly MenuItem CmiSessionSeverityTwenty = new MenuItem($"20 - {LangHelper.GetString("Blue")} ({LangHelper.GetString("False Positive")})");

        private readonly MenuItem CmiSessionSeverityThirty = new MenuItem($"30 - {LangHelper.GetString("Green")} ({LangHelper.GetString("Normal")})");

        private readonly MenuItem CmiSessionSeverityFourty = new MenuItem($"40 - {LangHelper.GetString("Orange")} ({LangHelper.GetString("Warning")})");

        private readonly MenuItem CmiSessionSeverityFifty = new MenuItem($"50 - {LangHelper.GetString("Black")} ({LangHelper.GetString("Concerning")})");

        private readonly MenuItem CmiSessionSeveritySixty = new MenuItem($"60 - {LangHelper.GetString("Red")} ({LangHelper.GetString("Severe")})");

        private bool IsInitialized { get; set; }

        public void initialize()
        {
            if (IsInitialized) return;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Adding context menu to UI.");

            CmiAnalyzeSelectedSessions.Click += new EventHandler(CmiAnalyzeSelectedSessions_Click);

            CmiAnalyzeAllSessions.Click += new EventHandler(CmiAnalyzeAllSessions_Click);

            CmiClearAnalysisSelectedSessions.Click += new EventHandler(CmiClearAnalysisSelectedSessions_Click);

            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(0, CmiAnalyzeAllSessions);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(1, CmiAnalyzeSelectedSessions);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(2, CmiClearAnalysisSelectedSessions);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(3, Separator1);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(4, CmiSetSessionSeverity);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(5, Separator2);

            this.CmiSetSessionSeverity.MenuItems.AddRange(new MenuItem[] {
                this.CmiRecalculateAnalysisSelectedSessions,
                this.SubMenuSeparator,
                this.CmiSessionSeverityTen,
                this.CmiSessionSeverityTwenty,
                this.CmiSessionSeverityThirty,
                this.CmiSessionSeverityFourty,
                this.CmiSessionSeverityFifty,
                this.CmiSessionSeveritySixty
            });

            CmiRecalculateAnalysisSelectedSessions.Click += new EventHandler(CmiRecalculateAnalysisSelectedSessions_Click);

            CmiSessionSeverityTen.Click += new EventHandler(CmiSessionSeverityTen_Click);

            CmiSessionSeverityTwenty.Click += new EventHandler(CmiSessionSeverityTwenty_Click);

            CmiSessionSeverityThirty.Click += new EventHandler(CmiSessionSeverityThirty_Click);

            CmiSessionSeverityFourty.Click += new EventHandler(CmiSessionSeverityFourty_Click);

            CmiSessionSeverityFifty.Click += new EventHandler(CmiSessionSeverityFifty_Click);

            CmiSessionSeveritySixty.Click += new EventHandler(CmiSessionSeveritySixty_Click);
        }

        private void CmiRecalculateAnalysisSelectedSessions_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.CmiRecalculateAnalysisSelectedSessions();
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
        
        private void CmiClearAnalysisSelectedSessions_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.ClearAnalysisSelectedSessions();
        }

        private void CmiAnalyzeAllSessions_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.AnalyzeAllSessions();
        }

        private void CmiAnalyzeSelectedSessions_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.AnalyzeSelectedSessions();
        }
    }
}

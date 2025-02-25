﻿using Fiddler;
using Office365FiddlerExtension.Services;
using System;
using System.Windows.Forms;
using System.Reflection;
using Office365FiddlerExtension.UI;

namespace Office365FiddlerExtension
{
    /// <summary>
    /// Create and add context menu into Fiddler application UI.
    /// </summary>
    public class ContextMenuUI
    {
        private static ContextMenuUI _instance;

        public static ContextMenuUI Instance => _instance ?? (_instance = new ContextMenuUI());

        public ContextMenuUI() { }

        private readonly MenuItem Separator1 = new MenuItem("-");

        private readonly MenuItem Separator2 = new MenuItem("-");

        private readonly MenuItem Separator3 = new MenuItem("-");

        private readonly MenuItem SubMenuSeparator = new MenuItem("-");

        //private readonly MenuItem CmiAnalyseAllSessions = new MenuItem(LangHelper.GetString("Analyse All Sessions"));

        private readonly MenuItem CmiAnalyseSelectedSessions = new MenuItem(LangHelper.GetString("Analyse Selected Sessions"));

        private readonly MenuItem CmiClearAnalysisSelectedSessions = new MenuItem(LangHelper.GetString("Clear Selected Sessions"));

        private readonly MenuItem CmiSetSessionSeverity = new MenuItem(LangHelper.GetString("Set Session Severity"));

        private readonly MenuItem CmiRecalculateAnalysisSelectedSessions = new MenuItem(LangHelper.GetString("Recalculate Selected Sessions"));

        private readonly MenuItem CmiSessionSeverityTen = new MenuItem($"10 - {LangHelper.GetString("Grey")} ({LangHelper.GetString("Uninteresting")})");

        private readonly MenuItem CmiSessionSeverityTwenty = new MenuItem($"20 - {LangHelper.GetString("Blue")} ({LangHelper.GetString("False Positive")})");

        private readonly MenuItem CmiSessionSeverityThirty = new MenuItem($"30 - {LangHelper.GetString("Green")} ({LangHelper.GetString("Normal")})");

        private readonly MenuItem CmiSessionSeverityFourty = new MenuItem($"40 - {LangHelper.GetString("Orange")} ({LangHelper.GetString("Warning")})");

        private readonly MenuItem CmiSessionSeverityFifty = new MenuItem($"50 - {LangHelper.GetString("Black")} ({LangHelper.GetString("Concerning")})");

        private readonly MenuItem CmiSessionSeveritySixty = new MenuItem($"60 - {LangHelper.GetString("Red")} ({LangHelper.GetString("Severe")})");

        private readonly MenuItem CmiCreateConsolidatedAnalysisReport = new MenuItem($"{LangHelper.GetString("Create Consolidated Analysis Report")}");

        private bool IsInitialized { get; set; }

        /// <summary>
        /// Create menu and add to Fiddler UI.
        /// </summary>
        public void initialize()
        {
            if (IsInitialized) return;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add context menu to Fiddler UI.");

            try
            {
                CmiAnalyseSelectedSessions.Click += new EventHandler(CmiAnalyseSelectedSessions_Click);

                //CmiAnalyseAllSessions.Click += new EventHandler(CmiAnalyseAllSessions_Click);

                CmiClearAnalysisSelectedSessions.Click += new EventHandler(CmiClearAnalysisSelectedSessions_Click);

                //FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(0, CmiAnalyseAllSessions);
                FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(0, CmiAnalyseSelectedSessions);
                FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(1, CmiClearAnalysisSelectedSessions);
                FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(2, Separator1);
                FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(3, CmiSetSessionSeverity);
                FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(4, Separator2);
                FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(5, CmiCreateConsolidatedAnalysisReport);
                FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(6, Separator3);

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

                CmiCreateConsolidatedAnalysisReport.Click += new EventHandler(CmiCreateConsolidatedAnalysisReport_Click);

                if (!SettingsJsonService.Instance.ExtensionSessionProcessingEnabled)
                {
                    CmiAnalyseSelectedSessions.Enabled = false;
                    CmiClearAnalysisSelectedSessions.Enabled = false;
                    CmiSetSessionSeverity.Enabled = false;
                    CmiCreateConsolidatedAnalysisReport.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
            
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmiRecalculateAnalysisSelectedSessions_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.CmiRecalculateAnalysisSelectedSessions();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmiSessionSeverityTen_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionUninteresting();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmiSessionSeverityTwenty_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionFalsePositive();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmiSessionSeverityThirty_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionNormal();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmiSessionSeverityFourty_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionWarning();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmiSessionSeverityFifty_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionConcerning();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmiSessionSeveritySixty_Click(object sender, EventArgs e)
        {
            EnhanceSessionUX.Instance.SetSessionSevere();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmiClearAnalysisSelectedSessions_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.ClearAnalysisSelectedSessions();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmiAnalyseSelectedSessions_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.AnalyseSelectedSessions();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmiCreateConsolidatedAnalysisReport_Click(object sender, EventArgs e)
        {
            ConsolidatedAnalysisReportService.Instance.CreateCAR();
        }

        /// <summary>
        /// Function to update the Conext Menu UI controls.
        /// </summary>
        public void UpdateUIControls()
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            CmiAnalyseSelectedSessions.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            CmiClearAnalysisSelectedSessions.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            CmiSetSessionSeverity.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            CmiCreateConsolidatedAnalysisReport.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
        }
    }
}

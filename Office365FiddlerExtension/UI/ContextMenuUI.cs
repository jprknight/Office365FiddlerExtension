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
    public class ContextMenuUI
    {
        private static ContextMenuUI _instance;

        public static ContextMenuUI Instance => _instance ?? (_instance = new ContextMenuUI());

        public ContextMenuUI() { }

        private readonly MenuItem Separator = new MenuItem("-");

        private readonly MenuItem CmiProcessSelectedSessions = new MenuItem("Process Selected Session(s)");

        private readonly MenuItem CmiProcessAllSessions = new MenuItem("Process All Sessions");

        private readonly MenuItem CmiClearAllSessionProcessing = new MenuItem("Clear All Session Processing");

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
        }

        private void CmiClearAllSessionProcessing_Click(object sender, EventArgs e)
        {
            SessionFlagHandler.Instance.ClearAllSessionProcessing();
        }

        private void CmiProcessAllSessions_Click(object sender, EventArgs e)
        {
            SessionFlagHandler.Instance.ProcessAllSessions();
        }

        private void CmiProcessSelectedSessions_Click(object sender, EventArgs e)
        {
            SessionFlagHandler.Instance.ProcessSelectedSessions();
        }
    }
}

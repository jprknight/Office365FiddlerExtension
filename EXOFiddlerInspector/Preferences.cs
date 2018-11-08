using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXOFiddlerInspector
{
    class Preferences
    {
        /////////////////
        /// <summary>
        /// Developer Demo Mode. If enabled as much domain specific information as possible will be replaced with contoso.com.
        /// Note: This is not much right now, just outputs in response comments on the response inspector tab.
        /// </summary>
        Boolean DeveloperDemoMode = false;
        Boolean DeveloperDemoModeBreakScenarios = false;
        /////////////////

        List<string> Developers = new List<string>(new string[] { "jeknight", "brandev", "jasonsla" });
        public List<string> GetDeveloperList()
        {
            return Developers;
        }
        public Boolean GetDeveloperMode()
        {
            return DeveloperDemoMode;
        }

        public Boolean GetDeveloperDemoModeBreakScenarios()
        {
            return DeveloperDemoModeBreakScenarios;
        }
    }
}

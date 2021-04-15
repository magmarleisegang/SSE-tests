using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR_SSE.ReportingPlugins
{
    public class ReportPluginsManager
    {
        private readonly static Lazy<ReportPluginsManager> instance =
           new Lazy<ReportPluginsManager>(() => new ReportPluginsManager());
         
        internal List<IReportingAction> plugins;
        public ReportPluginsManager()
        {
            plugins = new List<IReportingAction>();
            var results = from type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                          where typeof(IReportingAction).IsAssignableFrom(type) && type.IsClass
                          select type;
            foreach (var _type in results)
            {
                var constr = _type.GetConstructor(Type.EmptyTypes);
                plugins.Add((IReportingAction)constr.Invoke(new object[] { }));

            }

        }
        public static bool PluginExists(string pluginName)
        {
            return instance.Value.plugins.Any(p => p.PluginName.ToLower() == pluginName.ToLower());
        }

        public static IReportingAction GetPlugin(string pluginName)
        {
            return instance.Value.plugins.Find(p => p.PluginName.ToLower() == pluginName.ToLower());
        }
    }
}
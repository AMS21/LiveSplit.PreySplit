using System;
using System.Reflection;
using LiveSplit.Model;
using LiveSplit.UI.Components;

[assembly: ComponentFactory(typeof(LiveSplit.PreySplit.Factory))]

namespace LiveSplit.PreySplit
{
    public class Factory : IComponentFactory
    {
        public ComponentCategory Category => ComponentCategory.Control;
        public string ComponentName => "PreySplit";
        public string Description => "In-Game Time and Auto-Split component that works with PreyRun.";

        public string UpdateName => ComponentName;
        public string UpdateURL => "http://NOURL.NO/";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => UpdateURL + "updates.xml";

        public IComponent Create(LiveSplitState state) => new Component(state);
    }
}

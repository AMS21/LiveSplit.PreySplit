using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;

namespace LiveSplit.PreySplit
{
    public partial class ComponentSettings : UserControl
    {
        private readonly List<string> mapnames = new List<string>
        {
            //"roadhouse.map", // Last Call
            "feedingtowera.map", // Escape Velocity
            "feedingtowerb.map", // Downward Spiral
            "lotaa.map", // Rites of Passage
            "feedingtowerc.map", // Seconds Chances
            "feedingtowerd.map", // All Fall Down
            "salvage.map", // Crash Landing
            "salvageboss.map", // Sacrifices
            "lotab.map", // There are Others
            "shuttlea.map", // Guiding Fires
            "shuttleb.map", // The Old Tribes
            "biolabsa.map", // Hidden Agenda
            "biolabsb.map", // Jen
            "superportal.map", // The Dark Harvest
            "harverstera.map", // Following Her
            "harversterb.map", // The Complex
            "spindlea.map", // Ascent
            "spindleb.map", // Center of Gravity
            "girlfriendx.map", // Resolutions
            "lotad.map", // Oath of Vengence
            "keeperfortress.map", // Facing the Enemy
            "spherebrain.map", // Mothers Embrace
        };

        public ComponentSettings()
        {
            InitializeComponent();
        }

        private void SetDefaultSettings()
        {
            EnableAutoSplitCheckbox.Checked = false;
            SplitOnGameEndCheckbox.Checked = true;
            SplitOnMapsCheckbox.Checked = true;
            SplitOnMapsList.Rows.Clear();

            EnableAutoResetCheckbox.Checked = true;
            EnableAutoStartCheckbox.Checked = true;
        }

        private static void AppendElement<T>(XmlDocument document, XmlElement parent, string name, T value)
        {
            XmlElement el = document.CreateElement(name);
            el.InnerText = value.ToString();
            parent.AppendChild(el);
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            XmlElement settingsNode = document.CreateElement("Settings");

            AppendElement(document, settingsNode, "Version", Assembly.GetExecutingAssembly().GetName().Version);

            AppendElement(document, settingsNode, "EnableAutoSplit", EnableAutoSplitCheckbox.Checked);
            AppendElement(document, settingsNode, "SplitOnGameEnd", SplitOnGameEndCheckbox.Checked);
            AppendElement(document, settingsNode, "SplitOnMaps", SplitOnMapsCheckbox.Checked);
            AppendElement(document, settingsNode, "SplitOnMapsList", string.Join("|", SplitOnMapsList.GetValues()));

            AppendElement(document, settingsNode, "EnableAutoReset", EnableAutoResetCheckbox.Checked);
            AppendElement(document, settingsNode, "EnableAutoStart", EnableAutoStartCheckbox.Checked);

            return settingsNode;
        }

        private bool FindSetting(XmlNode node, string name, bool previous)
        {
            var element = node[name];
            if (element == null)
                return previous;

            bool b;
            if (bool.TryParse(element.InnerText, out b))
                return b;

            return previous;
        }

        public void SetSettings(XmlNode settings)
        {
            SetDefaultSettings();
            SetAutoSplitCheckboxColors();

            var versionElement = settings["Version"];
            if (versionElement == null)
                return;
            Version ver;
            if (!Version.TryParse(versionElement.InnerText, out ver))
                return;
            if (ver.Major != 2 || ver.Minor != 0)
                return;

            EnableAutoSplitCheckbox.Checked = FindSetting(settings, "EnableAutoSplit", EnableAutoSplitCheckbox.Checked);
            SplitOnGameEndCheckbox.Checked = FindSetting(settings, "SplitOnGameEnd", SplitOnGameEndCheckbox.Checked);
            SplitOnMapsCheckbox.Checked = FindSetting(settings, "SplitOnMaps", SplitOnMapsCheckbox.Checked);
            var e = settings["SplitOnMapsList"];
            if (e != null)
                foreach (var map in e.InnerText.Split('|'))
                    SplitOnMapsList.Rows.Add(map);

            EnableAutoResetCheckbox.Checked = FindSetting(settings, "EnableAutoReset", EnableAutoResetCheckbox.Checked);
            EnableAutoStartCheckbox.Checked = FindSetting(settings, "EnableAutoStart", EnableAutoStartCheckbox.Checked);

            SetAutoSplitCheckboxColors();
        }

        public bool ShouldSplitOn(string map)
        {
            if (!IsAutoSplitEnabled())
                return false;

            if (SplitOnMapsCheckbox.Checked && SplitOnMapsList.GetValues().Contains(map))
                return true;

            return false;
        }

        public bool ShouldSplitOnGameEnd()
        {
            return IsAutoSplitEnabled() && SplitOnGameEndCheckbox.Checked;
        }

        public bool IsAutoSplitEnabled()
        {
            return EnableAutoSplitCheckbox.Checked;
        }

        public bool IsAutoResetEnabled()
        {
            return EnableAutoResetCheckbox.Checked;
        }

        public bool IsAutoStartEnabled()
        {
            return EnableAutoStartCheckbox.Checked;
        }

        private void SetAutoSplitCheckboxColors()
        {
            if (IsAutoSplitEnabled())
            {
                SplitOnGameEndCheckbox.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                SplitOnMapsCheckbox.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            }
            else
            {
                SplitOnGameEndCheckbox.ForeColor = Color.FromKnownColor(KnownColor.GrayText);
                SplitOnMapsCheckbox.ForeColor = Color.FromKnownColor(KnownColor.GrayText);
            }
        }

        private void EnableAutoSplitCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SetAutoSplitCheckboxColors();
        }
    }
}

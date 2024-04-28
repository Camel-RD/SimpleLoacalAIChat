using KlonsLIB.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLoacalAIChat.Models
{
    public record PromptTemplate : INotifyPropertyChanged, IConfigItem
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EConfigItemType ItemType => EConfigItemType.PromptTemplate;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Name { get; set; } = "?";
        [Category("Template")]
        [Editor(typeof(KlonsLib.Components.StringNEditor), typeof(UITypeEditor))]
        public string System {  get; set; }

        [Category("Template")]
        [Editor(typeof(KlonsLib.Components.StringNEditor), typeof(UITypeEditor))]
        public string Prompt { get; set; }
        [Category("Template")]
        [Editor(typeof(KlonsLib.Components.StringNEditor), typeof(UITypeEditor))]
        public string Response { get; set; }

        [Category("Options")]
        public string ExtraStopTokens { get; set; } = default;

        public string[] GetExtraStopTokensArray()
        {
            if (ExtraStopTokens.IsNOE()) return new string[0];
            var ret = ExtraStopTokens.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !x.IsNOE())
                .ToArray();
            return ret;
        }
        
        [Category("Options")]
        [DefaultValue(true)]
        public bool? PrependBosToken { get; set; } = true;

        [Category("Options")]
        [DefaultValue(true)]
        public bool ProcessSpecialTokens { get; set; } = true;


        public event PropertyChangedEventHandler PropertyChanged;
    }
}

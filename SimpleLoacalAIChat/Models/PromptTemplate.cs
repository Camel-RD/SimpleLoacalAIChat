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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

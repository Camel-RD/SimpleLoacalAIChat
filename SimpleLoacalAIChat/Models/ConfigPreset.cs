using KlonsLIB.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimpleLoacalAIChat.Models
{
    public record ConfigPreset : INotifyPropertyChanged, IMyPropertyValueListProvider, IConfigItem
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EConfigItemType ItemType => EConfigItemType.Preset;

        string[] IMyPropertyValueListProvider.GetPropertyValueList(string propertyname)
        {
            if (propertyname == "Model")
            {
                if (ChatConfig == null) return new string[0];
                return ChatConfig.GetModelList();
            }
            if (propertyname == "TemplateName")
            {
                if (ChatConfig == null) return new string[0];
                var ret = ChatConfig.PromptTemplates
                    .Select(x => x.Name)
                    .OrderBy(x => x)
                    .ToArray();
                return ret;
            }
            if (propertyname == "SamplerName")
            {
                if (ChatConfig == null) return new string[0];
                var ret = ChatConfig.SamplingParams
                    .Select(x => x.Name)
                    .OrderBy(x => x)
                    .ToArray();
                return ret;
            }
            return new string[0];
        }

        [XmlIgnore]
        public ChatConfig ChatConfig = null;


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Name { get; set; } = "?";

        [Category("General")]
        //[Editor("KlonsLIB.Design.GenericStringDropDownEditor, KlonsLIB.Design", "System.Drawing.Design.UITypeEditor, System.Windows.Forms")]
        [Editor(typeof(KlonsLIB.Components.MyDropDownPropertyEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Model { get; set; } = "?";
        [Category("General")]
        [Editor(typeof(KlonsLIB.Components.MyDropDownPropertyEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string TemplateName { get; set; } = "?";
        [Category("General")]
        [Editor(typeof(KlonsLIB.Components.MyDropDownPropertyEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SamplerName { get; set; } = "?";
        [Category("General")]
        public string SystemPrompt { get; set; } = "?";

        [Category("Model")]
        public int GpuLayers { get; set; } = 0;
        [Category("Model")]
        public int MainGpu { get; set; } = 0;
        [Category("Model")]
        public int Seed { get; set; } = -1;
        [Category("Model")]
        public int ContextLength { get; set; } = 0;
        [Category("Model")]
        public int BatchSize { get; set; } = 64;
        [Category("Model")]
        public int ThreadCount { get; set; } = 1;
        [Category("Model")]
        public int BatchThreadCount { get; set; } = 1;


        public event PropertyChangedEventHandler PropertyChanged;
    }
}

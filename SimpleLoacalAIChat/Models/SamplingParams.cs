using KlonsLIB.Misc;
using LlamaCppLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLoacalAIChat.Models
{
    public record SamplingParams : INotifyPropertyChanged, IConfigItem
    {

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EConfigItemType ItemType => EConfigItemType.SamplingOptions;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Name { get; set; } = "?";
        [Category("1.General")]
        public int? ResponseMaxTokenCount { get; set; } = default;
        [Category("1.General")]
        public float Temperature { get; set; } = 1f;
        [Category("1.General")]
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

        [Category("2.Extra")]
        public int TopK { get; set; } = 1;
        [Category("2.Extra")]
        public float TopP { get; set; } = 1f;
        [Category("2.Extra")]
        public float MinP { get; set; } = 0f;
        [Category("2.Extra")]
        public float TfsZ { get; set; } = 1.0f;
        [Category("2.Extra")]
        public float TypicalP { get; set; } = 1.0f;

        [Category("3.Mirostat")]
        public Mirostat Mirostat { get; set; } = Mirostat.Disabled;
        [Category("3.Mirostat")]
        public float MirostatTau { get; set; } = 5.0f;
        [Category("3.Mirostat")]
        public float MirostatEta { get; set; } = 0.1f;

        [Category("4.Penalty")]
        public int PenaltyLastN { get; set; } = 1024;
        [Category("4.Penalty")]
        public float PenaltyRepeat { get; set; } = 1f;
        [Category("4.Penalty")]
        public float PenaltyFreq { get; set; } = 0.0f;
        [Category("4.Penalty")]
        public float PenaltyPresent { get; set; } = 0.0f;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

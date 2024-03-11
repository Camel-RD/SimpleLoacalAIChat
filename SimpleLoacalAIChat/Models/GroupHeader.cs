using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLoacalAIChat.Models
{
    public record GroupHeader : IConfigItem
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EConfigItemType ItemType => EConfigItemType.Header;
        public EConfigItemType GroupType { get; set; } = EConfigItemType.None;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Name { get; set; } = "?";

    }
}

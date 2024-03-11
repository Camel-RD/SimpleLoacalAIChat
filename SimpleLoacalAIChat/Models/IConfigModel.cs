using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLoacalAIChat.Models
{
    public enum EConfigItemType { None, Setting, Preset, PromptTemplate, SamplingOptions, Header}
    public interface IConfigItem
    {
        string Name { get; set; }
        EConfigItemType ItemType { get; }
    }
}

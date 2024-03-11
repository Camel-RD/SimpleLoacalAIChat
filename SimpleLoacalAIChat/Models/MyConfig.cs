using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLoacalAIChat.Models
{
    public record MyConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ChatConfig ChatConfig { get; set; } = new();

    }
}

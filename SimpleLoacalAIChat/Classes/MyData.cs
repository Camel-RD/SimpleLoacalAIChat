using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KlonsLIB;
using KlonsLIB.Misc;
using SimpleLoacalAIChat.Models;

namespace SimpleLoacalAIChat
{
    public class MyData
    {
        protected static MyData _MyData = null;

        public static string MyBaseFolder = (new DirectoryInfo(GetMyFolderX())).FullName;
        public static string MyDataFolder = Path.Combine(MyBaseFolder, "Data");
        public static string SettingsFileName = Path.Combine(MyDataFolder, "Settings.xml");
        public static string ConfigFileName = Path.Combine(MyDataFolder, "Config.xml");
        public static string MyModelsFolder = Path.Combine(MyBaseFolder, "Models");


        private static bool _IsInDesignModeTested = false;
        private static bool _DesignModeTestResult = false;

        public Form1 MainForm = null;

        public BindingList<IConfigItem> MakeConfigItemList()
        {
            var ret = new BindingList<IConfigItem>();
            var header_app = new GroupHeader() { Name = "Application", GroupType = EConfigItemType.Setting };
            ret.Add(header_app);
            ret.Add(Settings);
            var header_presets = new GroupHeader() { Name = "Presets", GroupType = EConfigItemType.Preset };
            ret.Add(header_presets);
            foreach (var preset in Config.ChatConfig.ConfigPresets.OrderBy(x => x.Name.ToLower()))
                ret.Add(preset);
            var header_templates = new GroupHeader() { Name = "Prompt templates", GroupType = EConfigItemType.PromptTemplate };
            ret.Add(header_templates);
            foreach (var template in Config.ChatConfig.PromptTemplates.OrderBy(x => x.Name.ToLower()))
                ret.Add(template);
            var header_samplers = new GroupHeader() { Name = "Samplers", GroupType = EConfigItemType.SamplingOptions };
            ret.Add(header_samplers);
            foreach (var sampler in Config.ChatConfig.SamplingParams.OrderBy(x => x.Name.ToLower()))
                ret.Add(sampler);
            return ret;
        }

        public int InsertConfigItem(BindingList<IConfigItem> list, IConfigItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (item.ItemType == EConfigItemType.Setting ||
                item.ItemType == EConfigItemType.None ||
                item.ItemType == EConfigItemType.Header)
                throw new ArgumentException("wrong ItemTypw");
            var indexed_list = list.Select((x, k) => new { ind = k, item = x }).ToList();
            var header_item = indexed_list.FirstOrDefault(x => (x.item as GroupHeader)?.GroupType == item.ItemType);
            if (header_item == null) return -1;
            int pos1 = header_item.ind + 1;
            var next_header = indexed_list.Skip(header_item.ind + 1).FirstOrDefault(x => x.item is GroupHeader);
            int pos2 = next_header == null ? list.Count - 1 : next_header.ind - 1;
            int new_pos = pos1;
            if (new_pos == list.Count)
            {
                list.Add(item);
                return new_pos;
            }
            if (new_pos == pos2)
            {
                list.Insert(new_pos, item);
                return new_pos;
            }
            new_pos = pos2;
            for (int i = pos1; i <= pos2; i++)
            {
                if (string.Compare(list[i].Name, item.Name, true) > 0)
                {
                    new_pos = i;
                    break;
                }
            }
            list.Insert(new_pos, item);
            return new_pos;
        }

        public IConfigItem MakeNewConfigItem(EConfigItemType itemtype) =>
            itemtype switch
            {
                EConfigItemType.Preset => new ConfigPreset(),
                EConfigItemType.PromptTemplate => new PromptTemplate(),
                EConfigItemType.SamplingOptions => new SamplingParams(),
                _ => null,
            };

        public IConfigItem CloneConfigItem(IConfigItem item) =>
            item switch
            {
                ConfigPreset preset => preset with { Name = "?" },
                PromptTemplate prompttemplate => prompttemplate with { Name = "?" },
                SamplingParams samplingoptions => samplingoptions with { Name = "?" },
                _ => null,
            };

        public void AddNewConfigItem(IConfigItem item)
        {
            switch (item.ItemType)
            {
                case EConfigItemType.Preset:
                    var preset = item as ConfigPreset;
                    preset.ChatConfig = Config.ChatConfig;
                    Config.ChatConfig.ConfigPresets.Add(item as ConfigPreset);
                    return;
                case EConfigItemType.PromptTemplate:
                    Config.ChatConfig.PromptTemplates.Add(item as PromptTemplate);
                    return;
                case EConfigItemType.SamplingOptions:
                    Config.ChatConfig.SamplingParams.Add(item as SamplingParams);
                    return;
            }
            throw new InvalidOperationException();
        }

        public void RemoveConfigItem(IConfigItem item)
        {
            switch (item.ItemType)
            {
                case EConfigItemType.Preset:
                    Config.ChatConfig.ConfigPresets.Remove(item as ConfigPreset);
                    return;
                case EConfigItemType.PromptTemplate:
                    Config.ChatConfig.PromptTemplates.Remove(item as PromptTemplate);
                    return;
                case EConfigItemType.SamplingOptions:
                    Config.ChatConfig.SamplingParams.Remove(item as SamplingParams);
                    return;
            }
            throw new InvalidOperationException();
        }

        public static bool IsInDesignMode
        {
            get
            {
                if (!_IsInDesignModeTested)
                {
                    _DesignModeTestResult = Process.GetCurrentProcess().ProcessName == "DesignToolsServer";
                    _IsInDesignModeTested = true;
                }
                return _DesignModeTestResult;
            }
        }

        public MyData()
        {
            if (_MyData != null && _MyData != this)
                throw new InvalidOperationException("_MyData already created");
            LoadSettings();
            LoadConfig();
        }

        public static MyData St
        {
            get
            {
                if (_MyData == null)
                {
                    _MyData = new MyData();
                }
                return _MyData;
            }
        }

        public static string GetMyFolderX()
        {
            string s1 = Utils.GetMyFolder().ToLower();
            string s2 = s1.ToLower();
            string[] ss = new[] {
                "\\bin\\x64\\debug\\net8.0-windows7.0",
                "\\bin\\x64\\release\\net8.0-windows7.0",
            };
            var p1 = ss.FirstOrDefault(x => s2.EndsWith(x));
            if (p1 == null) return s1;
            var projrct_dir_name = s1.Substring(0, s1.Length - p1.Length);
            var ret = (new DirectoryInfo(projrct_dir_name)).Parent.FullName;
            return ret;
        }

        #region ============Settings=====================

        public new static MySettings Settings
        {
            get => (MySettings)KlonsLIB.MyData.Settings;
            set => KlonsLIB.MyData.Settings = value;
        }

        public static MyConfig Config { get; set; } = new MyConfig();

        private static MySettings LoadedSettings = null;
        private static MyConfig LoadedConfig = null;

        public static void Init()
        {
            var st = St;
        }

        public static bool HasChangesConfig() => Config != null && Config != LoadedConfig;
        public static bool HasChangesSettings() => Settings != null && Settings != LoadedSettings;

        public static bool LoadConfig()
        {
            Config = Utils.LoadDataFromXML<MyConfig>(ConfigFileName);
            bool rt = Config != null;
            if (!rt)
                Config = new MyConfig();
            LoadedConfig = Config with { };
            return rt;
        }

        public static bool SaveConfig()
        {
            //if (!HasChangesConfig()) return true;
            bool rt = Utils.SaveDataToXML(Config, ConfigFileName);
            if (!rt) return false;
            LoadedConfig = Config with { };
            return true;
        }

        public static bool LoadSettings()
        {
            Settings = Utils.LoadDataFromXML<MySettings>(SettingsFileName);
            bool rt = Settings != null;
            if (!rt)
                Settings = new MySettings();
            KlonsLIB.MyData.Settings = Settings;
            LoadedSettings = Settings with { };
            return rt;
        }

        public static bool SaveSettings()
        {
            if (!HasChangesSettings()) return true;
            bool rt = Utils.SaveDataToXML(Settings, SettingsFileName);
            if (!rt) return false;
            LoadedSettings = Settings with { };
            return true;
        }

        #endregion
    }
}

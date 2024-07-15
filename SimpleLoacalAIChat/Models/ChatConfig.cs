using KlonsLIB.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLoacalAIChat.Models
{
    public class ChatConfig
    {
        public BindingList<ConfigPreset> ConfigPresets { get; set; } = new();
        public BindingList<PromptTemplate> PromptTemplates { get; set; } = new();
        public BindingList<SamplingParams> SamplingParams { get; set; } = new();

        public string[] GetModelList()
        {
            var ret = new List<string>();
            if (Directory.Exists(MyData.MyModelsFolder))
            {
                var di = new DirectoryInfo(MyData.MyModelsFolder);
                var names = di.GetFiles("*.gguf").Select(x => x.Name).ToArray();
                ret.AddRange(names);
            }
            if (!MyData.Settings.ExtraModelsPath.IsNOE() && 
                Directory.Exists(MyData.Settings.ExtraModelsPath))
            {
                var di = new DirectoryInfo(MyData.Settings.ExtraModelsPath);
                var names = di.GetFiles("*.gguf").Select(x => x.Name).ToArray();
                ret.AddRange(names);
            }
            if (ret.Count == 0) return [];
            return ret.Distinct().Order().ToArray();
        }

        public string GetModelFullFileNmae(string model)
        {
            if (Directory.Exists(MyData.MyModelsFolder))
            {
                var di = new DirectoryInfo(MyData.MyModelsFolder);
                var fnms = di.GetFiles("*.gguf");
                var ret = fnms.FirstOrDefault(x => string.Equals(x.Name, model, StringComparison.InvariantCultureIgnoreCase));
                if(ret != null)
                    return ret.FullName;
            }
            if (!MyData.Settings.ExtraModelsPath.IsNOE() &&
                Directory.Exists(MyData.Settings.ExtraModelsPath))
            {
                var di = new DirectoryInfo(MyData.Settings.ExtraModelsPath);
                var fnms = di.GetFiles("*.gguf");
                var ret = fnms.FirstOrDefault(x => string.Equals(x.Name, model, StringComparison.InvariantCultureIgnoreCase));
                return ret?.FullName;
            }
            return null;
        }

        public ConfigPreset GetConfigPresetByName(string name) => 
            ConfigPresets.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
        
        public PromptTemplate GetPromptTemplateByName(string name) =>
            PromptTemplates.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));

        public SamplingParams GetSamplingParamsName(string name) =>
            SamplingParams.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));


        public string CheckConfigPreset(ConfigPreset configPreset)
        {
            var sb = new StringBuilder();
            configPreset.ChatConfig = this;
            if (configPreset.Model.IsNOE())
            {
                sb.AppendLine($"Model name not set in {configPreset.Name}");
            }
            var model_path = GetModelFullFileNmae(configPreset.Model);
            if (MyData.Settings.WarnAboutMissingModels && !File.Exists(model_path))
            {
                sb.AppendLine($"Model file not found in {configPreset.Name}\n   File: {model_path}");
            }
            if (configPreset.TemplateName.IsNOE())
            {
                sb.AppendLine($"Model prompt template name not set in {configPreset.Name}");
            }
            else
            {
                var prompt_template = GetPromptTemplateByName(configPreset.TemplateName);
                if (prompt_template == null)
                {
                    sb.AppendLine($"Prompt template {configPreset.TemplateName} not set in {configPreset.Name}");
                }
            }
            if (configPreset.SamplerName.IsNOE())
            {
                sb.AppendLine($"Sampling preset name not set in {configPreset.Name}");
            }
            else
            {
                var sampler = GetSamplingParamsName(configPreset.SamplerName);
                if (sampler == null)
                {
                    sb.AppendLine($"Prompt template {configPreset.SamplerName} not set in {configPreset.Name}");
                }
            }
            var ret = sb.ToString();
            if (ret.IsNOE()) ret = "Ok";
            return ret;
        }

        public string CheckLinks()
        {
            var sb = new StringBuilder();
            var modellist = GetModelList().Select(x => x.ToLower()).ToArray();
            foreach (var configPreset in ConfigPresets)
            {
                var rt = CheckConfigPreset(configPreset);
                if (rt != "Ok")
                    sb.Append(rt);
            }
            var ret = sb.ToString();
            if (ret.IsNOE()) ret = "Ok";
            return ret;
        }
    }
}

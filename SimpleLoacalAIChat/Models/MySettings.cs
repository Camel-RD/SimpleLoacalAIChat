using KlonsLIB.Components;
using KlonsLIB;
using KlonsLIB.Forms;
using KlonsLIB.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimpleLoacalAIChat.Models
{
    public record MySettings : IKlonsSettings, INotifyPropertyChanged,
        IMyPropertyValueListProvider, IConfigItem
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EConfigItemType ItemType => EConfigItemType.Setting;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Name { get; set; } = "Settings";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string[] IMyPropertyValueListProvider.GetPropertyValueList(string propertyname)
        {
            if (ChatConfig == null) return new string[0];
            if (propertyname == "AutoLoadPresetName")
            {
                var ret = ChatConfig.ConfigPresets.Select(x => x.Name).ToArray();
                return ret;
            }
            if (propertyname == "ColorThemeId")
            {
                var ret = new[] { "system", "dark1" };
                //var ret = new[] { "system", "dark1", "green", "blackonwhite" };
                return ret;
            }
            return new string[0];
        }

        [XmlIgnore]
        [Browsable(false)]
        public ChatConfig ChatConfig = null;

        private string colorThemeId = "system";
        private MyColorTheme colorTheme = null;

        private int formFontSize = 10;
        private string formFontName = "";
        private int formFontStyle = 0;
        private Font formFont = null;

        [XmlIgnore]
        [Browsable(false)]
        public MyColorTheme ColorTheme
        {
            get
            {
                if (colorTheme == null)
                {
                    colorTheme = ColorThemeHelper.ColorTheme_System;
                }
                return colorTheme;
            }
        }

        [Category("Appearance")]
        [Editor(typeof(KlonsLIB.Components.MyDropDownPropertyEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ColorThemeId
        {
            get { return colorThemeId; }
            set
            {
                if (colorThemeId == value) return;
                colorThemeId = value;
                colorTheme = null;
                switch (value)
                {
                    case "system":
                        colorTheme = ColorThemeHelper.ColorTheme_System;
                        break;
                    case "dark1":
                        colorTheme = ColorThemeHelper.ColorTheme_Dark1;
                        break;
                    case "green":
                        colorTheme = ColorThemeHelper.ColorTheme_Green;
                        break;
                    case "blackonwhite":
                        colorTheme = ColorThemeHelper.ColorTheme_BlackOnWhite;
                        break;
                }
                if (colorTheme == null)
                {
                    colorTheme = ColorThemeHelper.ColorTheme_System;
                    colorThemeId = "system";
                }
                OnPropertyChanged(nameof(ColorThemeId));
            }
        }

        [Browsable(false)]
        public int FormFontSize
        {
            get { return formFontSize; }
            set
            {
                if (formFontSize == value) return;
                formFontSize = value;
                CheckFont(true);
            }
        }

        [Browsable(false)]
        public string FormFontName
        {
            get { return formFontName; }
            set
            {
                if (formFontName == value) return;
                formFontName = value;
                CheckFont(true);
            }
        }

        [Browsable(false)]
        public int FormFontStyle
        {
            get { return formFontStyle; }
            set
            {
                if (formFontStyle == value) return;
                FormFontStyle = value;
                CheckFont(true);
            }
        }

        private void CheckFont(bool fireevent = false)
        {
            if (formFont != null)
            {
                if (formFont.Name == formFontName &&
                    formFont.FontSizeX() == formFontSize &&
                    formFont.Style == (FontStyle)formFontStyle)
                    return;
            }
            string fnm = formFontName;
            if (string.IsNullOrEmpty(fnm))
            {
                fnm = SystemFonts.DefaultFont.Name;
            }
            FontStyle fs = (FontStyle)formFontStyle;
            formFont = new Font(fnm, formFontSize, fs);
            if (fireevent)
                OnPropertyChanged(nameof(FormFont));
        }

        [XmlIgnore]
        [Category("Appearance")]
        public Font FormFont
        {
            get
            {
                CheckFont(false);
                return formFont;
            }
            set
            {
                if (value == null) return;
                if (formFont != null)
                {
                    if (formFont.Name == value.Name &&
                        formFont.FontSizeX() == value.SizeInPoints &&
                        formFont.Style == value.Style)
                        return;
                }
                formFontName = value.Name;
                formFontSize = value.FontSizeX();
                formFontStyle = (int)value.Style;
                formFont = new Font(formFontName, formFontSize, (FontStyle)formFontStyle);
                OnPropertyChanged(nameof(FormFont));
            }
        }

        [Category("AutoLoad")]
        public bool AutoLoadPreset { get; set; } = false;
        [Category("AutoLoad")]
        [Editor(typeof(KlonsLIB.Components.MyDropDownPropertyEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string AutoLoadPresetName { get; set; }

        [Category("Behaviour")]
        public bool WarnAboutMissingModels { get; set; } = false;

        [Category("Colors")]
        [DefaultValue(typeof(Color), "GreenYellow")]
        [XmlIgnore]
        public Color ColorDarkRequest { get; set; } = Color.GreenYellow;

        [Category("Colors")]
        [DefaultValue(typeof(Color), "77, 128, 0")]
        [XmlIgnore]
        public Color ColorLightRequest { get; set; } = Color.FromArgb(77, 128, 0);

        [Category("Colors")]
        [DefaultValue(typeof(Color), "SkyBlue")]
        [XmlIgnore]
        public Color ColorDarkDebud { get; set; } = Color.SkyBlue;

        [Category("Colors")]
        [DefaultValue(typeof(Color), "18, 82, 109")]
        [XmlIgnore]
        public Color ColorLightDebug { get; set; } = Color.FromArgb(18, 82, 109);


        [Browsable(false)]
        public string SColorDarkRequest 
        { 
            get => XmlColor.FromColor(ColorDarkRequest);
            set => ColorDarkRequest = XmlColor.ToColor(value);
        }
        [Browsable(false)]
        public string SColorLightRequest
        {
            get => XmlColor.FromColor(ColorLightRequest);
            set => ColorLightRequest = XmlColor.ToColor(value);
        }
        [Browsable(false)]
        public string SColorDarkDebud
        {
            get => XmlColor.FromColor(ColorDarkDebud);
            set => ColorDarkDebud = XmlColor.ToColor(value);
        }
        [Browsable(false)]
        public string SColorLightDebug
        {
            get => XmlColor.FromColor(ColorLightDebug);
            set => ColorLightDebug = XmlColor.ToColor(value);
        }


        [Browsable(false)]
        public int ReportZoom { get; set; }
        [Browsable(false)]
        public string WindowPos { get; set; }

        [Browsable(false)]
        public BoundsRectange WindowsRect { get; set; } = new();

    }

    public class BoundsRectange
    {
        public BoundsRectange() { }
        public BoundsRectange(int x, int y, int width, int height)
        {
            X = x; Y = y; Width = width; Height = height;
        }
        [XmlAttribute]
        public int X { get; set; } = -1;
        [XmlAttribute]
        public int Y { get; set; } = -1;
        [XmlAttribute]
        public int Width { get; set; } = -1;
        [XmlAttribute]
        public int Height { get; set; } = -1;
    }

}

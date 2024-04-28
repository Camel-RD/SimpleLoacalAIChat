using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using KlonsLIB.Misc;

namespace SimpleLoacalAIChat.Classes
{
    public class ModelTypeDescriptor : CustomTypeDescriptor
    {
        public ModelTypeDescriptor(object target) : base(GetTD(target))
        {
        }

        public static ICustomTypeDescriptor GetTD(object target)
        {
            var base_tdp = TypeDescriptor.GetProvider(target);
            var base_td = base_tdp.GetTypeDescriptor(target);
            return base_td;
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var properties = base.GetProperties(attributes);
            var newProperties = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor property in properties)
            {
                var attrs = property.Attributes.OfType<Attribute>().ToArray();
                var new_attr = new DisplayNameAttribute(SplitPascalCaseString(property.Name));
                attrs = attrs.Concat(new[] { new_attr }).ToArray();
                var newProperty = TypeDescriptor.CreateProperty(property.ComponentType, property, attrs);
                newProperties.Add(newProperty);
            }
            return newProperties;
        }
        public string SplitPascalCaseString(string str)
        {
            if (str.IsNOE()) return str;
            var result = str.SelectMany((c, i) => i != 0 && i < str.Length-1 && char.IsUpper(c) && !char.IsUpper(str[i - 1]) ? new char[] { ' ', c } : new char[] { c });
            return new string(result.ToArray());
        }

        static Regex regex_pascal_case = null;
        public static string SplitPascalCaseString2(string str)
        {
            if (str.IsNOE()) return str;
            if (regex_pascal_case == null) regex_pascal_case = new Regex(
                @"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])"
              );
            return regex_pascal_case.Replace(str, " ");
        }

    }
}

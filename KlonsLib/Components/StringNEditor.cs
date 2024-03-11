using KlonsLIB.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace KlonsLib.Components
{
    public class StringNEditor : UITypeEditor
    {
        public StringNEditor(Type type)
        {
            CollectionType = type;
        }

        Type CollectionType { get; }

        public override object? EditValue(ITypeDescriptorContext? context, 
            IServiceProvider provider, object? value)
        {
            if (value != null && value is not string) return value;
            var svalue = value as string;
            var cvalue = svalue.Split("\\n").ToList();
            var tp = typeof(AnchorEditor).Assembly.GetType("System.Windows.Forms.Design.StringCollectionEditor", true);
            var ctr = tp.GetConstructor(new Type[] { typeof(Type) });
            var sed = ctr.Invoke(new object[] { CollectionType });
            var arg_types = new Type[]
            {
                typeof(ITypeDescriptorContext),
                typeof(IServiceProvider),
                typeof(object)
            };
            var method_EditValue = tp.GetMethod("EditValue", arg_types);
            var cvalue2 = method_EditValue.Invoke(sed, new object[] { context, provider, cvalue }) as List<string>;
            var ret = string.Join("\\n", cvalue2);
            return ret;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) =>
            UITypeEditorEditStyle.Modal;

    }
}

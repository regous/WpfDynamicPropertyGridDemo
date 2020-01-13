using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace WpfDynamicPropertyGridDemo
{
    public class CustomColorEditor: Xceed.Wpf.Toolkit.PropertyGrid.Editors.ColorEditor
    {
        public override FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            FrameworkElement element = base.ResolveEditor(propertyItem);
            if (propertyItem.IsReadOnly)
                Editor.IsEnabled = false;
            return element;
        }
    }
}

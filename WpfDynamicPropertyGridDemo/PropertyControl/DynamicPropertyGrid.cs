using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfDynamicPropertyGridDemo
{
    public class DynamicPropertyGrid: Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid
    {
        public void UpdateProperties()
        {
            object obj = this.SelectedObject;
            this.SelectedObject = null;
            this.SelectedObject = obj;
        }

        public ContextMenu PropertyItemContextMenu
        {
            get { return (ContextMenu)GetValue(PropertyItemContextMenuProperty); }
            set { SetValue(PropertyItemContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyItemContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyItemContextMenuProperty =
            DependencyProperty.Register("PropertyItemContextMenu", typeof(ContextMenu), typeof(DynamicPropertyGrid), new PropertyMetadata(null));

        public ContextMenu CategoryContextMenu
        {
            get { return (ContextMenu)GetValue(CategoryContextMenuProperty); }
            set { SetValue(CategoryContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CategoryContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryContextMenuProperty =
            DependencyProperty.Register("CategoryContextMenu", typeof(ContextMenu), typeof(DynamicPropertyGrid), new PropertyMetadata(null));

        public ContextMenu ThumbContextMenu
        {
            get { return (ContextMenu)GetValue(ThumbContextMenuProperty); }
            set { SetValue(ThumbContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbContextMenuProperty =
            DependencyProperty.Register("ThumbContextMenu", typeof(ContextMenu), typeof(DynamicPropertyGrid), new PropertyMetadata(null));
    }

    public class CategoryValueConterter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MenuItem)
            {
                return Get(value as MenuItem);
            }
            else if (value is ContextMenu)
            {
                return Get(value as ContextMenu);
            }
            else if (value is TextBlock)
            {
                return Get(value as TextBlock);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string Get(MenuItem aMenuItem)
        {
            if (aMenuItem == null)
                return null;
            return Get(aMenuItem.Parent as ContextMenu);
        }

        private string Get(ContextMenu aContextMenu)
        {
            if (aContextMenu == null)
                return null;
            return Get(aContextMenu.PlacementTarget as TextBlock);
        }

        private string Get(TextBlock aTextBlock)
        {
            if (aTextBlock == null)
                return null;
            return aTextBlock.Text;
        }
    }

    public class PropertyItemValueConterter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is MenuItem)
            {
                return Get(FindAncestor<Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem>(((value as MenuItem).Parent as ContextMenu).PlacementTarget));
            }
            else if(value is ContextMenu)
            {
                return Get(FindAncestor<Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem>((value as ContextMenu).PlacementTarget));
            }
            else if(value is Visual)
            {
                return Get(FindAncestor<Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem>(value as Visual));
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private T FindAncestor<T>(Visual objVisual) where T : Visual
        {
            if (objVisual is T)
                return objVisual as T;
            Visual aVisual = VisualTreeHelper.GetParent(objVisual) as Visual;
            if (aVisual == null)
                return null;
            return FindAncestor<T>(aVisual);
        }

        private System.ComponentModel.PropertyDescriptor Get(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem aPropertyItem)
        {
            if (aPropertyItem == null)
                return null;
            return aPropertyItem.PropertyDescriptor;
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDynamicPropertyGridDemo
{
    /// <summary>
    /// DynamicView.xaml 的交互逻辑
    /// </summary>
    public partial class DynamicView : UserControl
    {
        public DynamicView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            myProperties = new CustomClass();
            myProperties.Add(new CustomProperty("Card", 0.1, typeof(double), false, true, "Cat2"));
            myProperties.Add(new CustomProperty("Bank", true, typeof(bool), false, true, "Cat2"));
            myProperties.Add(new CustomProperty("Name", "Sven", typeof(string), false, true, "Cat1", typeof(CustomTextBlockEditor)));
            myProperties.Add(new CustomProperty("Surname", "Bendo", typeof(string), false, true, "Cat1", typeof(CustomTextBlockEditor)));

            wndDynamicPropertyGrid.AutoGenerateProperties = true;
            wndDynamicPropertyGrid.SelectedObject = myProperties;
            wndDynamicPropertyGrid.ShowSortOptions = true;
            wndDynamicPropertyGrid.ShowSearchBox = false;
            wndDynamicPropertyGrid.ShowPreview = false;
            wndDynamicPropertyGrid.ShowSummary = true;
            wndDynamicPropertyGrid.ShowDescriptionByTooltip = true;
            wndDynamicPropertyGrid.ShowAdvancedOptions = true;
            wndDynamicPropertyGrid.ShowHorizontalScrollBar = true;
            wndDynamicPropertyGrid.ShowTitle = true;

            //wndDynamicPropertyGrid.PropertyDefinitions.Add(new Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinition() { Category = "Cat1", DisplayName = "Name", TargetProperties = new string[] { "Name" } });
            //wndDynamicPropertyGrid.Update();
            //Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinitionCollection aPropertyDefinitionCollection = new Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinitionCollection();
            //Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinition aPropertyDefinition = new Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinition();
            //aPropertyDefinition.DisplayName = "A";
            //aPropertyDefinition.Category = "AA";
            //aPropertyDefinitionCollection.Add(aPropertyDefinition);
            //wndDynamicPropertyGrid.PropertyDefinitions = aPropertyDefinitionCollection;
            wndDynamicPropertyGrid.SelectedObjectTypeName = "CustomClass";
            wndDynamicPropertyGrid.SelectedObjectName = "Instance";
        }

        private CustomClass myProperties;
        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddPropertyWindow dlg = new AddPropertyWindow();
            dlg.Owner = FindAncestor<Window>(this);
            dlg.Categories = this.myProperties.Categories;
            dlg.PropertyNames = this.myProperties.PropertyNames;
            if (true == dlg.ShowDialog())
            {
                myProperties.Add(new CustomProperty(dlg.PropertyName, dlg.DefaultValue, typeof(string), false, true, dlg.Category));
                wndDynamicPropertyGrid.UpdateProperties();
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            MenuItem aMenuItem = sender as MenuItem;
            string sCategory = (aMenuItem.Parent as ContextMenu).DataContext as string;
            myProperties.RemoveCategory(sCategory);
            wndDynamicPropertyGrid.UpdateProperties();
        }
        private void AddCategoryProperty_Click(object sender, RoutedEventArgs e)
        {
            MenuItem aMenuItem = sender as MenuItem;
            string sCategory = (aMenuItem.Parent as ContextMenu).DataContext as string;
            AddPropertyWindow dlg = new AddPropertyWindow();
            dlg.Owner = FindAncestor<Window>(this);
            dlg.Category = sCategory;
            dlg.Categories = myProperties.Categories;
            dlg.PropertyNames = myProperties.PropertyNames;
            if (true == dlg.ShowDialog())
            {
                myProperties.Add(new CustomProperty(dlg.PropertyName, dlg.DefaultValue, typeof(string), false, true, dlg.Category));
                wndDynamicPropertyGrid.UpdateProperties();
            }
            wndDynamicPropertyGrid.UpdateProperties();
        }
        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            MenuItem aMenuItem = sender as MenuItem;
            CustomPropertyDescriptor aCustomPropertyDescriptor = (aMenuItem.Parent as ContextMenu).DataContext as CustomPropertyDescriptor;
            AddPropertyWindow dlg = new AddPropertyWindow();
            dlg.Owner = FindAncestor<Window>(this);
            dlg.Category = aCustomPropertyDescriptor.Category;
            dlg.Categories = myProperties.Categories;
            dlg.PropertyNames = myProperties.PropertyNames;
            if (true == dlg.ShowDialog())
            {
                myProperties.Add(new CustomProperty(dlg.PropertyName, dlg.DefaultValue, typeof(string), false, true, dlg.Category));
                wndDynamicPropertyGrid.UpdateProperties();
            }
        }

        private void DeleteProperty_Click(object sender, RoutedEventArgs e)
        {
            MenuItem aMenuItem = sender as MenuItem;
            CustomPropertyDescriptor aCustomPropertyDescriptor = (aMenuItem.Parent as ContextMenu).DataContext as CustomPropertyDescriptor;
            myProperties.Remove(aCustomPropertyDescriptor.Name);
            wndDynamicPropertyGrid.UpdateProperties();
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
    }
}

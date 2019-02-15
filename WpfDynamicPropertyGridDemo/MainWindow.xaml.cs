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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private CustomClass myProperties;
        private void WndWindow_Loaded(object sender, RoutedEventArgs e)
        {
            myProperties = new CustomClass();
            myProperties.Add(new CustomProperty("Card", 0.1, typeof(double), false, true, "Cat2"));
            myProperties.Add(new CustomProperty("Bank", true, typeof(bool), false, true, "Cat2"));
            myProperties.Add(new CustomProperty("Name", "Sven", typeof(string), false, true, "Cat1", typeof(CustomTextBlockEditor)));
            myProperties.Add(new CustomProperty("Surname", "Bendo", typeof(string), false, true, "Cat1", typeof(CustomTextBlockEditor)));

            wndPropertyGrid.AutoGenerateProperties = true;
            wndPropertyGrid.SelectedObject = myProperties;
            wndPropertyGrid.ShowSortOptions = true;
            wndPropertyGrid.ShowSearchBox = false;
            wndPropertyGrid.ShowPreview = false;
            wndPropertyGrid.ShowSummary = true;
            wndPropertyGrid.ShowDescriptionByTooltip = true;
            wndPropertyGrid.ShowAdvancedOptions = true;
            wndPropertyGrid.ShowHorizontalScrollBar = true;
            wndPropertyGrid.ShowTitle = true;

            //wndPropertyGrid.PropertyDefinitions.Add(new Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinition() { Category = "Cat1", DisplayName = "Name", TargetProperties = new string[] { "Name" } });
            //wndPropertyGrid.Update();
            //Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinitionCollection aPropertyDefinitionCollection = new Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinitionCollection();
            //Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinition aPropertyDefinition = new Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinition();
            //aPropertyDefinition.DisplayName = "A";
            //aPropertyDefinition.Category = "AA";
            //aPropertyDefinitionCollection.Add(aPropertyDefinition);
            //wndPropertyGrid.PropertyDefinitions = aPropertyDefinitionCollection;
            //wndPropertyGrid.SelectedObjectTypeName = "AAA";
            //wndPropertyGrid.SelectedObjectName = "BBB";
        }


       
        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddPropertyWindow dlg = new AddPropertyWindow();
            dlg.Owner = this;
            dlg.Categories = this.myProperties.Categories;
            dlg.PropertyNames = this.myProperties.PropertyNames;
            if(true==dlg.ShowDialog())
            {
                myProperties.Add(new CustomProperty(dlg.PropertyName, dlg.DefaultValue, typeof(string), false, true, dlg.Category));
                wndPropertyGrid.UpdateProperties();
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            MenuItem aMenuItem = sender as MenuItem;
            string sCategory = (aMenuItem.Parent as ContextMenu).DataContext as string;
            myProperties.RemoveCategory(sCategory);
            wndPropertyGrid.UpdateProperties();
        }
        private void AddCategoryProperty_Click(object sender, RoutedEventArgs e)
        {
            MenuItem aMenuItem = sender as MenuItem;
            string sCategory = (aMenuItem.Parent as ContextMenu).DataContext as string;
            AddPropertyWindow dlg = new AddPropertyWindow();
            dlg.Owner = this;
            dlg.Category = sCategory;
            dlg.Categories = myProperties.Categories;
            dlg.PropertyNames = myProperties.PropertyNames;
            if (true == dlg.ShowDialog())
            {
                myProperties.Add(new CustomProperty(dlg.PropertyName, dlg.DefaultValue, typeof(string), false, true, dlg.Category));
                wndPropertyGrid.UpdateProperties();
            }
            wndPropertyGrid.UpdateProperties();
        }
        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            MenuItem aMenuItem = sender as MenuItem;
            CustomPropertyDescriptor aCustomPropertyDescriptor = (aMenuItem.Parent as ContextMenu).DataContext as CustomPropertyDescriptor;
            AddPropertyWindow dlg = new AddPropertyWindow();
            dlg.Owner = this;
            dlg.Category = aCustomPropertyDescriptor.Category;
            dlg.Categories = myProperties.Categories;
            dlg.PropertyNames = myProperties.PropertyNames;
            if(true==dlg.ShowDialog())
            {
                myProperties.Add(new CustomProperty(dlg.PropertyName, dlg.DefaultValue, typeof(string), false, true, dlg.Category));
                wndPropertyGrid.UpdateProperties();
            }
        }

        private void DeleteProperty_Click(object sender, RoutedEventArgs e)
        {
            MenuItem aMenuItem = sender as MenuItem;
            CustomPropertyDescriptor aCustomPropertyDescriptor = (aMenuItem.Parent as ContextMenu).DataContext as CustomPropertyDescriptor;
            myProperties.Remove(aCustomPropertyDescriptor.Name);
            wndPropertyGrid.UpdateProperties();
        }
    }
}

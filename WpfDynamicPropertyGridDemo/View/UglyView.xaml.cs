using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace WpfDynamicPropertyGridDemo
{
    /// <summary>
    /// UglyView.xaml 的交互逻辑
    /// </summary>
    public partial class UglyView : UserControl
    {
        private Person person;
        public UglyView()
        {
            InitializeComponent();
            person = new Man();
            person.FirstName = "John";
            person.LastName = "Doe";
            person.WritingFontSize = 12;
            person.Friends = new List<Person>() { new Man() { FirstName = "First", LastName = "Friend" }, new Woman() { FirstName = "Second", LastName = "Friend" } };
            person.Spouse = new Woman()
            {
                FirstName = "Jane",
                LastName = "Doe"
            };
            
            PreparePropertyGrid();
            this.DataContext = person;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.propertyGrid.AutoGenerateProperties = false;
        }

        private void PreparePropertyGrid()
        {
            PropertyDefinitionCollection propertyDefinitions = new PropertyDefinitionCollection();

            var properties = TypeDescriptor.GetProperties(person.GetType());
            // Allowing for multiple selection, if on further iterations through the selected items we will remove properties that do not exist in both PropertySets
            foreach (var p in properties.Cast<PropertyDescriptor>())
            {
                if (p.PropertyType != typeof(System.Windows.Media.Color?))
                    continue;
                string category = p.Category;
                string description = p.Description;
                string displayName = p.DisplayName ?? p.Name;
                int? displayOrder = null;
                bool? isBrowsable = p.IsBrowsable;
                bool? isExpandable = null;

                var orderAttribute = p.Attributes[typeof(PropertyOrderAttribute)] as PropertyOrderAttribute;
                if (orderAttribute != null)
                {
                    displayOrder = orderAttribute.Order;
                }

                var expandableAttribute = p.Attributes[typeof(ExpandableObjectAttribute)] as ExpandableObjectAttribute;
                if (expandableAttribute != null)
                {
                    isExpandable = true;
                }
                
                var aPropertyDefinition = new PropertyDefinition
                {
                    Category = category,
                    Description = description,
                    DisplayName = displayName,
                    DisplayOrder = displayOrder,
                    IsBrowsable = isBrowsable,
                    IsExpandable = isExpandable,
                    TargetProperties = new[] { p.Name },
                };
                if (p.PropertyType == typeof(System.Windows.Media.Color?))
                {
                    aPropertyDefinition.IsExpandable = true;
                    aPropertyDefinition.PropertyDefinitions.Add(new PropertyDefinition() { TargetProperties = new[] { "R", "G", "B" } });
                }
                propertyDefinitions.Add(aPropertyDefinition);

            }
            this.propertyGrid.PropertyDefinitions = propertyDefinitions;
        }

        public void UpdateProperties(Tuple<string, bool?, Visibility?>[] newPropertyStates)
        {
            // Note this currently works under the assumption that an Item has to be selected in order to have a value changed.

            foreach (var property in newPropertyStates)
            {
                string propertyName = property.Item1;

                string[] splits = propertyName.Split('.');
                if (splits.Length == 1)
                {
                    this.propertyGrid.Properties.OfType<PropertyItem>()
                                                .Where(p => string.Equals(p.PropertyDescriptor.Name, propertyName, StringComparison.Ordinal))
                                                .Map(p =>
                                                {
                                                    if (property.Item2.HasValue)
                                                    {
                                                        p.IsEnabled = property.Item2.Value;
                                                    }

                                                    if (property.Item3.HasValue)
                                                    {
                                                        p.Visibility = property.Item3.Value;
                                                    }
                                                });

                }
                else // We currently don't expect to go any lower than 1 level.
                {
                    var parent = this.propertyGrid.Properties.OfType<PropertyItem>()
                                                             .Where(p => string.Equals(p.PropertyDescriptor.Name, splits[0], StringComparison.Ordinal))
                                                             .FirstOrDefault();

                    if (parent != null)
                    {
                        parent.Properties.OfType<PropertyItem>()
                                         .Where(p => string.Equals(p.PropertyDescriptor.Name, splits[1], StringComparison.Ordinal))
                                         .Map(p =>
                                         {
                                             if (property.Item2.HasValue)
                                             {
                                                 p.IsEnabled = property.Item2.Value;
                                             }
                                             if (property.Item3.HasValue)
                                             {
                                                 p.Visibility = property.Item3.Value;
                                             }
                                         });
                    }
                }
            }

        }

        void PropertyGrid_PreparePropertyItem(object sender, PropertyItemEventArgs e)
        {
            if (e.PropertyItem.ParentElement is PropertyItem)
            {
                PropertyItem aParentPropertyItem = e.PropertyItem.ParentElement as PropertyItem;
                if (aParentPropertyItem == null)
                    return;
                PropertyItem aChildPropertyItem = e.PropertyItem as PropertyItem;
                if (aChildPropertyItem.PropertyName != "R" &&
                    aChildPropertyItem.PropertyName != "G" &&
                    aChildPropertyItem.PropertyName != "B")
                {
                    e.PropertyItem.Visibility = Visibility.Collapsed;
                    e.Handled = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Tuple<string, bool?, Visibility?>> lsTuple = new List<Tuple<string, bool?, Visibility?>>();
            lsTuple.Add(new Tuple<string, bool?, Visibility?>("IsMale", true, Visibility.Collapsed));
            UpdateProperties(lsTuple.ToArray());
        }
    }
}

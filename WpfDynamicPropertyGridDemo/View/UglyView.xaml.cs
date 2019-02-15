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
        public UglyView()
        {
            InitializeComponent();
            var person = new Man();
            person.FirstName = "John";
            person.LastName = "Doe";
            person.WritingFontSize = 12;
            person.Friends = new List<Person>() { new Man() { FirstName = "First", LastName = "Friend" }, new Woman() { FirstName = "Second", LastName = "Friend" } };
            person.Spouse = new Woman()
            {
                FirstName = "Jane",
                LastName = "Doe"
            };

            this.DataContext = person;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var pds = propertyGrid.PropertyDefinitions;
        }
        private List<object> currentPropertySelection = new List<object>();
        private bool suppressPropertyUpdates = false;
        private void PreparePropertyGrid()
        {
            PropertyDefinitionCollection propertyDefinitions = new PropertyDefinitionCollection();

            // This is how I determine 
            var mainPropertySet = this.currentPropertySelection.FirstOrDefault();

            if (mainPropertySet != null)
            {
                var properties = TypeDescriptor.GetProperties(mainPropertySet.GetType());
                // Allowing for multiple selection, if on further iterations through the selected items we will remove properties that do not exist in both PropertySets
                bool firstIteration = true;

                foreach (var x in this.currentPropertySelection)
                {
                    foreach (var p in properties.Cast<PropertyDescriptor>())
                    {
                        if (!firstIteration)
                        {
                            // Perhaps we should be checking a little more safely for TargetProperties but if the collection is empty we have bigger problems.
                            var definition = propertyDefinitions.FirstOrDefault(d => string.Equals(d.TargetProperties[0] as string, p.Name, StringComparison.Ordinal));

                            // Someone in the selection does not have this property so we can ignore it.
                            if (definition == null)
                            {
                                continue;
                            }

                            // If this item doesn't have the property remove it from the display collection and proceed.
                            var localProperty = x.GetType().GetProperty(p.Name);
                            if (localProperty == null)
                            {
                                propertyDefinitions.Remove(definition);
                                continue;
                            }

                            // There is actually no point in proceeding if this is not the first iteration and we have checked whether the property exists.
                            continue;
                        }

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

                        propertyDefinitions.Add(new PropertyDefinition
                        {
                            Category = category,
                            Description = description,
                            DisplayName = displayName,
                            DisplayOrder = displayOrder,
                            IsBrowsable = isBrowsable,
                            IsExpandable = isExpandable,
                            TargetProperties = new[] { p.Name },
                        });
                    }
                }

                firstIteration = false;

                this.propertyGrid.PropertyDefinitions = propertyDefinitions;
            }
        }

        public void UpdateProperties(Tuple<string, bool?, Visibility?>[] newPropertyStates)
        {
            // Note this currently works under the assumption that an Item has to be selected in order to have a value changed.
            this.suppressPropertyUpdates = true;

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

            this.suppressPropertyUpdates = false;
        }

        void PropertyGrid_PreparePropertyItem(object sender, PropertyItemEventArgs e)
        {
            foreach (var x in this.currentPropertySelection)
            {
                // If we are in read-only mode do not allow the editing of any property.

                string propertyName = ((PropertyItem)e.PropertyItem).PropertyDescriptor.Name;
                System.Reflection.PropertyInfo property = x.GetType().GetProperty(propertyName);
                var propertyItem = e.Item as PropertyItem;

                // If the property doesn't exist then check to see if it is on an expandable item.
                if (property == null)
                {
                    property = propertyItem.Instance.GetType().GetProperty(propertyName);
                }

                bool hasProperty = property != null;

                if (hasProperty)
                {
                    var browsableAttribute = property.GetCustomAttribute<BrowsableAttribute>(true);
                    if (browsableAttribute != null &&
                        !browsableAttribute.Browsable)
                    {
                        e.PropertyItem.Visibility = Visibility.Collapsed;
                        e.Handled = true;
                        break;
                    }

                    var visibilityAttribute = property.GetCustomAttribute<VisibilityAttribute>(true);
                    if (visibilityAttribute != null)
                    {
                        e.PropertyItem.Visibility = visibilityAttribute.Visibility;
                        e.Handled = true;
                    }
                }
            }
        }
    }
}

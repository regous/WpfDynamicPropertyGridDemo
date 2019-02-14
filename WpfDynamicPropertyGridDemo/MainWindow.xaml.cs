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
    public class CustomClass : System.Dynamic.DynamicObject, System.ComponentModel.ICustomTypeDescriptor, System.ComponentModel.INotifyPropertyChanged
    {
        private List<CustomProperty> Properties = new List<CustomProperty>();
        public void Add(CustomProperty Value)
        {
            Properties.Add(Value);
        }

        public void Remove(string Name)
        {
            foreach (CustomProperty prop in Properties)
            {
                if (prop.Name == Name)
                {
                    Properties.Remove(prop);
                    return;
                }
            }
        }

        public void RemoveCategory(string Category)
        {
            for (int i = Properties.Count-1; i >=0 ; i--)
            {
                if (Properties[i].Category == Category)
                    Properties.RemoveAt(i);
            }
        }
        public List<string> Categories => Properties.Select(x => x.Category).Distinct().ToList();
        public List<string> PropertyNames => Properties.Select(x => x.Name).Distinct().ToList();

        public CustomProperty this[int index]
        {
            get
            {
                return (CustomProperty)Properties[index];
            }
            set
            {
                Properties[index] = (CustomProperty)value;
            }
        }

        #region ICustomTypeDescriptor Members
        public System.ComponentModel.AttributeCollection GetAttributes()
        {
            return System.ComponentModel.TypeDescriptor.GetAttributes(this, true);
        }
        public string GetClassName()
        {
            return System.ComponentModel.TypeDescriptor.GetClassName(this, true);
        }
        public string GetComponentName()
        {
            return null;
        }
        public System.ComponentModel.TypeConverter GetConverter()
        {
            return System.ComponentModel.TypeDescriptor.GetConverter(this, true);
        }
        public System.ComponentModel.EventDescriptor GetDefaultEvent()
        {
            return System.ComponentModel.TypeDescriptor.GetDefaultEvent(this, true);
        }
        public System.ComponentModel.PropertyDescriptor GetDefaultProperty()
        {
            return System.ComponentModel.TypeDescriptor.GetDefaultProperty(this, true);
        }
        public object GetEditor(Type editorBaseType)
        {
            return null;
        }
        public System.ComponentModel.EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return System.ComponentModel.TypeDescriptor.GetEvents(this, attributes, true);
        }
        public System.ComponentModel.EventDescriptorCollection GetEvents()
        {
            return System.ComponentModel.TypeDescriptor.GetEvents(this, true);
        }
        public object GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd)
        {
            return this;
        }
        public System.ComponentModel.PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return this.GetProperties();
        }

        // Method implemented to expose Volume and PayLoad properties conditionally, depending on TypeOfCar
        public System.ComponentModel.PropertyDescriptorCollection GetProperties()
        {
            var props = new System.ComponentModel.PropertyDescriptorCollection(null);

            for (int i = 0; i < Properties.Count; i++)
            {
                var item = Properties[i];
                props.Add(new CustomPropertyDescriptor(ref item, item.EditorType == null ? null : new Attribute[] { new System.ComponentModel.EditorAttribute(item.EditorType, item.EditorType) }));
            }

            return props;
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }

    /// <summary>
    /// Custom property class 
    /// </summary>
    public class CustomProperty
    {
        private string sName = string.Empty;
        private string sCategory = "";
        private bool bReadOnly = false;
        private bool bVisible = true;
        private object objValue = null;
        private Type objEditorType = null;

        public CustomProperty(string sName, object value, Type type, bool bReadOnly, bool bVisible, string sCategory, Type editorType = null)
        {
            this.sName = sName;
            this.objValue = value;
            this.type = type;
            this.bReadOnly = bReadOnly;
            this.bVisible = bVisible;
            this.sCategory = sCategory;
            this.objEditorType = editorType;
        }

        private Type type;
        public Type Type
        {
            get { return type; }
        }

        public bool ReadOnly
        {
            get
            {
                return bReadOnly;
            }
        }

        public string Name
        {
            get
            {
                return sName;
            }
        }

        public string Category
        {
            get { return sCategory; }
        }

        public bool Visible
        {
            get { return bVisible; }
        }

        public object Value
        {
            get { return objValue; }
            set { objValue = value; }
        }

        public Type EditorType
        {
            get { return objEditorType; }
            set { objEditorType = value; }
        }

    }


    /// <summary>
    /// Custom PropertyDescriptor
    /// </summary>
    public class CustomPropertyDescriptor : System.ComponentModel.PropertyDescriptor
    {
        CustomProperty m_Property;
        public CustomPropertyDescriptor(ref CustomProperty myProperty, Attribute[] attrs) : base(myProperty.Name, attrs)
        {
            m_Property = myProperty;
        }

        #region PropertyDescriptor specific

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override object GetValue(object component)
        {
            return m_Property.Value;
        }

        public override string Description
        {
            get { return m_Property.Name; }
        }

        public override string Category
        {
            get { return m_Property.Category; }
        }

        public override string DisplayName
        {
            get { return m_Property.Name; }
        }

        public override bool IsReadOnly
        {
            get { return m_Property.ReadOnly; }
        }

        public override void ResetValue(object component)
        {
            //Have to implement
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override void SetValue(object component, object value)
        {
            m_Property.Value = value;
        }

        public override Type PropertyType
        {
            get { return m_Property.Type; }
        }

        #endregion
    }

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
            if (!(aMenuItem.Tag is string))
                return;
            string category = aMenuItem.Tag as string;
            myProperties.RemoveCategory(category);
            wndPropertyGrid.UpdateProperties();
        }

        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            MenuItem aMenuItem = sender as MenuItem;
            CustomClass aCustomClass = aMenuItem.Tag as CustomClass;
            CustomPropertyDescriptor aCustomPropertyDescriptor = aMenuItem.DataContext as CustomPropertyDescriptor;
            AddPropertyWindow dlg = new AddPropertyWindow();
            dlg.Owner = this;
            dlg.Category = aCustomPropertyDescriptor.Category;
            dlg.Categories = aCustomClass.Categories;
            dlg.PropertyNames = aCustomClass.PropertyNames;
            if(true==dlg.ShowDialog())
            {
                aCustomClass.Add(new CustomProperty(dlg.PropertyName, dlg.DefaultValue, typeof(string), false, true, dlg.Category));
                wndPropertyGrid.UpdateProperties();
            }
        }

        private void DeleteProperty_Click(object sender, RoutedEventArgs e)
        {
            MenuItem aMenuItem = sender as MenuItem;
            CustomClass aCustomClass = aMenuItem.Tag as CustomClass;
            CustomPropertyDescriptor aCustomPropertyDescriptor = aMenuItem.DataContext as CustomPropertyDescriptor;
            aCustomClass.Remove(aCustomPropertyDescriptor.Name);
            wndPropertyGrid.UpdateProperties();
        }
    }
}

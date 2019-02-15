using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            for (int i = Properties.Count - 1; i >= 0; i--)
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
}

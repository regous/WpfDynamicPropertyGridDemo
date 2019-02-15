using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDynamicPropertyGridDemo
{
    public class DataObjects
    {
        public DataObjects()
        {
            this.CollectionProperty = new System.Collections.ObjectModel.Collection<string> { "Item 1", "Item 2", "Item 3" };
            this.StringProperty = "Hi!";
        }

        [System.ComponentModel.Category("Information")]
        [System.ComponentModel.DisplayName("First Name")]
        [System.ComponentModel.Description("This property uses a TextBox as the default editor.")]
        //This custom editor is a Class that implements the ITypeEditor interface
        //[System.ComponentModel.Editor(typeof(FirstNameEditor), typeof(FirstNameEditor))]
        public string FirstName { get; set; }
        [System.ComponentModel.Category("Information")]
        [System.ComponentModel.DisplayName("Last Name")]
        [System.ComponentModel.Description("This property uses a TextBox as the default editor.")]
        //This custom editor is a UserControl that implements the ITypeEditor interface
        //[System.ComponentModel.Editor(typeof(LastNameUserControlEditor), typeof(LastNameUserControlEditor))]
        public string LastName { get; set; }

        public string StringProperty { get; set; }

        [System.ComponentModel.Editor(typeof(ReadOnlyCollectionEditor), typeof(ReadOnlyCollectionEditor))]
        public ICollection<string> CollectionProperty { get; private set; }
    }
}

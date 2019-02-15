using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDynamicPropertyGridDemo
{
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
}

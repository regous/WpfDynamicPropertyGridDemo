using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDynamicPropertyGridDemo
{
    public class VisibilityAttribute: Attribute
    {
        public System.Windows.Visibility Visibility { get; private set; }

        public VisibilityAttribute(System.Windows.Visibility visibility)
        {
            this.Visibility = visibility;
        }
    }
}

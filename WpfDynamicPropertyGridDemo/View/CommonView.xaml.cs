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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace WpfDynamicPropertyGridDemo
{
    /// <summary>
    /// CommonView.xaml 的交互逻辑
    /// </summary>
    public partial class CommonView : UserControl
    {
        public CommonView()
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
    }
}

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
using System.Windows.Shapes;

namespace WpfDynamicPropertyGridDemo
{
    /// <summary>
    /// AddPropertyWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddPropertyWindow : Window
    {
        public AddPropertyWindow()
        {
            InitializeComponent();
        }

        public bool CategoryEditable => !string.IsNullOrEmpty(Category);

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Category.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(AddPropertyWindow), new PropertyMetadata(""));

        

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(AddPropertyWindow), new PropertyMetadata(""));

        public string DefaultValue
        {
            get { return (string)GetValue(DefaultValueProperty); }
            set { SetValue(DefaultValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultValueProperty =
            DependencyProperty.Register("DefaultValue", typeof(string), typeof(AddPropertyWindow), new PropertyMetadata(""));

        public List<string> Categories { get; set; }
        public List<string> PropertyNames { get; set; }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

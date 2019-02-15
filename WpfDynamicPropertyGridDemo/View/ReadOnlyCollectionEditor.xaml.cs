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
    /// ReadOnlyCollectionEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ReadOnlyCollectionEditor : UserControl, Xceed.Wpf.Toolkit.PropertyGrid.Editors.ITypeEditor
    {
        public ReadOnlyCollectionEditor()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
                "Value", typeof(IList<string>), typeof(ReadOnlyCollectionEditor), new PropertyMetadata(default(IList<string>)));

        public IList<string> Value
        {
            get { return (IList<string>)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public FrameworkElement ResolveEditor(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem propertyItem)
        {
            var binding = new Binding("Value")
            {
                Source = propertyItem,
                Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
            };
            BindingOperations.SetBinding(this, ValueProperty, binding);
            return this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReadOnlyCollectionViewer viewer = new ReadOnlyCollectionViewer { DataContext = this };
            viewer.ShowDialog();
        }
    }
}

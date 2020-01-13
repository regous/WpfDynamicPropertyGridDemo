using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfDynamicPropertyGridDemo
{
    public class CustomTextBlockEditor : Xceed.Wpf.Toolkit.PropertyGrid.Editors.TextBoxEditor
    {
        protected override void SetControlProperties(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem propertyItem)
        {
            Editor.IsReadOnly = true;
            base.SetControlProperties(propertyItem);
            Editor.PreviewMouseUp += Editor_MouseDown;
        }
        public override FrameworkElement ResolveEditor(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem propertyItem)
        {
            var fe = base.ResolveEditor(propertyItem);
            return fe;
        }
        private void Editor_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CustomTextBlockEditorWindow dlg = new CustomTextBlockEditorWindow();
            dlg.Text = Editor.Text;
            dlg.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            dlg.Loaded += (x,y)=> 
            {
                System.Windows.Window win = x as System.Windows.Window;
                System.Windows.Point targetLoc = Editor.PointToScreen(new System.Windows.Point(Editor.ActualWidth / 2, Editor.ActualHeight / 2));
                double windowWidth = win.ActualWidth;
                double windowHeight = win.ActualHeight;
                win.Left = targetLoc.X - (windowWidth / 2);
                win.Top = targetLoc.Y - (windowHeight / 2);
            };
            if (true == dlg.ShowDialog())
            {

            }
            Editor.Text = dlg.Text;
        }

    }
}

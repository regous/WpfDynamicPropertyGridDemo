using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfDynamicPropertyGridDemo
{
    public class MyPropertyGrid: Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid
    {
        public void UpdateProperties()
        {
            object obj = this.SelectedObject;
            this.SelectedObject = null;
            this.SelectedObject = obj;
        }

        public ContextMenu PropertyItemContextMenu
        {
            get { return (ContextMenu)GetValue(PropertyItemContextMenuProperty); }
            set { SetValue(PropertyItemContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyItemContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyItemContextMenuProperty =
            DependencyProperty.Register("PropertyItemContextMenu", typeof(ContextMenu), typeof(MyPropertyGrid), new PropertyMetadata(null, new PropertyChangedCallback(PropertyItemContextMenuChanged)));

        public ContextMenu CategoryContextMenu
        {
            get { return (ContextMenu)GetValue(CategoryContextMenuProperty); }
            set { SetValue(CategoryContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CategoryContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryContextMenuProperty =
            DependencyProperty.Register("CategoryContextMenu", typeof(ContextMenu), typeof(MyPropertyGrid), new PropertyMetadata(null, new PropertyChangedCallback(CategoryContextMenuChanged)));

        public ContextMenu ThumbContextMenu
        {
            get { return (ContextMenu)GetValue(ThumbContextMenuProperty); }
            set { SetValue(ThumbContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbContextMenuProperty =
            DependencyProperty.Register("ThumbContextMenu", typeof(ContextMenu), typeof(MyPropertyGrid), new PropertyMetadata(null));

        private static void CategoryContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyPropertyGrid aMyPropertyGrid = d as MyPropertyGrid;
            if (aMyPropertyGrid.CategoryContextMenu == null || aMyPropertyGrid.CategoryContextMenu.Items.Count == 0)
            {
                aMyPropertyGrid.InternalCategoryContextMenu = null;
            }
            else
            {
                ContextMenu aContextMenu = new ContextMenu();
                foreach (MenuItem aMenuItem in aMyPropertyGrid.CategoryContextMenu.Items)
                {
                    MenuItem aMirror = new MenuItem()
                    {
                        Header = aMenuItem.Header,
                        Icon = aMenuItem.Icon
                    };
                    aMirror.Click += aMyPropertyGrid.CategoryContextMenuItem_Click;
                    aContextMenu.Items.Add(aMirror);
                }
                aMyPropertyGrid.InternalCategoryContextMenu = aContextMenu;
            }
        }

        private void CategoryContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryContextMenu == null)
                return;

            MenuItem aMirrorItem = sender as MenuItem;

            UIElement aUIElement = ((System.Windows.Controls.ContextMenu)aMirrorItem.Parent).PlacementTarget;
            foreach (MenuItem aMenuItem in CategoryContextMenu.Items)
            {
                if (aMirrorItem.Header == aMenuItem.Header)
                {
                    aMenuItem.Tag = (aUIElement is TextBlock) ? (aUIElement as TextBlock).Text : null;
                    aMenuItem.DataContext = aUIElement;
                    aMenuItem.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                    return;
                }
            }
        }

        private static void PropertyItemContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyPropertyGrid aMyPropertyGrid = d as MyPropertyGrid;
            if (aMyPropertyGrid.PropertyItemContextMenu == null || aMyPropertyGrid.PropertyItemContextMenu.Items.Count == 0)
            {
                aMyPropertyGrid.InternalPropertyItemContextMenu = null;
            }
            else
            {
                ContextMenu aContextMenu = new ContextMenu();
                foreach(MenuItem aMenuItem in aMyPropertyGrid.PropertyItemContextMenu.Items)
                {
                    MenuItem aMirror = new MenuItem()
                    {
                        Header = aMenuItem.Header,
                        Icon = aMenuItem.Icon
                    };
                    aMirror.Click += aMyPropertyGrid.PropertyItemContextMenuItem_Click;
                    aContextMenu.Items.Add(aMirror);
                }
                aMyPropertyGrid.InternalPropertyItemContextMenu = aContextMenu;
            }
            aMyPropertyGrid.AdvancedOptionsMenu = aMyPropertyGrid.InternalPropertyItemContextMenu;
        }

        private void PropertyItemContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (PropertyItemContextMenu == null)
                return;

            MenuItem aMirrorItem = sender as MenuItem;

            UIElement aUIElement = ((System.Windows.Controls.ContextMenu)aMirrorItem.Parent).PlacementTarget;
            Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem aPropertyItem = FindPropertyItem(aUIElement as Visual);
            if (aPropertyItem == null)
                return;

            object inst = aPropertyItem.Instance;
            System.ComponentModel.PropertyDescriptor desc = aPropertyItem.PropertyDescriptor;

            foreach (MenuItem aMenuItem in PropertyItemContextMenu.Items)
            {
                if (aMirrorItem.Header == aMenuItem.Header)
                {
                    aMenuItem.Tag = inst;
                    aMenuItem.DataContext = desc;
                    aMenuItem.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                }
            }
        }

        private ContextMenu InternalPropertyItemContextMenu = null;
        private ContextMenu InternalCategoryContextMenu = null;

        public MyPropertyGrid()
        {
            base.MouseDown += MyPropertyGrid_MouseDown;
            System.ComponentModel.DependencyPropertyDescriptor prop = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(
                Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid.AdvancedOptionsMenuProperty,
                typeof(Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid)
            );

            prop.AddValueChanged(this, (x, y) =>
            {
                MyPropertyGrid wndPropertyGrid = x as MyPropertyGrid;
                if (wndPropertyGrid.AdvancedOptionsMenu == null)
                {
                    if (wndPropertyGrid.InternalPropertyItemContextMenu != null)
                        wndPropertyGrid.AdvancedOptionsMenu = wndPropertyGrid.InternalPropertyItemContextMenu;
                }
            });

            this.Loaded += MyPropertyGrid_Loaded;
        }

        private void MyPropertyGrid_Loaded(object sender, RoutedEventArgs e)
        {
            PropertyItemContextMenuChanged(this, new DependencyPropertyChangedEventArgs());
            CategoryContextMenuChanged(this, new DependencyPropertyChangedEventArgs());
        }

        enum MyEnum
        {
            Category,
            Thumb,
            Unknown
        }
        private void MyPropertyGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton != System.Windows.Input.MouseButton.Right)
                return;
            var p = e.GetPosition(sender as UIElement);
            MyEnum eEnum = MyEnum.Unknown;
            UIElement aPlacementTarget = sender as UIElement;
            VisualTreeHelper.HitTest(sender as UIElement, null, f =>
            {
                if(f.VisualHit is System.Windows.Controls.TextBlock)
                {
                    if(FindAnchor<System.Windows.Controls.GroupItem>(f.VisualHit as System.Windows.Controls.TextBlock))
                    {
                        aPlacementTarget = f.VisualHit as UIElement;
                        eEnum = MyEnum.Category;
                    }
                }
                else if(f.VisualHit is System.Windows.Controls.Grid)
                {
                    System.Windows.Controls.Grid aGrid = f.VisualHit as System.Windows.Controls.Grid;
                    foreach(UIElement item in aGrid.Children)
                    {
                        if(item is System.Windows.Controls.Primitives.Thumb)
                        {
                            eEnum = MyEnum.Thumb;
                        }
                    }
                }
                string s = f.VisualHit.ToString();
                return HitTestResultBehavior.Stop;
            }, new PointHitTestParameters(p));
            ContextMenu aContextMenu = null;
            if (MyEnum.Category==eEnum)
            {
                aContextMenu = InternalCategoryContextMenu;
            }
            else if(MyEnum.Thumb==eEnum)
            {
                aContextMenu = ThumbContextMenu;
            }
            if (aContextMenu != null)
            {
                //目标
                aContextMenu.PlacementTarget = aPlacementTarget as UIElement;
                //位置
                aContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
                //ThumbContextMenu.HorizontalOffset = p.X;
                //ThumbContextMenu.VerticalOffset = p.Y;
                //显示菜单
                aContextMenu.IsOpen = true;
            }
        }

        static MyPropertyGrid()
        {

        }

        public Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem FindPropertyItem(Visual objVisual)
        {
            if (objVisual is Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem)
                return objVisual as Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem;
            Visual aVisual = VisualTreeHelper.GetParent(objVisual) as Visual;
            if (aVisual == null)
                return null;
            return FindPropertyItem(aVisual);
        }
        public bool FindAnchor<T>(Visual objVisual)
        {
            if (objVisual is T)
                return true;
            else if (objVisual is Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid)
                return false;
            Visual aVisual = VisualTreeHelper.GetParent(objVisual) as Visual;
            if (aVisual == null)
                return false;
            return FindAnchor<T>(aVisual);
        }
        public void EnumVisualTreeParent(int Ident, Visual visualObj)
        {
            Visual aVisual = VisualTreeHelper.GetParent(visualObj) as Visual;
            if (aVisual == null)
                return;
            EnumVisualTreeParent(Ident + 1, aVisual);
        }
        //遍历视觉树
        public void EnumVisualTree(int Ident, Visual visualObj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visualObj); i++)
            {
                //接收特定索引的子元素
                Visual childVisual = (Visual)VisualTreeHelper.GetChild(visualObj, i);
                Console.WriteLine(new string(' ', Ident) + childVisual);

                EnumVisualTree(Ident + 1, childVisual);
            }
        }

        //遍历逻辑树
        public void EnumLogicalTree(int Ident, object logObj)
        {
            if (!(logObj is DependencyObject))//对象必须派生自DependencyObject对象
                return;

            foreach (object childLogical in LogicalTreeHelper.GetChildren(logObj as DependencyObject))
            {
                Console.WriteLine(new string(' ', Ident) + childLogical);

                EnumLogicalTree(Ident + 1, childLogical);
            }
        }
    }
}

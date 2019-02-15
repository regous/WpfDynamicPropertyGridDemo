using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfDynamicPropertyGridDemo
{
    public class UniformTabPanel : UniformGrid
    {
        public UniformTabPanel()
        {
            this.IsItemsHost = true;
            this.Rows = 1;

            //Default, so not really needed..
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var children = this.Children.OfType<TabItem>();
            var totalMaxWidth = children.Sum(tab => tab.MaxWidth);
            if (!double.IsInfinity(totalMaxWidth))
            {
                this.HorizontalAlignment = (constraint.Width > totalMaxWidth)
                                                    ? HorizontalAlignment.Left
                                                    : HorizontalAlignment.Stretch;
                foreach (var child in children)
                {
                    child.Width = this.HorizontalAlignment == System.Windows.HorizontalAlignment.Left
                            ? child.MaxWidth
                            : Double.NaN;
                }
            }
            return base.MeasureOverride(constraint);
        }
    }
}

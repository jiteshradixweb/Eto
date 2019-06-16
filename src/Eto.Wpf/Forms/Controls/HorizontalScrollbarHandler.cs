using System;
using swc = System.Windows.Controls;
using sw = System.Windows;
using swm = System.Windows.Media;
using Eto.Forms;
using Eto.Drawing;

namespace Eto.Wpf.Forms.Controls
{
	public class HorizontalScrollbarHandler : WpfControl<swc.Primitives.ScrollBar, HorizontalScrollbar, HorizontalScrollbar.ICallback>, HorizontalScrollbar.IHandler
	{
		public string Name { get => Control.Name; set => Control.Name = value; }
		public int Maximum { get => Convert.ToInt32(Control.Maximum); set => Control.Maximum = value; }
		public int Minimum { get => Convert.ToInt32(Control.Minimum); set => Control.Minimum = value; }
		public int Value { get => Convert.ToInt32(Control.Value); set => Control.Value = value; }

		public int LargeChange { get => Convert.ToInt32(Control.LargeChange); set => Control.LargeChange = Control.ViewportSize = value; }

		public int SmallChange { get => Convert.ToInt32(Control.SmallChange); set => Control.SmallChange = value; }

		public HorizontalScrollbarHandler()
		{
			Control = new swc.Primitives.ScrollBar();
			Control.Orientation = swc.Orientation.Horizontal;
			Control.Scroll += (sender, e) =>
			{
				Callback.OnScroll(Widget, new ScrollBarEventArgs(Convert.ToInt32(e.NewValue)));
			};
		}
	}
}

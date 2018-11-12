using System;
using swc = System.Windows.Controls;
using sw = System.Windows;
using swm = System.Windows.Media;
using Eto.Forms;
using Eto.Drawing;

namespace Eto.Wpf.Forms.Controls
{
	public class VerticalScrollbarHandler : WpfControl<swc.Primitives.ScrollBar, VerticalScrollbar, VerticalScrollbar.ICallback>, VerticalScrollbar.IHandler
	{
		public string Name { get => Control.Name; set => Control.Name = value; }
		public int Maximum { get => Convert.ToInt32(Control.Maximum); set => Control.Maximum = value; }
		public int Minimum { get => Convert.ToInt32(Control.Minimum); set => Control.Minimum = value; }
		public int Value { get => Convert.ToInt32(Control.Value); set => Control.Value = value; }

		public VerticalScrollbarHandler()
		{
			Control = new swc.Primitives.ScrollBar();
			Control.Orientation = swc.Orientation.Vertical;
			Control.Scroll += (sender, e) =>
			{
				Callback.OnScroll(Widget, new ScrollBarEventArgs(Convert.ToInt32(e.NewValue)));
			};
		}
	}
}

using Eto.Drawing;
using Eto.Forms;
using swf = System.Windows.Forms;

namespace Eto.WinForms.Forms.Controls
{
	public class VerticalScrollbarHandler : WindowsControl<swf.VScrollBar, VerticalScrollbar, VerticalScrollbar.ICallback>, VerticalScrollbar.IHandler
	{
		public string Name
		{
			get { return Control.Name; }
			set { Control.Name = value; }
		}

		public int Maximum
		{
			get => Control.Maximum;
			set => Control.Maximum = value;
		}

		public int Minimum
		{
			get => Control.Minimum;
			set => Control.Minimum = value;
		}

		public int Value
		{
			get => Control.Value;
			set => Control.Value = value;
		}

		public VerticalScrollbarHandler()
		{
			this.Control = new swf.VScrollBar();
			this.Control.Visible = true;
			this.Control.Scroll += (sender, e) =>
			{
				Callback.OnScroll(Widget, new ScrollBarEventArgs(e.NewValue));
			};
		}

		public int LargeChange { get; set; }
		public int SmallChange { get; set; }
	}
}

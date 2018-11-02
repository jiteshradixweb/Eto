using Eto.Forms;
using swf = System.Windows.Forms;

namespace Eto.WinForms.Forms.Controls
{
	class VerticalScrollbarHandler : WindowsControl<swf.VScrollBar, VerticalScrollbar, VerticalScrollbar.ICallback>, VerticalScrollbar.IHandler
	{
		public string Name
		{
			get { return Control.Name; }
			set { Control.Name = value; }
		}

		public VerticalScrollbarHandler()
		{
			this.Control = new swf.VScrollBar();
			this.Control.Visible = true;
			//this.Control.Scroll += (sender, e) =>
			//{
				////Callback.OnScroll(Widget, new ScrollEventArgs(Point.Empty));
			//};
		}
	}
}

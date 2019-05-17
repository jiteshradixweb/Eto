using Eto.Forms;

namespace Eto.GtkSharp.Forms.Controls
{
	public class PanelHandler : GtkPanel<Gtk.EventBox, Panel, Panel.ICallback>, Panel.IHandler
	{
#if GTK3
		readonly GtkShrinkableVBox box;

		public PanelHandler()
		{
			Control = new Gtk.EventBox();
			box = new GtkShrinkableVBox();
			box.Resizable = true;
			Control.CanFocus = true;
			Control.Add(box);
		}

		protected override void SetContainerContent(Gtk.Widget content)
		{
			//Control.Add(content);
			box.Add(content);
		}
#else
		//readonly Gtk.VBox box;
		readonly GtkShrinkableVBox box;

		public PanelHandler()
		{
			Control = new Gtk.EventBox();
			//Control.VisibleWindow = false; // can't use this as it causes overlapping widgets
			//box = new Gtk.VBox();
			box = new GtkShrinkableVBox();
			box.Resizable = true;
			Control.CanFocus = true;
			Control.KeyPressEvent += (s, arg) =>
			{
				if (Connector != null)
				{
					Connector.HandleKeyPressEvent(Control, arg);
				}
			};
			Control.FocusInEvent += (s, arg) =>
			{
				if (Connector != null)
				{
					Connector.FocusInEvent(Control, arg);
				}
			};
			Control.FocusOutEvent += (s, arg) =>
			{
				if (Connector != null)
				{
					Connector.FocusOutEvent(Control, arg);
				}
			};
			Control.Add(box);
		}

		protected override void SetContainerContent(Gtk.Widget content)
		{
			box.Add(content);
		}
#endif
	}
}

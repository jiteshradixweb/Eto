using Eto.Forms;
using System.IO;
using Eto.Drawing;
using Eto.Shared.Drawing;

namespace Eto.GtkSharp.Forms
{
	public class CursorHandler : WidgetHandler<Gdk.Cursor, Cursor>, Cursor.IHandler
	{
		public void Create(Stream stream)
		{
			if (EtoEnvironment.Platform.IsUnix)
			{
				//Below function works on only Linux OS, because in windows OS it crashes due to Gdk.Display value
				Control = new Gdk.Cursor(Gdk.Display.Default, new Gdk.Pixbuf(stream), 0, 0);
			}
			else
			{
				Control = new Gdk.Cursor(CursorType.Arrow.ToGdk());
			}
		}

		public void Create (CursorType cursor)
		{
			Control = new Gdk.Cursor(cursor.ToGdk ());
		}
	}
}


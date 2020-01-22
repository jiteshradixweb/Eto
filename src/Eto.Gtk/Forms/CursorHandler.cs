using System.IO;
using Eto.Drawing;
using Eto.Forms;
using System.IO;
using Eto.Drawing;
using Eto.Shared.Drawing;

namespace Eto.GtkSharp.Forms
{
	public class CursorHandler : WidgetHandler<Gdk.Cursor, Cursor>, Cursor.IHandler
	{
		{
			Control = new Gdk.Cursor(cursor.ToGdk ());
			Control = new Gdk.Cursor(cursor.ToGdk());
		}

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

		public void Create(Bitmap image, PointF hotspot)
		{
			Control = new Gdk.Cursor(Gdk.Display.Default, image.ToGdk(), (int)hotspot.X, (int)hotspot.Y);
		}

		public void Create(string fileName) => Create(new Gdk.Pixbuf(fileName));

		public void Create(Stream stream) => Create(new Gdk.Pixbuf(stream));

		void Create(Gdk.Pixbuf pixbuf)
		{
			var hotspot = PointF.Empty;

			// get hotspot from pixbuf if available
			var xhot = pixbuf.GetOption("x_hot");
			if (float.TryParse(xhot, out var xhotf))
				hotspot.X = xhotf;
			var yhot = pixbuf.GetOption("y_hot");
			if (float.TryParse(yhot, out var yhotf))
				hotspot.Y = yhotf;

			Control = new Gdk.Cursor(Gdk.Display.Default, pixbuf, (int)hotspot.X, (int)hotspot.Y);
		}
	}
}


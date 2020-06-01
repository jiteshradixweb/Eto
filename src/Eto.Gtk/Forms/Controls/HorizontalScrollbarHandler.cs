using System;
using Eto.Forms;

namespace Eto.GtkSharp.Forms.Controls
{
	public class HorizontalScrollbarHandler : GtkControl<Gtk.HScrollbar, HorizontalScrollbar, HorizontalScrollbar.ICallback >, HorizontalScrollbar.IHandler
    {
		public string Name { get => Control.Name; set => Control.Name = value; }
		public int Maximum { get => Convert.ToInt32(newAdjustment.Upper); set => newAdjustment.Upper = value; }
		public int Minimum { get => Convert.ToInt32(newAdjustment.Lower); set => newAdjustment.Lower = value; }
		public int Value { get => Convert.ToInt32(Control.Value); set => Control.Value = value; }
		Gtk.Adjustment newAdjustment;
        public HorizontalScrollbarHandler()
        {
			newAdjustment = new Gtk.Adjustment(0.5, 0, 500, 0.5, 0, 0);//TODO: Need to check last 2 argument
			Control = new Gtk.HScrollbar(newAdjustment);
			Control.ValueChanged += (sender, e) =>
			{
				Callback.OnScroll(Widget, new ScrollBarEventArgs(Convert.ToInt32(Control.Value)));
			};
        }

		// TODO:
		public int LargeChange { get; set; }
		public int SmallChange { get; set; }
	}
}

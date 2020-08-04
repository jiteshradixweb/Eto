using Eto.Forms;
using System;

namespace Eto.GtkSharp.Forms
{

	public class NativeOverlayLayoutHandler : GtkContainer<Gtk.Overlay, NativeOverlayLayout, NativeOverlayLayout.ICallback>, NativeOverlayLayout.IHandler
	{
		public partial class EtoOverlay : Gtk.Overlay
		{
			WeakReference _handler;
			public IGtkControl Handler
			{
				get => _handler?.Target as IGtkControl;
				set => _handler = new WeakReference(value);
			}
        
	#if GTK3        
			protected override void OnGetPreferredWidth(out int minimum_width, out int natural_width)
			{
				base.OnGetPreferredWidth(out minimum_width, out natural_width);
				var h = Handler;
				if (h != null)
				{
					var userPreferredSize = h.UserPreferredSize;
					if (userPreferredSize.Width > 0)
						natural_width = userPreferredSize.Width;

					minimum_width = Math.Min(natural_width, minimum_width);
				}
			}
        
			protected override void OnGetPreferredWidthForHeight(int height, out int minimum_width, out int natural_width)
			{
				base.OnGetPreferredWidthForHeight(height, out minimum_width, out natural_width);
				var h = Handler;
				if (h != null)
				{
					var userPreferredSize = h.UserPreferredSize;
					if (userPreferredSize.Width > 0)
						natural_width = userPreferredSize.Width;

					minimum_width = Math.Min(natural_width, minimum_width);
				}
			}

			protected override void OnGetPreferredHeight(out int minimum_height, out int natural_height)
			{
				base.OnGetPreferredHeight(out minimum_height, out natural_height);
				var h = Handler;
				if (h != null)
				{
					var userPreferredSize = h.UserPreferredSize;
					if (userPreferredSize.Height > 0)
						natural_height = userPreferredSize.Height;

					minimum_height = Math.Min(natural_height, minimum_height);
				}
			}
        
			protected override void OnAdjustSizeAllocation(Gtk.Orientation orientation, out int minimum_size, out int natural_size, out int allocated_pos, out int allocated_size)
			{
				base.OnAdjustSizeAllocation(orientation, out minimum_size, out natural_size, out allocated_pos, out allocated_size);
				var h = Handler;
				if (h != null)
				{
					var preferredSize = orientation == Gtk.Orientation.Horizontal ? h.UserPreferredSize.Width : h.UserPreferredSize.Height;

					if (preferredSize > 0)
						natural_size = preferredSize;

					minimum_size = Math.Min(natural_size, minimum_size);
				}
			}

			protected override void OnAdjustSizeRequest(Gtk.Orientation orientation, out int minimum_size, out int natural_size)
			{
				base.OnAdjustSizeRequest(orientation, out minimum_size, out natural_size);

				// Gtk.Overlay doesnt use sizes of overlays (but only main widget) so have to measure children:
				foreach(var child in Children)
				{
					child.GetPreferredSize(out var minSize, out var naturalSize);

					var minValue = orientation == Gtk.Orientation.Horizontal ? minSize.Width : minSize.Height;
					var naturalValue = orientation == Gtk.Orientation.Horizontal ? naturalSize.Width : naturalSize.Height;

					minimum_size = Math.Max(minimum_size, minValue);
					natural_size = Math.Max(natural_size, naturalValue);
				}

				var h = Handler;
				if (h != null)
				{
					var preferredSize = orientation == Gtk.Orientation.Horizontal ? h.UserPreferredSize.Width : h.UserPreferredSize.Height;

					if (preferredSize > 0)
						natural_size = preferredSize;

					minimum_size = Math.Min(natural_size, minimum_size);
				}
			}
	#endif
		}

		public NativeOverlayLayoutHandler()
		{
			Control = new EtoOverlay() { Handler = this };
		}

		public void Add(Control child, HorizontalAlignment halign, VerticalAlignment valign)
		{
			var ctl = child.GetGtkControlHandler();

			var widget = ctl.ContainerControl;
			if (widget.Parent != null)
				((Gtk.Container)widget.Parent).Remove(widget);
			widget.ShowAll();

			var alignment = new Gtk.Alignment(
				halign == HorizontalAlignment.Center ? 0.5f :
				halign == HorizontalAlignment.Right ? 1f : 0,
				valign == VerticalAlignment.Center ? 0.5f :
				valign == VerticalAlignment.Bottom ? 1f : 0,
				halign == HorizontalAlignment.Stretch ? 1f : 0,
				valign == VerticalAlignment.Stretch ? 1f : 0
				);

			alignment.Child = widget;

			Control.AddOverlay(alignment);
			Control.SetOverlayPassThrough(alignment, true);
		}

		public void Remove(Control child)
		{
			Control.Remove(child.GetContainerWidget().Parent);
		}

		public void Update()
		{
#if GTK3
			Control.QueueResize();
#else
			Control.ResizeChildren();
#endif
		}

		public override void OnLoadComplete(System.EventArgs e)
		{
			base.OnLoadComplete(e);
			SetFocusChain();
		}
	}
}

using System;
using Eto.Forms;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;

namespace Eto.Mac.Forms.Controls
{
	public class VerticalScrollbarHandler : MacControl<NSScroller, VerticalScrollbar, VerticalScrollbar.ICallback>, VerticalScrollbar.IHandler
	{
		public class EtoScroller : NSScroller, IMacControl
		{
			public WeakReference WeakHandler { get; set; }

			public VerticalScrollbarHandler Handler
			{
				get { return (VerticalScrollbarHandler)WeakHandler.Target; }
				set { WeakHandler = new WeakReference(value); }
			}

			public override void ResetCursorRects()
			{
				var cursor = Handler.Cursor;
				if (cursor != null)
					AddCursorRect(new CGRect(CGPoint.Empty, Frame.Size), cursor.ControlObject as NSCursor);
			}

			public EtoScroller(VerticalScrollbarHandler handler)
				: base(new CGRect(0, 0, 12, 100))
			{
				AutoresizesSubviews = false;
			}

			public override void Layout()
			{
				if (MacView.NewLayout)
					base.Layout();
				//Handler?.PerformScrollLayout();
				if (!MacView.NewLayout)
					base.Layout();
			}
		}

		public string Name { get; set; }
		public int Maximum { get; set; }
		public int Minimum { get; set; }
		public int Value { get; set; }

		protected override NSScroller CreateControl() => new EtoScroller(this);

		// TODO:
		public int LargeChange { get; set; }
		public int SmallChange { get; set; }
	}
}

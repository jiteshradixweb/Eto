using System;
using Eto.Forms;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;

namespace Eto.Mac.Forms.Controls
{
	public class HorizontalScrollbarHandler : MacControl<NSScroller, HorizontalScrollbar, HorizontalScrollbar.ICallback>, HorizontalScrollbar.IHandler
	{
		public class EtoScroller : NSScroller, IMacControl
		{
			public WeakReference WeakHandler { get; set; }

			public HorizontalScrollbarHandler Handler
			{
				get { return (HorizontalScrollbarHandler)WeakHandler.Target; }
				set { WeakHandler = new WeakReference(value); }
			}

			public override void ResetCursorRects()
			{
				var cursor = Handler.Cursor;
				if (cursor != null)
					AddCursorRect(new CGRect(CGPoint.Empty, Frame.Size), cursor.ControlObject as NSCursor);
			}

			public EtoScroller(HorizontalScrollbarHandler handler)
				: base(new CGRect(0, 0, 100, 12))
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

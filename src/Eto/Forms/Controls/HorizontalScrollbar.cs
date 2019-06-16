using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Eto.Forms
{
	[Handler(typeof(HorizontalScrollbar.IHandler))]
	public class HorizontalScrollbar : Scrollbar
	{
		new IHandler Handler { get { return (IHandler)base.Handler; } }

		public HorizontalScrollbar()
		{
		}

		static HorizontalScrollbar()
		{
			EventLookup.Register<HorizontalScrollbar>(c => c.OnScroll(null), Scrollable.ScrollEvent);
		}
		public const string ScrollEvent = "HorizontalScrollbar.ScrollEvent";

		public override event EventHandler<ScrollBarEventArgs> Scroll
		{
			add { Properties.AddHandlerEvent(ScrollEvent, value); }
			remove { Properties.RemoveEvent(ScrollEvent, value); }
		}

		protected virtual void OnScroll(ScrollBarEventArgs e)
		{
			Properties.TriggerEvent(ScrollEvent, this, e);
		}

		public string Name
		{
			get { return Handler.Name; }
			set { Handler.Name = value; }
		}

		/// <summary>
		/// Maximum.
		/// </summary>
		public override int Maximum
		{
			get { return Handler.Maximum; }
			set { Handler.Maximum = value; }
		}

		/// <summary>
		/// Minimum.
		/// </summary>
		public override int Minimum
		{
			get { return Handler.Minimum; }
			set { Handler.Minimum = value; }
		}

		/// <summary>
		/// Value.
		/// </summary>
		public override int Value
		{
			get { return Handler.Value; }
			set { Handler.Value = value; }
		}

		public override int LargeChange
		{
			get { return Handler.LargeChange; }
			set { Handler.LargeChange = value; }
		}

		public override int SmallChange
		{
			get { return Handler.SmallChange; }
			set { Handler.SmallChange = value; }
		}

		#region Callback

		/// <summary>
		/// Handler.
		/// </summary>
		public new interface IHandler : CommonControl.IHandler
		{
			/// <summary>
			/// Name.
			/// </summary>
			string Name { get; set; }

			/// <summary>
			/// Maximum.
			/// </summary>
			[DefaultValue(0)]
			int Maximum { get; set; }

			/// <summary>
			/// Minimum.
			/// </summary>
			[DefaultValue(0)]
			int Minimum { get; set; }

			/// <summary>
			/// Value.
			/// </summary>
			[DefaultValue(0)]
			int Value { get; set; }

			[DefaultValue(0)]
			int LargeChange { get; set; }

			[DefaultValue(0)]
			int SmallChange { get; set; }
		}

		static readonly object callback = new Callback();
		/// <summary>
		/// Gets an instance of an object used to perform callbacks to the widget from handler implementations
		/// </summary>
		/// <returns>The callback instance to use for this widget</returns>
		protected override object GetCallback() { return callback; }

		public new interface ICallback : CommonControl.ICallback
		{
			/// <summary>
			/// Raises the scroll event.
			/// </summary>
			void OnScroll(HorizontalScrollbar widget, ScrollBarEventArgs e);
		}

		/// <summary>
		/// Callback implementation for the <see cref="Scrollable"/>
		/// </summary>
		protected new class Callback : CommonControl.Callback, ICallback
		{
			/// <summary>
			/// Raises the scroll event.
			/// </summary>
			public void OnScroll(HorizontalScrollbar widget, ScrollBarEventArgs e)
			{
				using (widget.Platform.Context)
					widget.OnScroll(e);
			}
		}

		#endregion

	}
}

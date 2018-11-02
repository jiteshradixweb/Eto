using Eto.Drawing;


namespace Eto.Forms
{
	/// <summary>
	/// Vertical ScrollBar
	/// </summary>
	[Handler(typeof(VerticalScrollbar.IHandler))]
	public class VerticalScrollbar : CommonControl
	{
		new IHandler Handler { get { return (IHandler)base.Handler; } }

		/// <summary>
		/// Vertical Scroll Bar.
		/// </summary>
		public VerticalScrollbar()
		{
		}

		/// <summary>
		/// Name property for vertical scroll bar.
		/// </summary>
		/// <remarks>
		/// Name
		/// </remarks>
		/// <value>Name of Vertical scroll bar</value>
		public string Name
		{
			get { return Handler.Name; }
			set { Handler.Name = value; }
		}

		/// <summary>
		/// Handler.
		/// </summary>
		public new interface IHandler : CommonControl.IHandler
		{
			/// <summary>
			/// Name.
			/// </summary>
			string Name { get; set; }
		}


		//public new interface ICallback : CommonControl.ICallback
		//{
		//	/// <summary>
		//	/// Raises the scroll event.
		//	/// </summary>
		//	void OnScroll(CustomScrollbar widget, ScrollEventArgs e);
		//}
	}
}

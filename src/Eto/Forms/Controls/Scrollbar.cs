using System;
using System.Collections.Generic;
using System.Text;

namespace Eto.Forms
{
	public class ScrollBarEventArgs : EventArgs
	{
		public int NewValue { get; private set; }

		public ScrollBarEventArgs(int value)
		{
			this.NewValue = value;
		}
	}

	public abstract class Scrollbar : CommonControl
	{
		public abstract event EventHandler<ScrollBarEventArgs> Scroll;
		public abstract int Maximum { get; set; }
		public abstract int Minimum { get; set; }
		public abstract int Value { get; set; }
		public abstract int LargeChange { get; set; }
		public abstract int SmallChange { get; set; }
	}
}

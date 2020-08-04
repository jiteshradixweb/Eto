using System;
using System.Collections;
using System.Collections.ObjectModel;
using Eto.Drawing;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace Eto.Forms
{

	/// <summary>
	/// Item for a single control in a <see cref="StackLayout"/>.
	/// </summary>
	[ContentProperty("Control")]
	// TODO [sc.TypeConverter(typeof(StackLayoutItemConverter))]
	public class OverlayLayoutItem
	{
		/// <summary>
		/// Gets or sets the control for this item.
		/// </summary>
		/// <value>The item's control.</value>
		public Control Control { get; set; }

		/// <summary>
		/// Gets or sets the horizontal alignment for the control for vertical stack layouts, or null to use <see cref="StackLayout.HorizontalContentAlignment"/>.
		/// </summary>
		/// <value>The horizontal alignment of the control.</value>
		public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;

		/// <summary>
		/// Gets or sets the vertical alignment for the control for horizontal stack layouts, or null to use <see cref="StackLayout.VerticalContentAlignment"/>.
		/// </summary>
		/// <value>The vertical alignment of the control.</value>
		public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.StackLayoutItem"/> class.
		/// </summary>
		/// <param name="control">Control for the item.</param>
		public OverlayLayoutItem(
			Control control = null,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Top
			)
		{
			Control = control;
			HorizontalAlignment = horizontalAlignment;
			VerticalAlignment = verticalAlignment;
		}

		/// <summary>
		/// Converts a control to a StackLayoutItem implicitly.
		/// </summary>
		/// <param name="control">Control to convert.</param>
		public static implicit operator OverlayLayoutItem(Control control)
		{
			return new OverlayLayoutItem { Control = control };
		}

		/// <summary>
		/// Converts a string to a StackLayoutItem with a label control implicitly.
		/// </summary>
		/// <remarks>
		/// This provides an easy way to add labels to your layout through code, without having to create <see cref="Label"/> instances.
		/// </remarks>
		/// <param name="labelText">Text to convert to a Label control.</param>
		public static implicit operator OverlayLayoutItem(string labelText)
		{
			return new OverlayLayoutItem { Control = new Label { Text = labelText } };
		}

		/// <summary>
		/// Converts an <see cref="Image"/> to a StackLayoutItem with an <see cref="ImageView"/> control implicitly.
		/// </summary>
		/// <remarks>
		/// This provides an easy way to add images to your layout through code, without having to create <see cref="ImageView"/> instances manually.
		/// </remarks>
		/// <param name="image">Image to convert to a StackLayoutItem with a ImageView control.</param>
		public static implicit operator OverlayLayoutItem(Image image)
		{
			return new OverlayLayoutItem { Control = new ImageView { Image = image } };
		}
	}

	/// <summary>
	/// Layout to stack controls horizontally or vertically, with the ability for each child to be aligned to a side
	/// of the layout.
	/// </summary>
	[ContentProperty("Items")]
	public class OverlayLayout : Panel
	{
		class ItemsCollection : Collection<OverlayLayoutItem>, IList
		{
			public OverlayLayout Parent { get; set; }

			protected override void InsertItem(int index, OverlayLayoutItem item)
			{
				base.InsertItem(index, item);
				if (item != null)
					Parent.SetLogicalParent(item.Control);
				Parent.CreateIfNeeded(true);
			}

			protected override void RemoveItem(int index)
			{
				var item = this[index];
				if (item != null)
					Parent.RemoveLogicalParent(item.Control);
				base.RemoveItem(index);
				Parent.CreateIfNeeded(true);
			}

			protected override void ClearItems()
			{
				foreach (var item in this)
				{
					if (item != null)
						Parent.RemoveLogicalParent(item.Control);
				}
				base.ClearItems();
				Parent.CreateIfNeeded(true);
			}

			protected override void SetItem(int index, OverlayLayoutItem item)
			{
				var last = this[index];
				if (last != null)
					Parent.RemoveLogicalParent(last.Control);
				base.SetItem(index, item);
				if (item != null)
					Parent.SetLogicalParent(item.Control);
				Parent.CreateIfNeeded(true);
			}

			int IList.Add(object value)
			{
				// allow adding a control directly from xaml
				var control = value as Control;
				if (control != null)
					Add((OverlayLayoutItem)control);
				else
					Add((OverlayLayoutItem)value);
				return Count - 1;
			}
		}

		readonly ItemsCollection items;

		/// <summary>
		/// Gets the collection of items in the stack layout.
		/// </summary>
		/// <value>The item collection.</value>
		public Collection<OverlayLayoutItem> Items { get { return items; } }

		/// <summary>
		/// Gets the controls for the layout
		/// </summary>
		/// <remarks>
		/// This will return the list of controls in the stack layout when not created, and when it is,
		/// it will return the embedded TableLayout.
		/// </remarks>
		public override IEnumerable<Control> Controls
		{
			get
			{
				return Items.Where(r => r?.Control != null).Select(r => r.Control);
			}
		}

		/// <summary>
		/// Gets an enumeration of controls that are in the visual tree.
		/// </summary>
		/// <remarks>This is used to specify which controls are contained by this instance that are part of the visual tree.
		/// This should include all controls including non-logical Eto controls used for layout.</remarks>
		/// <value>The visual controls.</value>
		public override IEnumerable<Control> VisualControls
		{
			get
			{
				return base.Controls;
			}
		}

		bool isCreated;
		int suspended;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Eto.Forms.StackLayout"/> class.
		/// </summary>
		public OverlayLayout()
		{
			items = new ItemsCollection { Parent = this };
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Eto.Forms.StackLayout"/> class with the specified items.
		/// </summary>
		/// <param name="items">Initial items to add to the StackLayout.</param>
		public OverlayLayout(params OverlayLayoutItem[] items)
		{
			this.items = new ItemsCollection { Parent = this };
			foreach (var item in items)
			{
				this.items.Add(item);
			}
		}

		/*
		VerticalAlignment GetVerticalAlign(StackLayoutItem item)
		{
			var align = item.VerticalAlignment ?? VerticalContentAlignment;
			var label = item.Control as Label;
			if (!AlignLabels || label == null)
				return align;
			label.VerticalAlignment = align;
			return VerticalAlignment.Stretch;
		}

		HorizontalAlignment GetHorizontalAlign(StackLayoutItem item)
		{
			var align = item.HorizontalAlignment ?? HorizontalContentAlignment;
			var label = item.Control as Label;
			if (!AlignLabels || label == null)
				return align;
			switch (align)
			{
				case HorizontalAlignment.Left:
					label.TextAlignment = TextAlignment.Left;
					break;
				case HorizontalAlignment.Center:
					label.TextAlignment = TextAlignment.Center;
					break;
				case HorizontalAlignment.Right:
					label.TextAlignment = TextAlignment.Right;
					break;
				default:
					return align;
			}
			return HorizontalAlignment.Stretch;
		}
		*/

		public void Add(Control control, HorizontalAlignment halign = HorizontalAlignment.Left, VerticalAlignment valign = VerticalAlignment.Top)
		{
			items.Add(new OverlayLayoutItem(control, halign, valign));
		}

		/// <summary>
		/// Removes the specified child from the container
		/// </summary>
		/// <param name="child">Child to remove.</param>
		public override void Remove(Control child)
		{
			items.Remove(child);
			(Content as Layout)?.Remove(child);
		}

		/// <summary>
		/// Raises the <see cref="Control.PreLoad"/> event, and recurses to this container's children
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnPreLoad(EventArgs e)
		{
			if (!isCreated && suspended <= 0)
				Create();
			base.OnPreLoad(e);
		}

		/// <summary>
		/// Raises the <see cref="Control.Load"/> event, and recurses to this container's children
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			if (!isCreated && suspended <= 0)
				Create();
			base.OnLoad(e);
		}

		/// <summary>
		/// Suspends the layout of child controls
		/// </summary>
		public override void SuspendLayout()
		{
			base.SuspendLayout();
			suspended++;
		}

		/// <summary>
		/// Resumes the layout after it has been suspended, and performs a layout
		/// </summary>
		public override void ResumeLayout()
		{
			if (suspended == 0)
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Must balance ResumeLayout with SuspendLayout calls"));
			suspended--;
			base.ResumeLayout();
			CreateIfNeeded();
		}

		void CreateIfNeeded(bool force = false)
		{
			if (suspended > 0 || !Loaded)
			{
				if (force)
					isCreated = false;
				return;
			}
			if (!isCreated || force)
				Create();
		}

		private void LayoutItems(PixelLayout pixelLayout)
		{
			var layoutSize = pixelLayout.Size;

			for (int i = 0; i < items.Count; i++)
			{
				var item = items[i];
				var ctrl = item.Control;
				var size = ctrl.Size;

				if(size.IsEmpty) continue; // Mac has sometimes size 0,0 for labels !?

				int x = 0;
				int y = 0;

				switch(item.HorizontalAlignment)
				{
					case HorizontalAlignment.Center: x = (layoutSize.Width / 2) - (size.Width / 2); break;
					case HorizontalAlignment.Right: x = layoutSize.Width - size.Width; break;
					case HorizontalAlignment.Stretch: ctrl.Width = layoutSize.Width; break;
				}

				switch(item.VerticalAlignment)
				{
					case VerticalAlignment.Center: y = (layoutSize.Height / 2) - (size.Height / 2); break;
					case VerticalAlignment.Bottom: y = layoutSize.Height - size.Height; break;
					case VerticalAlignment.Stretch: ctrl.Height = layoutSize.Height; break;
				}

				pixelLayout.Move(item.Control, x, y);
			}
		}

		void Create()
		{
			Control content;

			if(Platform.Supports<NativeOverlayLayout.IHandler>())
			{
				var nativeLayout = new NativeOverlayLayout();

				for (int i = 0; i < items.Count; i++)
				{
					var item = items[i];
					nativeLayout.Add(item.Control, item.HorizontalAlignment, item.VerticalAlignment);
				}

				content = nativeLayout;
			}
			else
			{
				var pixelLayout = new PixelLayout();

				pixelLayout.SizeChanged += (s,e) => LayoutItems(pixelLayout);
				pixelLayout.Shown += (s,e) => LayoutItems(pixelLayout);

				for (int i = 0; i < items.Count; i++)
				{
					var item = items[i];
					pixelLayout.Add(item.Control, 0, 0);
				}

				content = pixelLayout;
			}

			Content = content;
			isCreated = true;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Eto.Forms
{
	/// <summary>
	/// Layout to position controls over each other (z-stack)
	/// </summary>
	/// <remarks>
	/// Note that controls will automatically size themselves.
	/// </remarks>
	[ContentProperty("Contents")]
	[Handler(typeof(NativeOverlayLayout.IHandler))]
	public class NativeOverlayLayout : Layout
	{
		new IHandler Handler { get { return (IHandler)base.Handler; } }

		List<Control> children;
		readonly List<Control> controls = new List<Control>();

		/// <summary>
		/// Gets an enumeration of controls that are directly contained by this container
		/// </summary>
		/// <value>The contained controls.</value>
		public override IEnumerable<Control> Controls { get { return controls; } }

		/// <summary>
		/// Gets a collection of controls that are contained by this layout
		/// </summary>
		/// <remarks>
		/// When adding children using this, you can position them using the <see cref="SetLocation"/> static method.
		/// </remarks>
		/// <value>The contents of the container.</value>
		public List<Control> Contents
		{
			get
			{
				if (children == null)
					children = new List<Control>();
				return children;
			}
		}

		static readonly EtoMemberIdentifier HorizontalAlignmentProperty = new EtoMemberIdentifier(typeof(NativeOverlayLayout), "HorizontalAlignment");
		static readonly EtoMemberIdentifier VerticalAlignmentProperty = new EtoMemberIdentifier(typeof(NativeOverlayLayout), "VerticalAlignment");

		public static HorizontalAlignment GetHorizontalAlignment(Control control)
		{
			return control.Properties.Get<HorizontalAlignment>(HorizontalAlignmentProperty);
		}

		public static VerticalAlignment GetVerticalAlignment(Control control)
		{
			return control.Properties.Get<VerticalAlignment>(VerticalAlignmentProperty);
		}

		///// <summary>
		///// Sets the location of the specified control
		///// </summary>
		///// <param name="control">Control to set the location.</param>
		///// <param name="value">Location of the control</param>
		//public static void SetAlignment(Control control, HorizontalAlignment halign = HorizontalAlignment.Left, VerticalAlignment valign = VerticalAlignment.Top)
		//{
		//	control.Properties[HorizontalAlignmentProperty] = halign;
		//	control.Properties[VerticalAlignmentProperty] = valign;
		//	var layout = control.Parent as NativeOverlayLayout;
		//	if (layout != null)
		//		layout.Align(control, halign, valign);
		//}

		/// <summary>
		/// Adds a control to the layout with the specified pixel coordinates
		/// </summary>
		/// <param name="control">Control to add</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public void Add(Control control, HorizontalAlignment halign = HorizontalAlignment.Left, VerticalAlignment valign = VerticalAlignment.Top)
		{
			control.Properties[HorizontalAlignmentProperty] = halign;
			control.Properties[VerticalAlignmentProperty] = valign;
			controls.Add(control);
			SetParent(control, () => Handler.Add(control, halign, valign));
		}

		///// <summary>
		///// Moves the control to the specified coordinates
		///// </summary>
		///// <param name="control">Control to move</param>
		///// <param name="x">The x coordinate.</param>
		///// <param name="y">The y coordinate.</param>
		//public void Align(Control control, HorizontalAlignment halign = HorizontalAlignment.Left, VerticalAlignment valign = VerticalAlignment.Top)
		//{
		//	control.Properties[HorizontalAlignmentProperty] = halign;
		//	control.Properties[VerticalAlignmentProperty] = valign;
		//	Handler.Align(control, halign, valign);
		//}

		/// <summary>
		/// Remove the specified child control.
		/// </summary>
		/// <param name="child">Child to remove</param>
		public override void Remove(Control child)
		{
			if (controls.Remove(child))
			{
				Handler.Remove(child);
				RemoveParent(child);
			}
		}

		[OnDeserialized]
		void OnDeserialized(StreamingContext context)
		{
			OnDeserialized();
		}

		/// <summary>
		/// Ends the initialization when loading from xaml or other code generated scenarios
		/// </summary>
		public override void EndInit()
		{
			base.EndInit();
			OnDeserialized(Parent != null); // mono calls EndInit BEFORE setting to parent
		}

		void OnDeserialized(bool direct = false)
		{
			if (Loaded || direct)
			{
				if (children != null)
				{
					foreach (var control in children)
					{
						Add(control, GetHorizontalAlignment(control), GetVerticalAlignment(control));
					}
				}
			}
			else
			{
				PreLoad += HandleDeserialized;
			}
		}

		void HandleDeserialized(object sender, EventArgs e)
		{
			OnDeserialized(true);
			PreLoad -= HandleDeserialized;
		}

		/// <summary>
		/// Handler interface for the <see cref="NativeOverlayLayout"/> control
		/// </summary>
		public new interface IHandler : Layout.IHandler
		{
			void Add(Control control, HorizontalAlignment halign, VerticalAlignment valign);

			/// <summary>
			/// Removes the specified child from this layout
			/// </summary>
			/// <remarks>
			/// This assumes that the control is already a child of this layout.  This will make the child control
			/// invisible to the user
			/// </remarks>
			/// <param name="control">Child control to remove</param>
			void Remove(Control control);
		}
	}
}

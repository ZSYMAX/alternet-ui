using System;
using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Drawing;
using Alternet.Base.Collections;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the base class for controls, which are components with visual representation.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class Control : Component, ISupportInitialize
    {
        private static readonly SizeF DefaultSize = new SizeF(float.NaN, float.NaN);
        private static Font? defaultFont;
        private SizeF size = DefaultSize;
        private Thickness margin;
        private Thickness padding;
        private ControlHandler? handler;
        private Brush? background;
        private Brush? foreground;
        private Font? font;
        private Brush? borderBrush;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Stretch;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch;

        private bool visible = true;
        private bool enabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        public Control()
        {
            Children.ItemInserted += Children_ItemInserted;
            Children.ItemRemoved += Children_ItemRemoved;
        }

        /// <summary>
        /// Occurs when the control is clicked.
        /// </summary>
        public event EventHandler? Click;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderBrush"/> property changes.
        /// </summary>
        public event EventHandler? BorderBrushChanged;

        /// <summary>
        /// Occurs when the control is redrawn.
        /// </summary>
        /// <remarks>
        /// The <see cref="Paint"/> event is raised when the control is redrawn. It passes an instance of <see cref="PaintEventArgs"/> to the method(s) that handles the <see cref="Paint"/> event.
        /// </remarks>
        public event EventHandler<PaintEventArgs>? Paint;

        /// <summary>
        /// Occurs when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        public event EventHandler? MarginChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Padding"/> property changes.
        /// </summary>
        public event EventHandler? PaddingChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        public event EventHandler? VisibleChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Enabled"/> property changes.
        /// </summary>
        public event EventHandler? EnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Background"/> property changes.
        /// </summary>
        public event EventHandler? BackgroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Foreground"/> property changes.
        /// </summary>
        public event EventHandler? ForegroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Font"/> property changes.
        /// </summary>
        public event EventHandler? FontChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalAlignment"/> property changes.
        /// </summary>
        public event EventHandler? VerticalAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalAlignment"/> property changes.
        /// </summary>
        public event EventHandler? HorizontalAlignmentChanged;

        /// <summary>
        /// Gets the default font used for controls.
        /// </summary>
        /// <value>
        /// The default <see cref="Font"/> for UI controls. The value returned will
        /// vary depending on the user's operating system and the local settings of their system.
        /// </value>
        public static Font DefaultFont => defaultFont == null ? defaultFont = Font.CreateDefaultFont() : defaultFont;

        /// <summary>
        /// Gets or sets the object that contains data about the control.
        /// </summary>
        /// <value>An <see cref="object"/> that contains data about the control. The default is <c>null</c>.</value>
        /// <remarks>
        /// Any type derived from the <see cref="object"/> class can be assigned to this property.
        /// A common use for the <see cref="Tag"/> property is to store data that is closely associated with the control.
        /// </remarks>
        public object? Tag { get; set; }

        /// <summary>
        /// Gets or sets the identifying name of the control.
        /// The name provides a reference so that code-behind, such as event handler code,
        /// can refer to a markup control after it is constructed during processing by a UIXML processor.
        /// </summary>
        /// <value>The name of the control. The default is <c>null</c>.</value>
        public string? Name { get; set; } // todo: maybe use Site.Name?

        /// <summary>
        /// Gets or sets a value indicating whether the control and all its child controls are displayed.
        /// </summary>
        /// <value><c>true</c> if the control and all its child controls are displayed; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        public bool Visible
        {
            get => visible;

            set
            {
                if (visible == value)
                    return;

                visible = value;
                OnVisibleChanged(EventArgs.Empty);
                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond to user interaction.
        /// </summary>
        /// <value><c>true</c> if the control can respond to user interaction; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        /// <remarks>
        /// With the <see cref="Enabled"/> property, you can enable or disable controls at run time.
        /// For example, you can disable controls that do not apply to the current state of the application.
        /// You can also disable a control to restrict its use. For example, a button can be disabled to prevent the user from clicking it.
        /// </remarks>
        public bool Enabled
        {
            get => enabled;

            set
            {
                if (enabled == value)
                    return;

                enabled = value;
                OnEnabledChanged(EventArgs.Empty);
                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the border brush of the control.
        /// </summary>
        public Brush? BorderBrush // todo: Do we need border property for all the controls at all?
        {
            get => borderBrush;
            set
            {
                if (borderBrush == value)
                    return;

                borderBrush = value;
                BorderBrushChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a <see cref="ControlHandler"/> associated with this class.
        /// </summary>
        public ControlHandler Handler
        {
            get
            {
                EnsureHandlerCreated();
                return handler ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets whether <see cref="Dispose(bool)"/> has been called.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets the collection of child controls contained within the control.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> representing the collection of controls contained within the control.</value>
        /// <remarks>
        /// A Control can act as a parent to a collection of controls.
        /// For example, when several controls are added to a <see cref="Window"/>, each of the controls is a member
        /// of the <see cref="Collection{T}"/> assigned to the <see cref="Children"/> property of the window, which is derived from the <see cref="Control"/> class.
        /// <para>You can manipulate the controls in the <see cref="Collection{T}"/> assigned to the <see cref="Children"/>
        /// property by using the methods available in the <see cref="Collection{T}"/> class.</para>
        /// <para>When adding several controls to a parent control, it is recommended that you call the <see cref="SuspendLayout"/> method before initializing the controls to be added.
        /// After adding the controls to the parent control, call the <see cref="ResumeLayout"/> method. Doing so will increase the performance of applications with many controls.</para>
        /// </remarks>
        [Content]
        public Collection<Control> Children { get; } = new Collection<Control>();

        /// <summary>
        /// Gets the parent container of the control.
        /// </summary>
        public Control? Parent { get; internal set; } // todo: allow users to set the Parent property?

        /// <summary>
        /// Gets or sets the suggested size of the control.
        /// </summary>
        /// <value>The suggested size of the control, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="SizeF"/>(<see cref="float.NaN"/>, <see cref="float.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested size of the control. An actual size is calculated by the layout system.
        /// Set this property to <see cref="SizeF"/>(<see cref="float.NaN"/>, <see cref="float.NaN"/>) to specify auto sizing behavior.
        /// The value of this property is always the same as the value that was set to it and is not changed by the layout system.
        /// </remarks>
        public virtual SizeF Size
        {
            get
            {
                return size;
            }

            set
            {
                if (size == value)
                    return;

                size = value;
            }
        }

        /// <summary>
        /// Gets or sets the suggested width of the control.
        /// </summary>
        /// <value>The suggested width of the control, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="float.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested width of the control. An actual width is calculated by the layout system.
        /// Set this property to <see cref="float.NaN"/> to specify auto sizing behavior.
        /// The value of this property is always the same as the value that was set to it and is not changed by the layout system.
        /// </remarks>
        public virtual float Width
        {
            get => size.Width;

            set
            {
                Size = new SizeF(value, Size.Height);
            }
        }

        /// <summary>
        /// Gets or sets the suggested height of the control.
        /// </summary>
        /// <value>The suggested height of the control, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="float.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested height of the control. An actual height is calculated by the layout system.
        /// Set this property to <see cref="float.NaN"/> to specify auto sizing behavior.
        /// The value of this property is always the same as the value that was set to it and is not changed by the layout system.
        /// </remarks>
        public virtual float Height
        {
            get => size.Height;

            set
            {
                Size = new SizeF(Size.Width, value);
            }
        }

        /// <summary>
        /// Gets or set a value indicating whether the control paints itself rather than the operating system doing so.
        /// </summary>
        /// <value>If <c>true</c>, the control paints itself rather than the operating system doing so.
        /// If <c>false</c>, the <see cref="Paint"/> event is not raised.</value>
        public bool UserPaint { get; set; } // todo: rethink design of this

        /// <summary>
        /// Gets or sets the outer margin of an control.
        /// </summary>
        /// <value>Provides margin values for the control. The default value is a <see cref="Thickness"/> with all properties equal to 0 (zero).</value>
        /// <remarks>
        /// The margin is the space between this control and the adjacent control.
        /// Margin is set as a <see cref="Thickness"/> structure rather than as a number so that the margin can be set asymmetrically.
        /// The <see cref="Thickness"/> structure itself supports string type conversion so that you can specify an asymmetric <see cref="Margin"/> in UIXML attribute syntax also.
        /// </remarks>
        public Thickness Margin
        {
            get => margin;
            set
            {
                if (margin == value)
                    return;

                margin = value;

                OnMarginChanged(EventArgs.Empty);
                MarginChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the padding inside a control.
        /// </summary>
        /// <value>Provides padding values for the control. The default value is a <see cref="Thickness"/> with all properties equal to 0 (zero).</value>
        /// <remarks>
        /// The padding is the amount of space between the content of a <see cref="Control"/> and its border.
        /// Padding is set as a <see cref="Thickness"/> structure rather than as a number so that the padding can be set asymmetrically.
        /// The <see cref="Thickness"/> structure itself supports string type conversion so that you can specify an asymmetric <see cref="Padding"/> in UIXML attribute syntax also.
        /// </remarks>
        public Thickness Padding
        {
            get => padding;
            set
            {
                if (padding == value)
                    return;

                padding = value;

                OnPaddingChanged(EventArgs.Empty);
                PaddingChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the background brush for the control.
        /// </summary>
        public Brush? Background
        {
            get => background;
            set
            {
                if (background == value)
                    return;

                background = value;
                BackgroundChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the foreground brush for the control.
        /// </summary>
        public Brush? Foreground
        {
            get => foreground;
            set
            {
                if (foreground == value)
                    return;

                foreground = value;
                ForegroundChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value>The <see cref="Font"/> to apply to the text displayed by the control. The default is the value of <c>null</c>.</value>
        public Font? Font
        {
            get => font;
            set
            {
                if (font == value)
                    return;

                font = value;
                FontChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment applied to this control when it is positioned within a parent control.
        /// </summary>
        /// <value>A vertical alignment setting. The default is <see cref="VerticalAlignment.Stretch"/>.</value>
        public VerticalAlignment VerticalAlignment
        {
            get => verticalAlignment;
            set
            {
                if (verticalAlignment == value)
                    return;

                verticalAlignment = value;
                VerticalAlignmentChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment applied to this control when it is positioned within a parent control.
        /// </summary>
        /// <value>A horizontal alignment setting. The default is <see cref="HorizontalAlignment.Stretch"/>.</value>
        public HorizontalAlignment HorizontalAlignment
        {
            get => horizontalAlignment;
            set
            {
                if (horizontalAlignment == value)
                    return;

                horizontalAlignment = value;
                HorizontalAlignmentChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Returns a collection of controls which can be treated as "logical children" of this control.
        /// </summary>
        protected virtual IEnumerable<Control> LogicalChildren => Children; // todo: consider changing this to Site.GetService()

        private IControlHandlerFactory? ControlHandlerFactory { get; set; }

        /// <summary>
        /// Raises the <see cref="Click"/> event and calls <see cref="OnClick(EventArgs)"/>.
        /// See <see cref="Click"/> event description for more details.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseClick(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnClick(e);
            Click?.Invoke(this, e);
        }

        /// <summary>
        /// Recursively searches all <see cref="LogicalChildren"/> for a control with the specified name, and returns that control if found.
        /// </summary>
        /// <param name="name">The name of the control to be found.</param>
        /// <returns>The found control, or <c>null</c> if no control with the provided name is found.</returns>
        public Control? TryFindControl(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (Name == name)
                return this;

            foreach (var child in LogicalChildren)
            {
                var result = child.TryFindControl(name);
                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// Displays the control to the user.
        /// </summary>
        /// <remarks>Showing the control is equivalent to setting the <see cref="Visible"/> property to <c>true</c>.
        /// After the <see cref="Show"/> method is called, the <see cref="Visible"/> property
        /// returns a value of <c>true</c> until the <see cref="Hide"/> method is called.</remarks>
        public void Show() => Visible = true;

        /// <summary>
        /// Conceals the control from the user.
        /// </summary>
        /// <remarks>
        /// Hiding the control is equivalent to setting the <see cref="Visible"/> property to <c>false</c>.
        /// After the <see cref="Hide"/> method is called, the <see cref="Visible"/> property
        /// returns a value of <c>false</c> until the <see cref="Show"/> method is called.
        /// </remarks>
        public void Hide() => Visible = false;

        /// <summary>
        /// Recursively searches all <see cref="LogicalChildren"/> for a control with the specified name,
        /// and throws an exception if the requested control is not found.
        /// </summary>
        /// <param name="name">The name of the control to be found.</param>
        /// <returns>The requested resource. If no control with the provided name was found, an exception is thrown.</returns>
        /// <exception cref="InvalidOperationException">A control with the provided name was found.</exception>
        public Control FindControl(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return TryFindControl(name) ?? throw new InvalidOperationException($"Control with name '{name}' was not found.");
        }

        /// <summary>
        /// Creates the <see cref="DrawingContext"/> for the control.
        /// </summary>
        /// <returns>The <see cref="DrawingContext"/> for the control.</returns>
        /// <remarks>
        /// The <see cref="DrawingContext"/> object that you retrieve through the <see cref="CreateDrawingContext"/> method should not normally
        /// be retained after the current UI event has been processed, because anything painted
        /// with that object will be erased with the next paint event. Therefore you cannot cache
        /// the <see cref="DrawingContext"/> object for reuse, except to use non-visual methods like <see cref="DrawingContext.MeasureText"/>.
        /// Instead, you must call <see cref="CreateDrawingContext"/> every time that you want to use the <see cref="DrawingContext"/> object,
        /// and then call <see cref="Dispose"/> when you are finished using it.
        /// </remarks>
        public DrawingContext CreateDrawingContext() => Handler.CreateDrawingContext();

        /// <summary>
        /// Causes the control to redraw.
        /// </summary>
        public void Update() => Handler.Update(); // todo: consider Invalidate()

        /// <summary>
        /// Temporarily suspends the layout logic for the control.
        /// </summary>
        /// <remarks>
        /// The layout logic of the control is suspended until the <see cref="ResumeLayout"/> method is called.
        /// <para>
        /// The <see cref="SuspendLayout"/> and <see cref="ResumeLayout"/> methods are used in tandem to suppress
        /// multiple layouts while you adjust multiple attributes of the control.
        /// For example, you would typically call the <see cref="SuspendLayout"/> method, then set some
        /// properties of the control, or add child controls to it, and then call the <see cref="ResumeLayout"/>
        /// method to enable the changes to take effect.
        /// </para>
        /// </remarks>
        public void SuspendLayout()
        {
            Handler.SuspendLayout();
        }

        /// <summary>
        /// Resumes the usual layout logic.
        /// </summary>
        /// <param name="performLayout"><c>true</c> to execute pending layout requests; otherwise, <c>false</c>.</param>
        /// <remarks>
        /// Resumes the usual layout logic after <see cref="SuspendLayout"/> has been called.
        /// When the <c>performLayout</c> parameter is set to <c>true</c>, an immediate layout occurs.
        /// <para>
        /// The <see cref="SuspendLayout"/> and <see cref="ResumeLayout"/> methods are used in tandem to suppress
        /// multiple layouts while you adjust multiple attributes of the control.
        /// For example, you would typically call the <see cref="SuspendLayout"/> method, then set some
        /// properties of the control, or add child controls to it, and then call the <see cref="ResumeLayout"/>
        /// method to enable the changes to take effect.
        /// </para>
        /// </remarks>
        public void ResumeLayout(bool performLayout = true)
        {
            Handler.ResumeLayout(performLayout);
            if (performLayout)
                PerformLayout();
        }

        /// <summary>
        /// Maintains performance while performing slow operations on a control by preventing the control from
        /// drawing until the <see cref="EndUpdate"/> method is called.
        /// </summary>
        public void BeginUpdate()
        {
            Handler.BeginUpdate();
        }

        /// <summary>
        /// Resumes painting the control after painting is suspended by the <see cref="BeginUpdate"/> method.
        /// </summary>
        public void EndUpdate()
        {
            Handler.EndUpdate();
        }

        /// <summary>
        /// Forces the control to apply layout logic to child controls.
        /// </summary>
        /// <remarks>
        /// If the <see cref="SuspendLayout"/> method was called before calling the <see cref="PerformLayout"/> method,
        /// the layout is suppressed.
        /// </remarks>
        public void PerformLayout()
        {
            Handler.PerformLayout();
        }

        /// <summary>
        /// Called when the control should reposition the child controls of the control.
        /// </summary>
        protected virtual void OnLayout()
        {
            Handler.OnLayout();
        }

        internal void InvokeOnLayout()
        {
            OnLayout();
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can be fitted, in device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="availableSize">The available space that a parent element can allocate a child control.</param>
        /// <returns>A <see cref="Size"/> representing the width and height of a rectangle, in device-independent units (1/96th inch per unit).</returns>
        public virtual SizeF GetPreferredSize(SizeF availableSize)
        {
            return Handler.GetPreferredSize(availableSize);
        }

        /// <summary>
        /// Starts the initialization process for this control.
        /// </summary>
        /// <remarks>
        /// Runtime environments and design tools can use this method to start the initialization of a control.
        /// The <see cref="EndInit"/> method ends the initialization. Using the <see cref="BeginInit"/> and <see cref="EndInit"/> methods
        /// prevents the control from being used before it is fully initialized.
        /// </remarks>
        public virtual void BeginInit()
        {
            SuspendLayout();
        }

        /// <summary>
        /// Ends the initialization process for this control.
        /// </summary>
        /// <remarks>
        /// Runtime environments and design tools can use this method to end the initialization of a control.
        /// The <see cref="BeginInit"/> method starts the initialization. Using the <see cref="BeginInit"/> and <see cref="EndInit"/> methods
        /// prevents the control from being used before it is fully initialized.
        /// </remarks>
        public virtual void EndInit()
        {
            ResumeLayout();
        }

        internal void RaisePaint(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnPaint(e);
            Paint?.Invoke(this, e);
        }

        /// <summary>
        /// Ensures that the control <see cref="Handler"/> is created, creating and attaching it if necessary.
        /// </summary>
        protected internal void EnsureHandlerCreated()
        {
            if (handler == null)
            {
                CreateAndAttachHandler();
            }
        }

        /// <summary>
        /// Disconnects the current control <see cref="Handler"/> from the control.
        /// This method calls <see cref="ControlHandler.Detach"/>.
        /// </summary>
        protected internal void DetachHandler()
        {
            if (handler == null)
                throw new InvalidOperationException();
            OnHandlerDetaching(EventArgs.Empty);
            handler.Detach();
            handler = null;
        }

        /// <summary>
        /// Called when the control is clicked.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnClick(EventArgs e)
        {
        }

        private protected void SetVisibleValue(bool value) => visible = value;

        /// <summary>
        /// Called when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnVisibleChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Enabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnEnabledChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Forces the re-creation of the handler for the control.
        /// </summary>
        /// <remarks>
        /// The <see cref="RecreateHandler"/> method is called whenever re-execution of handler creation logic is needed.
        /// For example, this may happen when visual theme changes.
        /// </remarks>
        protected void RecreateHandler()
        {
            if (handler != null)
                DetachHandler();

            Update();
        }

        /// <summary>
        /// Called before the current control handler is detached.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnHandlerDetaching(EventArgs e)
        {
        }

        /// <summary>
        /// Gets an <see cref="IControlHandlerFactory"/> to use when creating new control handlers for this control.
        /// </summary>
        protected IControlHandlerFactory GetEffectiveControlHandlerHactory() =>
            ControlHandlerFactory ?? Application.Current.VisualTheme.ControlHandlerFactory; // todo: maybe reconsider naming?

        /// <summary>
        /// Creates a handler for the control.
        /// </summary>
        /// <remarks>
        /// You typically should not call the <see cref="CreateHandler"/> method directly.
        /// The preferred method is to call the <see cref="EnsureHandlerCreated"/> method, which forces a handler to be created for the control.
        /// </remarks>
        protected virtual ControlHandler CreateHandler() => GetEffectiveControlHandlerHactory().CreateControlHandler(this);

        /// <summary>
        /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!IsDisposed)
            {
                if (disposing)
                {
                    var children = Handler.AllChildren.ToArray();

                    SuspendLayout();
                    Children.Clear();
                    Handler.VisualChildren.Clear();
                    ResumeLayout(performLayout: false);

                    foreach (var child in children)
                        child.Dispose();

                    DetachHandler();
                }

                IsDisposed = true;
            }
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if <see cref="IsDisposed"/> is <c>true</c>.
        /// </summary>
        protected void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }

        /// <summary>
        /// Called when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMarginChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Padding"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnPaddingChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the control is redrawn. See <see cref="Paint"/> for details.
        /// </summary>
        /// <param name="e">An <see cref="PaintEventArgs"/> that contains the event data.</param>
        protected virtual void OnPaint(PaintEventArgs e)
        {
        }

        /// <summary>
        /// Called after a new control handler is attached.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnHandlerAttached(EventArgs e)
        {
        }

        private void CreateAndAttachHandler()
        {
            handler = CreateHandler();
            handler.Attach(this);
            OnHandlerAttached(EventArgs.Empty);
        }

        private void Children_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = this;
        }

        private void Children_ItemRemoved(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = null;
        }
    }
}
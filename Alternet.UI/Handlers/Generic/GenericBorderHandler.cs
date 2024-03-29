using Alternet.Drawing;

namespace Alternet.UI
{
    internal class GenericBorderHandler : ControlHandler<Border>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext drawingContext)
        {
            if (Control.Background != null)
                drawingContext.FillRectangle(Control.Background, ClientRectangle);
            
            if (Control.BorderBrush != null)
                drawingContext.DrawRectangle(new Pen(Control.BorderBrush), ClientRectangle);
        }

        public override RectangleF ChildrenLayoutBounds
        {
            get
            {
                var bounds = base.ChildrenLayoutBounds;
                bounds.X++;
                bounds.Y++;
                bounds.Width -= 2;
                bounds.Height -= 2;
                return bounds; // todo: border thickness.
            }
        }

        public override SizeF GetPreferredSize(SizeF availableSize)
        {
            return base.GetPreferredSize(availableSize) + new SizeF(2, 2);
        }
    }
}
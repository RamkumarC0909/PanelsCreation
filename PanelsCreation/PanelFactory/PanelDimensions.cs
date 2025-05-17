namespace PanelsCreation.PanelFactory
{
    public struct PanelDimensions : IEquatable<PanelDimensions>
    {
        public double Width { get; }
        public double Height { get; }

        public PanelDimensions(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public bool Equals(PanelDimensions other)
        {
            const double tolerance = 0.001;
            return Math.Abs(Width - other.Width) < tolerance &&
                   Math.Abs(Height - other.Height) < tolerance;
        }

    }
}

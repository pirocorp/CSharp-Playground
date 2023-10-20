namespace CSharp11Demo.AbstractingOverStaticMembers;

public class Point : IPoint<Point>
{
    public Point(double x, double y)
    {
        this.X = x;
        this.Y = y;
    }

    public double X { get; }

    public double Y { get; }

    public static Point operator +(Point a, Point b)
    {
        var x = a.X + b.X;
        var y = a.Y + b.Y;

        return new Point(x, y);
    }

    public override string ToString()
    {
        return $"(X: {this.X}, Y: {this.Y})";
    }
}

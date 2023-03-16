using System.Numerics;

namespace rt004;

public interface ISolid
{
    public Vector3 Position { get; set; }
    public Vector3 Color { get; set; }
    public bool Intersect(Vector3 p0, Vector3 p1, out float t);

    public Vector3 GetNormal(Vector3 p1, Vector3 p);

}

public class Sphere : ISolid
{
    public Vector3 Position { get; set; }
    public Vector3 Color { get; set; }
    public float Radius { get; set; }

    public Sphere(Vector3 position, float radius, Vector3 color)
    {
        this.Position = position;
        this.Radius = radius;
        this.Color = color;
    }

    /// <summary>
    /// Returns true if the ray intersects the sphere, and sets t to the distance along the ray where the intersection occurs.
    /// </summary>
    /// <param name="p0">The origin of the ray.</param>
    /// <param name="p1">The direction of the ray.</param>
    /// <param name="t">The distance along the ray where the intersection occurs.</param>
    /// <returns>True if the ray intersects the sphere, false otherwise.</returns>
    public bool Intersect(Vector3 p0, Vector3 p1, out float t)
    {
        p1 = Vector3.Normalize(p1);
        var p0c = p0 - this.Position;
        var b = 2 * Vector3.Dot(p1, p0c);
        var c = Vector3.Dot(p0c, p0c) - this.Radius * this.Radius;
        var d = b * b - 4 * c;
        if (d < 0)
        {
            t = 0;
            return false;
        }
        else
        {
            var t1 = (float)(-b + System.Math.Sqrt(d)) / (2);
            var t2 = (float)(-b - System.Math.Sqrt(d)) / (2);
            if (t1 < 0 && t2 < 0)
            {
                t = 0;
                return false;
            }
            else if (t1 < 0)
            {
                t = t2;
                return true;
            }
            else if (t2 < 0)
            {
                t = t1;
                return true;
            }
            else
            {
                t = System.Math.Min(t1, t2);
                return true;
            }
        }
    }

    /// <summary>
    /// Returns the normal vector at the point p assuming that p1 is the direction of the ray that intersects the sphere at p.
    /// </summary>
    /// <param name="p1">The direction of the ray that intersects the sphere at p.</param>
    /// <param name="p">The point on the sphere.</param>
    /// <returns>The normal vector at the point p.</returns>
    public Vector3 GetNormal(Vector3 p1, Vector3 p)
    {
        var cp = this.Position - p;
        if (Vector3.Dot(cp, p1) > 0){
            // p1 points to the inside of the sphere
            return Vector3.Normalize(cp);
        }
        // p1 points to the outside of the sphere, so we want p - c = -(c-p) = -cp
        return -Vector3.Normalize(cp);
    }
}

public class Plane : ISolid{
    public Vector3 Position { get; set; }
    public Vector3 Color { get; set; }
    public Vector3 Normal { get; set; }

    /// <summary>
    /// Creates a plane with the given position, normal, and side length (SideLength).
    /// </summary>
    /// <param name="position">The position of the plane.</param>
    /// <param name="normal">The normal vector of the plane.</param>
    /// <param name="color">The color of the plane.</param>
    public Plane(Vector3 position, Vector3 normal, Vector3 color)
    {
        this.Position = position;
        this.Normal = Vector3.Normalize(normal);
        this.Color = color;
    }

    /// <summary>
    /// Returns true if the ray intersects the plane, and sets t to the distance along the ray where the intersection occurs.
    /// </summary>
    /// <param name="p0">The origin of the ray.</param>
    /// <param name="p1">The direction of the ray</param>
    /// <param name="t">The distance along the ray where the intersection occurs.</param>
    /// <returns>True if the ray intersects the plane, false otherwise.</returns>
    public bool Intersect(Vector3 p0, Vector3 p1, out float t)
    {
        var d = Vector3.Dot(this.Normal, p1);
        if (d < 1e-8 && d > -1e-8)
        {
            t = 0;
            return false;
        }
        else
        {
            t = Vector3.Dot(this.Position - p0, this.Normal) / d;
            return t >= 0;
        }
    }

    /// <summary>
    /// Returns the normal vector at the point p assuming that p1 is the direction of the ray that intersects the plane at p.
    /// </summary>
    /// <param name="p1">The direction of the ray that intersects the plane at p.</param>
    /// <param name="p">The point on the plane.</param>
    /// <returns>The normal vector at the point p.</returns>
    public Vector3 GetNormal(Vector3 p1, Vector3 p)
    {
        if (Vector3.Dot(this.Normal, p1) > 0){
            return this.Normal;
        }
        return -this.Normal;
    }
}
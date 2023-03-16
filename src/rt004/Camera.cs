using Util;
using System.Numerics;

namespace rt004;

class PerspectiveCamera {
    // perspective center 
    public Vector3 center;
    // direction of the camera
    public Vector3 direction;
    // up direction of the camera
    public Vector3 up;
    // center of the near plane
    public Vector3 nearPlaneCenter;
    // horizontal field of view in radians, i.e. value of pi means 180 degrees, so the camera would have infinitely large near plane in horizontal direction
    public double FOV;
    public int imageWidth;
    public int imageHeight;

    private Vector3 dx;
    private Vector3 dy;

    public PerspectiveCamera(Vector3 center, Vector3 direction, Vector3 up, double nearPlaneDistance, double FOV, int imageWidth, int imageHeight) {
        this.center = center;
        this.direction = Vector3.Normalize(direction);
        this.up = Vector3.Normalize(up);
        this.nearPlaneCenter = center + direction * (float)nearPlaneDistance;
        // assert that FOV is between 0.1 (~5.72958 degrees) and pi - 0.1 (~174.27 degrees), otherwise the near plane would be infinitely large in horizontal direction
        if (FOV <= 0.1 || FOV >= System.Math.PI - 0.1) {
            throw new System.Exception("FOV should be between 0 and pi - 0.1");
        }
        this.FOV = FOV;
        this.imageWidth = imageWidth;
        this.imageHeight = imageHeight;

        // bottom left corner is at dx=-1, dy=-1, top right corner is at dx=1, dy=1
        var right = Vector3.Normalize(Vector3.Cross(direction, up));
        var top = Vector3.Normalize(Vector3.Cross(right, direction));
        double halfWidth = nearPlaneDistance * System.Math.Tan(FOV / 2);
        double halfHeight = halfWidth * imageHeight / imageWidth;

        this.dx = right * (float)halfWidth;
        this.dy = top * (float)halfHeight;
    }

    /// returns the ray for the pixel at (x, y), where (-1, -1) is the bottom left corner and (1, 1) is the top right corner
    public bool GetRay(float x, float y, out Vector3 p0, out Vector3 p1) {
        p0 = nearPlaneCenter + dx * x + dy * y;
        p1 = Vector3.Normalize(p0 - center);
        return true;
    }

}
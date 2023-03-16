using Util;
using System.Numerics;

namespace rt004;

public interface ILightSource {
    Vector3 color { get; set; }
    Vector3 DirectionAtPoint(Vector3 p);
}

public class PointLight : ILightSource {
    public Vector3 color { get; set; }
    public Vector3 position { get; set; }

    public PointLight(Vector3 position, Vector3 color){
        this.position = position;
        this.color = color;
    }

    public Vector3 DirectionAtPoint(Vector3 p){
        return Vector3.Normalize(p - this.position);
    }
}

public class AmbientLight : ILightSource {
    public Vector3 color { get; set; }

    public AmbientLight(Vector3 color){
        this.color = color;
    }

    public Vector3 DirectionAtPoint(Vector3 p){
        return new Vector3(0, 0, 0);
    }
}
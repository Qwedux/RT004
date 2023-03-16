using Util;
using System.Numerics;

namespace rt004;

public interface ILightSource {
    Vector3 color { get; set; }
    float intensity { get; set; }
    Vector3 DirectionAtPoint(Vector3 p);
    float IntensityAtPoint(Vector3 p);
}

public class PointLight : ILightSource {
    public Vector3 color { get; set; }
    public Vector3 position { get; set; }
    public float intensity { get; set; }

    public PointLight(Vector3 position, Vector3 color, float intensity){
        this.position = position;
        this.color = color;
        this.intensity = intensity;
    }

    public Vector3 DirectionAtPoint(Vector3 p){
        return Vector3.Normalize(p - this.position);
    }

    public float IntensityAtPoint(Vector3 p){
        return this.intensity / Vector3.Distance(p, this.position);
    }
}

public class AmbientLight : ILightSource {
    public Vector3 color { get; set; }
    public float intensity { get; set; }

    public AmbientLight(Vector3 color, float intensity){
        this.color = color;
        this.intensity = intensity;
    }

    public Vector3 DirectionAtPoint(Vector3 p){
        return new Vector3(0, 0, 0);
    }

    public float IntensityAtPoint(Vector3 p){
        return this.intensity;
    }
}
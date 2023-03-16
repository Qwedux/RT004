using Util;
using System.Numerics;

namespace rt004;

public class Material{
    public Vector3 color { get; set; }
    public Vector3 specularColor { get; set; }
    public double specularCoefficient { get; set; }
    public double specularity { get; set; }
    public double diffuseCoefficient { get; set; }
    public float ambientCoefficient { get; set; }

    public Material(){
        this.color = new Vector3(1, 1, 1);
        this.specularColor = new Vector3(1, 1, 1);
        this.specularCoefficient = 0.0;
        this.specularity = 0.0;
        this.diffuseCoefficient = 1;
        this.ambientCoefficient = 0.1F;
    }

}

class BRDF {
    public static Vector3 SimpleReflectance(Vector3 n, Vector3 l, Vector3 v, Material m){
        // E = E_A + \sum{E_D + E_S}
        // E_D = m.color * m.diffuseCoefficient * max(0, n dot l)
        // E_A = m.color * m.ambientCoefficient # only added up once, so not here
        // E_S = m.specularColor * m.specularCoefficient * max(0, n dot h)^m.specularity

        var E_D = m.color * (float)m.diffuseCoefficient * System.Math.Max(0, Vector3.Dot(n, l));
        var E_S = m.specularColor * (float)m.specularCoefficient * (float)System.Math.Pow(System.Math.Max(0, Vector3.Dot(n, Vector3.Normalize(l + v))), m.specularity);

        return E_D + E_S;
    }
}

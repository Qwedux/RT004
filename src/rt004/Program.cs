using Util;
using System.Numerics;

namespace rt004;

class Config
{
    public int imageWidth;
    public int imageHeight;
    public string outputFileName = "output.pfm";

    // overwrite to string
    public override string ToString()
    {
        return $"imageWidth: {imageWidth}, imageHeight: {imageHeight}, outputFileName: {outputFileName}";
    }

    public Config(string fileName)
    {
        // open file
        var lines = System.IO.File.ReadAllLines(fileName);
        // for each line split by ":"
        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                if (key == "imageWidth")
                {
                    this.imageWidth = int.Parse(value);
                }
                else if (key == "imageHeight")
                {
                    this.imageHeight = int.Parse(value);
                }
                else if (key == "outputPath")
                {
                    this.outputFileName = value;
                }
            }
        }
        if (this.outputFileName == null)
        {
            throw new System.Exception("Invalid config file");
        }
    }
}

class CommandLineParser
{
    public static Config ParseCommandLine(string[] args)
    {
        // there should be 1 argument - the config file name
        if (args.Length != 1)
        {
            throw new System.Exception("Invalid number of arguments");
        }
        return new Config(args[0]);
    }
}

class Scene
{
    public List<ISolid> solids = new List<ISolid>();
    public List<ILightSource> lights = new List<ILightSource>();
    public List<Material> materials = new List<Material>();
    public PerspectiveCamera? camera;

    public Scene ExampleScene()
    {
        materials = new List<Material> {
            new Material() { color = new Vector3(1, 1, 0.2F), ambientCoefficient = 0.1F, diffuseCoefficient = 0.8F, specularCoefficient = 0.2F, specularity = 10F },
            new Material() { color = new Vector3(0.2F, 0.3F, 1F), ambientCoefficient = 0.1F, diffuseCoefficient = 0.5F, specularCoefficient = 0.5F, specularity = 150F },
            new Material() { color = new Vector3(0.8F, 0.2F, 0.2F), ambientCoefficient = 0.1F, diffuseCoefficient = 0.6F, specularCoefficient = 0.4F, specularity = 80F },
        };
        solids = new List<ISolid> {
            new Sphere(new Vector3(0, 0, 0), radius: 1.0F, materials[0]),
            new Sphere(new Vector3(1.4F, -0.7F, -0.5F), radius: 0.6F, materials[1]),
            new Sphere(new Vector3(-0.7F, 0.7F, -0.8F), radius: 0.1F, materials[2])
        };
        lights = new List<ILightSource> {
            new PointLight(new Vector3(-10.0F, 8.0F, -6.0F), new Vector3(1, 1, 1), 1.0F),
            new PointLight(new Vector3(0.0F, 20.0F, -3.0F), new Vector3(1, 1, 1), 0.3F),
            new AmbientLight(new Vector3(1.0F, 1.0F, 1.0F), 1.0F)
        };
        camera = new PerspectiveCamera(new Vector3(0.60F, 0.00F, -5.60F), new Vector3(0, -0.03F, 1.00F), new Vector3(0, 1, 0), 1.0004498988F, 0.698132F, 600, 450);
        return this;
    }

    public void RayCast()
    {
        if (camera == null)
        {
            throw new System.Exception("Camera not set");
        }
        FloatImage fi = new FloatImage(camera.imageWidth, camera.imageHeight, 3);
        // for each pixel in camera get ray, find intersection of ray with scene, get contribution of each light source
        for (int y = 0; y < camera.imageHeight; y++)
        {
            for (int x = 0; x < camera.imageWidth; x++)
            {
                Vector3 rayOrigin, rayDirection;
                // get ray
                var ray = camera.GetRay(2 * (x / (float)camera.imageWidth - 0.5F), 2 * (y / (float)camera.imageHeight - 0.5F), out rayOrigin, out rayDirection);
                // find intersection of ray with scene
                float nearest = -1;
                Material? material = null;
                Vector3? normal = null;
                Vector3 intersectionPoint = new Vector3();

                foreach (var solid in solids)
                {
                    float t;
                    var intersects = solid.Intersect(rayOrigin, rayDirection, out t);
                    if (intersects)
                    {
                        if (nearest == -1 || t < nearest)
                        {
                            nearest = t;
                            material = solid.m;
                            intersectionPoint = rayOrigin + rayDirection * t;
                            normal = solid.GetNormal(rayDirection, intersectionPoint);
                        }
                    }
                }
                // get contribution of each light source
                var color = new float[] { 0, 0, 0 };
                if (material != null && normal != null)
                {
                    foreach (var light in lights)
                    {
                        var lightIntensity = light.IntensityAtPoint(intersectionPoint);
                        // if we have ambient light
                        if (light is AmbientLight)
                        {
                            color[0] += light.color.X * lightIntensity * material.color.X * material.ambientCoefficient;
                            color[1] += light.color.Y * lightIntensity * material.color.Y * material.ambientCoefficient;
                            color[2] += light.color.Z * lightIntensity * material.color.Z * material.ambientCoefficient;
                        }
                        else
                        {
                            var relativeColorCoefficient = BRDF.SimpleReflectance((Vector3)normal, light.DirectionAtPoint(intersectionPoint), rayDirection, material);
                            color[0] += light.color.X * lightIntensity * relativeColorCoefficient.X;
                            color[1] += light.color.Y * lightIntensity * relativeColorCoefficient.Y;
                            color[2] += light.color.Z * lightIntensity * relativeColorCoefficient.Z;
                        }
                    }
                }
                // put pixel in image
                fi.PutPixel(x, y, color);
            }
        }
        fi.SavePFM("demo.pfm");
        Console.WriteLine("HDR image is finished.");
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        var scene = new Scene().ExampleScene();
        scene.RayCast();

        // var test = new Vector3(1, 2, 3);
        // // Parameters.
        // // TODO: parse command-line arguments and/or your config file.
        // var cfg = CommandLineParser.ParseCommandLine(args);

        // Console.WriteLine(cfg.ToString());

        // // HDR image.
        // FloatImage fi = new FloatImage(cfg.imageWidth, cfg.imageHeight, 3);

        // // TODO: put anything interesting into the image.
        // // TODO: use fi.PutPixel() function, pixel should be a float[3] array [R, G, B]

        // // draw a red circle
        // for (int y = 0; y < cfg.imageHeight; y++)
        // {
        //     for (int x = 0; x < cfg.imageWidth; x++)
        //     {
        //         float[] pixel = new float[3];
        //         float dx = x - cfg.imageWidth / 2;
        //         float dy = y - cfg.imageHeight / 2;
        //         float r = (float)Math.Sqrt(dx * dx + dy * dy);
        //         if (r < 100)
        //         {
        //             pixel[0] = 2;
        //             pixel[1] = 0;
        //             pixel[2] = 0;
        //             fi.PutPixel(x, y, pixel);
        //         }
        //     }
        // }

        // // draw a green circle
        // for (int y = 0; y < cfg.imageHeight; y++)
        // {
        //     for (int x = 0; x < cfg.imageWidth; x++)
        //     {
        //         float[] pixel = new float[3];
        //         float dx = x - cfg.imageWidth / 2;
        //         float dy = y - cfg.imageHeight / 2;
        //         float r = (float)Math.Sqrt(dx * dx + dy * dy);
        //         if (r < 50)
        //         {
        //             pixel[0] = 0;
        //             pixel[1] = 1;
        //             pixel[2] = 1;
        //             fi.PutPixel(x, y, pixel);
        //         }
        //     }
        // }

        // //fi.SaveHDR(fileName);   // Doesn't work well yet...
        // fi.SavePFM(cfg.outputFileName);

        // Console.WriteLine("HDR image is finished.");
    }
}

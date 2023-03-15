using Util;
//using System.Numerics;

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
}


class CommandLineParser
{
    public static Config ParseConfigFile(string fileName)
    {
        Config cfg = new Config();
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
                if (key == "image_width")
                {
                    cfg.imageWidth = int.Parse(value);
                }
                else if (key == "image_height")
                {
                    cfg.imageHeight = int.Parse(value);
                }
                else if (key == "output_path")
                {
                    cfg.outputFileName = value;
                }
            }
        }
        if (cfg.outputFileName == null)
        {
            throw new System.Exception("Invalid config file");
        }
        return cfg;
    }

    public static Config ParseCommandLine(string[] args)
    {
        // there should be 1 argument - the config file name
        if (args.Length != 1)
        {
            throw new System.Exception("Invalid number of arguments");
        }
        return ParseConfigFile(args[0]);
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        // Parameters.
        // TODO: parse command-line arguments and/or your config file.
        var cfg = CommandLineParser.ParseCommandLine(args);

        Console.WriteLine(cfg.ToString());

        // HDR image.
        FloatImage fi = new FloatImage(cfg.imageWidth, cfg.imageHeight, 3);

        // TODO: put anything interesting into the image.
        // TODO: use fi.PutPixel() function, pixel should be a float[3] array [R, G, B]

        // draw a red circle
        for (int y = 0; y < cfg.imageHeight; y++)
        {
            for (int x = 0; x < cfg.imageWidth; x++)
            {
                float[] pixel = new float[3];
                float dx = x - cfg.imageWidth / 2;
                float dy = y - cfg.imageHeight / 2;
                float r = (float)Math.Sqrt(dx * dx + dy * dy);
                if (r < 100)
                {
                    pixel[0] = 2;
                    pixel[1] = 0;
                    pixel[2] = 0;
                    fi.PutPixel(x, y, pixel);
                }
            }
        }

        // draw a green circle
        for (int y = 0; y < cfg.imageHeight; y++)
        {
            for (int x = 0; x < cfg.imageWidth; x++)
            {
                float[] pixel = new float[3];
                float dx = x - cfg.imageWidth / 2;
                float dy = y - cfg.imageHeight / 2;
                float r = (float)Math.Sqrt(dx * dx + dy * dy);
                if (r < 50)
                {
                    pixel[0] = 0;
                    pixel[1] = 1;
                    pixel[2] = 1;
                    fi.PutPixel(x, y, pixel);
                }
            }
        }
        
        //fi.SaveHDR(fileName);   // Doesn't work well yet...
        fi.SavePFM(cfg.outputFileName);

        Console.WriteLine("HDR image is finished.");
    }
}

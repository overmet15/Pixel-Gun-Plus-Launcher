using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections;
public static class Utils
{
    public static Color ColorToUColor(int r, int g, int b, int a = 255)
    {
        Color color = new(r / 255f, g / 255f, b / 255f, a / 255f);
        return color;
    }

    public static IEnumerator Unzip(string path, string destination)
    {
        Process process = new();

        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = $"/C tar -xf {path} -C {destination}";

        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = true;

        process.Start();

        while (!process.HasExited) yield return null;
    }
}
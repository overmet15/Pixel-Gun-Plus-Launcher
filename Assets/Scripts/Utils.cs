using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
public static class Utils
{
    public static Color ColorToUColor(int r, int g, int b, int a = 255)
    {
        Color color = new(r / 255f, g / 255f, b / 255f, a / 255f);
        return color;
    }

    public static Process Unzip(string path, string destination)
    {
        Process process = new();

        process.StartInfo.FileName = "C:/Windows/System32/tar.exe";
        process.StartInfo.Arguments = $"-xf \"{path}\" -C \"{destination}\"";

        process.StartInfo.CreateNoWindow = false;
        process.StartInfo.UseShellExecute = false;

        process.Start();

        return process;
    }
}
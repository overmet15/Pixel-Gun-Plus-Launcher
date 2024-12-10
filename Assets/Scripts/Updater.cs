using UnityEngine;
using System;
using System.Diagnostics;

public static class Updater
{
    public static void Update(string launcherFolder, string zipFolder)
    {
        Process process = new Process();

        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = "/C echo hi & pause";

        process.StartInfo.UseShellExecute = true;
        process.StartInfo.CreateNoWindow = false;

        process.Start();
    }
}

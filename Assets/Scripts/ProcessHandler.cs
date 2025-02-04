using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ProcessHandler
{
    public static Process process;

    public static void StartMonitoringProcess()
    {
        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Global.GameExePath,
                UseShellExecute = true,
            },
            EnableRaisingEvents = true // Enable Exited event
        };

        try
        {
            process.Start();
            UnityEngine.Debug.Log("Started and monitoring process.");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Failed to start process: " + e.Message);
        }
        finally
        {
            Application.Quit();
        }
    }
}

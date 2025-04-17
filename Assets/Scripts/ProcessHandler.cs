using System;
using System.Diagnostics;
using UnityEngine;

public static class ProcessHandler
{
    public static Process process;

    public static void StartMonitoringProcess()
    {
        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Global.GameExecutablePath,
                UseShellExecute = true,
            },
            EnableRaisingEvents = true // Enable Exited event
        };

        try
        {
            process.Start();
            UnityEngine.Debug.Log("Started and monitoring process.");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Failed to start process: " + e.Message);
        }
        finally
        {
            #if UNITY_EDITOR
            if (PrefsManager.closeOnPlay) UnityEditor.EditorApplication.ExitPlaymode();
            #else
            if (PrefsManager.closeOnPlay) Application.Quit();
            #endif
            
        }
    }
}

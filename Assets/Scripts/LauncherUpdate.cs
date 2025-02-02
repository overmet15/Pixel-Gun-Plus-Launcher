using System;
using System.Diagnostics;
using UnityEngine;
using System.Threading.Tasks;

public static class LauncherUpdate
{
    public static async Task RestartApplication()
    {
        try
        {
            string exePath = GetApplicationExecutablePath();

            // Log the path to confirm it is correct
            UnityEngine.Debug.Log("Executable path: " + exePath);

            // Optionally, exit the current application first
            Application.Quit();

            // Start a new process (the same application)
            Process.Start(exePath);

            await Task.Delay(1000);
            // Log to confirm restart has been triggered
            UnityEngine.Debug.Log("Application restarted successfully.");
        }
        catch (Exception ex)
        {
            // Handle any potential errors
            UnityEngine.Debug.LogError("Error restarting application: " + ex.Message);
        }
    }

    private static string GetApplicationExecutablePath()
    {
#if UNITY_EDITOR
        // In the Editor, we can't restart the app, so return a placeholder or handle it differently.
        return string.Empty;
#else
        // In a build, use Process to get the executable path
        return Process.GetCurrentProcess().MainModule.FileName;
#endif
    }
}

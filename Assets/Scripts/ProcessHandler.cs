using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ProcessHandler
{
    public static string path => Application.streamingAssetsPath + "/../../Game/Pixel Gun Plus.exe";
    public static Process process;
    public static GameObject unaviableScreen, mainScreen, backgrnd;
    public static void StartMonitoringProcess()
    {
        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true,
            },
            EnableRaisingEvents = true // Enable Exited event
        };

        process.Exited += OnProcessEnd;
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
            if (PrefsManager.closeOnGameStart) Application.Quit();

            unaviableScreen.SetActive(true);
            mainScreen.SetActive(false);
            backgrnd.SetActive(false);
        }
    }

    public static void OnProcessEnd(object sender, EventArgs e)
    {
        unaviableScreen.SetActive(false);

        UnityEngine.Debug.Log("The monitored process has exited.");
        
        process.Exited -= OnProcessEnd;
        process.Dispose();

        SceneManager.LoadScene("Launcher");
    }
}

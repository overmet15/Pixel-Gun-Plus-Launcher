using System.Diagnostics;
using System.Threading.Tasks;

public static class CMDUtils
{
    public static async Task Unzip(string pathToZip, string destination)
    {
        Process process = new Process();

        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = $"/C tar -xf {pathToZip} -C {destination}";

        process.StartInfo.CreateNoWindow = false;

        process.Start();

        await Task.Run(() => process.WaitForExit());
    }
}
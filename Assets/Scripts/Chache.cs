using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class Chache
{
    public static async Task<Texture2D> Texture(string url)
    {
        string path = Path.GetFileName(url);
        string p = Path.GetDirectoryName(url);
        Debug.Log(path);
        Debug.Log(p);
        return null;
    }
}

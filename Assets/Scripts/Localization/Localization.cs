using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Localization
{
    /*public static Dictionary<string, LocalizationItem> current = new();
    public static Dictionary <string, TextAsset> languageAssetChache = new();

    // for switching
    public static List<string> textAssetNames = new();
    public static int currentNum = 0;

    public static UnityEvent onLocalizationChange = new();

    public static void Init()
    {
        TextAsset[] assets = Resources.LoadAll<TextAsset>("Localization/");

        foreach (TextAsset textAsset in assets) languageAssetChache.Add(textAsset.name, textAsset);

        TextAsset toRead = null;

        if (languageAssetChache.TryGetValue(PrefsManager.language, out TextAsset asset)) toRead = asset;
        else toRead = languageAssetChache["english"];

        foreach (TextAsset textAsset in languageAssetChache.Values) textAssetNames.Add(textAsset.name);
        ChangeLocalization(toRead);
    }

    public static void ChangeLocalization(string languageName)
    {
        if (languageAssetChache.ContainsKey(languageName)) ChangeLocalization(languageAssetChache[languageName]);
    }

    static void ChangeLocalization(TextAsset localizationAsset)
    {
        current = ReadFile(localizationAsset);
        currentNum = textAssetNames.IndexOf(localizationAsset.name);
        onLocalizationChange.Invoke();
    }

    public static Dictionary<string, LocalizationItem> ReadFile(TextAsset textAsset)
    {
        Dictionary<string, LocalizationItem> result = new();

        string file = textAsset.text;

        // FIRST LINE IS SETUP LINE
        string[] lines = file.Split('\n');

        int baseFontSize = 0;

        foreach (string line in lines)
        {
            // Comments
            if (line.StartsWith("#")) continue;

            string[] parts = line.Split('\\');
            if (parts.Length < 2) continue;

            string key = parts[0];
            string value = parts[1];

            int fontSize = 0;
            if (parts.Length > 2)
            {
                if (int.TryParse(parts[2], out int parse)) fontSize = parse;
                if (baseFontSize == 0) baseFontSize = fontSize;
            }

            if (fontSize == 0) fontSize = baseFontSize;

            result.Add(key, new(key, value, fontSize));
        }
        return result;
    }

    public static LocalizationItem GetItem(string key)
    {
        return current.ContainsKey(key) == false ? new(key, key, current["LanguageName"].fontSize) : current[key];
    }*/
}

public class LocalizationItem
{
    /*public string key;
    public string value;

    public int fontSize;

    public LocalizationItem(string key, string value, int fontSize)
    {
        this.key = key;
        this.value = value;
        this.fontSize = fontSize;
    }*/
}
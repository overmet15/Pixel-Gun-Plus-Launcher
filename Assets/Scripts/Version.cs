using System;
public class Version
{
    public int major;
    public int minor;
    public int patch;

    // Parsing Methods
    public static Version Parse(string toParse)
    {
        if (string.IsNullOrWhiteSpace(toParse))
            throw new ArgumentException("Input version string cannot be null or empty.");

        string[] ss = toParse.Split('.');
        if (!TryParseParts(ss, out int major, out int minor, out int patch))
            throw new ArgumentException("Invalid version format. Expected major.minor.patch");

        return new Version { major = major, minor = minor, patch = patch };
    }

    public static bool TryParse(string toParse, out Version version)
    {
        version = null;

        if (string.IsNullOrWhiteSpace(toParse))
            return false;

        string[] ss = toParse.Split('.');
        if (!TryParseParts(ss, out int major, out int minor, out int patch))
            return false;

        version = new Version { major = major, minor = minor, patch = patch };
        return true;
    }

    private static bool TryParseParts(string[] parts, out int major, out int minor, out int patch)
    {
        major = minor = patch = 0;
        return parts.Length == 3 &&
               int.TryParse(parts[0], out major) &&
               int.TryParse(parts[1], out minor) &&
               int.TryParse(parts[2], out patch);
    }

    // Comparison Operators
    public static bool operator >(Version ver1, Version ver2)
    {
        if (ver1.major > ver2.major) return true;
        if (ver1.major < ver2.major) return false;

        if (ver1.minor > ver2.minor) return true;
        if (ver1.minor < ver2.minor) return false;

        return ver1.patch > ver2.patch;
    }

    public static bool operator <(Version ver1, Version ver2)
    {
        if (ver1.major < ver2.major) return true;
        if (ver1.major > ver2.major) return false;

        if (ver1.minor < ver2.minor) return true;
        if (ver1.minor > ver2.minor) return false;

        return ver1.patch < ver2.patch;
    }

    public static bool operator >=(Version ver1, Version ver2)
    {
        return ver1 > ver2 || ver1.Equals(ver2);
    }

    public static bool operator <=(Version ver1, Version ver2)
    {
        return ver1 < ver2 || ver1.Equals(ver2);
    }

    // Equality Overrides
    public override bool Equals(object obj)
    {
        if (obj is Version other)
        {
            return major == other.major &&
                   minor == other.minor &&
                   patch == other.patch;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(major, minor, patch);
    }

    public static bool operator ==(Version ver1, Version ver2)
    {
        if (ReferenceEquals(ver1, ver2)) return true;
        if (ReferenceEquals(ver1, null) || ReferenceEquals(ver2, null)) return false;

        return ver1.major == ver2.major &&
            ver1.minor == ver2.minor &&
            ver1.patch == ver2.patch;
    }

    public static bool operator !=(Version ver1, Version ver2)
    {
        return !(ver1 == ver2);
    }


    public override string ToString()
    {
        return $"{major}.{minor}.{patch}";
    }
}
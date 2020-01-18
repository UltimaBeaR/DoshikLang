using System.Collections.Generic;
using System.IO;
using System.Text;

public static class TestLogger
{
    private const string _filePath = @"C:\Users\BeaR\Desktop\udon_logs\";

    private static Dictionary<string, StringBuilder> _sbList;

    public static void Begin()
    {
        _sbList = new Dictionary<string, StringBuilder>();
    }

    public static void End()
    {
        foreach (var pair in _sbList)
        {
            File.WriteAllText(_filePath + pair.Key + ".txt", pair.Value.ToString());
        }
    }

    public static void LogLine(string name, string text)
    {
        StringBuilder sb;
        if (!_sbList.TryGetValue(name, out sb))
        {
            sb = new StringBuilder();
            _sbList[name] = sb;
        }

        sb.AppendLine(text);
    }
}
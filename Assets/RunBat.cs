#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class RunBat : Editor {
    static Process CreateShellExProcess (string cmd, string args, string workingDir = "") {
        var pStartInfo = new ProcessStartInfo (cmd);
        pStartInfo.Arguments = args;
        pStartInfo.CreateNoWindow = false;
        pStartInfo.UseShellExecute = true;
        pStartInfo.RedirectStandardError = false;
        pStartInfo.RedirectStandardInput = false;
        pStartInfo.RedirectStandardOutput = false;
        if (!string.IsNullOrEmpty (workingDir))
            pStartInfo.WorkingDirectory = workingDir;
        return Process.Start (pStartInfo);
    }

    static void RunRealBat (string batfile, string args, string workingDir = "") {
        var p = CreateShellExProcess (batfile, args, workingDir);
        p.Close ();
    }

    static string FormatPath (string path) {
        path = path.Replace ("/", "\\");
        if (Application.platform == RuntimePlatform.OSXEditor)
            path = path.Replace ("\\", "/");
        return path;
    }

    private static void RunMyBat (string batFile, string workingDir) {
        var path = FormatPath (workingDir);
        var file = path + batFile;
        if (!System.IO.File.Exists (file)) {
            Debug.LogError ($"bat文件不存在: {file}");
        } else {
            RunRealBat (batFile, "", path);
        }
    }

    [MenuItem ("Assets/执行外部Bat文件", false, 5)]
    private static void Run () {
        // 执行bat脚本
        RunMyBat ("TestBat.bat", Directory.GetParent (Application.dataPath) + "\\BatFiles\\");
    }
}
#endif
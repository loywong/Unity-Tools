#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class SvnHelper : Editor {
    static string SVN_BASE = Application.dataPath.Replace ("/", "\\").Remove (Application.dataPath.Replace ("/", "\\").Length - 6, 6);
    [MenuItem ("Assets/SVN Logs", false, 1)]
    public static void Logs () {
        RunCmd ("TortoiseProc.exe", "/command:log /path:\"" + SVN_BASE + "\"");
    }

    [MenuItem ("Assets/SVN 提交 Assets目录", false, 1)]
    private static void Commit_Assets () {
        RunCmd ("TortoiseProc.exe", string.Format ("/command:commit /path:\"{0}\\{1}\" /closeonend:0", System.Environment.CurrentDirectory, "Assets"));
    }

    [MenuItem ("Assets/SVN 更新 Assets目录", false, 1)]
    private static void Update_Assets () {
        RunCmd ("TortoiseProc.exe", string.Format ("/command:update /path:\"{0}\\{1}\" /closeonend:0", System.Environment.CurrentDirectory, "Assets"));
    }

    #region 提交指定文件或文件夹
    [MenuItem ("Assets/SVN 提交 选中的 File|Folder", false, 2)]
    private static bool Commit_FileOrFolder () {
        if (Selection.activeObject == null)
            return false;
        else {
            RunCmd ("TortoiseProc.exe", string.Format ("/command:commit /path:\"{0}\\{1}\" /closeonend:0", System.Environment.CurrentDirectory, AssetDatabase.GetAssetPath (Selection.activeObject)));
            return true;
        }
    }
    #endregion

    #region 更新指定文件或文件夹
    [MenuItem ("Assets/SVN 更新 选中的 File|Folder", false, 2)]
    private static bool Update_FileOrFolder () {
        if (Selection.activeObject == null)
            return false;
        else {
            RunCmd ("TortoiseProc.exe", string.Format ("/command:update /path:\"{0}\\{1}\" /closeonend:0", System.Environment.CurrentDirectory, AssetDatabase.GetAssetPath (Selection.activeObject)));
            return true;
        }
    }
    #endregion

    [MenuItem ("Assets/SVN 提交 ProjectSettings", false, 3)]
    private static void Commit_ProjectSettings () {
        RunCmd ("TortoiseProc.exe", string.Format ("/command:commit /path:\"{0}\\{1}\" /closeonend:0", System.Environment.CurrentDirectory, "ProjectSettings"));
    }

    [MenuItem ("Assets/SVN 更新 ProjectSettings", false, 3)]
    private static void Update_ProjectSettings () {
        RunCmd ("TortoiseProc.exe", string.Format ("/command:update /path:\"{0}\\{1}\" /closeonend:0", System.Environment.CurrentDirectory, "ProjectSettings"));
    }

    /// <summary>
    /// 运行外部程序 
    /// </summary>
    /// <param name="cmdExe">指定应用程序的完整路径，如果该程序在系统环境变量中，只需要填写对用的程序名称就可以</param>
    /// <param name="cmdStr">执行命令行参数</param>
    private static bool RunCmd (string cmdExe, string cmdStr) {
        bool result = false;
        try {
            using (Process myPro = new Process ()) {
                //指定启动进程是调用的应用程序和命令行参数
                ProcessStartInfo psi = new ProcessStartInfo (cmdExe, cmdStr);
                myPro.StartInfo = psi;
                myPro.Start ();
                // 是否加上这句话，看个人需求。如果加上的话，我们必须关掉弹出的SVN窗口才能继续操作。如果不加上，则可以弹出SVN，也可以继续修改unity项目。个人建议加上比较好
                myPro.WaitForExit ();
                result = true;
            }
        } catch {

        }
        return result;
    }
}
#endif
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FixedStartupScene : Editor {
    // 建立规则，指定只有以下特定的场景 适用Initialize()逻辑
    private static string[] scenePaths = new string[] {
        "Assets/Scene/Splash.unity",
        "Assets/Scene/Login.unity",
        "Assets/Scene/Lobby.unity",
        "Assets/Scene/Battle.unity"
    };

    [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize () {
        // 固定从Entry场景启动
        Scene scene = SceneManager.GetActiveScene ();
        bool isParticularScene = false;
        for (int i = 0; i < scenePaths.Length; i++) {
            if (string.Equals (scenePaths[i], scene.path)) {
                isParticularScene = true;
                break;
            }
        }
        if (isParticularScene)
            SceneManager.LoadScene ("Entry");
    }
}
#endif
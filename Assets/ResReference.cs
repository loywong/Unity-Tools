#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResReference : EditorWindow {
    //需要反向检查那些对象引用此对象的文件夹路径，可能引用此对象的目录
    static string[] checkPaths = new string[] {
        // "Assets/Prefabs",
        // "Assets/Scenes",
        // "Assets/Configs",
        "Assets",
    };
    static public ResReference instance;
    Vector2 mScroll = Vector2.zero;
    public Dictionary<string, List<string>> dict;

    void OnEnable () { instance = this; }
    void OnDisable () { instance = null; }

    void OnGUI () {
        if (dict == null)
            return;

        mScroll = GUILayout.BeginScrollView (mScroll);

        List<string> list = dict["level"];
        if (list != null && list.Count > 0) {
            if (DrawHeader ("Level")) {
                foreach (string item in list) {
                    SceneAsset go = AssetDatabase.LoadAssetAtPath (item, typeof (SceneAsset)) as SceneAsset;
                    EditorGUILayout.ObjectField ("Level:", go, typeof (SceneAsset), false);
                }
            }
            list = null;
        }

        list = dict["prefab"];
        if (list != null && list.Count > 0) {
            if (DrawHeader ("Prefab")) {
                foreach (string item in list) {
                    GameObject go = AssetDatabase.LoadAssetAtPath (item, typeof (GameObject)) as GameObject;
                    EditorGUILayout.ObjectField ("Prefab", go, typeof (GameObject), false);

                }
            }
            list = null;
        }

        list = dict["fbx"];
        if (list != null && list.Count > 0) {
            if (DrawHeader ("FBX")) {
                foreach (string item in list) {
                    GameObject go = AssetDatabase.LoadAssetAtPath (item, typeof (GameObject)) as GameObject;
                    EditorGUILayout.ObjectField ("FBX", go, typeof (GameObject), false);

                }
            }
            list = null;
        }

        list = dict["cs"];
        if (list != null && list.Count > 0) {
            if (DrawHeader ("Script")) {
                foreach (string item in list) {
                    MonoScript go = AssetDatabase.LoadAssetAtPath (item, typeof (MonoScript)) as MonoScript;
                    EditorGUILayout.ObjectField ("Script", go, typeof (MonoScript), false);

                }
            }
            list = null;
        }

        list = dict["texture"];
        if (list != null && list.Count > 0) {
            if (DrawHeader ("Texture")) {
                foreach (string item in list) {
                    Texture2D go = AssetDatabase.LoadAssetAtPath (item, typeof (Texture2D)) as Texture2D;
                    EditorGUILayout.ObjectField ("Texture:" + go.name, go, typeof (Texture2D), false);

                }
            }
            list = null;
        }

        list = dict["mat"];
        if (list != null && list.Count > 0) {
            if (DrawHeader ("Material")) {
                foreach (string item in list) {
                    Material go = AssetDatabase.LoadAssetAtPath (item, typeof (Material)) as Material;
                    EditorGUILayout.ObjectField ("Material", go, typeof (Material), false);

                }
            }
            list = null;
        }

        list = dict["shader"];
        if (list != null && list.Count > 0) {
            if (DrawHeader ("Shader")) {
                foreach (string item in list) {
                    Shader go = AssetDatabase.LoadAssetAtPath (item, typeof (Shader)) as Shader;
                    EditorGUILayout.ObjectField ("Shader", go, typeof (Shader), false);
                }
            }
            list = null;
        }

        list = dict["font"];
        if (list != null && list.Count > 0) {
            if (DrawHeader ("Font")) {
                foreach (string item in list) {
                    Font go = AssetDatabase.LoadAssetAtPath (item, typeof (Font)) as Font;
                    EditorGUILayout.ObjectField ("Font", go, typeof (Font), false);
                }
            }
            list = null;
        }

        list = dict["anim"];
        if (list != null && list.Count > 0) {
            if (DrawHeader ("Animation")) {
                foreach (string item in list) {
                    AnimationClip go = AssetDatabase.LoadAssetAtPath (item, typeof (AnimationClip)) as AnimationClip;
                    EditorGUILayout.ObjectField ("Animation:", go, typeof (AnimationClip), false);
                }
            }
            list = null;
        }

        list = dict["animTor"];
        if (list != null && list.Count > 0) {
            if (DrawHeader ("Animator")) {
                foreach (string item in list) {
                    RuntimeAnimatorController go = AssetDatabase.LoadAssetAtPath (item, typeof (RuntimeAnimatorController)) as RuntimeAnimatorController;
                    EditorGUILayout.ObjectField ("Animator:", go, typeof (RuntimeAnimatorController), true);
                }
            }
            list = null;
        }

        GUILayout.EndScrollView ();
        //DrawList("Objects", list.ToArray(), "");
    }

    /// <summary>
    /// 查找对象所有依赖，和对它的引用
    /// </summary>
    [MenuItem ("Assets/Find Resource Dependencies", false, 6)]
    public static void FindObjectDependencies () {
        ShowProgress (0, 0, 0);
        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>> ();
        List<string> prefabList = new List<string> ();
        List<string> fbxList = new List<string> ();
        List<string> scriptList = new List<string> ();
        List<string> textureList = new List<string> ();
        List<string> matList = new List<string> ();
        List<string> shaderList = new List<string> ();
        List<string> fontList = new List<string> ();
        List<string> animList = new List<string> ();
        List<string> levelList = new List<string> ();
        List<string> animTorList = new List<string> ();
        string curPathName = AssetDatabase.GetAssetPath (Selection.activeObject.GetInstanceID ());
        //搜索对象的依赖资源
        string[] names = AssetDatabase.GetDependencies (new string[] { curPathName });
        int i = 0;
        foreach (string name in names) {
            if (name.ToLower ().EndsWith (".fbx")) {
                fbxList.Add (name);
            } else if (name.EndsWith (".cs")) {
                scriptList.Add (name);
            } else if (name.EndsWith (".png")) {
                textureList.Add (name);
            } else if (name.EndsWith (".mat")) {
                matList.Add (name);
            } else if (name.EndsWith (".shader")) {
                shaderList.Add (name);
            } else if (name.EndsWith (".ttf")) {
                fontList.Add (name);
            } else if (name.EndsWith (".anim")) {
                animList.Add (name);
            } else if (name.EndsWith (".controller")) {
                animTorList.Add (name);
            }
            Debug.Log ("Dependence:" + name);
            ShowProgress ((float) i / (float) names.Length, names.Length, i);
            i++;
        }
        //这里只检查是否在Prefab和Scene有引用，如果要检查其他，在这里添加筛选类型，并且打开下面的对应的注释
        string checkType = "t:Prefab t:Scene";
        i = 0;
        string[] allGuids = AssetDatabase.FindAssets (checkType, checkPaths);
        foreach (string guid in allGuids) {
            string assetPath = AssetDatabase.GUIDToAssetPath (guid);
            names = AssetDatabase.GetDependencies (new string[] { assetPath });
            foreach (string name in names) {
                if (name.Equals (curPathName)) {
                    //Debug.Log("Refer:" + assetPath);
                    if (assetPath.EndsWith (".prefab")) {
                        prefabList.Add (assetPath);
                        break;
                    } else if (assetPath.ToLower ().EndsWith (".unity")) {
                        levelList.Add (assetPath);
                        break;
                    }
                    //else if (assetPath.ToLower().EndsWith(".fbx"))
                    //{
                    //    fbxList.Add(assetPath);
                    //    break;
                    //}
                    //else if (assetPath.EndsWith(".cs"))
                    //{
                    //    scriptList.Add(assetPath);
                    //    break;
                    //}
                    //else if (assetPath.EndsWith(".png"))
                    //{
                    //    textureList.Add(assetPath);
                    //    break;
                    //}
                    //else if (assetPath.EndsWith(".mat"))
                    //{
                    //    matList.Add(assetPath);
                    //    break;
                    //}
                    //else if (assetPath.EndsWith(".shader"))
                    //{
                    //    shaderList.Add(assetPath);
                    //    break;
                    //}
                    //else if (assetPath.EndsWith(".ttf"))
                    //{
                    //    fontList.Add(assetPath);
                    //    break;
                    //}
                }
            }
            ShowProgress ((float) i / (float) allGuids.Length, allGuids.Length, i);
            i++;
        }
        dic.Add ("prefab", prefabList);
        dic.Add ("fbx", fbxList);
        dic.Add ("cs", scriptList);
        dic.Add ("texture", textureList);
        dic.Add ("mat", matList);
        dic.Add ("shader", shaderList);
        dic.Add ("font", fontList);
        dic.Add ("level", levelList);
        dic.Add ("animTor", animTorList);
        dic.Add ("anim", animList);
        EditorWindow.GetWindow<ResReference> (false, "Object Dependencies", true).Show ();
        if (ResReference.instance.dict != null) ResReference.instance.dict.Clear ();
        ResReference.instance.dict = dic;
        EditorUtility.ClearProgressBar ();
    }

    //集成NGUI方法，显示可折叠窗口
    public bool DrawHeader (string text) { return DrawHeader (text, text, false, false); }
    public bool DrawHeader (string text, string key, bool forceOn, bool minimalistic) {
        bool state = EditorPrefs.GetBool (key, true);

        if (!minimalistic) GUILayout.Space (3f);
        if (!forceOn && !state) GUI.backgroundColor = new Color (0.8f, 0.8f, 0.8f);
        GUILayout.BeginHorizontal ();
        GUI.changed = false;

        if (minimalistic) {
            if (state) text = "\u25BC" + (char) 0x200a + text;
            else text = "\u25BA" + (char) 0x200a + text;

            GUILayout.BeginHorizontal ();
            GUI.contentColor = EditorGUIUtility.isProSkin ? new Color (1f, 1f, 1f, 0.7f) : new Color (0f, 0f, 0f, 0.7f);
            if (!GUILayout.Toggle (true, text, "PreToolbar2", GUILayout.MinWidth (20f))) state = !state;
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal ();
        } else {
            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle (true, text, "dragtab", GUILayout.MinWidth (20f))) state = !state;
        }

        if (GUI.changed) EditorPrefs.SetBool (key, state);

        if (!minimalistic) GUILayout.Space (2f);
        GUILayout.EndHorizontal ();
        GUI.backgroundColor = Color.white;
        if (!forceOn && !state) GUILayout.Space (3f);
        return state;
    }

    /// <summary>
    /// 显示进度条
    /// </summary>
    /// <param name="val"></param>
    /// <param name="total"></param>
    /// <param name="cur"></param>
    public static void ShowProgress (float val, int total, int cur) {
        EditorUtility.DisplayProgressBar ("Searching", string.Format ("Checking ({0}/{1}), please wait...", cur, total), val);
    }
}
#endif
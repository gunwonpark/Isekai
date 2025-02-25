using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class QuickSceneSelector : EditorWindow
{
    [MenuItem("Scene/Select Scene")]
    private static void Init()
    {
        QuickSceneSelector window = GetWindow<QuickSceneSelector>();
        window.titleContent = new GUIContent("Scene Selector");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a Scene", EditorStyles.boldLabel);

        //EditorBuildSettings에 등록된 씬들을 가져온다
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                //씬의 이름만 가져온다.
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
                if (GUILayout.Button(sceneName))
                {
                    OpenScene(scene.path);
                }
            }
        }
    }

    private static void OpenScene(string scenePath)
    {

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}
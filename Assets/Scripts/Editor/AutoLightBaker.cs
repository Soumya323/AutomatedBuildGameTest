using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class AutoLightBaker : OdinEditorWindow
{
    [MenuItem("Tools/Debug/AutoLightBaker")]
    private static void OpenWindow()
    {
        GetWindow<AutoLightBaker>("AutoLightBaker").Show();
    }

    [Button("Test Lighting"), PropertySpace(SpaceBefore = 50)]
    public void TestLighting()
    {

        BakeLightingForScenesInBuildSettings();
        /*
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        string openScenePath = scenes[0].path;
        EditorSceneManager.OpenScene(openScenePath, OpenSceneMode.Single);

        GameObject lightbakeObj = GameObject.Find("AutoLightBakerPrepareScene");

        if (lightbakeObj != null)
        {
            if (lightbakeObj.TryGetComponent<AutoLightBakerPrepareScene>(out AutoLightBakerPrepareScene lightBakePrepareScene))
            {
                lightBakePrepareScene.MakeObjectsStatic();
                BakeScenes(openScenePath);
                lightBakePrepareScene.MakeObjectsNonStatic();
                EditorSceneManager.SaveOpenScenes();
            }
            else
                Debug.LogWarning("AutoLightBaker : AutoLightBakerPrepareScene object found but the script on it wasn't found, scene is " + openScenePath);
        }
        else
        {
            Debug.LogWarning("AutoLightBaker : AutoLightBakerPrepareScene object is missing add it for building the light, scene is " + openScenePath);
        }
        */
    }

    private void BakeScenes(string scenePath)
    {
        if (Lightmapping.Bake())
        {
            Debug.Log("AutoLightBaker : Light baked successfully for " + scenePath);
        }
        else
        {
            Debug.LogWarning("AutoLightBaker : Light bake failed for " + scenePath);
        }
    }


    public void BakeLightingForScenesInBuildSettings()
    {
        // Will bake lighting for all scenes added and active in the build settings

        List<string> scenePaths = new List<string>();
        foreach (EditorBuildSettingsScene scenePath in EditorBuildSettings.scenes)
        {
            if (scenePath.enabled)
                scenePaths.Add(scenePath.path);
        }


        GameObject lightBakeObj = null;

        for (int i = 0; i < scenePaths.Count; i++)
        {
            lightBakeObj = null;
            EditorSceneManager.OpenScene(scenePaths[i], OpenSceneMode.Single);

            lightBakeObj = GameObject.Find("AutoLightBakerPrepareScene");

            if (lightBakeObj != null)
            {
                if (lightBakeObj.TryGetComponent<AutoLightBakerPrepareScene>(out AutoLightBakerPrepareScene lightBakerPrepareScene))
                {
                    lightBakerPrepareScene.MakeObjectsStatic();
                    BakeScenes(scenePaths[i]);
                    lightBakerPrepareScene.MakeObjectsNonStatic();
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                    EditorSceneManager.SaveOpenScenes();
                }
                else
                {
                    Debug.LogWarning("AutoLightBaker : AutoLightBakerPrepareScene object found but the script on it wasn't found, scene is " + scenePaths[i]);
                }
            }
            else
            {
                Debug.LogWarning("AutoLightBaker : AutoLightBakerPrepareScene object is missing add it for building the light, scene is " + scenePaths[i]);
            }
        }
    }
}

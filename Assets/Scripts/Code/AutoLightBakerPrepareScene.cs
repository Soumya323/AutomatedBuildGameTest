using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteInEditMode]
public class AutoLightBakerPrepareScene : MonoBehaviour
{
    public List<GameObject> gameObjectsListToBeStaticOnBake = default;
    string sceneName = default;

    public void MakeObjectsStatic()
    {
        if (gameObjectsListToBeStaticOnBake != null)
        {
            GameObject tempGameObject = null;
            for (int i = 0; i < gameObjectsListToBeStaticOnBake.Count; i++)
            {
                if (gameObjectsListToBeStaticOnBake[i] != null)
                {
                    tempGameObject = gameObjectsListToBeStaticOnBake[i];
                    tempGameObject.isStatic = true;
                    Debug.Log("AutoLightBakerPrepareScene : object made in static true " + tempGameObject.name);
                    // GameObjectUtility.SetStaticEditorFlags(tempGameObject, StaticEditorFlags.BatchingStatic);
                }
            }
        }
        sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("AutoLightBakerPrepareScene : Light build pre process done. " + sceneName);
    }

    public void MakeObjectsNonStatic()
    {
        if (gameObjectsListToBeStaticOnBake != null)
        {
            GameObject tempGameObject = null;
            for (int i = 0; i < gameObjectsListToBeStaticOnBake.Count; i++)
            {
                if (gameObjectsListToBeStaticOnBake[i] != null)
                {
                    tempGameObject = gameObjectsListToBeStaticOnBake[i];
                    tempGameObject.isStatic = false;
                    Debug.Log("AutoLightBakerPrepareScene : object made in static false " + tempGameObject.name);
                    // StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(tempGameObject) & ~(StaticEditorFlags.BatchingStatic);
                    // GameObjectUtility.SetStaticEditorFlags(tempGameObject, flags);
                }
            }
        }
        sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("AutoLightBakerPrepareScene : Light build post process done. " + sceneName);
    }
}

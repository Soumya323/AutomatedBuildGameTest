using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugLoadNextScene : MonoBehaviour
{
    [SerializeField] private string nextScene = "Scene2";

    private enum Hand
    {
        Left, Right
    };

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            IndexTriggerPressed(Hand.Right);
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            IndexTriggerPressed(Hand.Left);
        }
    }



    private void IndexTriggerPressed(Hand hand)
    {
        if (hand == Hand.Right)
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}

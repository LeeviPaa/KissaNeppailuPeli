using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SceneListProperties
{
    public Transform cameraTarget;
    public int SceneIndex;
}

public class MainMenuController : MonoBehaviour {

    public GameObject winText;
    public GameObject nepCounter;
    public GameObject MainCamera;
    private int currIndex = 0;

    public float lerpSpeed = 10;

    public List<SceneListProperties> Scenelist = new List<SceneListProperties>();

	void Start () {
        if (MainCamera == null)
            Debug.LogError("MainCamera not found!!");

	}
	
	void Update () {
        if (Scenelist[currIndex].cameraTarget != null)
        {
            MainCamera.transform.position = Vector3.Lerp(
                MainCamera.transform.position,
                Scenelist[currIndex].cameraTarget.position,
                lerpSpeed * Time.deltaTime);
        }
        else
            Debug.LogError("CameraTarget not found!!");
	}

    public void TryMapRight()
    {
        if(currIndex+1 < Scenelist.Count)
        {
            currIndex++;
        }
    }
    public void TryMapLeft()
    {
        if(currIndex > 0)
        {
            currIndex--;
        }
    }
    public void PlayMap()
    {
        try
        {
            SceneManager.LoadScene(Scenelist[currIndex].SceneIndex);
        }
        catch
        {
            Debug.LogError("SceneWith scene index: " + Scenelist[currIndex].SceneIndex + " not found!");
        }
    }
}

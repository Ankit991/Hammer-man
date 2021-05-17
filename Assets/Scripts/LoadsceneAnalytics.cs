using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using UnityEngine.SceneManagement;
public class LoadsceneAnalytics : MonoBehaviour
{
  [HideInInspector] public int sceneindex;
    // Start is called before the first frame update
    void Start()
    {
        sceneindex = PlayerPrefs.GetInt("Level");
        StartCoroutine(Analytics());
      
    }

    IEnumerator Analytics()
    {
        yield return null;
        GameAnalytics.Initialize();
       
        if (sceneindex >= 1)
        {
            SceneManager.LoadScene(sceneindex);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}

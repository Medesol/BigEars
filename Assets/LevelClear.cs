using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClear : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //AnalyticsResult gameResult = Analytics.CustomEvent(
            //    "gameComplete",
            //    new Dictionary<string, object>
            //    {
            //        { "level name", SceneManager.GetActiveScene().name },
            //        { "time",  }
            //    }
            //);
            Debug.Log("done");
            //Debug.Log("status: " + gameResult);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

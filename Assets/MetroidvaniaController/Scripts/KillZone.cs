using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Player entered!");
            col.gameObject.GetComponent<CharacterController2D>().ApplyDamage(0, Vector3.zero);
            // send analytics
            AnalyticsResult gameResult = Analytics.CustomEvent(
                "gameOver_fall_below",
                new Dictionary<string, object>
                {
                    { "level name", SceneManager.GetActiveScene().name },
                    { "progress", (col.gameObject.transform.position.x + 5.5)/70 }
                }
            );
            //Debug.Log("scecne: " + SceneManager.GetActiveScene().name);
            //Debug.Log("Log: " + (col.gameObject.transform.position.x + 5.5)/70);

            //Debug.Log("status: " + gameResult);
            StartCoroutine(WaitForLoad());
        }
        else if (col.gameObject.tag != "Line")
        {
            Destroy(col.gameObject);
        }
    }

    IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}

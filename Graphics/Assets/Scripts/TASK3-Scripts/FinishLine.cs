using UnityEngine.SceneManagement;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public bool finishHit = false;

    public void OnTriggerEnter(Collider col)
    {
        if (SceneManager.GetActiveScene().name == "Level1" && col.gameObject.tag == "Player")
        {
            Debug.Log("checkpoint");
            finishHit = true;
            SceneManager.LoadScene("Level2");
        }
        if (SceneManager.GetActiveScene().name == "Level2" && col.gameObject.tag == "Player")
        {
            Debug.Log("checkpoint");
            finishHit = true;
            SceneManager.LoadScene("Level3");
        }
        if (SceneManager.GetActiveScene().name == "Level3" && col.gameObject.tag == "Player")
        {
            Debug.Log("checkpoint");
            finishHit = true;
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}

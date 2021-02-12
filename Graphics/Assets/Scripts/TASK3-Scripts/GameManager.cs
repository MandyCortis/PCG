using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public int wayPointCount = 0;

    public GameObject[] waypointReference;

 

    // Update is called once per frame
    public void Update()
    {
        GoToNextLvl();
        CameraColorShift();
    }

    public void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Destroy(this); }
        //DontDestroyOnLoad(gameObject);
    }

    public void GoToNextLvl()
    {
        if (wayPointCount >= waypointReference.Length && SceneManager.GetActiveScene().name == "Level1")
        {

            wayPointCount = 0;
            SceneManager.LoadScene("Level2");

        }

        if (wayPointCount >= waypointReference.Length && SceneManager.GetActiveScene().name == "Level2")
        {
            wayPointCount = 0;
            //SceneManager.LoadScene("Level3");

        }
        if (wayPointCount >= waypointReference.Length && SceneManager.GetActiveScene().name == "Level3")
        {
            wayPointCount = 0;
            //SceneManager.LoadScene("Win");

        }
    }

    public void CameraColorShift()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            Camera.main.GetComponent<Camera>().backgroundColor = new Color32(45, 89, 159, 255);
        }

        if (SceneManager.GetActiveScene().name == "Level2")
        {
            Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            Camera.main.GetComponent<Camera>().backgroundColor = new Color32(20, 31, 46, 255);
        }

        if (SceneManager.GetActiveScene().name == "Level3")
        {
            Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            Camera.main.GetComponent<Camera>().backgroundColor = new Color32(70, 63, 19, 255);
        }

    }
}
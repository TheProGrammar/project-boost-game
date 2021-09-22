using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly contact");
                break;
            case "Finish":
                Debug.Log("Finish contact");
                break;
            case "Fuel":
                Debug.Log("Fuel contact");
                break;
            default:
                ReloadLevel();
                break;
        }
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Reloads the currently active scene
        SceneManager.LoadScene(currentSceneIndex);
    }

}

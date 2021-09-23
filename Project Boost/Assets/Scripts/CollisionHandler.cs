using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float reloadLvlDelay = 1.5f;
    [SerializeField] float nextLvlDelay = 1.5f;

    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip finishSound;

    AudioSource audioSource;

    bool isTransitioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartFinishSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartFinishSequence()
    {
        isTransitioning = true;
        // todo add particle effect
        GetComponent<Movement>().enabled = false;
        GetComponent<Rigidbody>().freezeRotation = true;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        Invoke("LoadNextLevel", nextLvlDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        // todo add particle effect
        GetComponent<Movement>().enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        Invoke("ReloadLevel", reloadLvlDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Reloads the currently active scene
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

}

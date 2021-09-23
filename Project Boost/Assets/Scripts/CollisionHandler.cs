using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // SceneManagement Delays
    [SerializeField] float reloadLvlDelay = 1.5f;
    [SerializeField] float nextLvlDelay = 1.5f;

    // Audio Files
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip finishSound;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem finishParticles;

    [SerializeField] BoxCollider legColliderOne;
    [SerializeField] BoxCollider legColliderTwo;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckDebugInputs();
    }

    private void CheckDebugInputs()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled) { return; }

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

        finishParticles.Play();
        GetComponent<Movement>().enabled = false;
        GetComponent<Rigidbody>().freezeRotation = true;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        Invoke("LoadNextLevel", nextLvlDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;

        crashParticles.Play();
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

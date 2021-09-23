using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationSpeed = 100f;

    [SerializeField] AudioClip engineSound;

    [SerializeField] ParticleSystem jetFire;
    [SerializeField] ParticleSystem leftThruster;
    [SerializeField] ParticleSystem rightThruster;

    Rigidbody rb;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }

        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }

        else
        {
            StopRotating();
        }
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!jetFire.isEmitting && !audioSource.isPlaying)
        {
            jetFire.Play();
            audioSource.PlayOneShot(engineSound);
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        if (jetFire.isEmitting)
        {
            jetFire.Stop();

        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationSpeed);
        if (!rightThruster.isEmitting)
        {
            rightThruster.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationSpeed);
        if (!leftThruster.isEmitting)
        {
            leftThruster.Play();
        }
    }

    private void StopRotating()
    {
        rightThruster.Stop();
        leftThruster.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate to avoid rotation bug
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}

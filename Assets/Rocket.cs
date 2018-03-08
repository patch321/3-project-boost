using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    enum State { Alive, Dying, Trancending}

    State state = State.Alive;
	
    // Use this for initialization
	void Start () 
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update () 
    {
        Thrust();
		Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag){
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                state = State.Trancending;
                Invoke("LoadNextScene", 1f);
                break;
            default :
                state = State.Dying;
                Invoke("LoadFirstScene", 1f);
                break;
                
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {
        if (state != State.Dying)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rigidBody.AddRelativeForce(Vector3.up * mainThrust);
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.Stop();
            }
        }
    }

    private void Rotate()
    {
        if (state != State.Dying)
        {
            rigidBody.freezeRotation = true;
            float rotationThisFrame = rcsThrust * Time.deltaTime;

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * rotationThisFrame);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(-Vector3.forward * rotationThisFrame);
            }

            rigidBody.freezeRotation = false;
        }
    }

}

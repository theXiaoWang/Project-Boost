using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    
    AudioSource _audioSource;
    
    bool isTransitioning = false;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(UnityEngine.Collision other)
    {
        if (isTransitioning)
        {
            return;
        }
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
            break;
        }
    }

    void StartSuccessSequence()
    {        
        isTransitioning = true;
        _audioSource.Stop();
        _audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", this.levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        _audioSource.Stop();
        _audioSource.PlayOneShot(crash, 0.2f);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", this.levelLoadDelay);
        
    }

    void LoadNextLevel()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevel == SceneManager.sceneCountInBuildSettings)
        {
            nextLevel = 0;
        }
        SceneManager.LoadScene(nextLevel);
    }

    void ReloadLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }
}

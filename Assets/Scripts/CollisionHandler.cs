using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    
    [SerializeField] AudioClip successAudio;
    [SerializeField] AudioClip crashAudio;
    
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    
    AudioSource _audioSource;
    
    bool _isTransitioning;
    bool _disableCollision;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // _disableCollision = GetComponent<Cheat>().disableCollision;
    }

    //Entry of all collisions
    void OnCollisionEnter(Collision other)
    {
        if (_isTransitioning || _disableCollision)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                ProcessSuccessSequence();
                break;
            default:
                ProcessCrashSequence();
            break;
        }
    }

    void ProcessSuccessSequence()
    {
        ApplySequence("LoadNextLevel", successParticles, successAudio, 0.2f);
    }

    void ProcessCrashSequence()
    {
        ApplySequence("ReloadLevel", crashParticles, crashAudio, 0.2f);
    }

    void ApplySequence(string levelAction, ParticleSystem particle, AudioClip audioClip, float volume)
    {
        _isTransitioning = true;
        particle.Play();
        _audioSource.Stop();
        _audioSource.PlayOneShot(audioClip, volume);
        //Deprive the control right of the player
        GetComponent<Movement>().enabled = false;
        //Invoke is used to load a certain level, 2nd para is delay time
        Invoke(levelAction, levelLoadDelay);
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

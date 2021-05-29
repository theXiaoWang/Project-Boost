using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] float superBoostRate = 1.5f;
    
    //Audio file:
    [SerializeField] AudioClip mainEngineAudio;

    [SerializeField] ParticleSystem mainBoosterParticles;
    [SerializeField] ParticleSystem superBoosterParticles;
    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;
    
    //Rigid body is needed to manipulate this object's physical properties
    Rigidbody _rigidbody; 
    //Audio listener(added in main camera) - audio source - audio file
    AudioSource _audioSource;

    float _superBoostedForce;
    float thrustAudioVolume = 0.5f;
    bool _isSuperBoosted;

    void Start()
    {
        _superBoostedForce = mainThrust * superBoostRate;
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }
    
    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                StartSuperBoosting();
                return; 
            }
            StopSuperBoosting(); //Effective after super boosted is active, then disable it by assigning false
            StartThrusting(mainThrust);
        }
        else
        {
            StopThrusting();
            StopSuperBoosting();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateRight();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }
    }
    
    void StartThrusting(float thrustForce)
    {
        _rigidbody.AddRelativeForce(Vector3.up * (thrustForce * Time.deltaTime));
        
        if (!_audioSource.isPlaying && !_isSuperBoosted)
        {
            //PlayOneShot method allows a specific audio chosen from many
            _audioSource.PlayOneShot(mainEngineAudio, thrustAudioVolume);
        }
        if (!mainBoosterParticles.isPlaying && !_isSuperBoosted)
        {
            mainBoosterParticles.Play();
        }
    }

    void StartSuperBoosting()
    {
        StopThrusting(); //Mute the normal thrust audio when super boosted is inactive(false), otherwise keep playing super boosted audio(true)
        _isSuperBoosted = true;
        StartThrusting(_superBoostedForce);
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(mainEngineAudio, thrustAudioVolume * 2);
        }
        if (!superBoosterParticles.isPlaying)
        {
            superBoosterParticles.Play();
        }
    }
    
    private void StopThrusting()
    {
        if (!_isSuperBoosted)
        {
            _audioSource.Stop();
        }
        mainBoosterParticles.Stop();
    }
    
    void StopSuperBoosting()
    {
        if (_isSuperBoosted)
        {
            _audioSource.Stop();
        }
        _isSuperBoosted = false;
        superBoosterParticles.Stop();
    }
    
    void RotateRight()
    {
        ApplyRotation(rotationThrust);
        if (!rightBoosterParticles.isPlaying)
        {
            rightBoosterParticles.Play();
        }
    }

    void RotateLeft()
    {
        ApplyRotation(-rotationThrust);
        if (!leftBoosterParticles.isPlaying)
        {
            leftBoosterParticles.Play();
        }
    }
    
    private void StopRotating()
    {
        rightBoosterParticles.Stop();
        leftBoosterParticles.Stop();
    }
  
    void ApplyRotation(float rotationThisFrame)
    {
        _rigidbody.freezeRotation = true; //Disable system physics to control manually
        //Time.deltaTime is used to be independent from Frame rate. High FPS has low deltaTime, low FPS has high deltaTime
        transform.Rotate(Vector3.forward * (rotationThisFrame * Time.deltaTime));
        _rigidbody.freezeRotation = false; //Enable system physics once manually control is about to over
    }
}
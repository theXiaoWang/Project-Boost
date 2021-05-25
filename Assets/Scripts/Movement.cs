 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] AudioClip mainEngine;
    Rigidbody _rigidbody;
    AudioSource _audioSource;
   
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);
        }        
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationThrust);
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        _rigidbody.freezeRotation = true; //Disable system physics to control manually
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        _rigidbody.freezeRotation = false; //Enable system physics
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(mainEngine);
            }
        }
        else
        {
            _audioSource.Stop();
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}

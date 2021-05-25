 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float mainThrust = 100f;
    [SerializeField] private float rotationThrust = 1f;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
   
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

    private void ProcessRotation()
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

    private void ApplyRotation(float rotationThisFrame)
    {
        _rigidbody.freezeRotation = true; //Disable system physics to control manually
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        _rigidbody.freezeRotation = false; //Enable system physics
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.Stop();
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}

using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] float SuperBoostRate = 1.5f;
    
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
    
    void ProcessThrust()
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
    
    void StartThrusting()
    {
        float tempThrust = mainThrust;
        bool superBoosted = false;
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            superBoosted = true;
            tempThrust *= SuperBoostRate;
            if (!superBoosterParticles.isPlaying)
            {
                mainBoosterParticles.Stop();
                superBoosterParticles.Play();
            }
        }
        else
        {
            superBoosterParticles.Stop();
        }
        _rigidbody.AddRelativeForce(Vector3.up * tempThrust * Time.deltaTime);
        if (!_audioSource.isPlaying)
        {
            //PlayOneShot method allows a specific audio chosen from many
            _audioSource.PlayOneShot(mainEngineAudio);
        }

        if (!mainBoosterParticles.isPlaying && !superBoosted)
        {
            mainBoosterParticles.Play();
        }
    }

    void applySuperBoost()
    {
        
    }
    
    private void StopThrusting()
    {
        _audioSource.Stop();
        mainBoosterParticles.Stop();
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
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        _rigidbody.freezeRotation = false; //Enable system physics once manually control is about to over
    }
}
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 1f;
    
    //Audio file:
    [SerializeField] AudioClip mainEngineAudio;

    [SerializeField] ParticleSystem mainBoosterParticles;
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

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);
            if (!rightBoosterParticles.isPlaying)
            {
                rightBoosterParticles.Play();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationThrust);
            if (!leftBoosterParticles.isPlaying)
            {
                leftBoosterParticles.Play();
            }
        }
        else
        {
            rightBoosterParticles.Stop();
            leftBoosterParticles.Stop();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        _rigidbody.freezeRotation = true; //Disable system physics to control manually
        //Time.deltaTime is used to be independent from Frame rate. High FPS has low deltaTime, low FPS has high deltaTime
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
                //PlayOneShot method allows a specific audio chosen from many
                _audioSource.PlayOneShot(mainEngineAudio);
            }

            if (!mainBoosterParticles.isPlaying)
            {
                mainBoosterParticles.Play();
            }
        }
        else
        {
            _audioSource.Stop();
            mainBoosterParticles.Stop();
        }
    }
}
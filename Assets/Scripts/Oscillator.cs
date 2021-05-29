using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float frequency;

    float _cycles;
    float _sinWave;
    const float Tau = Mathf.PI * 2; // 2π is the a full oscillation or a unit circle circumference, here 2π = 1τ
    float _movementFactor;

    Vector3 _startPosition;
    Vector3 _offset;
    
    void Start()
    {
        _startPosition = transform.position; // current position at starting point
    }
    void Update()
    {
        _cycles = Time.time * frequency; // Time.time means the seconds elapse since the program has started
        _sinWave = Mathf.Sin(_cycles * Tau);
        switch (gameObject.tag)
        {
            case "Sphere Dropper":
                SphereDropperOscillator();
                break;
            default:
                DefaultOscillator();
                break;
        }
    }

    void DefaultOscillator()
    {
        _movementFactor = (_sinWave + 1) / 2; // Because the range of Sine is (-1, 1), here we make the oscillation range (0, 1)
        StartOscillating();
    }
    
    private void SphereDropperOscillator()
    {
        _movementFactor = _sinWave; // Because the range of Sine is (-1, 1), here we make the oscillation range (0, 1)
        StartOscillating();
    }
    void StartOscillating()
    {
        _offset = movementVector * _movementFactor; // offset means the location of a piece of data compared to another location.
        transform.position = _startPosition + _offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private ParticleSystem boostParticle;

    [SerializeField] private float yValue;

    // Update is called once per frame
    void Update()
    {
        boostParticle.Play();
        RocketPatrol();

    }

    private void RocketPatrol()
    {
        transform.Translate(0,yValue,0);
    }
}

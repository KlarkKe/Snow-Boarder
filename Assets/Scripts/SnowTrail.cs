using UnityEngine;

public class SnowTrail : MonoBehaviour
{
    [SerializeField] ParticleSystem snowParticle;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        snowParticle.Play();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        snowParticle.Stop();
    }
}

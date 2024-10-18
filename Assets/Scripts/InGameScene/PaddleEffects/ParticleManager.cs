using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem paddleSizeUpEffectParticle;
    [SerializeField] private ParticleSystem paddleSizeDownEffectParticle;
    [SerializeField] private ParticleSystem paddleSpeedUpEffectParticle;
    [SerializeField] private ParticleSystem paddleSpeedDownEffectParticle;

    public void PlaySizeUpEffect(float duration)
    {
        PlayEffect(paddleSizeUpEffectParticle, duration);
    }

    public void PlaySizeDownEffect(float duration)
    {
        PlayEffect(paddleSizeDownEffectParticle, duration);
    }

    public void PlaySpeedUpEffect()
    {
        PlayEffect(paddleSpeedUpEffectParticle);
    }

    public void StopSpeedUpEffect()
    {
        StopEffect(paddleSpeedUpEffectParticle);
    }

    public void PlaySpeedDownEffect()
    {
        PlayEffect(paddleSpeedDownEffectParticle);
    }

    public void StopSpeedDownEffect()
    {
        StopEffect(paddleSpeedDownEffectParticle);
    }

    private void PlayEffect(ParticleSystem effect, float duration = 0f)
    {
        if (effect != null && !effect.isPlaying)
        {
            effect.Play();
            if (duration > 0f)
            {
                StartCoroutine(StopEffectAfterDuration(effect, duration));
            }
        }
    }

    private IEnumerator StopEffectAfterDuration(ParticleSystem effect, float duration)
    {
        yield return new WaitForSeconds(duration);
        StopEffect(effect);
    }

    private void StopEffect(ParticleSystem effect)
    {
        if (effect != null && effect.isPlaying)
        {
            effect.Stop();
        }
    }
}

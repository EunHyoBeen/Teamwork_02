using UnityEngine;

public class PaddleEffectManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] paddleSizeUpParticles;
    [SerializeField] private ParticleSystem[] paddleSizeDownParticles;
    [SerializeField] private TrailRenderer[] fastTrailRenderers;
    [SerializeField] private TrailRenderer[] slowTrailRenderers;

    private void Start()
    {
        for (int i = 0; i < fastTrailRenderers.Length; i++)
        {
            fastTrailRenderers[i].enabled = false;
        }
        for (int i = 0; i < slowTrailRenderers.Length; i++)
        {
            slowTrailRenderers[i].enabled = false;
        }
    }

    public void StopSizeEffects(int paddleIndex)
    {
        if (paddleSizeUpParticles[paddleIndex].isPlaying)
            paddleSizeUpParticles[paddleIndex].Stop();
        if (paddleSizeDownParticles[paddleIndex].isPlaying)
            paddleSizeDownParticles[paddleIndex].Stop();
    }

    public void StopTrailEffects(int paddleIndex)
    {
        fastTrailRenderers[paddleIndex].enabled = false;
        slowTrailRenderers[paddleIndex].enabled = false;
    }

    public void PlayPaddleSizeUpEffect(int paddleIndex)
    {
        StopSizeEffects(paddleIndex);
        paddleSizeUpParticles[paddleIndex].Play();
    }

    public void PlayPaddleSizeDownEffect(int paddleIndex)
    {
        StopSizeEffects(paddleIndex);
        paddleSizeDownParticles[paddleIndex].Play();
    }

    public void PlayFastTrail(int paddleIndex)
    {
        StopTrailEffects(paddleIndex);
        fastTrailRenderers[paddleIndex].enabled = true;
    }

    public void PlaySlowTrail(int paddleIndex)
    {
        StopTrailEffects(paddleIndex);
        slowTrailRenderers[paddleIndex].enabled = true;
    }

    public void ResetAllEffects(int paddleIndex)
    {
        StopSizeEffects(paddleIndex);
        StopTrailEffects(paddleIndex);
    }
}
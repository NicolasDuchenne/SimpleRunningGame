using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource serumAudioSource;
    private AudioSource catalyseurAudioSource;
    private AudioSource bonusAudioSource;
    private AudioSource malusAudioSource;
    
    private AudioSource disappearAudioSource;

    void Start()
    {
        serumAudioSource = transform.Find("Sounds").transform.Find("Serums").GetComponent<AudioSource>();        
        catalyseurAudioSource = transform.Find("Sounds").transform.Find("Catalyseurs").GetComponent<AudioSource>();  
        bonusAudioSource = transform.Find("Sounds").transform.Find("Bonus").GetComponent<AudioSource>();  
        malusAudioSource = transform.Find("Sounds").transform.Find("Malus").GetComponent<AudioSource>();  
        disappearAudioSource = transform.Find("Sounds").transform.Find("Disappear").GetComponent<AudioSource>();  
    }
    public void PlaySerum()
    {
        playSound(serumAudioSource);
    }
    public void PlayCatalyseur()
    {
        playSound(catalyseurAudioSource);
    }
    public void PlayBonus()
    {
        playSound(bonusAudioSource);
    }
    public void PlayMalus()
    {
        playSound(malusAudioSource);
    }
    public void PlayDisappear()
    {
        playSound(disappearAudioSource);
    }

    private void playSound(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();
    }
}

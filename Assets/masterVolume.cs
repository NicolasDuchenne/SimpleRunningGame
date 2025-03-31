using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    private Slider slider;
    [SerializeField] SharedOptions sharedOptions;

    void Start()
    {
        slider = GetComponent<Slider>();
        Debug.Log(sharedOptions.masterVolume);
        slider.value = sharedOptions.masterVolume;
        SetMasterVolume(sharedOptions.masterVolume);

    }

    // volume: entre 0.0001f et 1f
    public void SetMasterVolume(float volume)
    {
        float dB;
        if (volume > 0)
            dB = Mathf.Log10(volume) * 20;
        else
            dB = -80f; // silence total

        masterMixer.SetFloat("MasterVolume", dB);
        sharedOptions.masterVolume = volume;
    }

    public void OnVolumeSliderChanged(float value)
    {
        SetMasterVolume(value); // de 0.0001 à 1
    }
}

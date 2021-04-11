using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] SoundName soundName;
    [SerializeField] float soundStopTime;
    [SerializeField] bool randomPitch;
    [SerializeField] bool playOneShot;
    [SerializeField] bool checkInteractable;

    Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void PlaySound()
    {
        if (checkInteractable && !button.interactable) return;

        var soundManager = SoundManager.Instance;

        if (playOneShot)
            soundManager.PlayOneShot(soundName, randomPitch);
        else
            soundManager.Play(soundName, randomPitch);
    }

    public void StopSound()
    {
        if (checkInteractable && !button.interactable) return;

        SoundManager.Instance.Stop(soundName, soundStopTime);
    }

    public void StopAllSounds()
    {
        if (checkInteractable && !button.interactable) return;

        SoundManager.Instance.StopAll();
    }
}

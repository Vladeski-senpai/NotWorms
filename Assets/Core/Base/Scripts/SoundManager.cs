using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;

    public static SoundManager Instance;

    Sound foundSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(this);
            SetUpSounds();
        }
        else Destroy(gameObject);
    }

    // ����������� ��� �����
    void SetUpSounds()
    {
        foreach (var sound in sounds)
        {
            sound.AudioSource = gameObject.AddComponent<AudioSource>();

            sound.AudioSource.clip = sound.AudioClip;
            sound.AudioSource.volume = sound.Volume;
            sound.AudioSource.pitch = sound.MaxPitch;
            sound.AudioSource.loop = sound.Loop;
        }
    }

    // ������������� ����
    public Sound Play(SoundName soundName, bool randomPitch = false)
    {
        foundSound = System.Array.Find(sounds, sound => sound.SoundName == soundName);

        if (randomPitch)
            foundSound.AudioSource.pitch = Random.Range(foundSound.MinPitch, foundSound.MaxPitch);

        foundSound.AudioSource.Play();

        return foundSound;
    }

    // ������������� ���� ��������
    public Sound PlayOneShot(SoundName soundName, bool randomPitch = false)
    {
        foundSound = System.Array.Find(sounds, sound => sound.SoundName == soundName);

        if (randomPitch)
            foundSound.AudioSource.pitch = Random.Range(foundSound.MinPitch, foundSound.MaxPitch);

        foundSound.AudioSource.PlayOneShot(foundSound.AudioClip);

        return foundSound;
    }

    // ������������� ����
    public void Stop(SoundName soundName, float stopTime = 0)
    {
        foundSound = System.Array.Find(sounds, sound => sound.SoundName == soundName);

        if (stopTime > 0)
            StartCoroutine(foundSound.StopSound(stopTime));
        else
            foundSound.AudioSource.Stop();
    }

    // ������������� ��� �����
    public void StopAll()
    {
        foreach (var sound in sounds)
            sound.AudioSource.Stop();
    }
}

[System.Serializable]
public class Sound
{
    [SerializeField] SoundName soundName;
    [SerializeField] AudioClip audioClip;
    [SerializeField, Range(0, 1)] float volume = 1;
    [SerializeField, Range(0.1f, 3)] float minPitch = 1;
    [SerializeField, Range(0.1f, 3)] float maxPitch = 1;
    [SerializeField] bool loop;

    public SoundName SoundName => soundName;
    public AudioClip AudioClip => audioClip;
    public float Volume => volume;
    public float MinPitch => minPitch;
    public float MaxPitch => maxPitch;
    public bool Loop => loop;
    public AudioSource AudioSource { get; set; }

    public IEnumerator StopSound(float time)
    {
        float startValue = volume;
        float endValue = 0;
        float t = 0;

        while (t < time)
        {
            t += Time.deltaTime;
            AudioSource.volume = Mathf.Lerp(startValue, endValue, t / time);

            yield return null;
        }

        AudioSource.Stop();
        AudioSource.volume = volume;
    }
}

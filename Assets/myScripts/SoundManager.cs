using UnityEngine;
using System.Collections;

public enum AudioSources{Expolsion, Hit, UI};

public class SoundManager : MonoBehaviour {

    //Singleton private instance
    private static SoundManager instance;

    //Singleton instance getter
    public static SoundManager GetInstance{get{return instance;}}

    public AudioSource explosionSource;     
    public AudioSource hitSource;
    public AudioSource uiSource;

    public AudioClip hitSound;
    public AudioClip startSound;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioSources source ,AudioClip clip)
    {
        switch(source)
        {
            case AudioSources.Expolsion:
                explosionSource.clip = clip;
                explosionSource.Play();
                break;
            case AudioSources.Hit:
                hitSource.clip = clip;
                hitSource.Play();
                break;
            case AudioSources.UI:
                uiSource.clip = clip;
                uiSource.Play();
                break;
        }
    }
}

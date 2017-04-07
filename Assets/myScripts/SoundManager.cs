using UnityEngine;
using System.Collections;

public enum AudioSources{Expolsion, Hit, UI};

public class SoundManager : MonoBehaviour {


    private static SoundManager instance;                           //Singleton private instance
    public static SoundManager GetInstance{get{return instance;}}   //Singleton instance getter

    #region Audio sources
    public AudioSource explosionSource;                             //Audio source for explosion
    public AudioSource hitSource;                                   //Audio source for being hit by asteriods
    public AudioSource uiSource;                                    //Audio source for UI
    #endregion

    #region Audio clips
    public AudioClip hitSound;                                      //Audio clip of being hit by asteriods
    public AudioClip startSound;                                    //Audio clip of game start
    public AudioClip VictorySound;                                  //Audio clip of victory
    public AudioClip LoseSound;                                     //Audio clip of game lost
    public AudioClip RecoverySound;                                 //Audio clip of recovery lives
    #endregion

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

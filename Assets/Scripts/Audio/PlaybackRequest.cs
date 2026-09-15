using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Audio;

public class PlaybackRequest : MonoBehaviour
{
    #region Singleton Pattern
    private static PlaybackRequest instance;
    public static PlaybackRequest Instance => instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            return;
        }

        Destroy(gameObject);
    }
    #endregion

    [Header("Player Sounds")]
    public AudioClip jumpSound;
    public AudioClip fireSound;
    public AudioClip pointSound;
    public AudioClip pauseSound;
    public AudioClip powerUpSound;
    public AudioClip popSound;
    public AudioClip deathSound;

    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;

    //if you want to play a sound that is attached to a specific game object, use this method
    public void RequestOneShotSound(AudioClip clip, GameObject requestor, AudioMixerGroup mixerGroup = null)
    {
        if (!requestor) return;
        if (!mixerGroup) { Debug.LogWarning($"Mixer group not specified on {requestor.ToString()}. Using default mixer group."); mixerGroup = masterMixerGroup; }

        AudioSource audioSource = requestor.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = requestor.AddComponent<AudioSource>();
        }

        if (mixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = mixerGroup;
        }

        audioSource.PlayOneShot(clip);
    }

    //if your gameobject ends up being destroyed before the sound finishes, you can use this method to play the sound in a new gameobject that will be destroyed after the sound finishes playing
    public void RequestOneShotSound(AudioClip clip, AudioMixerGroup mixerGroup = null)
    {
        if (!mixerGroup) { Debug.LogWarning($"Mixer group not specified. Using default mixer group."); mixerGroup = masterMixerGroup; }

        GameObject soundObj = new GameObject("OneShotSound");
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.PlayOneShot(clip);

        Destroy(soundObj, clip.length);
    }
}
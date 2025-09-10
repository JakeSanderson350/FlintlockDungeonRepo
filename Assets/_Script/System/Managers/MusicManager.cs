using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioMixerGroup musicMixer;
    [SerializeField] AudioClip defaultMusicClip;
    AudioSource activeTrack;
    AudioSource fadeTrack;
    AudioSource introTrack;
    const float FADE_DURATION = 0.5f;

    private void OnEnable()
    {
        EventManager.onSceneLoaded += PlayOnAwake;
    }
    private void OnDisable()
    {
        EventManager.onSceneLoaded -= PlayOnAwake;
    }

    void PlayOnAwake(LevelData levelData)
    {
        if (introTrack != null)
            KillTrack(introTrack);

        if (activeTrack != null)
                StopMusic();

        if(levelData.playOnSceneLoad)
            PlayMusic(levelData.soundtrack);
    }

    public void PlayMusic(AudioClip music)
    {
        if(activeTrack != null)
            return;

        if (introTrack != null) 
            StartCoroutine(FadeTrack(introTrack, FADE_DURATION));

        activeTrack = AudioPlayer.PlaySFXLoop(music, transform, 1);
        activeTrack.spatialBlend = 0;
        activeTrack.outputAudioMixerGroup = musicMixer;
        activeTrack.Play();
    }

    public void StopMusic()
    {
        if(fadeTrack != null) 
            KillTrack(fadeTrack);

        if (activeTrack == null)
            return;

        fadeTrack = activeTrack;
        activeTrack = null;
        StartCoroutine(FadeTrack(fadeTrack));
    }

    IEnumerator FadeTrack(AudioSource track, float duration = 1f)
    {
        float tick = 0.02f;
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(tick);
        float countdown = duration;

        while (countdown > 0f && track != null)
        {
            countdown -= tick;
            track.volume = Mathf.InverseLerp(0f, duration, countdown);
            yield return delay;
        }

        if (fadeTrack != null)
            KillTrack(fadeTrack);

        fadeTrack = null;
    }

    void KillTrack(AudioSource track)
    {
        track.Stop();
        Destroy(track.gameObject);
    }
}

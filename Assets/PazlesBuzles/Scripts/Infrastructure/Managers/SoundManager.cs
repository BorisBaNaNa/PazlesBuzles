using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour, IService
{
    public float SoundVolume
    {
        get
        {
            MainMixer.GetFloat(SoundVolParametr, out var decibelsVal);
            return Mathf.Pow(10, decibelsVal / _decibelMultiplier);
        }

        set
        {
            float decibelsVal = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1)) * _decibelMultiplier;
            MainMixer.SetFloat(SoundVolParametr, decibelsVal);
        }
    }

    public float MusicVolume
    {
        get
        {
            MainMixer.GetFloat(MusicVolParametr, out var decibelsVal);
            return Mathf.Pow(10, decibelsVal / _decibelMultiplier);
        }

        set
        {
            float decibelsVal = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1)) * _decibelMultiplier;
            MainMixer.SetFloat(MusicVolParametr, decibelsVal);
        }
    }

    [Header("MusicSettings")]
    public AudioClip MainMenuClip;
    public List<AudioClip> MusicClips;
    public AudioClip clickSound;
    public AudioClip DropPieceSound;

    [Header("Mixer settings")]
    [SerializeField]
    private AudioMixer MainMixer;
    [SerializeField]
    public string MusicVolParametr = "MasterVol";
    [SerializeField]
    private AudioSource MusicSource;
    [SerializeField]
    public string SoundVolParametr = "MasterVol";
    [SerializeField]
    private AudioSource SoundSource;

    private Coroutine _playMusicCoroutine;
    private const float _decibelMultiplier = 20f;
    private bool _isPlayingAllClips;
    private int _curMusicId = 0;

    public static void PlaySfx_(AudioClip clip, AudioSource audioOut = null)
        => AllServices.GetService<SoundManager>().PlaySfx(clip, audioOut);

    public static void PlayMusic_(AudioClip clip, AudioSource audioOut = null)
        => AllServices.GetService<SoundManager>().PlayMusic(clip, audioOut);

    public static void PlayMainMenuClip_()
        => AllServices.GetService<SoundManager>().PlayMainMenuClip();

    public static void PlayAllMusicClips_()
        => AllServices.GetService<SoundManager>().PlayAllMusicClips();

    public void Init()
    {
        if (MusicSource == null || SoundSource == null)
        {
            MusicSource = gameObject.AddComponent<AudioSource>();
            SoundSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("Sound is not initialized!");
        }

        AllServices.RegisterService(this);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySfx(AudioClip clip, AudioSource audioOut = null)
    {
        audioOut ??= SoundSource;
        PlaySound(clip, audioOut);
    }

    public void PlayMusic(AudioClip clip, AudioSource audioOut = null)
    {
        audioOut ??= MusicSource;
        PlaySound(clip, audioOut);
    }

    public void PlayMainMenuClip()
    {
        if (_isPlayingAllClips)
            StopCoroutine(_playMusicCoroutine);

        MusicSource.loop = true;
        PlayMusic(MainMenuClip);
    }

    public void PlayAllMusicClips()
    {
        if (_isPlayingAllClips)
            return;

        MusicSource.loop = false;
        _isPlayingAllClips = true;
        _playMusicCoroutine = StartCoroutine(AllMusicPlay());
    }

    public void PlayClickBtn()
        => PlaySfx(clickSound);

    public void PlayDropPiece()
        => PlaySfx(DropPieceSound);

    private void PlaySound(AudioClip clip, AudioSource audioOut)
    {
        if (clip == null)
            return;

        if (audioOut == MusicSource)
        {
            audioOut.clip = clip;
            audioOut.Play();
        }
        else
            audioOut.PlayOneShot(clip);
    }

    private IEnumerator AllMusicPlay()
    {
        if (MusicClips.Count == 0)
            yield break;
        MixMusicClips();

        while (true)
        {
            AudioClip curClip = MusicClips[_curMusicId];
            PlayMusic(curClip);
            yield return new WaitForSeconds(curClip.length);

            if (++_curMusicId == MusicClips.Count)
            {
                MixMusicClips();
                _curMusicId = 0;
            }
        }
    }

    private void MixMusicClips()
    {
        for (int i = 0; i < MusicClips.Count - 1; i++)
        {
            int randId = Random.Range(i + 1, MusicClips.Count);
            AudioClip randClip = MusicClips[randId];
            MusicClips[randId] = MusicClips[i];
            MusicClips[i] = randClip;
        }
    }
}
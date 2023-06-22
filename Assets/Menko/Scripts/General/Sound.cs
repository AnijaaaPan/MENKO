using UnityEngine;
using System.Threading.Tasks;

public class Sound : MonoBehaviour
{
    public static Sound instance;

    public AudioSource TitleSource;
    public AudioSource PlayGame;
    public AudioSource PlayGameKugumori;
    public AudioSource SoundEffect;

    public AudioClip Faa;
    public AudioClip HitToBowl;
    public AudioClip Hyoushigi;
    public AudioClip Iyoh;
    public AudioClip Nudge;
    public AudioClip Ochinchin;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        TitleSource.volume = 1;
    }

    public async void UpdateBGM(bool IsPLaying)
    {
        if (TitleSource != null) TitleSource.volume = 0;
        for (int i = 0; i < 100; i++)
        {
            if (PlayGameKugumori != null) PlayGameKugumori.volume += IsPLaying == false ? 0.01f : -0.01f;
            if (PlayGame != null) PlayGame.volume += IsPLaying == true ? 0.01f : -0.01f;
            await Task.Delay(10);
        }
    }

    public void SoundFaa()
    {
        SoundEffect.PlayOneShot(Faa);
    }

    public void SoundHitToBowl()
    {
        SoundEffect.PlayOneShot(HitToBowl);
    }

    public void SoundHyoushigi()
    {
        SoundEffect.PlayOneShot(Hyoushigi);
    }

    public void SoundIyoh()
    {
        SoundEffect.PlayOneShot(Iyoh);
    }

    public void SoundNudge()
    {
        SoundEffect.PlayOneShot(Nudge);
    }

    public void SoundOchinchin()
    {
        SoundEffect.PlayOneShot(Ochinchin);
    }
}

using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        // Müzik ve Sabit 2D sesler (Loop gerekenler) için baþlangýçta Source oluþturuyoruz
        foreach (Sound s in sounds)
        {
            // Sadece Loop olacak veya 2D müzik olacaksa Source'u baþtan oluþturup saklýyoruz.
            // Anlýk efektler (silah, çýðlýk) için buna gerek yok, onlarý aþaðýda dinamik oluþturacaðýz.
            if (s.loop || s.spatialBlend == 0)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clips.Length > 0 ? s.clips[0] : null;
                s.source.outputAudioMixerGroup = s.mixerGroup;
                s.source.loop = s.loop;
                s.source.spatialBlend = s.spatialBlend;
            }
        }
    }

    /// <summary>
    /// EN ÖNEMLÝ FONKSÝYON: Ýster 2D, Ýster 3D ses çal.
    /// Eðer pozisyon (null) verirsen 2D çalar. Pozisyon verirsen orada 3D çalar.
    /// </summary>
    public void Play(string name, Vector3? position = null)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clips.Length == 0)
        {
            Debug.LogWarning("Ses veya Klip bulunamadý: " + name);
            return;
        }

        // 1. Rastgelelik Hesapla (Hangi klip? Hangi ton?)
        AudioClip selectedClip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];
        float finalVolume = s.volume + UnityEngine.Random.Range(-s.volumeVariance, s.volumeVariance);
        float finalPitch = s.pitch + UnityEngine.Random.Range(-s.pitchVariance, s.pitchVariance);

        // 2. Duruma Göre Çal

        // DURUM A: Eðer bu bir arka plan müziðiyse (Loop varsa veya SpatialBlend 0 ise)
        // Kendi üzerindeki sabit Source'u kullanýr.
        if (s.loop || s.spatialBlend == 0)
        {
            s.source.clip = selectedClip;
            s.source.volume = finalVolume;
            s.source.pitch = finalPitch;
            if (!s.source.isPlaying) s.source.Play();
            return;
        }

        // DURUM B: Anlýk Efekt (Silah, Çýðlýk, Adým Sesi)
        // Geçici bir obje oluþturup sesi orada çalýp yok ederiz.
        GameObject tempGO = new GameObject("TempAudio_" + name);

        // Pozisyon verildi mi? Verildiyse oraya git, verilmediyse Manager'ýn üstünde kal.
        if (position.HasValue)
            tempGO.transform.position = position.Value;
        else
            tempGO.transform.position = Camera.main.transform.position; // Oyuncunun kafasýnda çal (2D gibi hissettirir)

        AudioSource aSource = tempGO.AddComponent<AudioSource>();

        aSource.clip = selectedClip;
        aSource.outputAudioMixerGroup = s.mixerGroup;
        aSource.volume = finalVolume;
        aSource.pitch = finalPitch;
        aSource.spatialBlend = s.spatialBlend;
        aSource.minDistance = s.minDistance;
        aSource.maxDistance = s.maxDistance;
        aSource.rolloffMode = AudioRolloffMode.Logarithmic;

        aSource.Play();
        Destroy(tempGO, selectedClip.length + 0.1f); // Ses bitince çöpü temizle
    }
}
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

        foreach (Sound s in sounds)
        {
            // Müzikler (Loop) VE Çakýþmasý Ýstenmeyen Sesler için
            if (s.loop || s.preventOverlap)
            {
                // HATALI OLAN ESKÝ KOD:
                // s.source = gameObject.AddComponent<AudioSource>(); 

                // --- DÜZELTÝLMÝÞ YENÝ KOD ---
                // 1. Yeni, boþ bir GameObject oluþtur
                GameObject soundObj = new GameObject("SoundSource_" + s.name);

                // 2. Bunu AudioManager'ýn evladý (Child) yap ki ortalýk daðýlmasýn
                soundObj.transform.SetParent(this.transform);

                // 3. AudioSource'u BU YENÝ OBJEYE ekle
                s.source = soundObj.AddComponent<AudioSource>();

                // 4. Ayarlarý yükle
                s.source.clip = s.clips.Length > 0 ? s.clips[0] : null;
                s.source.outputAudioMixerGroup = s.mixerGroup;
                s.source.loop = s.loop;
                s.source.spatialBlend = s.spatialBlend;
                s.source.minDistance = s.minDistance;
                s.source.maxDistance = s.maxDistance;
                s.source.rolloffMode = AudioRolloffMode.Logarithmic;
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
        if (s == null || s.clips.Length == 0) return;

        // Varyasyon hesaplamalarý
        AudioClip selectedClip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];
        float finalVolume = s.volume + UnityEngine.Random.Range(-s.volumeVariance, s.volumeVariance);
        float finalPitch = s.pitch + UnityEngine.Random.Range(-s.pitchVariance, s.pitchVariance);

        // --- SENARYO 1: Tek Ses Olmasý Gerekenler (Müzik veya Kalp Atýþý) ---
        if (s.loop || s.preventOverlap)
        {
            if (s.source == null) return;

            // ÖNEMLÝ KISIM: Zaten çalýyorsa ne yapalým?
            if (s.source.isPlaying)
            {
                // Eðer bu bir 'Loop' deðilse ama 'Overlap' istenmiyorsa (Kalp atýþý gibi)
                // ve þu an zaten çalýyorsa -> HÝÇBÝR ÞEY YAPMA, ÇIK.
                if (s.preventOverlap && !s.loop) return;

                // Müzikse zaten devam etsin.
                if (s.loop) return;
            }

            // Çalmýyorsa baþlat
            s.source.clip = selectedClip;
            s.source.volume = finalVolume;
            s.source.pitch = finalPitch;
            // Pozisyon güncelle (Kalp atýþý oyuncuyu takip etsin diye 2D çalabilir veya konumu güncellenebilir)
            if (position.HasValue) s.source.transform.position = position.Value;

            s.source.Play();
            return;
        }



        // --- SENARYO 2: Üst Üste Binebilenler (Silah, Patlama) ---
        GameObject tempGO = new GameObject("TempAudio_" + name);
        if (position.HasValue) tempGO.transform.position = position.Value;
        else tempGO.transform.position = Camera.main.transform.position;

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
        Destroy(tempGO, selectedClip.length + 0.1f);
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Durdurulacak ses bulunamadý: " + name);
            return;
        }

        // Sadece kendi Source'u olan (Loop/PreventOverlap) sesleri durdurabiliriz.
        // Anlýk efektler (Silah gibi) zaten bitince yok oluyor.
        if (s.source != null)
        {
            s.source.Stop();
        }
    }
    public void StopWithFade(string name, float duration = 1f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null && s.source != null && s.source.isPlaying)
        {
            StartCoroutine(FadeOutCoroutine(s, duration));
        }
    }

    private System.Collections.IEnumerator FadeOutCoroutine(Sound s, float duration)
    {
        float startVolume = s.source.volume;

        while (s.source.volume > 0)
        {
            // Sesi yavaþ yavaþ kýs
            s.source.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        s.source.Stop();
        s.source.volume = startVolume; // Bir sonraki sefer için sesi eski haline getir
    }
}
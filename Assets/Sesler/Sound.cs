using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name; // Sesin kod içindeki adý (örn: "ZombieIdle")

    [Header("Audio Dosyalarý")]
    [Tooltip("Birden fazla eklersen rastgele birini seçer.")]
    public AudioClip[] clips; // ARTIK TEK DEÐÝL, DÝZÝ (ARRAY)

    [Header("Temel Ayarlar")]
    public AudioMixerGroup mixerGroup; // SFX, Music vs.
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop; // Sadece Müzik/Ambiyans için (PlaySound ile çalýþýr, PlayClipAtPoint ile çalýþmaz)

    [Header("Rastgelelik (Varyasyon)")]
    [Tooltip("Sesi her çaldýðýnda ses þiddeti bu kadar azalýp artabilir.")]
    [Range(0f, 0.5f)] public float volumeVariance = 0.1f;
    [Tooltip("Sesi her çaldýðýnda tonu bu kadar deðiþebilir. (Robotik sesi önler)")]
    [Range(0f, 0.5f)] public float pitchVariance = 0.1f;

    [Header("3D Ayarlarý")]
    [Range(0f, 1f)] public float spatialBlend = 1f; // 0=2D, 1=3D
    public float minDistance = 5f;
    public float maxDistance = 25f;

    [HideInInspector]
    public AudioSource source; // Müzik gibi sabit kaynaklar için
}
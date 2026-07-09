using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class scan : MonoBehaviour
{
    // Inspector'dan sürüklediðin Prefab listesi (Asýl Belgeler)
    [SerializeField] List<GameObject> scanObject = new();

    [SerializeField] public float duration = 10;
    [SerializeField] public float size = 5;
    [SerializeField] public float simSpeed = 1;
    [SerializeField] public List<Collider> colliders = new();

    // DÝKKAT: Start fonksiyonunu sildik! 
    // Çünkü oyun baþlar baþlamaz Prefab dosyalarýna dokunmamalýyýz.

    // Dalga oluþturma fonksiyonu
    public void StartWave(float? duration = null, float? size = null, float? simSpeed = null, Vector3? position = null, int waveIndex = 0, bool TriggersEnabled = true)
    {
        // --- 1. GÜVENLÝK KONTROLÜ (Hata vermemesi için) ---
        // Eðer liste boþsa veya istenen sýra (index) listede yoksa oyunu çökertme.
        if (scanObject == null || scanObject.Count == 0)
        {
            Debug.LogError("Scan Object listesi boþ!");
            return;
        }
        if (waveIndex >= scanObject.Count || waveIndex < 0)
        {
            Debug.LogWarning("Ýstenen waveIndex listede yok, 0. eleman kullanýlýyor.");
            waveIndex = 0;
        }
        // ----------------------------------------------------


        // Pozisyon belirleme
        Vector3 spawnPos = (position != null) ? (Vector3)position : transform.position;

        // --- 2. FOTOKOPÝYÝ ÇEKME (Instantiate) ---
        // Asýl dosyadan (Prefab) sahnede canlý bir kopya (Clone) oluþturuyoruz.
        // terrainscanner deðiþkeni artýk sahnede duran CANLI bir objedir.
        GameObject terrainscanner = Instantiate(scanObject[waveIndex], spawnPos, quaternion.identity) as GameObject;


        // --- 3. REFERANS ATAMA (Asýl Çözüm Burasý) ---
        // Yeni oluþturduðumuz bu canlý objenin içindeki scripti buluyoruz.
        var detector = terrainscanner.GetComponentInChildren<scandetector>();

        // Eðer o script varsa, içindeki "scan" deðiþkenine "BENÝ" (this) ata.
        // Artýk o obje, bu scripti tanýyor.
        if (detector != null)
        {
            detector.scan = this;
        }


        // --- 4. PARTICLE AYARLARI (Geri kalan iþlemler) ---
        ParticleSystem psys = terrainscanner.GetComponentInChildren<ParticleSystem>();

        if (psys != null)
        {
            var a = psys.main;
            if (TriggersEnabled)
            {
                // Listede boþ collider varsa hata vermesin diye kontrol ekledik
                colliders.ForEach(col =>
                {
                    if (col != null) psys.trigger.AddCollider(col);
                });
            }

            // Süre ve boyut ayarlamalarý...
            // (Eðer parametre gelmediyse kendi ayarlarýmýzý kullanýyoruz)
            a.startLifetime = (duration != null) ? (float)duration : this.duration;
            a.startSize = (size != null) ? (float)size : this.size;
            a.simulationSpeed = (simSpeed != null) ? (float)simSpeed : this.simSpeed;
        }

        // --- 5. TEMÝZLÝK ---
        // Ýþi biten objeyi yok etme süresi
        float destroyTime = (duration != null) ? (float)duration : this.duration;
        Destroy(terrainscanner, destroyTime + 1);
    }
}
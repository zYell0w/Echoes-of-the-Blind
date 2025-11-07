using UnityEngine;
using System.Collections;

public class WaveController : MonoBehaviour
{
    [Header("Wave Settings")]
    public float waveSpeed = 5f;
    public float waveDuration = 3f;
    public float waveIntensity = 1f;
    public KeyCode waveKey = KeyCode.Space;
    
    [Header("Visual Settings")]
    public Color outlineColor = Color.white;
    public float outlineWidth = 0.02f;
    
    [Header("Afterglow Settings")]
    public float afterglowIntensity = 0.5f;
    public float afterglowDuration = 1.0f;
    public Color afterglowColor = new Color(0.8f, 0.9f, 1.0f, 1f);
    public float afterglowWidth = 0.05f;
    
    [Header("Advanced Settings")]
    public float waveFalloff = 2.0f;
    
    private Material waveMaterial;
    private Coroutine currentWave;
    private float waveTime = -100f;

    void Start()
    {
        // Create material instance
        waveMaterial = new Material(Shader.Find("Custom/OutlineWave"));
        ApplyMaterialToScene();
        
        // Set initial properties
        UpdateShaderProperties();
    }

    void Update()
    {
        if (Input.GetKeyDown(waveKey))
        {
            StartWave();
        }
        
        // Update wave time in shader
        if (waveMaterial != null)
        {
            waveMaterial.SetFloat("_WaveTime", waveTime);
            waveTime += Time.deltaTime;
        }
    }

    void UpdateShaderProperties()
    {
        if (waveMaterial == null) return;
        
        waveMaterial.SetColor("_OutlineColor", outlineColor);
        waveMaterial.SetFloat("_OutlineWidth", outlineWidth);
        waveMaterial.SetFloat("_WaveSpeed", waveSpeed);
        waveMaterial.SetFloat("_WaveFalloff", waveFalloff);
        waveMaterial.SetFloat("_AfterglowIntensity", afterglowIntensity);
        waveMaterial.SetFloat("_AfterglowDuration", afterglowDuration);
        waveMaterial.SetColor("_AfterglowColor", afterglowColor);
        waveMaterial.SetFloat("_AfterglowWidth", afterglowWidth);
    }

    void StartWave()
    {
        // Stop existing wave if running
        if (currentWave != null)
        {
            StopCoroutine(currentWave);
        }
        currentWave = StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        // Set wave origin to player position
        Vector4 waveOrigin = new Vector4(transform.position.x, transform.position.y, transform.position.z, 0f);
        waveMaterial.SetVector("_WaveOrigin", waveOrigin);
        waveMaterial.SetFloat("_WaveIntensity", waveIntensity);
        
        // Reset wave time
        waveTime = 0f;
        
        // Wait for wave to complete
        yield return new WaitForSeconds(waveDuration);
        
        // Fade out wave intensity
        float fadeTime = 1f;
        float startIntensity = waveIntensity;
        float timer = 0f;
        
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float currentIntensity = Mathf.Lerp(startIntensity, 0f, timer / fadeTime);
            waveMaterial.SetFloat("_WaveIntensity", currentIntensity);
            yield return null;
        }
        
        waveMaterial.SetFloat("_WaveIntensity", 0f);
        currentWave = null;
    }

    void ApplyMaterialToScene()
    {
        // Apply material to all renderers in scene
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            // Skip UI and special objects
            if (renderer.gameObject.CompareTag("NoOutline") || 
                renderer.GetComponent<CanvasRenderer>() != null)
                continue;
                
            renderer.material = waveMaterial;
        }
    }

    void OnValidate()
    {
        // Update shader properties when inspector values change
        if (Application.isPlaying && waveMaterial != null)
        {
            UpdateShaderProperties();
        }
    }

    void OnDestroy()
    {
        if (waveMaterial != null)
        {
            DestroyImmediate(waveMaterial);
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

public class scandetector : MonoBehaviour
{
    ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger()
    {
        // Get particles that entered a trigger this frame
        List<ParticleSystem.Particle> enterParticles = new List<ParticleSystem.Particle>();
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles, out ParticleSystem.ColliderData colliderData);

        // Iterate through the particles that entered a trigger
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enterParticles[i];

            // Check how many colliders this particle interacted with
            int colliderCount = colliderData.GetColliderCount(i);
            for (int j = 0; j < colliderCount; j++)
            {
                // Get the specific collider for this particle
                Collider triggeredCollider = (Collider)colliderData.GetCollider(i, j);

                // Now you can use the collider's GameObject
                GameObject triggeredObject = triggeredCollider.gameObject;
                triggeredObject.GetComponent<scanlistener>().ScanDetected();
                Debug.LogError("yo");
                
            }
            enterParticles[i] = p; // Save the modified particle
        }

        // Apply the modified particles back to the system
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles);
    }
}
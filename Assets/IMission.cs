using UnityEngine;

public interface IMission : Iscanlistener , IInteractable
{
    abstract bool IsDone();
    //azaltma için negatif
    abstract void SetCompletion(float degreeOutOf100);

    abstract Vector3 GetSpawnPointForEnemy();

    public new void ScanDetected(Vector3? scanLocation = null, scan scan = null)
    {
        MonoBehaviour mono = this as MonoBehaviour;
        if (mono != null)
        {
            if (scan != null)
            {
                scan.StartWave(position: mono.transform.position, size: 2, TriggersEnabled: false, waveIndex: 3);
            }
        }
        

    }

}
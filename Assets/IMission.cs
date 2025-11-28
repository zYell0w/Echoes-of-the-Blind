using UnityEngine;

public interface IMission : Iscanlistener , IInteractable
{
    abstract bool IsDone();
    //azaltma için negatif
    abstract void SetCompletion(float degreeOutOf100);

    abstract Vector3 GetSpawnPointForEnemy();

 
}
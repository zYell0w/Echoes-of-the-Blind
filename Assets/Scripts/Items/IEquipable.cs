
using UnityEngine;

interface IEquipable
{
    

    abstract void Equip(Player interactee);
    abstract void Drop(Player interactee);

    
}
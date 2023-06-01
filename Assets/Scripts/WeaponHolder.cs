using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] Gun gun;

    public void Fire()
    {
        gun.Fire();
    }

   /* List<Gun> gunList =  new List<Gun>();
    public void Swap(int index)
    {
        gun = gunList[index];
    }

    public void GetWeapon(Gun gun)
    {
        gunList.Add(gun);
    }*/
}

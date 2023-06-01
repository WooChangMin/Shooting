using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] Rig aimRig; 
    [SerializeField] float reloadTime;
    [SerializeField] WeaponHolder weaponHolder;

    private Animator anim;

    private bool isReloading;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        // 직렬화 하지않고 weaponHolder를 직접받아와도 됨
    
    }
    private void OnReload(InputValue value)
    {
        if (isReloading)
            return;

        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        anim.SetTrigger("Reload");
        isReloading = true;
        aimRig.weight = 0f;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        aimRig.weight = 1f;
    }

    private void OnFire(InputValue value)
    {
        if (isReloading)
            return;
        Fire();
    }

    private void Fire()
    {
        weaponHolder.Fire();
        anim.SetTrigger("Fire");
    }
}           

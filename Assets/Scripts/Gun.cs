using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] float maxDistance;
    [SerializeField] int damage;
    [SerializeField] float bulletSpeed;


    public void Fire()
    {
        muzzleEffect.Play();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            IHitable hittable = hit.transform.GetComponent<IHitable>();
            ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            effect.transform.parent = hit.transform;
            Destroy(effect.gameObject, 3f);

            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, hit.point));

            hittable?.Hit(hit, damage);  //널 조건 연산자 밑의 두줄과 같은 코드임 
            /*if (hittable != null)
                hittable.Hit(hit,damage);*/
        }
        else
        {
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));
        }
    }
    IEnumerator TrailRoutine( Vector3 startPoint, Vector3 endPoint)
    {
        TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);  //identity -> 회전없이

        float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed;

        float time = 0;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPoint, endPoint, time);
            time += Time.deltaTime / totalTime;

            yield return null;
        }
        Destroy(trail.gameObject, 3f);
    }
}

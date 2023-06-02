using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] int damage;


    public void Fire()
    {
        muzzleEffect.Play();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            
            IHitable hittable = hit.transform.GetComponent<IHitable>();
            //ParticleSystem effect = GameManager.Resource.Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal),true);
            ParticleSystem effect = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/HitEffect", hit.point, Quaternion.LookRotation(hit.normal), true);
            // ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            effect.transform.parent = hit.transform;
            //Destroy(effect.gameObject, 3f);

            StartCoroutine(ReleaseRoutine(effect.gameObject));
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, hit.point));

            hittable?.Hit(hit, damage);  //�� ���� ������ ���� ���ٰ� ���� �ڵ��� 
            /*if (hittable != null)
                hittable.Hit(hit,damage);*/
        }
        else
        {
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));
        }
    }

    IEnumerator ReleaseRoutine(GameObject effect)
    {
        yield return new WaitForSeconds(3f);
        GameManager.Pool.Release(effect);
    }
    IEnumerator TrailRoutine( Vector3 startPoint, Vector3 endPoint)
    {
        //TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);  //identity -> ȸ������
        TrailRenderer trail = GameManager.Resource.Instantiate(bulletTrail, startPoint, Quaternion.identity, true);

        trail.Clear();

        float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed;

        float time = 0;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPoint, endPoint, time);
            time += Time.deltaTime / totalTime;

            yield return null;
        }
        //Destroy(trail.gameObject, 3f);
        GameManager.Resource.Destroy(trail.gameObject);

        /*yield return null;
        
        if (trail.IsValid())
        {
            Debug.Log("Ʈ������ ����.");
        }
        else
        {
            Debug.Log("Ʈ������ �ִ�.");
        }*/

        // ������Ʈ Ǯ���� ��� ���ӿ�����Ʈ�� ��Ȱ��ȭ�� ������ == null�� ���ӿ�����Ʈ�� �Ǻ��� ��ƴ�. -> (trail.gameObject.activeSelf == false || trail == null) Ȯ��޼��� ���

    }

}

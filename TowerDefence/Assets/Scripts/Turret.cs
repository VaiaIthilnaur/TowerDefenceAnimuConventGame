using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    private  Transform target;
    private EnemyMovement targetEnemy;


    [Header("General")]
    public float range = 15f;

    [Header("Use Bullets (defoult)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountDown = 0f;

    [Header("Use Lasers")]
    public bool useLaser = false;
    public LineRenderer lineRenderer;   
    public ParticleSystem laserImpactEffect;
    public Light laserImpactLight;
    public int damegeOverTime = 30;
    public float slowPct = .5f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    
    public Transform firePoint; 
   

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);   
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<EnemyMovement>();
        }
        else
        {
            target = null;
        }

    }
    void Update()
    {
        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    laserImpactEffect.Stop();
                    laserImpactLight.enabled = false;
                }
                    
            }

            return;
        }

        LockOnTarget();
        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (fireCountDown <= 0f)
            {
                Shoot();
                fireCountDown = 1f / fireRate;
            }
            fireCountDown -= Time.deltaTime;
        }

    }
    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
    void Laser()
    {
        targetEnemy.TakeDamage(damegeOverTime * Time.deltaTime);
        targetEnemy.Slow(slowPct);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            laserImpactEffect.Play();
            laserImpactLight.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1,target.position);

        Vector3 dir = firePoint.position - target.position;

        laserImpactEffect.transform.position = target.position+dir.normalized;

        laserImpactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }
    void Shoot()
    {
        GameObject bulletGo=(GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGo.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,range);
    }
}

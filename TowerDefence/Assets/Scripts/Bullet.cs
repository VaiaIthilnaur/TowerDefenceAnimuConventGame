using UnityEngine;

public class Bullet : MonoBehaviour {

    private Transform target;

    public float explosionRange = 0f;
    public float speed = 70f;
    public int damage = 50;
    public GameObject impactEffect;

    public void Seek(Transform _targer)
    {
        target = _targer;
    }
	
	void Update () {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized*distanceThisFrame,Space.World);
        transform.LookAt(target);

	}
    void HitTarget()
    {
        GameObject effectInst= (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectInst,5f);

        if (explosionRange > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }
        Destroy(gameObject);
    }
    void Damage(Transform enemy)
    {
        EnemyMovement e = enemy.GetComponent<EnemyMovement>();
        if (e != null)
        {
            e.TakeDamage(damage);
        }
    }
    void Explode()
    {
       Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

}

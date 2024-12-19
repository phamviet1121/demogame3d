using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBullet : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float explosionRadius;
    public float explosionForce;
    public float time;
    public float newtime = 10f;

    public int damage;

    private List<Health> oldvictins = new List<Health>();
    void Start()
    {
        time = newtime;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            explosion();
            Destroy(gameObject);

        }
        Debug.Log($"{time}");
    }
    public void OnCollisionEnter(Collision collision)
    {

        Invoke("explosion", 1f);
    }
    void explosion()
    {
        GameObject newboom = Instantiate(explosionPrefab, transform.position, transform.rotation);
        BlowObjects();
        Destroy(gameObject);


    }
    private void BlowObjects()
    {
        oldvictins.Clear();
        Collider[] affectedObjects = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < affectedObjects.Length; i++)
        {
            //Rigidbody rb = affectedObjects[i].attachedRigidbody;
            //if (rb)
            //{
            //    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.Impulse);
            //}

            DeliverDamage(affectedObjects[i]);
            AddForceToObject(affectedObjects[i]);

        }
    }
    private void DeliverDamage(Collider victim)
    {
        Health health = victim.GetComponentInParent<Health>();
        if (health != null && !oldvictins.Contains(health))
        {
            health.TakeDamage(damage);
            oldvictins.Add(health);
        }
    }
    private void AddForceToObject(Collider affecterObject)
    {
        Rigidbody rb = affecterObject.attachedRigidbody;
        if (rb)
        {
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.Impulse);
        }
    }
}

using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 40f;
    public float lifetime = 2f;
    public GameObject bulletHolePrefab;
    public GameObject impactEffectPrefab;
    public GameObject humanBulletHolePrefab;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Collider>().enabled = false;
        ContactPoint contactPoint = collision.contacts[0];

        if (collision.gameObject.tag == "Enemy")
        {
            SpawnHumanBulletHole(contactPoint);
        }
        else
        {
            SpawnBulletHole(contactPoint);
        }
        Destroy(gameObject);
    }
    void SpawnBulletHole(ContactPoint contactPoint)
    {
        Vector3 spawnPos = contactPoint.point + contactPoint.normal * 0.04f;
        Vector3 normal = contactPoint.normal;
        if(Vector3.Dot(transform.forward,contactPoint.normal) < 0)
        {
            normal = -normal;
        }
        Quaternion rot = Quaternion.LookRotation(normal);
        Instantiate(impactEffectPrefab, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
        GameObject hole = Instantiate(bulletHolePrefab, spawnPos, rot);
        hole.transform.SetParent(contactPoint.otherCollider.transform);
        Destroy(hole, 10f);
    }

    void SpawnHumanBulletHole(ContactPoint contactPoint)
    {
        Vector3 spawnPos = contactPoint.point + contactPoint.normal * 0.04f;
        Vector3 normal = contactPoint.normal;
        if (Vector3.Dot(transform.forward, contactPoint.normal) < 0)
        {
            normal = -normal;
        }
        
        Quaternion rot = Quaternion.LookRotation(normal);
        GameObject hole = Instantiate(humanBulletHolePrefab, spawnPos, rot);
        hole.transform.SetParent(contactPoint.otherCollider.transform);
        Destroy(hole, 10f);
    }

}

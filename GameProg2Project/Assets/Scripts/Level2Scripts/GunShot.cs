using UnityEngine;

public class GunShot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firepoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        Quaternion offset = Quaternion.Euler(0,90,0);
        
        Instantiate(bulletPrefab,firepoint.position, firepoint.rotation * offset);
    }
}

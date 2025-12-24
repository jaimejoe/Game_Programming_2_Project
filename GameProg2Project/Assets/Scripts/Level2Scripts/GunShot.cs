using UnityEngine;

public class GunShot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firepoint;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    // Update is called once per frame
    void Update()
    {
        bool IsPaused = GameManager.Instance.isPaused;
        if (Input.GetButtonDown("Fire1") && !GameManager.Instance.gameOver && !IsPaused)
        {
            //Debug.Log(!GameManager.Instance.gameOver);
            Shoot();
        }
    }
    void Shoot()
    {
        Quaternion offset = Quaternion.Euler(0,90,0);
        
        Instantiate(bulletPrefab,firepoint.position, firepoint.rotation * offset);
    }
}

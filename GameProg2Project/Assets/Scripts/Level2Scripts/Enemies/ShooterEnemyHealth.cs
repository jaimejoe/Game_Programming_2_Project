using UnityEngine;

public class ShooterEnemyHealth : MonoBehaviour
{
    public int HP = 100;
    public int MaxHP = 100;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("hit!");
        if (collision.gameObject.tag == "Laser")
        {
            dealDmg(20);
        }
    }
    void dealDmg(int damage)
    {
        //Debug.Log("Oww!");
        HP -= damage;
        if(HP < 0)
        {
            death();
        }
    }

    void death()
    {
        rb.freezeRotation = false;

    }
}

using UnityEngine;

public class ShooterEnemyHealth : MonoBehaviour
{
    public int HP = 100;
    public int MaxHP = 100;
    private Rigidbody rb;
    Animator anim;
    public GameObject hips;
    bool isAlive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        GameManager.Instance.SpawnedEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isAlive) return;
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
        if(HP <= 0)
        {
            death();
        }
    }

    void death()
    {
        //Quaternion targetRotation = Quaternion.Euler(90, transform.rotation.y, transform.rotation.z);

        //Quaternion targetRotation1 = Quaternion.Euler(hips.transform.position.x+70, hips.transform.position.y, hips.transform.position.z);

        //rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, 90 * Time.deltaTime));
        //transform.rotation = targetRotation;

        //hips.transform.rotation = targetRotation1;
        //anim.enabled = false;
        isAlive = false;
        GameManager.Instance.DeadEnemy();

    }
    public int GetHealth()
    {
        //Debug.Log(HP);
         
        return HP;
    }
}

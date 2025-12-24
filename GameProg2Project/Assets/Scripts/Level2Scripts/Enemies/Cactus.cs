using UnityEngine;

public class Cactus : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Collider>().enabled = false;
        ContactPoint contactPoint = collision.contacts[0];

        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.HealthDecrease(15);
            //Debug.Log("awesome ya got hit");
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject,5f);
        }
        
    }
}

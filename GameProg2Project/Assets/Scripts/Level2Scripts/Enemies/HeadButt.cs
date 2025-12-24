using UnityEngine;

public class HeadButt : MonoBehaviour
{
    bool ischassing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ischassing = gameObject.transform.parent.gameObject.GetComponent<EnemyPatrol>().isChasing;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Player" && !ischassing)
        {
            GameManager.Instance.HealthDecrease(25);
        }
        else
        {

        }
    }
}

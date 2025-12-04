using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] points; //patrol points
    public float speed = 2f;
    private int index = 0;
    public Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.freezeRotation == true)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                points[index].position,
                speed * Time.deltaTime
                );
            //transform.LookAt(points[index].position);
            
            if (Vector3.Distance(transform.position, points[index].position) < 0.2f)
            {
                index = (index + 1) % points.Length;
            }
        }
    }
}

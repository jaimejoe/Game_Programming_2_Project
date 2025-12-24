using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;  

    [Header("Hover Settings")]
    public float hoverHeight = 0.5f;   
    public float hoverSpeed = 2f;      

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; 
    }

    void Update()
    {
        
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        
        float newY = startPos.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

}

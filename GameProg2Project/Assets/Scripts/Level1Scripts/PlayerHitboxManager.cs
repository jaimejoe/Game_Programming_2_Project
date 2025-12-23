using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerHitboxManager : MonoBehaviour
{
    public Collider[] attackColliders;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Collider attackCollider in attackColliders)
        {
            attackCollider.enabled = false;
        }
    }

    public void EnableHitbox()
    {
        foreach (Collider attackCollider in attackColliders)
        {
            attackCollider.enabled = true;
        }
    }

    public void DisableHitbox()
    {
        foreach (Collider attackCollider in attackColliders)
        {
            attackCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    public float cooldownTime = 1f;
    public int noOfClicks = 0;
    float lastClickedTime;
    float maxComboDelay = 1;
    private bool comboContinue = false;
    private float cooldownEnd = 0f;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Combo();
        }
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
    }

    void Combo()
    {
        lastClickedTime = Time.time;

        if (Time.time < cooldownEnd)
            return;

        if (noOfClicks == 0)
        {
            noOfClicks = 1;
            comboContinue = false;
            anim.Play("Punch1", 0, 0f);
            return;
        }

        if (!comboContinue)
            return;  

        comboContinue = false; 
        noOfClicks++;

        if (noOfClicks == 2)
        {
            anim.Play("Punch2", 0, 0f);
        }
        else if (noOfClicks == 3)
        {
            anim.Play("FlyingKick", 0, 0f);
        }

        if (noOfClicks >= 3)
        {
            noOfClicks = 0;
            cooldownEnd = Time.time + cooldownTime;
        }

    }

    public void ComboWindow()
    {
        comboContinue = true;
    }

}


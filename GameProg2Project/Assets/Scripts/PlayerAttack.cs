using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum ComboState
{
    NONE,
    PUNCH_1,
    PUNCH_2,
    FLYING_KICK
}
public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    public float cooldownTime = 2f;
    public static int noOfClicks = 0;
    private float nextFireTime = 0f;
    float lastClickedTime = 0;
    float maxComboDelay = 1;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Punch1"))
        {
            anim.SetBool("Punch1", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Punch2"))
        {
            anim.SetBool("Punch2", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("FlyingKick"))
        {
            anim.SetBool("FlyingKick", false);
            noOfClicks = 0;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }

        void OnClick()
        {
            lastClickedTime = Time.time;
            noOfClicks++;
            if (noOfClicks == 1)
            {
                anim.SetBool("Punch1", true);
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

            if (noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Punch1"))
            {
                anim.SetBool("Punch1", false);
                anim.SetBool("Punch2", true);
            }

            if (noOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Punch2"))
            {
                anim.SetBool("Punch2", false);
                anim.SetBool("FlyingKick", true);
            }
        }
    }
}

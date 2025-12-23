using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    public float maxComboDelay = 1f;
    public float cooldownTime = 1f;
    private int noOfClicks = 0;
    private float lastClickedTime = 0f;
    private float cooldownEnd = 0f;
    private bool comboContinue = false;

    public float dashingTime = 5f;       
    public float dashingPower = 20f;         
    public float dashingCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetMouseButtonDown(0) && !isDashing)
        {
            Combo();
        }
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
    }

    private void Combo()
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


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;


        anim.Play("FlyingKick", 0, 0f);

        Vector3 targetPos = rb.position + transform.forward * dashingTime;

        float remaining = dashingTime;

        while (remaining > 0f)
        {
            float length = dashingPower * Time.fixedDeltaTime;
            Vector3 newPos = Vector3.MoveTowards(rb.position, targetPos, length);
            rb.MovePosition(newPos);
            remaining -= length;
            yield return new WaitForFixedUpdate();
        }


        rb.MovePosition(targetPos);

        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}

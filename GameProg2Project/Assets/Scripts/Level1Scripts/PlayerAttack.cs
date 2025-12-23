using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    private PlayerController playerController;

    public float maxComboDelay = 1f;
    private int noOfClicks = 0;
    private float lastClickedTime = 0f;
    private float comboCooldownEnd = 0f;
    private bool comboContinue = false;
    private float cooldownTime = 1f;

    public float dashingTime = 5f;
    public float dashingPower = 20f;
    public float dashingCooldown = 10f;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashCooldownEndTime = 0f;

    // Add timer to auto-end attack if combo doesn't continue
    private float attackEndTimer = 0f;
    private bool waitingForCombo = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!canDash && Time.time >= dashCooldownEndTime)
        {
            canDash = true;
        }

        // Auto-end attack if combo window expires
        if (waitingForCombo && Time.time >= attackEndTimer)
        {
            playerController.EndAttack();
            waitingForCombo = false;
            noOfClicks = 0;
        }

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
            if (playerController.IsAttacking)
            {
                playerController.EndAttack();
            }
        }
    }

    private void Combo()
    {
        lastClickedTime = Time.time;

        if (Time.time < comboCooldownEnd)
            return;

        if (noOfClicks == 0)
        {
            noOfClicks = 1;
            comboContinue = false;
            anim.Play("Punch1", 0, 0f);
            playerController.StartAttack();

            // Set timer to auto-end attack if no combo follows
            waitingForCombo = true;
            attackEndTimer = Time.time + maxComboDelay;
            return;
        }

        if (!comboContinue)
            return;

        comboContinue = false;
        noOfClicks++;

        // Reset the combo window timer
        waitingForCombo = true;
        attackEndTimer = Time.time + maxComboDelay;

        if (noOfClicks == 2)
        {
            anim.Play("Punch2", 0, 0f);
            playerController.StartAttack();
        }
        else if (noOfClicks == 3)
        {
            anim.Play("FlyingKick", 0, 0f);
            playerController.StartAttack();
        }

        if (noOfClicks >= 3)
        {
            noOfClicks = 0;
            comboCooldownEnd = Time.time + cooldownTime;
        }
    }

    // Call this from ANIMATION EVENT at the end of each attack animation
    public void OnAttackAnimationEnd()
    {
        // Don't end attack if we're waiting for combo continuation
        if (!waitingForCombo)
        {
            playerController.EndAttack();
        }
    }

    // Call this from ANIMATION EVENT at the end of FlyingKick
    public void OnFlyingKickEnd()
    {
        playerController.EndAttack();
        waitingForCombo = false;
        noOfClicks = 0;
    }

    public void ComboWindow()
    {
        comboContinue = true;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        playerController.StartDash();

        dashCooldownEndTime = Time.time + dashingCooldown;

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
        playerController.EndDash();
    }
}
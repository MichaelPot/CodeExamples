using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam, groundCheck;
    public LayerMask envMask;

    public int jumps = 0;
    public float moveSpeed = 6;
    public float jumpH = 5;
    public float gravity = -9.81f;
    public float turnSmoothTime = .1f;
    public float groundDist = .2f;
    public bool isGrounded;
    public float dashTime = .1f;
    public bool isSphere = false;

    float jumpMult = 1;
    float dashCd = 10;
    Rigidbody rb;
    float turnSmoothVelocity;
    Vector3 ySpeed;
    int currJumps = 0;
    float count = 0;
    bool jumped;
    float timeSinceLanding = 0, timeSinceJump = 0;
    bool inAir = false;
    Vector3 hitNormal;
    Vector3 moveDirection;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    ///  PROBABLY PUT THIS IS FIXED UPDATE \\\
    void Update()
    {
        dashCd += Time.deltaTime;
        timeSinceLanding += Time.deltaTime;
        timeSinceJump += Time.deltaTime;
        isGrounded = controller.isGrounded;//|| Physics.CheckSphere(groundCheck.position, groundDist, envMask);
        if (isGrounded && ySpeed.y < 0)
        {
            jumped = false;
            //ySpeed.y = 0;
            ySpeed.y = -.5f;
            controller.stepOffset = .3f;
        }
        else if (!isGrounded)
        {
            controller.stepOffset = .3f;
        }
        /*if (Input.GetButtonDown("Jump"))
        {
            jumped = true;
            timeSinceJump = 0;
        }
        */
        if (inAir && (isGrounded || Physics.CheckSphere(groundCheck.position, groundDist, envMask)) && timeSinceJump >= .1f)
        {
            inAir = false;
            currJumps = 0;
            timeSinceLanding = 0;
        }
        if (Input.GetButtonDown("Jump") && (isGrounded || Physics.CheckSphere(groundCheck.position, groundDist, envMask)))//&& controller.isGrounded)
        {
            //rb.AddForce(Vector3.up * Mathf.Sqrt(jumpH * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            //Debug.Log("OH YEAH" + rb.velocity);
            timeSinceJump = 0;
            if (isSphere && timeSinceLanding <= .25f)
            {
                jumpMult = Mathf.Clamp(jumpMult + .75f, 1, 7.5f);
            }
            else
                jumpMult = 1;
            ySpeed.y = Mathf.Sqrt(jumpH * (-2 * jumpMult) * gravity);
            //currJumps = 0;
            jumped = true;
            count = 0;
            inAir = true;
        }
        if (Input.GetButtonDown("Jump") && !(isGrounded || Physics.CheckSphere(groundCheck.position, groundDist, envMask)) && currJumps < jumps)
        {
            //rb.AddForce(Vector3.up * Mathf.Sqrt(jumpH * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            ySpeed.y = Mathf.Sqrt(jumpH * -2 * gravity);
            currJumps += 1;
            inAir = true;
        }

        //ySpeed = AdjustVelToSlope(ySpeed);
        ySpeed.y += Mathf.Clamp(gravity * Time.deltaTime, -100, 100);
        //transform.Translate(ySpeed * Time.deltaTime, Space.World);
        controller.Move(ySpeed * Time.deltaTime);

        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(horiz, 0, vert).normalized;
        moveDirection = new Vector3(0,0,0);
        //if (dir.magnitude >= 0f)
        //{
            //dir.y = gravity;
            //dir = AdjustVelToSlope(dir);
            //dir.y += ySpeed.y;//gravity;
            //ySpeed = dir;
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            //Vector3 res = new Vector3(moveDirection.normalized.x, rb.velocity.y, moveDirection.normalized.z);
            //rb.velocity = new Vector3(res.x * moveSpeed * Time.deltaTime, res.y, res.z * moveSpeed * Time.deltaTime);

            //transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
            //moveDirection.x += (1f - hitNormal.y) * hitNormal.x * (1f - .3f);
            //moveDirection.z += (1f - hitNormal.y) * hitNormal.z * (1f - .3f);
            if (dir.magnitude >= .1f)
                controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        //}

        /// MAYBE MAKE moveDirection private variable and call Dash() from controller, not here \\\
        /*if (GetComponentInChildren<Controller>().hasDash && GetComponentInChildren<Controller>().AbilityReady() && Input.GetKeyDown("q"))
        {
            dashCd = 0;
            StartCoroutine(Dash());
        }*/
        //isGrounded = (Vector3.Angle(Vector3.up, hitNormal) <= controller.slopeLimit);

        /*Vector3 inpRes = Vector3.zero;
        if (!isGrounded)
        {
            inpRes.x += (1f - hitNormal.y) * hitNormal.x * (1f - .3f);
            inpRes.z += (1f - hitNormal.y) * hitNormal.z * (1f - .3f);
        }
        controller.Move(inpRes);*/
        //controller.Move(new Vector3(moveDirection.normalized.x * moveSpeed * Time.deltaTime, ySpeed.y * Time.deltaTime, moveDirection.normalized.z * moveSpeed * Time.deltaTime));
    }

    public IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            controller.Move(moveDirection * 100 * Time.deltaTime);

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<CharacterController>().enabled == false)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                GetComponent<CharacterController>().enabled = true;
            }
        }
    }

    private Vector3 AdjustVelToSlope(Vector3 vel)
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, .2f))
        {
            Quaternion slopeRot = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Vector3 adjVel = slopeRot * vel;

            if (adjVel.y < 0)
            {
                Debug.Log("LOLOLOLOL");
                return adjVel;
            }
        }
        return vel;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }
}

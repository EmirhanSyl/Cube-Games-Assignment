using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float swerveSpeed = 1f;
    [SerializeField] private float maxSwerveAmount = 1f;
    [SerializeField] private float clampXPositionAmount = 1.6f;

    private float currentSpeed;
    private float targetSpeed;

    private float swerveAmount;

    private bool isGameStarted;
    private bool speedChanged;

    private Animator animator;
    private Rigidbody rig;
    private Material material;

    private SwerveInput swerveInput;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        material = GetComponentInChildren<SkinnedMeshRenderer>().material;    
        swerveInput = GetComponent<SwerveInput>();
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isGameStarted)
        {
            isGameStarted = true;
            animator.SetTrigger("StartRunning");

            targetSpeed = speed;
            speedChanged = true;
        }

        swerveAmount = swerveSpeed * Time.deltaTime * swerveInput.moveAmountOnXAxis;
        swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);

        //Clamp Position
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -clampXPositionAmount, clampXPositionAmount);
        transform.position = pos;

        if (speedChanged)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, .01f);
            if ((currentSpeed <= targetSpeed + 0.1f) && currentSpeed >= targetSpeed - 0.1f)
            {
                speedChanged = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isGameStarted)
        {
            rig.velocity = currentSpeed * new Vector3(0, 0, 1);
            transform.Translate(swerveAmount, 0 , 0);
        }
    }

    public void GetHitted()
    {
        currentSpeed = 3;
       
    }
}

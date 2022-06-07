using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float swerveSpeed = 1f;
    [SerializeField] private float maxSwerveAmount = 1f;
    [SerializeField] private float clampXPositionAmount = 1.6f;

    public ParticleSystem hittedParticles;

    private float currentSpeed;
    private float targetSpeed;

    private float swerveAmount;

    public bool isGameStarted;
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
        if (Input.GetMouseButtonDown(0) && !isGameStarted && !EventSystem.current.IsPointerOverGameObject())
        {
            StartCoroutine(WaitForCameraChange());

            //isGameStarted = true;
            //animator.SetTrigger("StartRunning");

            //targetSpeed = speed;
            //speedChanged = true;
        }

        swerveAmount = swerveSpeed * Time.deltaTime * swerveInput.moveAmountOnXAxis;
        swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);

        //Clamp Position
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -clampXPositionAmount, clampXPositionAmount);
        transform.position = pos;
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (speedChanged)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, .01f);
            if ((currentSpeed <= targetSpeed + 0.05f) && currentSpeed >= targetSpeed - 0.05f)
            {
                speedChanged = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isGameStarted)
        {
            //With Rigidbody
            //rig.velocity = currentSpeed * new Vector3(0, 0, 1);

            //With Transform
            transform.Translate(swerveAmount, 0 , currentSpeed);

        }
    }

    public void GetHitted()
    {
        if (GameManager.Instance.godMode)
        {
            hittedParticles.gameObject.SetActive(true);
            hittedParticles.Play();
            return;
        }
        currentSpeed = 0.2f;
        speedChanged = true;
        animator.SetTrigger("Hitted");
        hittedParticles.gameObject.SetActive(true);
        hittedParticles.Play();

    }
    public void Finished()
    {
        currentSpeed = 0;
        isGameStarted = false;
        animator.SetTrigger("VictoryAnim");
    }

    IEnumerator WaitForCameraChange()
    {
        yield return new WaitForSeconds(0.5f);
        isGameStarted = true;
        animator.SetTrigger("StartRunning");

        targetSpeed = speed;
        speedChanged = true;
        StopCoroutine(WaitForCameraChange());
    }
}

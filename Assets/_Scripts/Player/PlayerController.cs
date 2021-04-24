using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;

    [Header("Interaction")]
    public LayerMask interactionMask;
    private RaycastHit hitInfo;
    public bool isRedShroom;

    [Header("Camera Controls")]
    public CinemachineVirtualCamera virtualCamera;
    public Vector3 largeOffset;
    public Vector3 smallOffset;
    [Space]
    public GameObject followTarget;
    public float rotationPower;
    public float horizontalDampening;
    
    private Vector2 previousMouseDelta = Vector2.zero;

    // Components
    private Animator anim;
    private PlayerStates playerStates;

    // Animator Hashes
    public readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    public readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    public readonly int IsDizzyHash = Animator.StringToHash("IsDizzy");
    public readonly int PickupLargeHash = Animator.StringToHash("PickupLarge");
    public readonly int PickupSmallHash = Animator.StringToHash("PickupSmall");
    public readonly int LandingHash = Animator.StringToHash("Landing");

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            rotationPower = 2.0f;
        else
            rotationPower = 0.2f;

        anim = GetComponent<Animator>();
        playerStates = GetComponent<PlayerStates>();
    }

    public void OnMovement(InputValue value)
    {
        if (playerStates.isInteracting || GameManager.instance.isPaused)
            return;

        playerStates.isWalking = value.isPressed;
        anim.SetBool(IsWalkingHash, value.isPressed);
    }

    public void OnRunning(InputValue value)
    {
        playerStates.isRunning = value.isPressed;
        anim.SetBool(IsRunningHash, value.isPressed);
    }

    public void OnLook(InputValue value)
    {
        if (playerStates.isInteracting || GameManager.instance.isPaused)
            return;

        Vector2 aimValue = value.Get<Vector2>();
        Quaternion addedRotation = Quaternion.AngleAxis(Mathf.Lerp(previousMouseDelta.x, aimValue.x, 1.0f / horizontalDampening) * rotationPower, transform.up);

        followTarget.transform.rotation *= addedRotation;

        previousMouseDelta = aimValue;

        transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);

        followTarget.transform.localEulerAngles = Vector3.zero;
    }

    public void OnInteract(InputValue value)
    {
        if (playerStates.isWalking || playerStates.isRunning)
            return;
        
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1, interactionMask))
        {
            if (hitInfo.transform.CompareTag("Red Mushroom"))
                isRedShroom = true;
            else
                isRedShroom = false;

            if (playerStates.isLarge)
            {
                playerStates.isInteracting = true;
                anim.SetTrigger(PickupLargeHash);
            }
            else
            {
                playerStates.isInteracting = true;
                anim.SetTrigger(PickupSmallHash);
            }
        }
    }

    public void OnConsumeMushroom()
    {
        if (isRedShroom)
        {
            if (playerStates.isLarge)
            {
                anim.SetTrigger(LandingHash);

                transform.localScale = playerStates.shrinkScale;
                Vector3 pos = transform.position;
                pos.y = 0.5f;
                transform.position = pos;

                virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = smallOffset;

                playerStates.isLarge = false;
                gameObject.layer = 7;

                GameManager.instance.SmallPop();
            }
            else
            {
                SetDizziness(true);
                StartCoroutine(DizzyRoutine());
            }
        }
        else
        {
            if (playerStates.isLarge)
            {
                SetDizziness(true);
                StartCoroutine(DizzyRoutine());
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);

                virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = largeOffset;

                playerStates.isLarge = true;
                gameObject.layer = 6;

                GameManager.instance.LargePop();
            }
        }

        playerStates.isInteracting = false;
    }

    public void OnPause(InputValue value)
    {
        GameManager.instance.MenuClick();
        GameManager.instance.PauseGame();
    }

    private void Update()
    {
        if (playerStates.isInteracting || playerStates.isDizzy || GameManager.instance.isPaused)
            return;

        float currentSpeed = playerStates.isRunning ? runSpeed : walkSpeed;
        Vector3 moveDirection = transform.forward * (currentSpeed * Time.deltaTime);
        transform.position += playerStates.isWalking ? moveDirection : Vector3.zero;
    }

    private void SetDizziness(bool dizzy)
    {
        anim.SetBool(IsDizzyHash, dizzy);
        playerStates.isDizzy = dizzy;
    }

    IEnumerator DizzyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        SetDizziness(false);
    }
}

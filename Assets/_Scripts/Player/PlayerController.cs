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

        anim = GetComponent<Animator>();
        playerStates = GetComponent<PlayerStates>();
    }

    public void OnMovement(InputValue value)
    {
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

        // TODO: sphere cast for mushroom

        if (playerStates.isLarge)
        {
            transform.localScale = playerStates.shrinkScale;
            Vector3 pos = transform.position;
            pos.y = 0.5f;
            transform.position = pos;

            virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, 1.25f, -1);

            anim.SetTrigger(LandingHash);

            playerStates.isLarge = false;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);

            virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, 2.5f, -2);

            playerStates.isLarge = true;
        }
    }

    private void Update()
    {
        if (playerStates.isInteracting)
            return;

        float currentSpeed = playerStates.isRunning ? runSpeed : walkSpeed;
        Vector3 moveDirection = transform.forward * (currentSpeed * Time.deltaTime);
        transform.position += playerStates.isWalking ? moveDirection : Vector3.zero;
    }
}

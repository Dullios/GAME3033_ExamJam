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

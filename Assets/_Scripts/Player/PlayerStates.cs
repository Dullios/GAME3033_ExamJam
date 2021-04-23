using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    [Header("Movement")]
    public bool isWalking;
    public bool isRunning;

    [Header("Interaction")]
    public bool canInteract;
    public bool isInteracting;

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}

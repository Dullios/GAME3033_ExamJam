using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    [Header("Size")]
    public bool isLarge = true;
    public Vector3 shrinkScale;

    [Header("Movement")]
    public bool isWalking;
    public bool isRunning;
    public bool isDizzy;

    [Header("Interaction")]
    public bool isInteracting;
    public bool isPaused;
}

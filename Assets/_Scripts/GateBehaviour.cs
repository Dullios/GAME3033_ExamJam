using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBehaviour : MonoBehaviour
{
    private Animator anim;

    // Animator Hashes
    public readonly int IsOpenHash = Animator.StringToHash("IsOpen");

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && GameManager.instance.unlocked)
        {
            anim.SetTrigger(IsOpenHash);
        }
    }
}

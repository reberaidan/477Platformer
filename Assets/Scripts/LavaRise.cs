using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRise : MonoBehaviour
{
    public Animator lava;
    public RuntimeAnimatorController setAnim;

    private void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        lava.runtimeAnimatorController = setAnim;
    }
}

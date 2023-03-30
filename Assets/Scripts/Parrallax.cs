using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrallax : MonoBehaviour
{
    public float scaleX;
    public float scaleY;
    public Transform tfPlayer;
    private float origXPos;
    private float origYPos;
    private void Start()
    {
        origXPos = transform.position.x;
        origYPos = transform.position.y;

    }

    private void LateUpdate()
    {
        //transform.position = new Vector3((tfPlayer.position.x * scale), transform.position.y, transform.position.z);
        transform.position = new Vector3(origXPos - (tfPlayer.position.x * scaleX), origYPos - (tfPlayer.position.y * scaleY), transform.position.z);
    }
}

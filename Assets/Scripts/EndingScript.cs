using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class EndingScript : MonoBehaviour
{
    public GameObject anim;
    public GameObject player;
    public CinemachineVirtualCamera cam;
    public Transform nextFollow;
    // Start is called before the first frame update
    void Start()
    {
        anim.SetActive(false);
    }

    // Update is called once per frame


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player.SetActive(false);
            anim.SetActive(true);
            cam.Follow = nextFollow;

        }
    }
}

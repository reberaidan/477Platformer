using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Cinemachine;
using TMPro;

public class CameraChange : MonoBehaviour, IPointerDownHandler
{
    public GameObject anim;
    public GameObject button;
    public GameObject title;
    public GameObject pile;
    public bool buttonPressed;
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

   public void Start() {
        pile.SetActive(false);
   }

    public void Update()
    {
        if (buttonPressed)
        {
            cam1.Priority = 10;
            cam2.Priority = 11;
            anim.SetActive(false);
            button.SetActive(false);
            title.SetActive(false);
            pile.SetActive(true);
        }
    }
}
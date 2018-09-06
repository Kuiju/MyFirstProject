using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameObject .Find ("_MenuCenter").GetComponent <MenueCreator >().TriggerWithClick )
        {
            foreach (Transform child in transform)
            {
                if (child.tag == "Hide")
                {
                    child.gameObject.SetActive(true);
                    child.tag = "Normal";
                }

            }
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!GameObject.Find("_MenuCenter").GetComponent<MenueCreator>().TriggerWithClick)
        {
            foreach (Transform child in transform)
            {
                if (child.tag == "Hide")
                {
                    child.gameObject.SetActive(true);
                    child.tag = "Normal";
                }

            }
        }




    }

    public void OnPointerExit(PointerEventData eventData)
    {


        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (child.tag == "Normal")
            {
                child.gameObject.SetActive(false);
                child.tag = "Hide";

            }
        }

    }

}
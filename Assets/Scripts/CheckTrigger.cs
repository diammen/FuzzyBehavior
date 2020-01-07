
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTrigger : MonoBehaviour
{

    public bool isColliding;
    public bool canStoreObject;
    public LayerMask layers;
    public GameObject collidedObject;

    private void OnTriggerEnter(Collider collision)
    {
        if (layers == (layers | (1 << collision.gameObject.layer)))
        {
            isColliding = true;
            if (canStoreObject) collidedObject = collision.gameObject;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (layers == (layers | (1 << collision.gameObject.layer))) isColliding = true;
    }
    private void OnTriggerExit(Collider collision)
    {
        isColliding = false;
    }
}
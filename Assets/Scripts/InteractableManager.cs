using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    string objName;

    void Awake()
    {
        objName = gameObject.name;
    }

    public void Interact()
    {
        if (objName == "DoorEntrance")
        {
            // Handle interaction with the entrance door
            Debug.Log("Interacting with Entrance Door");
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    string objName;
    public PlayerController playerController;

    void Awake()
    {
        objName = gameObject.name;
    }

    public void Interact()
    {
        if (objName == "DoorEntrance")
        {
            SceneTransition.Instance.Transition();
            playerController.SetPlayerPosition(new Vector2(100, 100)); // Set player position to entrance
            Debug.Log("Interacting with Cabin Door");
        }
    }

}

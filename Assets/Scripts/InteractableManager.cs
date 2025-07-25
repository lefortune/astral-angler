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
            StartCoroutine(SceneTransition.Instance.Transition(playerController, new Vector2(50f, -20f)));
            Debug.Log("Interacting with Cabin Door");
        }

        if (objName == "DoorExit")
        {
            StartCoroutine(SceneTransition.Instance.Transition(playerController, new Vector2(-2f, -0.5f)));
            Debug.Log("Interacting with Cabin Exit");
        }

        if (objName == "Wheel")
        {

            Debug.Log("Interacting with Starfaring Wheel");
        }
        
        if (objName == "Bridge")
        {
            StartCoroutine(SceneTransition.Instance.Transition(playerController, new Vector2(60f, 10f)));
            Debug.Log("Interacting with Bridge");
        }
    }

}

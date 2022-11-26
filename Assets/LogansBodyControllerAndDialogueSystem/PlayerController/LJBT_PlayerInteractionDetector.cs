using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_PlayerInteractionDetector : MonoBehaviour
{
    //cast a box infront of the player to check if they're looking at an object or other interactable that they could pick up

    IInteractables lastInteractable;

    private void OnTriggerEnter(Collider other)
    {
        CheckAndAddInteractable(other);
    }

    private void OnTriggerExit(Collider other)
    {
        CheckAndRemoveInteractable(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;
        CheckAndAddInteractable(other);
    }
    private void OnCollisionExit(Collision collision)
    {
        Collider other = collision.collider;
        CheckAndRemoveInteractable(other);
    }

    void CheckAndAddInteractable(Collider collision)
    {
        
        if (collision.tag == "Interactable")
        {
            //IInteractables interact = collision.GetComponent(typeof(IInteractables));
            if (collision.gameObject.GetComponent<IInteractables>() != null)
            {
                Debug.Log("Player is colliding with an interactable object");
                lastInteractable = collision.GetComponent<IInteractables>();
                LJBT_GameManager.instance.DisplayInteractionPromptText(lastInteractable.PromptText());
            }
        }
        /*else
            Debug.Log("Player is colliding with an object");*/
    }

    void CheckAndRemoveInteractable(Collider collision)
    {
        if (collision.tag == "Interactable")
        {
            if (collision.GetComponent<IInteractables>() != null)
            {
                lastInteractable = null;
                LJBT_GameManager.instance.HideInteractionPromptText();
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (lastInteractable != null)
            {
                LJBT_GameManager.instance.HideInteractionPromptText();
                lastInteractable.Interact();
                lastInteractable = null;
            }
        }
    }

}

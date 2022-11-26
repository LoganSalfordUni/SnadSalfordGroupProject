using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_CameraController : MonoBehaviour
{
    //Attach this script to the players camera 


    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity;

    private float lookValueX;//Measures how far the player is looking left or right (or up and down) 
    private float lookValueY;

    //private bool lastNodWasUpwards;//these exist so we know which direction the next part of the nod or shake will go (if this is true, the player needs to move their head down to nod)
    //private bool lastShakeWasRight;
    enum directions
    {
        positive,//upwards or right
        neutral,//neither
        negative//downwards or left
    };
    private directions lastNod;
    private directions lastShake;

    private float nextNodValue;//How far the camera has to move for it to count as a nod/shake
    private float nextShakeValue;

    private int NodOrShakeValue;//positive means the player is nodding, negative means the player is shaking. 
    private float headMovementTimer;//If this reaches 0, then the player isn't nodding or shaking fast enough. 

    [Tooltip("How far does the mouse have to move up or down from 0 to be considered a nod")]
    [SerializeField] private float nodLimit = 20f;
    [Tooltip("How far does the mouse have to move left or right from 0 to be considered a shake")]
    [SerializeField] private float shakeLimit = 20f;

    [SerializeField] Vector2 cameraClamp = new Vector2(-90f, 90f);

    bool checkForNodAndShake;//true if the 

    void LockCursor()
    {
        //Lock the cursor in the center of the screen and make it invisible. 
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        LockCursor();
        checkForNodAndShake = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
        lookValueX = 0.0f;
        lookValueY = 0.0f;

        NodOrShakeValue = 0;

        //lastNodWasUpwards = false;//dont want these to be null, so we just give them a value
        //lastShakeWasRight = false;

        lastNod = directions.neutral;
        lastShake = directions.neutral;
        nextNodValue = nodLimit;
        nextShakeValue = shakeLimit;

    }

    private void Update()
    {
        //allow the camera to rotate, and check for the player nodding / shaking their head
        MoveCamera();   
    }

    void MoveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        lookValueX += mouseX;
        lookValueY += mouseY;

        //clamp camera so you can't loop around vertically
        if (lookValueY > cameraClamp.y)
        {
            mouseY = mouseY - (lookValueY - cameraClamp.y);
            lookValueY = cameraClamp.y;
        }
        else if (lookValueY < cameraClamp.x)
        {
            mouseY = mouseY - (lookValueY - cameraClamp.x);
            lookValueY = cameraClamp.x;
        }

        if (checkForNodAndShake)
        {
            NodAndShakeCheck(mouseX, mouseY);
        }



        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void NodAndShakeCheck(float mouseX, float mouseY)
    {
        //check if the player is inactive. 
        //oaef it would probably be better if this timer was for how long the player took to ask the question, rather than for how long it takes to 
        if (Mathf.Abs(mouseX) + Mathf.Abs(mouseY) > 1f)
            headMovementTimer -= Time.deltaTime;
        else
            headMovementTimer = 1.5f;
        if (headMovementTimer < 0f)
        {
            NodOrShakeValue = 0;
            lastNod = directions.neutral;
            lastShake = directions.neutral;
        }



        //Check for nods and shakes. 
        if (lastNod == directions.neutral)
        {
            if (mouseY > 1f)
            {
                //if the mouse has moved along the Y axis enough this second, then take it out of neutral.
                //1f is a default value, it might need to be adjusted
                lastNod = directions.positive;
                nextNodValue = lookValueY - nodLimit;
            }
            else if (mouseY < -1f)
            {
                lastNod = directions.negative;
                nextNodValue = lookValueY + nodLimit;
            }
        }
        else if (lastNod == directions.positive)
        {
            if (lookValueY < nextNodValue)
            {
                //if the player looks down enough, they've performed part of a nod. so we increase the nod counter. (positive nodorshake value)
                NodOrShakeValue += 1;
                lastNod = directions.negative;
                nextNodValue = lookValueY + nodLimit;
            }
            if (mouseY > 0f)
            {
                //if the camera is still moving upwards, or starts going up again: drag the nodValue with it
                nextNodValue = lookValueY - nodLimit;
            }

        }
        else if (lastNod == directions.negative)
        {
            if (lookValueY > nextNodValue)
            {
                NodOrShakeValue += 1;
                lastNod = directions.positive;
                nextNodValue = lookValueY - nodLimit;
            }
            if (mouseY < 0f)
            {
                //if the camera is still moving downwards, or starts going down again: drag the nodValue with it
                nextNodValue = lookValueY + nodLimit;
            }
        }


        if (lastShake == directions.neutral)
        {
            if (mouseX > 1f)
            {
                //if the mouse has moved along the Y axis enough this second, then take it out of neutral.
                //1f is a default value, it might need to be adjusted
                lastShake = directions.positive;
                nextShakeValue = lookValueX - shakeLimit;
            }
            else if (mouseX < -1f)
            {
                lastNod = directions.negative;
                nextShakeValue = lookValueX + shakeLimit;
            }
        }
        else if (lastShake == directions.positive)
        {
            if (lookValueX < nextShakeValue)
            {
                NodOrShakeValue -= 1;
                lastShake = directions.negative;
                nextShakeValue = lookValueX + shakeLimit;
            }
            if (mouseX > 0f)
            {
                nextShakeValue = lookValueX - shakeLimit;
            }
        }
        else if (lastShake == directions.negative)
        {
            if (lookValueX > nextShakeValue)
            {
                NodOrShakeValue -= 1;
                lastShake = directions.positive;
                nextShakeValue = lookValueX - shakeLimit;
            }
            if (mouseX < 0f)
            {
                nextShakeValue = lookValueX + shakeLimit;
            }
        }

        //oaef should only enable the nod / shake stuff when asked a question. alternativly, make a function that just resets the nodOrShake value
        //when the player nods but nodshakevalue is negative, set it to 1. and vice versa. 

        if (NodOrShakeValue > 3)
        {
            Debug.Log("Nod!");
            checkForNodAndShake = false;
            LJBT_DialogueManager.instance.QuestionNod();
        }
        else if (NodOrShakeValue < -3)
        {
            Debug.Log("Shake!");
            checkForNodAndShake = false;
            LJBT_DialogueManager.instance.QuestionShake();
        }
    }

    public void EnableQuestion()
    {
        //called by the text file reader
        NodOrShakeValue = 0;

        lastNod = directions.neutral;
        lastShake = directions.neutral;

        nextShakeValue = lookValueX + shakeLimit;
        nextNodValue = lookValueY + nodLimit;
        if (nextNodValue > cameraClamp.y)
        {
            nextNodValue = cameraClamp.y - 5f;//makes sure the game doesnt want the player to nod above the camera clamp
        }

        checkForNodAndShake = true;
    }

    public void DisableQuestion()
    {
        //just incase the camera is still waiting for the player to answer a question, but the game no longer wants that information
        //call this method when the dialogue closes for any reason 

        checkForNodAndShake = false;
    }
}

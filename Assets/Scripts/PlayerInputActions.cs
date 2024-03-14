using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInputActions : MonoBehaviour
{
    
    private PlayerMovement playerController;
    private PlayerCombat playerCombatController;

    PlayerInputs playerInput;
    // Start is called before the first frame update
    void OnEnable()
    {
        playerController = GetComponent<PlayerMovement>();
        playerCombatController = GetComponent<PlayerCombat>();


        if (playerInput == null ) 
        {
            playerInput = new PlayerInputs();
            playerInput.Player.Move.performed += i => playerController.Move(i.ReadValue<Vector2>());
            playerInput.Player.Jump.performed += i => playerController.Jump();
            playerInput.Player.Jump.canceled += i => playerController.JumpCanceled();
            playerInput.Player.VTransform.performed += i => playerController.VTransform();
            playerInput.Player.WTransform.performed += i => playerController.WTransform();
            playerInput.Player.Dodge.performed += i => playerController.Dodge();
            playerInput.Player.Fire.performed += i => playerCombatController.Attack();



        }
        playerInput.Enable();


    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

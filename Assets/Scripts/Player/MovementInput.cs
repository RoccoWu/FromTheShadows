using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class MovementInput : MonoBehaviour, PlayerInput.IPlayerActions
{
    private PlayerInput playerInput;
    private Animator anim;
    [SerializeField] private Camera cam;
    public CharacterController controller;
    private Vector3 desiredMoveDirection;
    private Vector3 moveVector;
     public Vector2 moveAxis;
    private float verticalVel;
    [Header("Settings")]
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] float fallSpeed = .2f;
    [SerializeField] float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    public float acceleration = 1;
    
    [Header("Booleans")]
    [SerializeField] bool blockRotationPlayer;
    private bool isGrounded = true;
    public bool isCrouching = false;
    public bool jumping = false;
    public bool IsWalking = false;
    public bool isRunning = false;
    public bool isIdle = true;
    public bool isSliding = false;
    public float slideOffset;
    private PlayerInput playerInput_
    {
        get  
        {
            playerInput ??= new PlayerInput(); //creates new playerInput when the player input is null
            return playerInput;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputMagnitude();

        if(controller != null)
        {
            isGrounded = controller.isGrounded;
            if (isGrounded)
            verticalVel -= 0;
            else
                verticalVel -= 1;
            
            controller.Move(moveVector);
            turnSmoothVelocity = .1f;
        }
    }

    void PlayerMoveAndRotation()
    {
        if (blockRotationPlayer == false)
        {
            //Camera
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            if(direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(moveAxis.x, moveAxis.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f) * Vector3.forward;
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                moveVector = new Vector3(0, verticalVel * fallSpeed * Time.deltaTime, 0);
                if(controller !=null)
                {
                    controller.Move(moveDir.normalized * Time.deltaTime * (movementSpeed * acceleration));
                }               
            }        
        }
        else
        {
            //Strafe
            controller.Move((transform.forward * moveAxis.y + transform.right * moveAxis.y) * Time.deltaTime * (movementSpeed * acceleration));
        }
    }

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), rotationSpeed);
    }

    public void RotateToCamera(Transform t)
    {
        var forward = cam.transform.forward;

        desiredMoveDirection = forward;
        Quaternion lookAtRotation = Quaternion.LookRotation(desiredMoveDirection);
        Quaternion lookAtRotationOnly_Y = Quaternion.Euler(transform.rotation.eulerAngles.x, lookAtRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        t.rotation = Quaternion.Slerp(transform.rotation, lookAtRotationOnly_Y, rotationSpeed);
    }

    void InputMagnitude()
    {
        //Calculate the Input Magnitude
        float inputMagnitude = new Vector2(moveAxis.x, moveAxis.y).sqrMagnitude;
        anim.SetBool("IsWalking", true); //player should be walking 

        //Physically move player
        if (inputMagnitude > 0.1f)
        {
            if(anim != null)
            {
                anim.SetFloat("InputMagnitude", inputMagnitude * acceleration, .1f, Time.deltaTime);
                PlayerMoveAndRotation();            
                anim.SetBool("IsWalking", true); //player should be walking 
            }            
            IsWalking = true;
            isIdle = false;
        }
        else
        {
            if(anim != null)
            {
                anim.SetBool("IsWalking", false); //idle
                anim.SetFloat("InputMagnitude", inputMagnitude * acceleration, .1f, Time.deltaTime);
            }           
            IsWalking = false;
            isIdle = true;
        }
    }

    private void OnEnable()
    {
        playerInput_.Enable();
        playerInput_.Player.SetCallbacks(this);
    }
    private void OnDisable()
    {
        playerInput_.Disable();       
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveAxis = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if( context.performed && isRunning == false && isCrouching == false)
        {
            isRunning = true;
            if(anim != null)
            {
                anim.SetBool("IsRunning", true);
            }            
            movementSpeed = 1.8f;
        }

        else if(context.canceled && isRunning)
        {
            isRunning = false;
            if(anim != null)
            {  
                anim.SetBool("IsRunning", false);
            }
            movementSpeed = 0.8f;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        
    }

    public void OnSonarKnife(InputAction.CallbackContext context)
    {
        
    }

    public void OnTeleportKnife(InputAction.CallbackContext context)
    {
       
    }

    public void OnDistractionKnife(InputAction.CallbackContext context)
    {
       
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if(isCrouching == false)
        {
            if (isRunning)
            {
                // isSliding = true;
                isCrouching = true;
                //if(anim != null)
                //{
                //    anim.SetBool("IsSliding", true);
                //}                
                //Vector3 final = new Vector3(transform.position.x, transform.position.y - slideOffset, transform.position.z);
                //transform.DOMove(final, 0.8f, false);

            }
            else
            {
                isCrouching = true;
                if(anim != null)
                {
                    anim.SetBool("IsCrouching", true);  
                }                
                movementSpeed = 0.5f;
            }
        }

        else
        {
            isCrouching = false;
            if(anim != null)
            {
                anim.SetBool("IsCrouching", false);
            }            
            movementSpeed = 0.8f;
        }
    }

    //public void FinishSlide()
    //{
    //    isSliding = false;
    //    if(anim != null)
    //    {
    //        anim.SetBool("IsSliding", false);
    //    }        
    //}

    public void OnAimKnife(InputAction.CallbackContext context)
    {
        
    }

    public void OnThrowKnife(InputAction.CallbackContext context)
    {
        
    }

    public void OnUnequipKnife(InputAction.CallbackContext context)
    {
        
    }
    public void OnActivateKnife(InputAction.CallbackContext context)
    {
        
    }

    public void OnToggleDebug(InputAction.CallbackContext context)
    {
        
    }
    public void OnDevCheatF1(InputAction.CallbackContext context)
    {
        //do something
    }

    public void OnDevCheatF2(InputAction.CallbackContext context)
    {
        
    }

    public void OnDevCheatF3(InputAction.CallbackContext context)
    {
        
    }

    public void OnDevCheatF4(InputAction.CallbackContext context)
    {
        
    }

    public void OnDevCheatF5(InputAction.CallbackContext context)
    {
        
    }

    public void OnDevCheatF6(InputAction.CallbackContext context)
    {
        
    }

    public void OnDevCheatF7(InputAction.CallbackContext context)
    {
        
    }
}

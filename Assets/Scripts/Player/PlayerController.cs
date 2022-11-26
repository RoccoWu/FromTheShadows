using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class PlayerController : MonoBehaviour, PlayerInput.IPlayerActions
{

    public delegate void playerfinishedteleportingEvent(PlayerController playerController);
    public playerfinishedteleportingEvent finishedTeleportingEvent;
    private PlayerInput playerInput;
    public Vector2 moveAxis;
    public Animator anim;
    public GameObject playerCanvas;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject cameraFocus;
    [SerializeField] private Transform originalCamFocus, aimCamFocus;
    public bool isAiming = false;
    public bool threwKnife = false;
    private EnemyController enemyController;
    private KnifeManager knifeManager;

    public float turnSpeed = 15;
    public float aimDuration = 0.3f;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    int isAimingParam = Animator.StringToHash("IsAiming");

    public Transform cameraLookAt;

    Camera mainCamera;
    [SerializeField] private DebugController debugScript;

    [Header("Cooldown")]
    [SerializeField] private float cooldownSonar;
    [SerializeField] private float cooldownDistract;
    [SerializeField] private float cooldownTeleport;
    private float currentCoolDownSonar = 0.0f;
    private float currentCoolDownDistract = 0.0f;
    private float currentCoolDownTeleport = 0.0f;

    [Header("Checking Knife")]
    [SerializeField] private bool canThrowSonar;
    [SerializeField] private bool canThrowDistract;
    [SerializeField] private bool canThrowTeleport;
    public GameObject currentKnifeLookingAt;

    [Header("Knife Metrics")]
    public int freqSonar = 0;
    public int freqDistract = 0;
    public int freqTeleport = 0;

    [Header("VFX")]
    public GameObject teleportVFX;


    [Header("Interactibles")]
    private GameObject securityKey;
   
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
        knifeManager = FindObjectOfType<KnifeManager>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        crosshair = playerCanvas.transform.GetChild(0).gameObject;
        crosshair.GetComponent<CanvasGroup>().alpha = 0;
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        if(anim != null)
        {
            anim.SetBool(isAimingParam, isAiming);
        }        

        if (isAiming)
        {
            float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.deltaTime);
        }

        // Handle cooldowns
        if(cooldownSonar > 0.0f)
        {
            cooldownSonar -= Time.deltaTime;
        }
        if (cooldownDistract > 0.0f)
        {
            cooldownDistract -= Time.deltaTime;
        }
        if (cooldownTeleport > 0.0f)
        {
            cooldownTeleport -= Time.deltaTime;
        }
        //Debug.Log(canThrowTeleport);
    }

    void FixedUpdate()
    {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Security")
        {
            Debug.Log("entering trigger space");
            securityKey = other.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Security")
        {
            securityKey = null;
        }
    }


    public void ThrowKnife()
    {
        //calls to throw the knife
        knifeManager.throwKnife();
        threwKnife = true;
        threwKnife = false;
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

    public void OnSonarKnife(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            knifeManager.knife = KnifeManager.KnifeType.sonarKnife;
            knifeManager.PickKnife();
            print("sonar knife");
        }      
    }

    public void OnTeleportKnife(InputAction.CallbackContext context)
    {    
        if(context.performed)
        {
            knifeManager.knife = KnifeManager.KnifeType.teleportKnife;
            knifeManager.PickKnife();
            print("teleport knife");
        }     
    }

    public void OnDistractionKnife(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            knifeManager.knife = KnifeManager.KnifeType.distractionKnife;
            knifeManager.PickKnife();
            print("distraction knife");
        }        
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnRun(InputAction.CallbackContext context)
    {

    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("pressed e");
            if (securityKey != null)
            {
                Debug.Log("pressed e and security key is not null");

                securityKey.GetComponent<PickupBehaviour>().AddSecurityBadge();
                Destroy(securityKey.gameObject);
                Debug.Log("lol");

            }
        }
        
    }

    public void OnAimKnife(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            crosshair.GetComponent<CanvasGroup>().alpha = 1;
            isAiming = true;  
        }

        else if(context.canceled)
        {
            crosshair.GetComponent<CanvasGroup>().alpha = 0;
            isAiming = false;
        }       
    }

    public void OnUnequipKnife(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            knifeManager.knife = KnifeManager.KnifeType.noKnife;
            knifeManager.PickKnife();
            print("no knife");
        }
    }

    public void OnThrowKnife(InputAction.CallbackContext context)
    {
        if(isAiming && knifeManager.knife != KnifeManager.KnifeType.noKnife)
        {
            //plays the throwing animation
            if(anim != null)
            {
                anim.SetTrigger("KnifeThrow");
            }                
        }
    }

    public void OnActivateKnife(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //check if crosshair is hovering over the knife
            //check what knife it is
            //check if it is on cooldown, if not, perform the action
            if (canThrowDistract || canThrowSonar || canThrowTeleport)
            {
                if(currentKnifeLookingAt != null)
                {
                    currentKnifeLookingAt.GetComponent<KnifeBehavior>().ActivateKnife();
                    Debug.Log("activated a knife");
                } 
            }
            //if (canThrowTeleport)
            //{
            //    if (anim != null)
            //    {
            //        anim.SetTrigger("TeleportThrow");
            //    }
            //    gameObject.GetComponent<TeleportController>().Teleport();
            //    cooldownTeleport = 15.0f;
            //}
        }
    }

    public void CheckKnife(GameObject knife)
    {
        currentKnifeLookingAt = knife;
        if (knife.GetComponent<KnifeBehavior>().knifeType == KnifeManager.KnifeType.distractionKnife)
        {
            if (cooldownDistract <= 0.0f)
            {
                canThrowDistract = true;
            }
        }
        else if(knife.GetComponent<KnifeBehavior>().knifeType == KnifeManager.KnifeType.sonarKnife)
        {
            if (cooldownSonar <= 0.0f)
            {
                canThrowSonar = true;
            }
        }
        else if(knife.GetComponent<KnifeBehavior>().knifeType == KnifeManager.KnifeType.teleportKnife)
        {
            if (cooldownTeleport <= 0.0f)
            {
                canThrowTeleport = true;
            }
        }
    }

    public void SetUnableThrowKnife(GameObject knife)
    {
        if (knife.GetComponent<KnifeBehavior>().knifeType == KnifeManager.KnifeType.distractionKnife)
        {
            canThrowDistract = false;
        }
        else if (knife.GetComponent<KnifeBehavior>().knifeType == KnifeManager.KnifeType.sonarKnife)
        {
            canThrowSonar = false;

        }
        else if (knife.GetComponent<KnifeBehavior>().knifeType == KnifeManager.KnifeType.teleportKnife)
        {
            canThrowTeleport = false;
        }
    }
    public void OnToggleDebug(InputAction.CallbackContext context)
    {
        debugScript.showConsole = !debugScript.showConsole;
    }

    public void OnDevCheatF1(InputAction.CallbackContext context)
    {
       if(context.performed)
        {
            debugScript.F1();
        } 
    }

    public void OnDevCheatF2(InputAction.CallbackContext context)
    {
         if(context.performed)
        {
            debugScript.F2();
        } 
    }

    public void OnDevCheatF3(InputAction.CallbackContext context)
    {
         if(context.performed)
        {
            debugScript.F3();
        } 
    }

    public void OnDevCheatF4(InputAction.CallbackContext context)
    {
         if(context.performed)
        {
            debugScript.F4();
        } 
    }

    public void OnDevCheatF5(InputAction.CallbackContext context)
    {
         if(context.performed)
        {
            debugScript.F5();
        } 
    }

    public void OnDevCheatF6(InputAction.CallbackContext context)
    {
         if(context.performed)
        {
            debugScript.F6();
        } 
    }

    public void OnDevCheatF7(InputAction.CallbackContext context)
    {
         if(context.performed)
        {
            debugScript.F7();
        } 
    }

    public void AddFreqSonar()
    {
        freqSonar += 1;
    }
    public void AddFreqDistract()
    {
        freqDistract += 1; 
    }
    public void AddFreqTeleport()
    {
        freqTeleport += 1;
    }
}

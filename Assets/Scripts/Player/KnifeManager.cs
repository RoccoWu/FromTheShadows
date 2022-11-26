using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KnifeManager : MonoBehaviour
{
    public Camera camera;
    public float range = 100f;
    public float throwPower;
    public Transform playerRightHand, playerLeftHand;
    [SerializeField] private GameObject sonarKnifeModel, distractionKnifeModel, teleportKnifeModel;
    
    [SerializeField] public GameObject currentKnife;
    [Header("Crosshair UI")]
    public GameObject playerCanvas;
    [Header("UI")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private float uiOriginalY, uiChooseY;
    [SerializeField] private GameObject currentAbilityUI;
    [SerializeField] private float sonarCooldown, distractionCooldown, teleportCooldown;
    [SerializeField] private float sonarCurrentTime, distractionCurrentTime, teleportCurrentTime;

    public bool sonarCardExist, distractionCardExist, teleportCardExist;

    public Sprite defaultCrosshair, sonarCrosshair, distractionCrosshair, teleportCrosshair;
    [SerializeField] private GameObject sonarUI, distractionUI, teleportUI;
    public AudioSource audioSource;
    [SerializeField] private AudioClip[] cardThrowSFX;
    public AudioClip[] cardhitSFX;

    public enum KnifeType
    {
        noKnife,
        sonarKnife,
        distractionKnife,
        teleportKnife
    }

    public KnifeType knife;
    // Start is called before the first frame update
    void Start()
    {
        knife = KnifeType.noKnife;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void PickKnife()
    {
        switch(knife)
        {
            //switches knives and the crosshair
            case KnifeType.noKnife:  
                if(playerRightHand.transform.childCount > 0)
                {
                    Destroy(currentKnife);
                }   
                crosshair.GetComponent<Image>().sprite = defaultCrosshair;
            break;

            case KnifeType.sonarKnife: //Sonar Knife
                if(playerRightHand.transform.childCount > 0)
                {
                  Destroy(currentKnife);
                } 
                if(currentAbilityUI != null)
                {
                    currentAbilityUI.transform.DOMoveY(uiOriginalY, 1, false);
                }                 
                currentKnife = Instantiate(sonarKnifeModel,playerRightHand.position,sonarKnifeModel.transform.rotation);
                currentKnife.GetComponent<KnifeBehavior>().SetKnifeType(KnifeType.sonarKnife);
                currentKnife.transform.SetParent(playerRightHand);
                crosshair.GetComponent<Image>().sprite = sonarCrosshair;
                currentAbilityUI = sonarUI;
                currentAbilityUI.transform.DOMoveY(uiChooseY, 1, false);
            break;

            case KnifeType.distractionKnife: //Distraction Knife
                if(playerRightHand.transform.childCount > 0)
                {
                    Destroy(currentKnife);
                } 
                if(currentAbilityUI != null)
                {
                    currentAbilityUI.transform.DOMoveY(uiOriginalY, 1, false);
                }     
                currentKnife = Instantiate(distractionKnifeModel,playerRightHand.position,distractionKnifeModel.transform.rotation);
                currentKnife.GetComponent<KnifeBehavior>().SetKnifeType(KnifeType.distractionKnife);
                currentKnife.transform.SetParent(playerRightHand);
                crosshair.GetComponent<Image>().sprite = distractionCrosshair;
                currentAbilityUI = distractionUI;
                currentAbilityUI.transform.DOMoveY(uiChooseY, 1, false);
            break;

            case KnifeType.teleportKnife: //Teleport Knife
                if(playerRightHand.transform.childCount > 0)
                {
                    Destroy(currentKnife);
                }   
                if(currentAbilityUI != null)
                {
                    currentAbilityUI.transform.DOMoveY(uiOriginalY, 1, false);
                }     
                currentKnife = Instantiate(teleportKnifeModel,playerRightHand.position,teleportKnifeModel.transform.rotation);
                currentKnife.GetComponent<KnifeBehavior>().SetKnifeType(KnifeType.teleportKnife);
                currentKnife.transform.SetParent(playerRightHand);
                crosshair.GetComponent<Image>().sprite = teleportCrosshair;
                currentAbilityUI = teleportUI;
                currentAbilityUI.transform.DOMoveY(uiChooseY, 1, false);
            break;
        }
    }

    public void throwKnife()
    {
        //play sound
        int cardThrowSFXSelect = Random.RandomRange(0, cardThrowSFX.Length);
        audioSource.PlayOneShot(cardThrowSFX[cardThrowSFXSelect]);
        //should hit the object
        RaycastHit hit;
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
        {
            //throws the actual knife
            float throwUpwardForce = 0.5f;
            if(currentKnife != null)
            {
                Rigidbody knifeRig = currentKnife.GetComponent<Rigidbody>();
                currentKnife.transform.parent = null;
                Vector3 forceToAdd = camera.transform.forward * throwPower + transform.up * throwUpwardForce;
                knifeRig.isKinematic = false;
                knifeRig.AddForce(forceToAdd, ForceMode.Impulse);
                currentKnife.GetComponent<KnifeBehavior>().SetIsThrown(true);
            }
        }
    }

    public void SetCurrentKnife(KnifeType kt)
    {
        knife = kt;
    }
}

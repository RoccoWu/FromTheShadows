using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class KnifeBehavior : MonoBehaviour
{
    public bool activated;

    public float rotationSpeed;

    public Camera cam;
    public GameObject player;

    private bool isThrown;

    public GameObject UICanvas;
    private GameObject InstantiatedUICanvas;

    public Material[] knifeMaterials;
    private int materialIndex;

    public KnifeManager.KnifeType knifeType;

    public bool canBeActivated;
    public bool canFinishTeleporting;

    private Animator anim;

    [SerializeField] private float teleportDuration = 0.5f;
    [SerializeField] private float sonarDuration = 3.0f;
    private float currSonarDuration = 999.9f;

    [SerializeField] private float distractionDistance = 10.0f;
    [SerializeField] TeleportController tpController;

    private KnifeManager _knifeManager;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        player = FindObjectOfType<PlayerController>().gameObject;
        _knifeManager = player.GetComponent<KnifeManager>();
        if (knifeType == KnifeManager.KnifeType.sonarKnife)
        {
            Debug.Log("setting up sonar anim");
            anim = GetComponent<Animator>();
        }
       tpController = player.GetComponent<TeleportController>();
    }

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        Vector2 middle = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        if(screenPos.x > middle.x - 20.0f && screenPos.x < middle.x + 20.0f
            && screenPos.y > middle.y - 20.0f && screenPos.y < middle.y + 20.0f)
        {
            //Debug.Log("hit");
            canBeActivated = true;
            player.GetComponent<PlayerController>().CheckKnife(this.gameObject);
            if(InstantiatedUICanvas != null)
            {
                InstantiatedUICanvas.transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 100);
            }
        }
        else
        {
            player.GetComponent<PlayerController>().SetUnableThrowKnife(this.gameObject);
            if (InstantiatedUICanvas != null)
            {
                InstantiatedUICanvas.transform.GetChild(0).GetComponent<Image>().color = new Color32(144, 144, 144, 100);
            }
        }
        if (activated)
        {
            transform.localEulerAngles += transform.forward * rotationSpeed * Time.deltaTime;
        }
        if(knifeType == KnifeManager.KnifeType.sonarKnife)
        {
            if (currSonarDuration <= sonarDuration)
            {
                //Debug.Log(currSonarDuration);
                currSonarDuration -= Time.deltaTime;
            }
            if (currSonarDuration <= 0.0f)
            {
                Debug.Log("Turning Sonar Off!");
                anim.SetTrigger("SonarOff");
                currSonarDuration = 999.9f;
                Destroy(gameObject);
            }
        }       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.GameOver();
        }
        activated = false;
        GetComponent<Rigidbody>().isKinematic = true;
        int cardHitSFXSelect = Random.RandomRange(0, player.GetComponent<KnifeManager>().cardhitSFX.Length);
        player.GetComponent<KnifeManager>().audioSource.PlayOneShot(player.GetComponent<KnifeManager>().cardhitSFX[cardHitSFXSelect]);
        if(InstantiatedUICanvas == null)
        {
            InstantiatedUICanvas = Instantiate(UICanvas, transform);
        }
    }
    
    public void SetKnifeType(KnifeManager.KnifeType kt)
    {
        knifeType = kt;
    }

    public void SetIsThrown(bool thrown)
    {
        isThrown = thrown;
    }

    public bool GetIsThrown()
    {
        return isThrown;
    }

    public bool GetCanBeActivated()
    {
        return canBeActivated;
    }

    public void ActivateKnife()
    {
        Debug.Log(knifeType);
        PlayerController pc = player.GetComponent<PlayerController>();
        if(knifeType == KnifeManager.KnifeType.distractionKnife)
        {
            pc.AddFreqDistract();
            Debug.Log("Activated Distraction!");
            //for every enemy in a radius, do enemy.go to knife
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject e in enemies)
            {
                Vector3 knifeToEnemy = e.transform.position - transform.position;
                if(knifeToEnemy.magnitude <= distractionDistance)
                {
                    e.GetComponent<EnemyController>().MoveTowards(gameObject);
                }
            }            
        }
        else if(knifeType == KnifeManager.KnifeType.sonarKnife)
        {
            pc.AddFreqSonar();
            anim.SetTrigger("EmitSonar");
            currSonarDuration = sonarDuration;
            Debug.Log("Activated Sonar!");
        }
        else if(knifeType == KnifeManager.KnifeType.teleportKnife)
        {
            pc.AddFreqTeleport();
            TeleportPlayer();
            //player.GetComponent<CharacterController>().gameObject.SetActive(false);
            player.GetComponent<Animator>().SetTrigger("Teleport");
            //player.transform.position = gameObject.transform.position;
            //player.GetComponent<CharacterController>().gameObject.SetActive(true);
            //player.transform.DOMove(gameObject.transform.position, teleportDuration); 
            Debug.Log("Activated Teleport!");
            
        }
    }
    private void TeleportPlayer()
    {         
        player.GetComponent<PlayerController>().finishedTeleportingEvent+= playerfinishedTeleporting;
        foreach(Transform child in player.GetComponent<PlayerController>().teleportVFX.transform)
        {
            child.GetComponent<ParticleSystem>().Play();
        }        
    }

    private void playerfinishedTeleporting(PlayerController playerController)
    {
        player.GetComponent<CharacterController>().gameObject.SetActive(false);
        playerController.finishedTeleportingEvent -= playerfinishedTeleporting;
        player.transform.position = gameObject.transform.position;
        player.GetComponent<CharacterController>().gameObject.SetActive(true);
        tpController.canFinishTeleporting = false;
        _knifeManager.SetCurrentKnife(KnifeManager.KnifeType.noKnife);
        Destroy(gameObject);
    }
}

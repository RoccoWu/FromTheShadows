using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TeleportController : MonoBehaviour
{
    [SerializeField] private float teleportDuration;
    [SerializeField] private Material glowMaterial, opaqueGlowMaterial;
    private KnifeManager knifeManager;
    private PlayerController playerController;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject cameraFocus;
    public bool canFinishTeleporting;

    // Start is called before the first frame update
    void Start()
    {
        knifeManager = FindObjectOfType<KnifeManager>();
        playerController = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        print("teleportanimation");
        anim.SetTrigger("Teleport");
        //move to the position of the teleport knife hovering over's offset
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
        Destroy(clone.GetComponent<KnifeManager>().currentKnife.gameObject);
        clone.GetComponent<Animator>().enabled = false;
        clone.GetComponent<TeleportController>().enabled = false;
        clone.GetComponent<MovementInput>().enabled = false;
        clone.GetComponent<PlayerController>().enabled = false;
        clone.GetComponent<CharacterController>().enabled = false;
        Destroy(clone.transform.GetChild(5).gameObject);        
        //Destroy(clone.GetComponent<Animator>());
        /*Destroy(clone.GetComponent<TeleportController>());
        Destroy(clone.GetComponent<MovementInput>());
        Destroy(clone.GetComponent<CharacterController>());
        Destroy(clone.transform.GetChild(5).gameObject);*/
        
        SkinnedMeshRenderer[] skinMeshList = clone.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.material = glowMaterial;
            smr.material.DOFloat(2, "_AlphaThreshold", 5f).OnComplete(()=>Destroy(clone));
        }

        ShowPlayerBody(false);
        anim.speed = 0;
        if(playerController.currentKnifeLookingAt != null)
        {
            Debug.Log("LOL");
            transform.DOMove(playerController.currentKnifeLookingAt.transform.position, teleportDuration)
                .SetEase(Ease.InExpo).OnComplete(()=>FinishTeleport()); 
            //move towards where the currentKnife is at. Might have to change it to current activated knife
        }
       
    }

    public void FinishTeleport()
    {
        cameraFocus = gameObject.transform.GetChild(2).gameObject;
        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            GlowAmount(30);
            DOVirtual.Float(30, 0, .5f, GlowAmount);
        }        
        anim.speed = 1;
        canFinishTeleporting = true;
        ShowPlayerBody(true);
    }

    public void NewTeleport()
    {
        playerController.finishedTeleportingEvent(playerController);
    }

    private void ShowPlayerBody(bool state)
    {
        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.enabled = state;
        }
    }

    void GlowAmount(float x)
    {
        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.material.SetVector("_FresnelAmount", new Vector4(x, x, x, x));
        }
    }
}

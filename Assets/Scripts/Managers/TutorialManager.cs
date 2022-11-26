using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private bool movementTutorial, crouchTutorial, runTutorial, enemyTutorial, sonarTutorial, aimTutorial, activateTutorial, distractionTutorial, teleportTutorial;
    [SerializeField] private CanvasGroup movementCG, crouchCG, runCG, enemyCG, sonarCG, knifeaimCG, knifeabilityactivateCG, distractionCG, teleportCG;
    [SerializeField] private string tutorialState;
    public UnityEvent tutorialEvents;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //gameObject.GetComponent<FadeController>().FadeInTutorial();
        }
        tutorialState = other.gameObject.name;
        switch(tutorialState)
        {
            case "movementTutorial":
            //fade the movement tutorial
            movementCG.GetComponent<FadeController>().FadeInTutorial();
            break;

            case "crouchTutorial":
            crouchCG.GetComponent<FadeController>().FadeInTutorial();
            break;
        }
    }*/
    
    public void showMovementTutorial()
    {
        movementTutorial = true;
        movementCG.gameObject.GetComponent<FadeController>();
    }
}

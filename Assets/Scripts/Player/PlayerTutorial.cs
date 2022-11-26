using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private string tutorialStateTrigger;
    [SerializeField] private bool movementShown, crouchShown, runShown, enemyShown, sonarShown, knifeaimShown, knifeabilityactivateShown, distractionShown, teleportShown;
    [SerializeField] private CanvasGroup movementCG, crouchCG, runCG, enemyCG, sonarCG, knifeaimCG, knifeabilityactivateCG, distractionCG, teleportCG, fadeCG, sonarIconCG,
        distractionIconCG, teleportIconCG;
    private KnifeManager knifeManager;
    [SerializeField] private GameObject aimGoldenPath; 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip streetAmbience, buildingAmbience, protestAmbience;
    // Start is called before the first frame update
    void Start()
    {
        knifeManager = FindObjectOfType<KnifeManager>();
        aimGoldenPath.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorialStateTrigger == "sonarTutorialTrigger")
        {
            if(gameObject.GetComponent<KnifeManager>().knife == KnifeManager.KnifeType.sonarKnife)
            {
                sonarCG.GetComponent<FadeController>().FadeOutTutorial(sonarCG);
                sonarShown = true;
                if(knifeaimShown == false)
                {
                    knifeaimCG.GetComponent<FadeController>().FadeInTutorial(knifeaimCG);
                    aimGoldenPath.gameObject.SetActive(true);
                }                 
                knifeaimShown = true;               
            }
            
        }
        else if(tutorialStateTrigger == "Sonar" && knifeManager.currentKnife != null) //sonar and throw
        {
            if (knifeManager.currentKnife.GetComponent<KnifeBehavior>().GetIsThrown())
            {
                knifeaimCG.GetComponent<FadeController>().FadeOutTutorial(knifeaimCG);
                knifeabilityactivateCG.GetComponent<FadeController>().FadeInTutorial(knifeabilityactivateCG);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        tutorialStateTrigger = other.gameObject.name;
        switch(tutorialStateTrigger)
        {
            case "movementTutorialTrigger": //enters the movement tutorial area
            if(movementShown == false)
            {
                crouchCG.GetComponent<FadeController>().FadeOutTutorial(crouchCG);
                movementCG.GetComponent<FadeController>().FadeInTutorial(movementCG);
                audioSource.clip = streetAmbience;
                audioSource.loop = true;
                audioSource.Play();
            }            
            break;

            case "crouchTutorialTrigger":            
            if(crouchShown == false && movementShown)
            {
                movementCG.GetComponent<FadeController>().FadeOutTutorial(movementCG);
                crouchCG.GetComponent<FadeController>().FadeInTutorial(crouchCG);
                audioSource.clip = streetAmbience;
                audioSource.loop = true;
                audioSource.Play();
            }
            break;

            case "runTutorialTrigger":
            if(runShown == false && crouchShown)
            {
                enemyCG.GetComponent<FadeController>().FadeOutTutorial(enemyCG);
                movementCG.GetComponent<FadeController>().FadeOutTutorial(movementCG);
                runCG.GetComponent<FadeController>().FadeInTutorial(runCG);
                audioSource.clip = streetAmbience;
                audioSource.loop = true;
                audioSource.Play();
            }
            break;

            case "enemyTutorialTrigger":
            if(enemyShown == false && runShown)
            {
                sonarCG.GetComponent<FadeController>().FadeOutTutorial(sonarCG);
                enemyCG.GetComponent<FadeController>().FadeInTutorial(enemyCG);
                audioSource.Stop();
                audioSource.clip = buildingAmbience;
                audioSource.loop = true;
                audioSource.Play();
                //audioSource.PlayOneShot(buildingAmbience, 1f);
            }            
            break;

            case "sonarTutorialTrigger":
                if(sonarShown == false && enemyShown)
                {
                    distractionCG.GetComponent<FadeController>().FadeOutTutorial(distractionCG);
                    sonarCG.GetComponent<FadeController>().FadeInTutorial(sonarCG);
                    sonarIconCG.GetComponent<FadingUI>().FadeIn(1.0f);
                }            
            break;

            case "knifeabilityTriggerExit":
            knifeabilityactivateCG.GetComponent<FadeController>().FadeOutTutorial(knifeabilityactivateCG);
            knifeabilityactivateShown = true;
            break;

            case "distractionTutorialTrigger":
            if(distractionShown == false && sonarShown)
                {
                    knifeabilityactivateCG.GetComponent<FadeController>().FadeOutTutorial(knifeabilityactivateCG); 
                    distractionCG.GetComponent<FadeController>().FadeInTutorial(distractionCG);
                    distractionIconCG.GetComponent<FadingUI>().FadeIn(1.0f);
                    audioSource.Stop();
                    audioSource.clip = protestAmbience;
                    audioSource.loop = true;
                    audioSource.Play();
                    //audioSource.PlayOneShot(protestAmbience, 0.5f);
                }
            break;

            case "teleportTutorialTrigger":
            if(teleportShown == false && distractionShown)
                {
                    sonarCG.GetComponent<FadeController>().FadeOutTutorial(sonarCG);
                    distractionCG.GetComponent<FadeController>().FadeOutTutorial(distractionCG);
                    teleportCG.GetComponent<FadeController>().FadeInTutorial(teleportCG);
                    teleportIconCG.GetComponent<FadingUI>().FadeIn(1.0f);
                    audioSource.Stop();
                    audioSource.clip = protestAmbience;
                    audioSource.loop = true;
                    audioSource.Play();
                    //audioSource.PlayOneShot(protestAmbience, 1f);
                }            
            break;

            case "tutorialEndTrigger":
            teleportCG.GetComponent<FadeController>().FadeOutTutorial(teleportCG);
            fadeCG.GetComponent<FadeController>().FadeInTutorial(fadeCG);
            StartCoroutine(LoadLabLevelTimer());
            //or go to menu or lobby
            break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        tutorialStateTrigger = other.gameObject.name;
        switch(tutorialStateTrigger)
        {
            case "movementTutorialTrigger":
            movementCG.GetComponent<FadeController>().FadeOutTutorial(movementCG);
            movementShown = true;
            print("show movement tutorial");
            break;

            case "crouchTutorialTrigger":
            crouchCG.GetComponent<FadeController>().FadeOutTutorial(crouchCG);
            crouchShown = true;
            break;

            case "runTutorialTrigger":
            runCG.GetComponent<FadeController>().FadeOutTutorial(runCG);
            runShown = true;
            break;

            case "enemyTutorialTrigger":
            enemyCG.GetComponent<FadeController>().FadeOutTutorial(enemyCG);
            enemyShown = true;
            break;

            case "KnifeAimTutorial":
            knifeaimCG.GetComponent<FadeController>().FadeOutTutorial(knifeaimCG);
            break;

            case "knifeabilityTriggerExit":
            //knifeabilityactivateCG.GetComponent<FadeController>().FadeOutTutorial(knifeabilityactivateCG);
            break;

            case "distractionTutorialTrigger":
            distractionCG.GetComponent<FadeController>().FadeOutTutorial(distractionCG);
            distractionShown = true;
            break;

            case "teleportTutorialTrigger":
            teleportCG.GetComponent<FadeController>().FadeOutTutorial(teleportCG);
            teleportShown = true;
            break;
        }
    }

    private IEnumerator LoadLabLevelTimer()
    {
        yield return new WaitForSeconds(4);
        gameManager.LoadMainMenuLevel();
    }
}

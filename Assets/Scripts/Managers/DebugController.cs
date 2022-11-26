using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
   public bool showConsole;
   public GameObject player;
   [SerializeField] private GameObject debugMenu;
   [SerializeField] float y;
   public bool inMainMenu, inAreaCheats, inAbilitiesCheat;

   [Header("Areas Cheats")]
   [SerializeField] private Transform movementTutorialSpawn, enemiessonarTutorialSpawn, distractionTutorialSpawn, teleportTutorialSpawn, lobbySpawn, labSpawn, officeSpawn;

   [Header("Abilities Cheats")]
   [SerializeField] private bool resetSonarCooldown, resetDistractionCooldown, resetTeleportCooldown;
    
    void Start()
    {
        debugMenu.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void ToggleDebugMenu()
    {
        if(showConsole)
        {
            debugMenu.GetComponent<CanvasGroup>().alpha = 1f;     
            debugMenu.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1; 
            inMainMenu = true;
        }    

        else
        {
            debugMenu.GetComponent<CanvasGroup>().alpha = 0f;
            debugMenu.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0;
            debugMenu.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0;
            debugMenu.transform.GetChild(3).GetComponent<CanvasGroup>().alpha = 0;
            inMainMenu = false;
            inAreaCheats = false;
            inAbilitiesCheat = false;
        }  
    }

    public void ShowAreaCheats()
    {
        if(inMainMenu)
        {
            debugMenu.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0;
            debugMenu.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1;
            inMainMenu = false;
        }
    }

    public void ShowAbilitiesCheats()
    {
        if(inMainMenu)
        {
            debugMenu.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0;
            debugMenu.transform.GetChild(3).GetComponent<CanvasGroup>().alpha = 1;
            inMainMenu = false;
        }
    }

    public void F1()
    {
        if(inMainMenu)
        {
            ShowAreaCheats();
            inAreaCheats = true;
        }

        else if(inAreaCheats)
        {
            if(SceneManager.GetActiveScene().name == "Tutorial Scene")
            {
                player.transform.position = new Vector3(0f,0f,0f);
                print(movementTutorialSpawn.position);
                print("tp");
            }

            else
            {
                SceneManager.LoadScene("Tutorial Scene");
                player.transform.position = movementTutorialSpawn.position;
                print("new scene and tp");
            }
        }

        else if(inAbilitiesCheat)
        {
            //reset timer for sonar knife
        }
    }

    public void F2()
    {
        if(inMainMenu)
        {
            ShowAbilitiesCheats();
            inAreaCheats = true;
        }

        else if(inAreaCheats)
        {
            if(SceneManager.GetActiveScene().name == "Tutorial Scene")
            {
                player.transform.position = enemiessonarTutorialSpawn.position;
            }

            else
            {
                SceneManager.LoadScene("Tutorial Scene");
                player.transform.position = enemiessonarTutorialSpawn.position;
            }
        }

        else if(inAbilitiesCheat)
        {
            //reset timer for distraction knife
        }
    }

    public void F3()
    {
        if(inAreaCheats)
        {
            if(SceneManager.GetActiveScene().name == "Tutorial Scene")
            {
                player.transform.position = distractionTutorialSpawn.position;
            }

            else
            {
                SceneManager.LoadScene("Tutorial Scene");
                player.transform.position = distractionTutorialSpawn.position;
            }
        }

        else if(inAbilitiesCheat)
        {
            //reset timer for distraction knife
        }
    }

    public void F4()
    {
        if(inAreaCheats)
        {
            if(SceneManager.GetActiveScene().name == "Tutorial Scene")
            {
                player.transform.position = teleportTutorialSpawn.position;
            }

            else
            {
                SceneManager.LoadScene("Tutorial Scene");
                player.transform.position = teleportTutorialSpawn.position;
            }
        }
    }

    public void F5()
    {
        if(inAreaCheats)
        {
            if(SceneManager.GetActiveScene().name == "Lobby Scene")
            {
                player.transform.position = lobbySpawn.position;
            }

            else
            {
                SceneManager.LoadScene("Lobby Scene");
                player.transform.position = lobbySpawn.position;
            }
        }
    }

    public void F6()
    {
        if(inAreaCheats)
        {
            if(SceneManager.GetActiveScene().name == "Lab Scene")
            {
                player.transform.position = labSpawn.position;
            }

            else
            {
                SceneManager.LoadScene("Lab Scene");
                player.transform.position = labSpawn.position;
            }
        }
    }

    public void F7()
    {
        if(inAreaCheats)
        {
            if(SceneManager.GetActiveScene().name == "Office Scene")
            {
                player.transform.position = officeSpawn.position;
            }

            else
            {
                SceneManager.LoadScene("Office Scene");
                player.transform.position = officeSpawn.position;
            }
        }
    }
    

    public void GoToMovementTutorial()
    {
        //transform
    }
}

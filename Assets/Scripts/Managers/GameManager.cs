using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameOver = false;
    public bool gameStarted = false;
    public GameObject menuCanvas, playerCanvas, gameOverScreen;
    public FadeController fade;
    [SerializeField] private GameObject player;    
    [SerializeField] private RuntimeAnimatorController playermenuAnim, playerAnim;

    //Inventory
    private int numBadges = 0;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().name == "Tutorial Scene")
        {
            gameStarted = false;
            /*player.GetComponent<MovementInput>().enabled = false;  
            player.GetComponent<PlayerController>().enabled = false;
            player.transform.GetChild(5).gameObject.SetActive(false);  */
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Tutorial Scene")
        {
            /*gameStarted = false;
            player.GetComponent<MovementInput>().enabled = false;  
            player.GetComponent<PlayerController>().enabled = false;
            player.transform.GetChild(5).gameObject.SetActive(false);
            player.GetComponent<Animator>().runtimeAnimatorController = playermenuAnim;   
            player.GetComponent<Animator>().SetTrigger("IsGameMenu");*/
            playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
            gameOverScreen = playerCanvas.transform.GetChild(2).gameObject;
            gameOverScreen.GetComponent<CanvasGroup>().alpha = 0;   
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Menu Stuff
    public void PlayGame()
    {  
       fade.FadeOut(1f);
       StartCoroutine(StartTimerGame());
      //run timelines     
    }

    public void GivePlayerControls()
    {     
      player.GetComponent<MovementInput>().enabled = true;  
      player.GetComponent<Animator>().runtimeAnimatorController = playerAnim;
      player.transform.GetChild(5).gameObject.SetActive(true);
      player.GetComponent<PlayerController>().enabled = true;
    }

    public void ShowCredits()
    {  
       fade.FadeOut(1f);
       StartCoroutine(StartTimerGame());
      //run timelines     
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverScreen.GetComponent<CanvasGroup>().alpha = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(GameOverTimer());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Lobby Scene");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        print("restart game");
    }

    private IEnumerator StartTimerGame()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Tutorial Scene");
        
    }
    
    private IEnumerator GameOverTimer()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    //Inventory Tracking

    public void AddSecurityBadge()
    {
        numBadges += 1;
        Debug.Log("I have this many badges: " + numBadges);

    }

    public void LoadLabLevel()
    {
        SceneManager.LoadScene("LabScene");
    }

    public void LoadMainMenuLevel()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

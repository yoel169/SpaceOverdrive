using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{

    [SerializeField] float delayFromgameOver = 4f;


    public void LoadGameOver()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(delayFromgameOver);
        SceneManager.LoadScene("Game Over");
   
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void PlayAgain()
    {
        FindObjectOfType<GameSession>().ResetSesh();
        SceneManager.LoadScene("Pick Ship");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadPickPlayer()
    {
       
        SceneManager.LoadScene("Pick Ship");
    }

    public void LoadLSScreen()
    {
        FindObjectOfType<GameSession>().ResetGame();
        SceneManager.LoadScene("Save Load Menu");
    }

    public void LoadPlayerHUb()
    {
        SceneManager.LoadScene("Player Hub");
    }

    public void LoadLoading()
    {
        SceneManager.LoadScene("Loading");
    }

    public void LoadLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void LoadMGame()
    {
        SceneManager.LoadScene("M Game");
    }

    public void LoadHowTo()
    {
        SceneManager.LoadScene("HowTo");
    }
}

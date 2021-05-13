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
        SceneManager.LoadScene("Pick Player");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PickPlayer()
    {
        FindObjectOfType<GameSession>().ResetGame();
        SceneManager.LoadScene("Pick Player");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HowTo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI body;

    [Header("Body Text")]
    [SerializeField] string gameplay = "";
    [SerializeField] string controlBody ="";
    [SerializeField] string playerBody = "";
    [SerializeField] string abilitiesBody = "";
    [SerializeField] string enemiesBody = "";

    string[] bodyTexts;
    string[] titleTexts;

    int state = 0;
    int maxState;

    // Start is called before the first frame update
    void Start()
    {
        titleTexts = new string[] { "Gameplay", "Controls", "Player", "Abilities", "Enemies" };
        bodyTexts = new string[] { gameplay, controlBody, playerBody, abilitiesBody, enemiesBody };
        maxState = titleTexts.Length;
        UpdateText();
    }

    private void UpdateText()
    {
        title.text = titleTexts[state];
        body.text = bodyTexts[state];
    }

    public void Foward()
    {
        if (state == maxState - 1)
        {
            state = 0;
        }
        else
        {
            state++;
        }

        UpdateText();
    }

    public void Backwards()
    {
        if(state != 0)
        {
            state--;
        }
        else
        {
            state = maxState - 1;
        }

        UpdateText();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

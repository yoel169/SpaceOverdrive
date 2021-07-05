using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHub : MonoBehaviour
{
    [SerializeField] TMP_Dropdown levelDropDown;
    [SerializeField] TextMeshProUGUI[] text;
    [SerializeField] GameObject[] playerPrefabs;
    Chapter[] chapterPack;

    PlayerData playerData;
    int[] stats; //currentChapter, chapterProgression, currentLevel, levelProgression, currentLives, playtime, enemiesKilled 
    float[,] shipProgress;

    GameSession session;
    SceneSelector sceneSelect;

    int levelSelected;

    void Start()
    {
        session = FindObjectOfType<GameSession>();
        //playerData = session.GetPlayerData();
        sceneSelect = FindObjectOfType<SceneSelector>();

        levelDropDown.onValueChanged.AddListener(delegate {
            OnSelectChange(levelDropDown);
        });

        SetupHub();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupHub()
    {
        stats = playerData.GetStats();
        shipProgress = playerData.GetShipProgression();

        text[0].text = stats[2].ToString();
        text[1].text = stats[5].ToString();
        text[2].text = stats[6].ToString();
        text[3].text = playerData.GetName();

        int progress = stats[2];
        List<string> options = new List<string>();

        for(int x = 0; x < progress; x++)
        {
            options.Add((x + 1).ToString());
        }

        levelDropDown.AddOptions(options);
    }

    public void OnSelectChange(TMP_Dropdown change)
    {
        levelSelected = change.value;
        print(levelSelected);
    }

    public void ContinueStory()
    {
        if(levelSelected != stats[2])
        {
           
            //get current level to play and send it to game session
            Level lv =chapterPack[stats[0]].GetLevels()[stats[2]];
           // session.SetLevel(lv);

        }

        sceneSelect.LoadGameScene();
    }


}

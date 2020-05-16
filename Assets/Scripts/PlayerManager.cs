using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Hashtable playerHash = new Hashtable();

    [SerializeField] private TextAsset playerList;
    private string[] playerLines;
    private string[] playerData;

    [SerializeField] private InputField playerInput;

    [SerializeField]
    private GameObject errorPanel;

    [SerializeField]
    private GameObject playerInfoPanel;

    [SerializeField]
    private Text playerName;

    [SerializeField]
    private Text playerScore;

    [SerializeField]
    private Text playerLevel;

    private void Start()
    {
        playerLines = playerList.text.Split('\n');
        
        foreach(string line in playerLines)
        {
            playerData = line.Split(' ');
            Player player = new Player(playerData[0], playerData[1], playerData[2]);
            playerHash.Add(player.Playername, player);
        }
    }

    public void OnInputChanged()
    {
        Player player = CheckPlayer(playerInput.text);

        if (player == null)
        {
            errorPanel.SetActive(true);
            playerInfoPanel.SetActive(false);
            return;
        }

        else
        {
            
            errorPanel.SetActive(false);
            playerInfoPanel.SetActive(true);

            playerName.text = "Name: " + player.Playername;
            playerScore.text = "Score: " + player.score;
            playerLevel.text = "Level: " + player.level;
        }
    }

    public Player CheckPlayer(string input)
    {
        if (playerHash.Contains(input))
        {
            return playerHash[input] as Player;
        }

        return null;
    }

}

public class Player
{
    public string Playername = "";
    public string score = "";
    public string level = "";

    public Player(string name, string score, string level)
    {
        this.Playername = name;
        this.score = score;
        this.level = level;
    }
}
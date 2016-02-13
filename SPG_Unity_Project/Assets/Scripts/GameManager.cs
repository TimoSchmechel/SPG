using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private static Dictionary<string, int> teams = new Dictionary<string, int>();
    private static Dictionary<string, int> playerKills = new Dictionary<string, int>();
    private static Dictionary<string, int> playerDeaths = new Dictionary<string, int>();
    private static int teamCount = 0;
    private static Text scoreboard;

    void Start()
    {
        scoreboard = GameObject.Find("Scoreboard").GetComponent<Text>();
    }

    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void RegisterPlayer2(string name, Player _player)
    {
        string _playerID = name;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
        playerKills.Add(_playerID, 0);
        playerDeaths.Add(_playerID, 0);
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    public static void printPlayers()
    {
        int teamValue = 1;

        foreach (string key in players.Keys)
        {
            print(GetPlayer(key)+ "teamValue = "+teamValue);
            if(teamValue == 1)
            {
                teamValue = 2;
            }
            else
            {
                teamValue = 1;
            }
        }
    }

    public static int GetTeam(string _playerID)
    {
        return teams[_playerID];
    }

    public static void RegisterTeam(string name, Teams currentTeam)
    {
        if(teams.Count == 0)
        {
            print("teamCount is 0");
            teamCount = 1;
        }


        print("RegisterPlayer " + name + " teamCount " + teamCount);
        string _playerID = name;
        teams.Add(_playerID, teamCount);
        currentTeam.team = teamCount;
        if (teamCount == 1)
        {
            teamCount = 2;
            print("newTeamCount = " + teamCount);
        }
        else if(teamCount == 2)
        {
            teamCount = 1;
            print("newTeamCount = " + teamCount);
        }
    }
    public static void AddKill(string name)
    {
        int newKills = playerKills[name] + 1;
        playerKills.Remove(name);
        playerKills.Add(name, newKills);
    }

    public static void AddDeath(string name)
    {
        int newDeaths = playerDeaths[name] + 1;
        playerDeaths.Remove(name);
        playerDeaths.Add(name, newDeaths);
    }

    public static void PrintAllKD()
    {
        foreach(KeyValuePair<string, int> killsEntry in playerKills)
        {
            print(killsEntry.Key + " - kills: " + killsEntry.Value + " Deaths: " + playerDeaths[killsEntry.Key]);
        }
    }

    public static void UpdateScoreboard()
    {
        string scoreboardText = "";
        foreach (KeyValuePair<string, int> killsEntry in playerKills)
        {
            scoreboardText += killsEntry.Key + " - kills: " + killsEntry.Value + " Deaths: " + playerDeaths[killsEntry.Key] + "\n";
        }

        scoreboard.text = scoreboardText;
    }
}

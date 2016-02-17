using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreboardController : MonoBehaviour {

    private Text players;
    private Text kills;
    private Text deaths;

    void Start()
    {
        players = transform.GetChild(0).gameObject.GetComponent<Text>();
        kills = transform.GetChild(1).gameObject.GetComponent<Text>();
        deaths = transform.GetChild(2).gameObject.GetComponent<Text>();

        transform.localScale = Vector3.zero; //effectively hiding the scoreboard
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            transform.localScale = Vector3.one;
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            transform.localScale = Vector3.zero;
        }

    }

    public void SetScoreboard(string[] text)
    {
        if (text.Length == 3)
        {
            players.text = "Player\n" + text[0];



            kills.text = "Kills\n" + text[1];
            deaths.text = "Deaths\n" + text[2];
        } else
        {
            print("scoreboard format is wrong - " + text.Length);
        }
    }
}

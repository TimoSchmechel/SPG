using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Teams : NetworkBehaviour {
	int teamVal;
    public Player player;
    private bool playerTeamReady = false;

	[SyncVar]
	public int team;

    public Color colour;
    
    void startTeam()
    {
        GameManager.RegisterTeam(this.gameObject.name, this);
       // print("Local: the name = " + this.gameObject.name +" team NO: "+team);
        playerTeamReady = true;
    }
   
    
    void startTeamNonLocal()
    {
        GameManager.RegisterTeam(this.gameObject.name, this);
       // print("NonLocal: the name = " + this.gameObject.name + " team NO: " + team);
    }

    void Start () {
		teamVal = int.Parse(GetComponent<NetworkIdentity> ().netId.ToString ());
        player = GetComponent<Player>();

        if (isLocalPlayer) {
            //calls after player name is assigned
            Invoke("startTeam", 1);
        }
        else
        {
            Invoke("startTeamNonLocal", 1);
        }

        //colour = new Color(Random.Range(0, 255)/255f, Random.Range(0, 255)/255f, Random.Range(0, 255)/255f); //have to be in range 0 - 1 for some reason????

        // colour = Random.ColorHSV(); --> these random colours look shit

    }

    void Update()
    {
        if (playerTeamReady && isLocalPlayer)
        {
            CmdAssignTeam();
        }
    }
    

    [Command]
	void CmdAssignTeam(){
		RpcAssignTeam ();
	}

	[ClientRpc]
	void RpcAssignTeam(){
        if (team == 1)
        {
            //team = 1;
            player = GetComponent<Player>();
            player.nameText.color = Color.red;
            colour = Color.red;
        }
        else {
            //team = 2;
            player = GetComponent<Player>();
            player.nameText.color = Color.blue;
            colour = Color.blue;
        }
    }
}

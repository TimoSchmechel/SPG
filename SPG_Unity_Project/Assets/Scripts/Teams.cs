using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Teams : NetworkBehaviour {
	int teamVal;
    public Player player;

	[SyncVar]
	public int team;

    // Color[] teamColours = { new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255) };

    public Color colour;

	void Start () {
		teamVal = int.Parse(GetComponent<NetworkIdentity> ().netId.ToString ());
        player = GetComponent<Player>();

		if (isLocalPlayer) {
			CmdAssignTeam ();
		}

        //colour = new Color(Random.Range(0, 255)/255f, Random.Range(0, 255)/255f, Random.Range(0, 255)/255f); //have to be in range 0 - 1 for some reason????

       // colour = Random.ColorHSV(); --> these random colours look shit

    }
    /*
    public void UpdateColor()
    {
        CmdUpdateColor();
    }

    [Command]
    void CmdUpdateColor()
    {
        RpcUpdateColor();
    }

    [ClientRpc]
    void RpcUpdateColor()
    {
        int temp = Random.Range(0, 2);
        if (team == 1)
        {
            switch(temp)
            {
                case 0:
                    colour = Color.red;
                    break;
                case 1:
                    colour = Color.magenta;
                    break;
                case 2:
                    colour = Color.yellow;
                    break;
            }
        }
        else
        {
            switch (temp)
            {
                case 0:
                    colour = Color.blue;
                    break;
                case 1:
                    colour = Color.cyan;
                    break;
                case 2:
                    colour = Color.white;
                    break;
            }
        }
    }*/
	
	[Command]
	void CmdAssignTeam(){
		RpcAssignTeam ();
	}

	[ClientRpc]
	void RpcAssignTeam(){
        
		if (teamVal % 2 == 0) {
			team = 1;
            player.nameText.color = Color.red;
            colour = Color.red;
        } else {
			team = 2;
            player.nameText.color = Color.blue;
            colour = Color.blue;
        }

	}
}

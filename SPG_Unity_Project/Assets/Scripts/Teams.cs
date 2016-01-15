using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Teams : NetworkBehaviour {
	int teamVal;

	[SyncVar]
	public int team;

    // Color[] teamColours = { new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255) };

    public Color colour;

	void Start () {
		teamVal = int.Parse(GetComponent<NetworkIdentity> ().netId.ToString ());

		if (isLocalPlayer) {
			CmdAssignTeam ();
		}

        colour = new Color(Random.Range(0, 255)/255f, Random.Range(0, 255)/255f, Random.Range(0, 255)/255f); //have to be in range 0 - 1 for some reason????

       // colour = Random.ColorHSV(); --> these random colours look shit

    }
	
	[Command]
	void CmdAssignTeam(){
		RpcAssignTeam ();
	}

	[ClientRpc]
	void RpcAssignTeam(){
        
		if (teamVal % 2 == 0) {
			team = 1;
		} else {
			team = 2;
		}

	}
}

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Teams : NetworkBehaviour {
	int teamVal;
	[SyncVar]
	public int team;
	// Use this for initialization
	void Start () {
		teamVal = int.Parse(GetComponent<NetworkIdentity> ().netId.ToString ());
		if (isLocalPlayer) {
			CmdAssignTeam ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
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

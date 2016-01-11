using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

    //[SerializeField]
    //string remoteLayerName = "RemotePlayer";

	Camera sceneCam;

    // if not local player, disable components in array. set components in unity inspector
	void Start(){

        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer(); // changes layer name
		} else {
			sceneCam = Camera.main;
			if(sceneCam != null)
			{
				sceneCam.gameObject.SetActive(false);
			}
		}

	}
  
    // set player name
    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
        
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }



    void AssignRemoteLayer ()
    {
        //Changes layer name so that racast hit only hits remote players
        gameObject.layer = LayerMask.NameToLayer("RemotePlayer");


        //int layNum = 9;
        //gameObject.layer = LayerMask.LayerToName();

    }

	void OnDisable()
	{
		if(sceneCam != null)
		{
			sceneCam.gameObject.SetActive(true);
		}
        GameManager.UnRegisterPlayer(transform.name);
    }


}

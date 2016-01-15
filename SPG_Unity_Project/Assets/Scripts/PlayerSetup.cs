using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

    //[SerializeField]
    //string remoteLayerName = "RemotePlayer";

	Camera sceneCam;

    private string playerID;
    private string nameString;
    private PlayerSetup PS;
    private string instanceName;



    // if not local player, disable components in array. set components in unity inspector
    void Start()
    {
        PS = this;
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

        //nameString = GlobalScript.instanceName;
        nameString = instanceName;
        Cmd_UpdateNameServer(playerID, nameString);

	}



    //Sends updated name call across all clients. 
    public void Updater(string ID, string name)
    {
        if (isLocalPlayer)
        {
            Cmd_UpdateNameServer(ID, name);
        }
    }

    [Command]
    public void Cmd_UpdateNameServer(string ID, string name)
    {
        Rpc_UpdateNameClient(ID, name);
    }

    [ClientRpc]
    public void Rpc_UpdateNameClient(string ID, string name)
    {
        if (GameObject.Find(ID))
        {
            GameManager.UnRegisterPlayer(ID);
            GameObject tempObject = GameObject.Find(ID);
            tempObject.transform.name = name;
            GameManager.RegisterPlayer2(name, GetComponent<Player>());
        }
    }



    void Update()
    {
        PS.Updater(playerID, nameString);
    }

    // set player name
    public override void OnStartClient()
    {
        //GlobalScript.instanceName = PlayerPrefs.GetString(GlobalScript.ppPlayerNameKey);
        instanceName = PlayerPrefs.GetString(GlobalScript.ppPlayerNameKey);
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        playerID = "Player " + _netID;

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

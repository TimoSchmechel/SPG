using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class ChatControl : NetworkBehaviour {
    [SyncVar]
    public SyncListString textL = new SyncListString();
    Text list;
    // Use this for initialization
    void Start () {
        list = GameObject.Find("ChatText").GetComponent<Text>();
        textL.Callback = OnTextLChanged;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    

    private void OnTextLChanged(SyncListString.Operation op, int index)
    {
        list.text += "\n" + textL[index];
    }
}

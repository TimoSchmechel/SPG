using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerChat : NetworkBehaviour {
    public bool active = false;//if this is true the player is using that chat box and game inputs are disabled
    public ChatControl chat;
    public GameObject input;
    public EventSystem eventSystem;
    private Animator myAnimator;

    // Use this for initialization
    public override void OnStartClient () {
        input = GameObject.Find("ChatInput");
        chat = GameObject.Find("Chat").GetComponent<ChatControl>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        input.GetComponent<InputField>().Select();
        input.GetComponent<InputField>().OnPointerClick(new PointerEventData(EventSystem.current));
        input.SetActive(false);
        myAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                
                if(active == true)
                {
                    active = false;
                    GetComponent<CharacterController>().enabled = true;
                    gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
                    gameObject.GetComponent<PlayerShoot>().enabled = true;
                    gameObject.GetComponent<AnimationController>().enabled = true;
                    CmdSendText(input.GetComponent<InputField>().text, gameObject.name);
                    input.GetComponent<InputField>().text = "";
                    eventSystem.SetSelectedGameObject(null);
                    input.SetActive(false);
                }
                else if (gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_CharacterController.isGrounded)
                {
                    active = true;
                    GetComponent<CharacterController>().enabled = false;
                    gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
                    gameObject.GetComponent<PlayerShoot>().enabled = false;
                    gameObject.GetComponent<AnimationController>().enabled = false;
                    input.SetActive(true);
                    eventSystem.SetSelectedGameObject(input);

                    myAnimator.SetFloat("VSpeed", 0f);
                    myAnimator.SetFloat("HSpeed", 0f);
                }
            }
        }
	}

    [Command]
    public void CmdSendText(string text, string player)
    {
        if (text.Trim(' ', '\n', '\t').Length != 0)
        {
            string t = player + ":   " + text.Trim(' ', '\n', '\t');
            chat.textL.Add(t);
        }
    }

}

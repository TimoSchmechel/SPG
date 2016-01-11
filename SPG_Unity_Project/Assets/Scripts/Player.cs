using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    bool CursorLockedVar;
    //Total Max Health
    [SerializeField]
    private int maxHealth = 100;
    private int maxAmmo = 20;
    private int ammoShotCount = 1;


    //Current Health synced across servers
    [SyncVar]
    public int currentHealth;

    //[SyncVar]
    public int currentAmmo;

    public TextMesh nameText;

    //sets defaults when game loaded
    void Awake()
    {
        //sets mouse cursor to lock and hide
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        CursorLockedVar = (true);

        SetDefaults();
        nameText.text = PlayerPrefs.GetString(GlobalScript.ppPlayerNameKey);
    }

    // when any damage is taken lowers current health
    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");
    }

    public void useAmmo()
    {
        currentAmmo -= ammoShotCount;
        Debug.Log(transform.name + " now has " + currentAmmo + " ammo.");
    }
    
    //default values
    public void SetDefaults()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
    }

    public void setMaxAmmo(Slider ammo)
    {
        ammo.maxValue = maxAmmo;
    }

    void Update()
    {
        //toggles mouse cursor to lock and hide by pressing escape
        if (Input.GetKeyDown(KeyCode.Escape) && !CursorLockedVar)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = (false);
            CursorLockedVar = (true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && CursorLockedVar)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
            CursorLockedVar = (false);
        }
        //nameText.text = this.name.ToString();

    }

}

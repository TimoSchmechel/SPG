using System;
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
    public int magazineAmmo;
    public int magazineSize = 10;

    public TextMesh nameText;

    //sets defaults when game loaded
    void Awake()
    {
        //sets mouse cursor to lock and hide
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        CursorLockedVar = (true);

        SetDefaults();
        //nameText.text = PlayerPrefs.GetString(GlobalScript.ppPlayerNameKey);
    }

    void Start()
    {
        currentAmmo = maxAmmo;
        magazineAmmo = magazineSize;
    }

    // when any damage is taken lowers current health
    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");
    }

    public void useAmmo()
    {
        magazineAmmo -= ammoShotCount;
        Debug.Log(transform.name + " now has " + currentAmmo + " ammo.");
    }
    
    //default values
    public void SetDefaults()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
    }

    /*
    public void setMaxAmmo(Slider ammo)
    {
        ammo.maxValue = maxAmmo;
    }
    */

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
        nameText.text = this.name.ToString();
    }

    public void Reload()
    {
        //only reload if the magazine is not full and there is ammo
        if (magazineAmmo < magazineSize && currentAmmo > 0)
        {
            //is there enough bullets to reload mag?
            if (currentAmmo - (magazineSize - magazineAmmo) >= 0)
            {
                currentAmmo -= magazineSize - magazineAmmo;
                magazineAmmo = magazineSize;
            } else //not enough bullets to reload mag fully
            { 
                magazineAmmo += currentAmmo;
                currentAmmo = 0;
            }
        }
    }
}

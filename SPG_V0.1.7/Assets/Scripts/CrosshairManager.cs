using UnityEngine;
using System.Collections;

public class CrosshairManager : MonoBehaviour {

    public Crosshair activeCrosshair;

    public void AssignCrosshair(Crosshair c)
    {
        activeCrosshair = c;
    }

    void Start()
    {
        AssignCrosshair(this.gameObject.transform.Find("DefaultCrosshair").GetComponent<Crosshair>());
    }

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Crosshair : MonoBehaviour {

    public float maxSpread = 65;
    public float defaultSpread = 50;
    public float aimingSpread = 15;
    public float kickbackSpread = 15;

    public float spreadSpeed = 0.5f;

    public bool allowSpread = true;

    private float currentSpread = 0;
    private float targetSpread = 0;

    public Image[] parts;

    void Start()
    {
        currentSpread = defaultSpread;
        targetSpread = defaultSpread;

        for (int i = 0; i < parts.Length; i++)
        {
            MovePart(parts[i]);
        }
    }

    void Update()
    {
        if (allowSpread)
        {
   
            if (currentSpread != targetSpread)
            {
                Spread();
            }

        }
    }

    public void Expand()
    {
        targetSpread = aimingSpread;
    }

    public void Shrink()
    {
        targetSpread = defaultSpread;
    }

    public void ShootKickback()
    {
        currentSpread = (currentSpread + kickbackSpread > maxSpread ? maxSpread : currentSpread + kickbackSpread); ;
    }

    public void Spread()
    {
        for(int i = 0; i < parts.Length; i++)
        {
            MovePart(parts[i]);
        }
    }

    public void MovePart(Image part)
    {
        if (targetSpread < currentSpread)
        {
            currentSpread -= spreadSpeed * Time.deltaTime;
        } else if(targetSpread > currentSpread)
        {
            currentSpread += spreadSpeed * Time.deltaTime;
        }
       
        part.rectTransform.anchoredPosition = part.rectTransform.up * currentSpread;

      //  if(currentSpread + kickbackSpread)
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
}

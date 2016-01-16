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
    //
    private bool reachedTarget = true;
    private bool shrinking = false;

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

    public void Shrink()
    {
        reachedTarget = false;
        shrinking = true;
        targetSpread = aimingSpread;
    }

    public void Expand()
    {
        reachedTarget = false;
        shrinking = false;
        targetSpread = defaultSpread;
    }

    public void ShootKickback()
    {
        reachedTarget = false;
        shrinking = true;
        currentSpread = (currentSpread + kickbackSpread > maxSpread ? maxSpread : currentSpread + kickbackSpread); 
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
        if (!reachedTarget)
        {
            if (shrinking)
            {
                if (targetSpread < currentSpread)
                {
                    currentSpread -= spreadSpeed * Time.deltaTime;
                }
                else
                {
                    reachedTarget = true;
                }
            }

            if (!shrinking)
            {
                if (targetSpread > currentSpread)
                {
                    currentSpread += spreadSpeed * Time.deltaTime;
                }
                else
                {
                    reachedTarget = true;
                }
            }
        }

        part.rectTransform.anchoredPosition = part.rectTransform.up * currentSpread;
    }

    public float getCurrentSpread()
    {
        return currentSpread;
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

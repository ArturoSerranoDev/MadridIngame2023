using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentiladorPlayer : MonoBehaviour
{
    public float hot = 0, maxHot = 1;
    public bool hotting=false;
    public bool start = false;
    public bool lose = false;
    public Sprite sprite01, sprite02, sprite03, sprite04;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        hotting = true;
    }
    private void OnTriggerExit(Collider other)
    {
        hotting = false;
    }

    private void FixedUpdate()
    {
        if (!start)
            return;

        if (hot == 0)
        {
            spriteRenderer.sprite = sprite01;
        }
        else if (hot > 0 && hot < (maxHot / 2))
        {
            spriteRenderer.sprite = sprite02;
        }
        else if (hot > (maxHot / 2) && hot < (maxHot / 1))
        {
            spriteRenderer.sprite = sprite03;
        }
        else if (hot > (maxHot / 1) && hot > maxHot)
        {
            spriteRenderer.sprite = sprite04;
            lose = true;
        }
        
        if (hotting)
        {
            hot+=Time.deltaTime;
        }
        else
        {
            hot -= Time.deltaTime / 2f;
            
            if (hot < 0)
            {
                hot = 0;
            }
        }
    }
}

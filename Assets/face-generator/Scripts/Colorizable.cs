using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorizable : MonoBehaviour
{
    public Color mainColor { get; private set; }
    public SkinColorManager skinColorManager { get; private set; }
    public HairColorManager hairColorManager { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }

    [Range(0f, 1f)] public float brightness = 1f;

    public bool RGBChannelColored;
    public bool DropShadowEnabled;

    public enum ColorType
    {
        None,
        RGB,
        Skin,
        Shadow,
        Highlight,
        Hair
    }

    public ColorType colorType;

    void Start()
    {
        skinColorManager = transform.GetComponentInParent<SkinColorManager>();
        hairColorManager = transform.GetComponentInParent<HairColorManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Init();
    }
    
    // Use this for initialization
    public void Init()
    {
        if (!spriteRenderer) return;
        switch (colorType)
        {
            case ColorType.None:
                spriteRenderer.material.SetFloat("_DoNotColorize", 1f);
                break;
            case ColorType.RGB:
                spriteRenderer.material.SetFloat("_RGBColorize", 1f);
                break;
            case ColorType.Skin:
                spriteRenderer.color = new Color(skinColorManager.skinColor.r * brightness,skinColorManager.skinColor.g * brightness,skinColorManager.skinColor.b * brightness,1f);
                break;
            case ColorType.Shadow:
                spriteRenderer.color = new Color(skinColorManager.shadowColor.r * brightness,skinColorManager.shadowColor.g * brightness,skinColorManager.shadowColor.b * brightness,1f);
                break;
            case ColorType.Highlight:
                spriteRenderer.color = new Color(skinColorManager.highlightColor.r * brightness,skinColorManager.highlightColor.g * brightness,skinColorManager.highlightColor.b * brightness,1f);
                break;
            case ColorType.Hair:
                spriteRenderer.color = hairColorManager.hairColor ;
                break;
            default:
                spriteRenderer.color = Color.white;
                break;
        }
            
        spriteRenderer.material.SetFloat("_EnableDropShadow", DropShadowEnabled ? 1f : 0f);
        spriteRenderer.material.SetColor("_SkinColor", skinColorManager.skinColor);
        spriteRenderer.material.SetColor("_ShadowColor", skinColorManager.shadowColor);
        spriteRenderer.material.SetColor("_HighlightColor", skinColorManager.highlightColor);
        
    }
}
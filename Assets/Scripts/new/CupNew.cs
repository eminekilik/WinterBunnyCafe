using UnityEngine;

public class CupNew : MonoBehaviour
{
    public Sprite emptySprite;
    public Sprite filledSprite;

    SpriteRenderer sr;
    public bool isFilled;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = emptySprite;
        isFilled = false;
    }

    public void Fill()
    {
        isFilled = true;
        sr.sprite = filledSprite;
    }
}

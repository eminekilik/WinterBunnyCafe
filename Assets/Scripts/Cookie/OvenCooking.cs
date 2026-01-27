using UnityEngine;
using UnityEngine.UI;

public class OvenCooking : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite idleSprite;        // Boþ fýrýn
    public Sprite cookingSprite;     // Piþiyor
    public Sprite cookedSprite;      // Piþmiþ
    public Sprite burnedSprite;      // Yanmýþ

    [Header("Cook Bar")]
    public GameObject cookBarBG;
    public Image cookBarFill;

    [Header("Times")]
    public float cookTime = 4f;
    public float burnTime = 2f;

    SpriteRenderer sr;

    float timer;
    bool isCooking;
    bool isBurning;

    [Header("Burned Double Click")]
    public float doubleClickTime = 0.4f;
    float lastClickTime;
    int clickCount;

    [Header("Cookie")]
    public GameObject cookiePrefab;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip cookingStartSound;
    public AudioClip takeCookieSound;
    public AudioClip doneSound;
    public AudioClip burnSound;
    public AudioClip trashSound;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ResetOven();
    }

    public bool IsEmpty()
    {
        return !isCooking && !isBurning && sr.sprite == idleSprite;
    }

    public void StartCooking()
    {
        isCooking = true;
        timer = 0f;

        cookBarBG.SetActive(true);
        cookBarFill.fillAmount = 0f;
        cookBarFill.color = Color.green;

        sr.sprite = cookingSprite;

        if (audioSource != null && cookingStartSound != null)
            audioSource.PlayOneShot(cookingStartSound);
    }

    void Update()
    {
        if (isCooking)
        {
            timer += Time.deltaTime;
            cookBarFill.fillAmount = timer / cookTime;

            if (timer >= cookTime)
                FinishCooking();
        }
        else if (isBurning)
        {
            timer += Time.deltaTime;
            cookBarFill.fillAmount = timer / burnTime;

            if (timer >= burnTime)
                Burn();
        }
    }

    void FinishCooking()
    {
        isCooking = false;
        isBurning = true;
        timer = 0f;

        cookBarFill.fillAmount = 0f;
        cookBarFill.color = Color.red;

        sr.sprite = cookedSprite;

        if (audioSource != null && doneSound != null)
            audioSource.PlayOneShot(doneSound);
    }

    void Burn()
    {
        isBurning = false;

        cookBarBG.SetActive(false);
        cookBarFill.fillAmount = 0f;

        sr.sprite = burnedSprite;

        if (audioSource != null && burnSound != null)
            audioSource.PlayOneShot(burnSound);
    }

    void OnMouseDown()
    {
        if (sr.sprite == cookedSprite)
        {
            TakeCookie();
        }
        else if (sr.sprite == burnedSprite)
        {
            HandleBurnedDoubleClick();
        }
    }

    void TakeCookie()
    {
        CookieSlot slot = CookieSlotManager.Instance.GetFirstEmptySlot();
        if (slot == null) return;

        GameObject cookieObj = Instantiate(cookiePrefab);
        slot.PlaceCookie(cookieObj);

        ResetOven();

        if (audioSource != null && takeCookieSound != null)
            audioSource.PlayOneShot(takeCookieSound);
    }


    void HandleBurnedDoubleClick()
    {
        if (Time.time - lastClickTime < doubleClickTime)
            clickCount++;
        else
            clickCount = 1;

        lastClickTime = Time.time;

        if (clickCount >= 2)
        {
            ResetOven();
            clickCount = 0;

            if (audioSource != null && trashSound != null)
                audioSource.PlayOneShot(trashSound);
        }
        
    }

    void ResetOven()
    {
        isCooking = false;
        isBurning = false;
        timer = 0f;

        sr.sprite = idleSprite;

        cookBarBG.SetActive(false);
        cookBarFill.fillAmount = 0f;
    }
}

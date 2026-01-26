using UnityEngine;
using UnityEngine.UI;

public class PotCooking : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite cookingSprite;
    public Sprite cookedSprite;
    public Sprite burnedSprite;

    [Header("Cook Bar")]
    public GameObject cookBarBG;   // gri parent
    public Image cookBarFill;      // yeþil/kýrmýzý filled image

    [Header("Times")]
    public float cookTime = 3f;
    public float burnTime = 2f;

    SpriteRenderer sr;

    float timer;
    bool isCooking;
    bool isBurning;

    [Header("Burned Double Click")]
    public float doubleClickTime = 0.4f;

    float lastClickTime;
    int clickCount;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip cookingStartSound;
    public AudioClip fillCupSound;


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ResetPot();
    }

    void OnMouseDown()
    {
        Debug.Log("Tencereye týklandý");

        if (!isCooking && sr.sprite == idleSprite)
        {
            Debug.Log("Piþirme baþlýyor");
            StartCooking();
        }
        else if (!isCooking && sr.sprite == cookedSprite)
        {
            Debug.Log("Kupa doldurma");
            TryFillCup();
        }
        else if (sr.sprite == burnedSprite)
        {
            HandleBurnedDoubleClick();
        }
    }

    void HandleBurnedDoubleClick()
    {
        if (Time.time - lastClickTime < doubleClickTime)
        {
            clickCount++;
        }
        else
        {
            clickCount = 1;
        }

        lastClickTime = Time.time;

        if (clickCount >= 2)
        {
            Debug.Log("Yanmýþ tencere çöpe atýldý");
            ResetPot();
            clickCount = 0;
        }
    }

    void TryFillCup()
    {
        Cup cup = CupManager.Instance.GetFirstEmptyCup();
        if (cup == null) return;

        

        cup.Fill();
        ResetPot();

        if (audioSource != null && fillCupSound != null)
            audioSource.PlayOneShot(fillCupSound);
    }


    void Update()
    {
        if (isCooking)
        {
            timer += Time.deltaTime;
            cookBarFill.fillAmount = timer / cookTime;

            if (timer >= cookTime)
            {
                FinishCooking();
            }
        }
        else if (isBurning)
        {
            timer += Time.deltaTime;
            cookBarFill.fillAmount = timer / burnTime;

            if (timer >= burnTime)
            {
                Burn();
            }
        }
    }

    void StartCooking()
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

    void FinishCooking()
    {
        isCooking = false;
        isBurning = true;
        timer = 0f;

        cookBarFill.fillAmount = 0f;
        cookBarFill.color = Color.red;

        sr.sprite = cookedSprite;

        //StopCookingSound();
    }

    void Burn()
    {
        isBurning = false;

        cookBarBG.SetActive(false);
        cookBarFill.fillAmount = 0f;

        sr.sprite = burnedSprite;

        StopCookingSound();
    }

    void ResetPot()
    {
        isCooking = false;
        isBurning = false;
        timer = 0f;

        sr.sprite = idleSprite;

        cookBarBG.SetActive(false);
        cookBarFill.fillAmount = 0f;

        StopCookingSound();
    }

    void StopCookingSound()
    {
        if (audioSource != null)
            audioSource.Stop();
    }

}

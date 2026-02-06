using UnityEngine;
using UnityEngine.UI;

public class RestrictionUI : MonoBehaviour
{
    [Header("Roots")]
    public GameObject restriction1Root;
    public GameObject restriction2Root;

    [Header("Single Restriction Icon")]
    public Image restriction1Icon;

    [Header("Sprites")]
    public Sprite noTrashIcon;
    public Sprite noUnhappyCustomerIcon;

    void Start()
    {
        Setup();
    }

    void Setup()
    {
        restriction1Root.SetActive(false);
        restriction2Root.SetActive(false);

        if (LevelLoader.SelectedLevel == null ||
            LevelLoader.SelectedLevel.restrictions == null ||
            LevelLoader.SelectedLevel.restrictions.Length == 0)
            return;

        var restrictions = LevelLoader.SelectedLevel.restrictions;

        // ?? Tek kýsýtlama
        if (restrictions.Length == 1)
        {
            restriction1Root.SetActive(true);
            restriction1Icon.sprite = GetIcon(restrictions[0]);
        }
        // ?? Ýki veya daha fazla kýsýtlama
        else
        {
            restriction2Root.SetActive(true);
            // ikon deðiþtirme yok
        }
    }

    Sprite GetIcon(LevelRestriction restriction)
    {
        switch (restriction)
        {
            case LevelRestriction.NoTrash:
                return noTrashIcon;

            case LevelRestriction.NoUnhappyCustomer:
                return noUnhappyCustomerIcon;
        }

        return null;
    }
}

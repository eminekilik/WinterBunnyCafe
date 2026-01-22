using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;

    private GameObject lockOverlay;
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        if (lockOverlay == null)
        {
            Transform lockTransform = transform.Find("Lock");
            if (lockTransform != null)
            {
                lockOverlay = lockTransform.gameObject;
            }
        }
    }

    void Start()
    {
        UpdateState();
    }


    public void UpdateState()
    {
        bool unlocked = LevelProgressManager.Instance.IsLevelUnlocked(levelIndex);

        if (lockOverlay != null)
            lockOverlay.SetActive(!unlocked);

        button.interactable = unlocked;
    }

    public void OnClickLevel()
    {
        if (!button.interactable) return;
    }
}

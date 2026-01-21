using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;

    public GameObject lockOverlay;
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        UpdateState();
    }

    public void UpdateState()
    {
        bool unlocked = LevelProgressManager.Instance.IsLevelUnlocked(levelIndex);

        lockOverlay.SetActive(!unlocked);
        button.interactable = unlocked;
    }

    public void OnClickLevel()
    {
        if (!button.interactable) return;
    }
}

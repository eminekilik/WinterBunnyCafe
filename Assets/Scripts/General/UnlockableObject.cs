using UnityEngine;

public class UnlockableObject : MonoBehaviour
{
    [Header("Unlock Settings")]
    public int unlockLevel;

    void Start()
    {
        UpdateState();
    }

    public void UpdateState()
    {
        bool unlocked = LevelProgressManager.Instance
            .IsLevelUnlocked(unlockLevel);

        gameObject.SetActive(unlocked);

        if (DecorationManager.Instance != null)
            DecorationManager.Instance.UpdateStates();
    }
}

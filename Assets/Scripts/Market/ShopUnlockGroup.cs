using UnityEngine;

public class ShopUnlockGroup : MonoBehaviour
{
    public string saveKey;

    void Start()
    {
        int unlocked = PlayerPrefs.GetInt(saveKey, 0);

        int index = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(index < unlocked);
            index++;
        }
    }
}

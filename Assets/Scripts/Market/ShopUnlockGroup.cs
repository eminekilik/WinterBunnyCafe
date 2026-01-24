using UnityEngine;

public class ShopUnlockGroup : MonoBehaviour
{
    public string saveKey;
    public int shopStartIndex = 1;

    void Start()
    {
        int unlocked = PlayerPrefs.GetInt(saveKey, 0);

        for (int i = shopStartIndex; i < transform.childCount; i++)
        {
            bool shouldBeActive = (i - shopStartIndex) < unlocked;
            transform.GetChild(i).gameObject.SetActive(shouldBeActive);
        }
    }
}

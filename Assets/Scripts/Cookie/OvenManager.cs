using System.Collections.Generic;
using UnityEngine;

public class OvenManager : MonoBehaviour
{
    public static OvenManager Instance;

    public List<OvenCooking> ovens = new List<OvenCooking>();

    void Awake()
    {
        Instance = this;
    }

    public OvenCooking GetFirstEmptyOven()
    {
        for (int i = 0; i < ovens.Count; i++)
        {
            if (ovens[i] == null)
                continue;

            if (!ovens[i].gameObject.activeInHierarchy)
                continue;

            if (ovens[i].IsEmpty())
                return ovens[i];
        }

        return null;
    }

    public bool HasAnyActiveOven()
    {
        for (int i = 0; i < ovens.Count; i++)
        {
            if (ovens[i] == null)
                continue;

            if (ovens[i].gameObject.activeInHierarchy)
                return true;
        }

        return false;
    }

}

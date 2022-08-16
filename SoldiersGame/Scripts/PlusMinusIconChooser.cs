using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusMinusIconChooser : MonoBehaviour
{
    public static PlusMinusIconChooser instance;

    [SerializeField] private Sprite[] pluses;
    [SerializeField] private Sprite[] minuses;

    public enum IconType
    {
        Plus,
        Minus
    }

    void Awake()
    {
        instance = this;
    }

    public Sprite GetIcon(IconType iconType, int number)
    {
        switch (iconType)
        {
            case IconType.Plus:
                return pluses[number - 1];
            case IconType.Minus:
                return minuses[number - 1];
        }
        return pluses[0];
    }
}

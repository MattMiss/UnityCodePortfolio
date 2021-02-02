using UnityEngine;
using UnityEngine.UI;

public static class CraftWheelUtils{

    public static float[] SetIconOffsets(int amtOfBtns){
        float iconRotationOffset = 0f;
        float btnRotationOffset = 0f;

        switch (amtOfBtns)
        {
            case 3:
                iconRotationOffset = -30f;
                btnRotationOffset = 30f;
                break;
            case 4:
                iconRotationOffset = -45f;
                btnRotationOffset = 0f;
                break;
            case 5:
                iconRotationOffset = -54f;
                btnRotationOffset = -17f;
                break;
            case 6:
                iconRotationOffset = -60f;
                btnRotationOffset = -30f;
                break;
            case 7:
                iconRotationOffset = -64.25f;
                btnRotationOffset = -36f;
                break;
            case 8:
                iconRotationOffset = -67.5f;
                btnRotationOffset = -45f;
                break;
            default:
                break;     
        } 
        float[] offsets = new float[2];
        offsets[0] = iconRotationOffset;
        offsets[1] = btnRotationOffset;
        return offsets;

    }

    public static int GetHighlightedButton(int buttonAmt, float angle)
    {
        int selectedBtn = -1;
        
        switch (buttonAmt)
        {
            case 3:
                selectedBtn = ThreeButtonSelect(angle);
                break;
            case 4:
                selectedBtn = FourButtonSelect(angle);
                break;
            case 5:
                selectedBtn = FiveButtonSelect(angle);
                break;
            case 6:
                selectedBtn = SixButtonSelect(angle);
                break;
            case 7:
                selectedBtn = SevenButtonSelect(angle);
                break;
            case 8:
                selectedBtn = EightButtonSelect(angle);
                break;
            
            default:
                break;
        }

        //SetSelectedBtnIndex(selectedBtn);

        return selectedBtn;
    }

    public static int ThreeButtonSelect(float angle)
    {
        int selectedInt = -1;

        if ((angle > 120 && angle <= 180) || angle <= -120 && angle >= -180)
        {
            selectedInt = 0;
        }else if (angle < 0 && angle >= -120)
        {
            selectedInt = 2;
        }else if (angle >= 0 && angle <= 120)
        {
            selectedInt = 1;
        }
        return selectedInt;
    }

    public static int FourButtonSelect(float angle)
    {
        int selectedInt = -1;

        if ((angle > 135 && angle <= 180) || angle <= -135 && angle >= -180)
        {
            selectedInt = 0;
        }
        else if (angle > 45 && angle <= 135)
        {
            selectedInt = 1;
        }
        else if ((angle < 0 && angle >= -45) || angle >= 0 && angle <= 135)
        {
            selectedInt = 2;
        }
        else if (angle < -45 && angle >= -135)
        {
            selectedInt = 3;
        }
        return selectedInt;
    }

    public static int FiveButtonSelect(float angle)
    {
        int selectedInt = -1;

        if ((angle > 144 && angle <= 180) || angle <= -144 && angle >= -180)
        {
            selectedInt = 0;
        }
        else if (angle > 72 && angle <= 144)
        {
            selectedInt = 1;
        }
        else if (angle >= 0 && angle < 72)
        {
            selectedInt = 2;
        }
        else if (angle < 0 && angle >= -72)
        {
            selectedInt = 3;
        }
        else if (angle < -72 && angle >= -144)
        {
            selectedInt = 4;
        }
        return selectedInt;
    }

    public static int SixButtonSelect(float angle)
    {
        int selectedInt = -1;

        if ((angle > 150 && angle <= 180) || angle < -150 && angle >= -180)
        {
            selectedInt = 0;
        }
        else if (angle >= 90 && angle <= 150)
        {
            selectedInt = 1;
        }
        else if (angle >= 30 && angle < 90)
        {
            selectedInt = 2;
        }
        else if ((angle < 0 && angle >= -30) || angle >= 0 && angle < 30)
        {
            selectedInt = 3;
        }
        else if (angle < -30 && angle >= -90)
        {
            selectedInt = 4;
        }
        else if (angle < -90 && angle >= -150)
        {
            selectedInt = 5;
        }
        return selectedInt;
    }

    public static int SevenButtonSelect(float angle)
    {
        int selectedInt = -1;

        if ((angle > 154.5 && angle <= 180) || angle < -154.5 && angle >= -180)
        {
            selectedInt = 0;
        }
        else if (angle >= 103 && angle <= 154.5)
        {
            selectedInt = 1;
        }
        else if (angle >= 51.5 && angle < 103)
        {
            selectedInt = 2;
        }
        else if (angle >= 0 && angle < 51.5)
        {
            selectedInt = 3;
        }
        else if (angle < 0 && angle >= -51)
        {
            selectedInt = 4;
        }
        else if (angle < -51 && angle >= -103)
        {
            selectedInt = 5;
        }
        else if (angle < -103 && angle >= -154.5)
        {
            selectedInt = 6;
        }
        return selectedInt;
    }

    public static int EightButtonSelect(float angle)
    {
        int selectedInt = -1;

        if ((angle > 157.5 && angle <= 180) || angle < -157.5 && angle >= -180)
        {
            selectedInt = 0;
        }
        else if (angle >= 112.5 && angle <= 157.5)
        {
            selectedInt = 1;
        }
        else if (angle >= 67.5 && angle < 112.5)
        {
            selectedInt = 2;
        }
        else if (angle >= 22.5 && angle < 67.5)
        {
            selectedInt = 3;
        }
        else if ((angle < 0 && angle >= -22.5) || angle >= 0 && angle < 22.5)
        {
            selectedInt = 4;
        }
        else if (angle < -22.5 && angle >= -67.5)
        {
            selectedInt = 5;
        }
        else if (angle < -67.5 && angle >= -112.5)
        {
            selectedInt = 6;
        }
        else if (angle < -112.5 && angle >= -157.5)
        {
            selectedInt = 7;
        }
        return selectedInt;
    }


    public static void GetCraftBtnPosition(int amtOfBtns, int btnIndex, float offset, Vector3 centerPos, float radius, out Vector3 iconPosition, out Quaternion iconRotation){
        float angle = 360f / (float)amtOfBtns;

        iconRotation = Quaternion.AngleAxis(btnIndex * angle + offset, Vector3.forward);
        Vector3 iconDirection = iconRotation * Vector3.right;
        iconPosition = centerPos + (iconDirection * radius);
    }
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    public enum Type
    {
        _NONE = -1,

        BonusLife,
        PaddleSizeUp, PaddleSizeDown,
        PaddleSpeedUp, PaddleSpeedDown,
        BallPowerUp, BallSpeedUp, BallTriple,
        PaddleStopDebuff,

        _MAX
    }

    public Type itemType { get; private set; }

    public void InitializeItem(float x, float y, Type _itemType)
    {
        itemType = _itemType;
        transform.position = new Vector3(x, y, 0);
    }

    public void Destroy(bool used)
    {
        if (used)
        {
            // TODO : Fx효과
        }
        else
        {
            // TODO : Fx효과
        }

        Destroy(gameObject);
    }
}

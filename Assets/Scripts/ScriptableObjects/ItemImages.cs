using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemImageList", menuName = "ScriptableObjects/ItemImageList", order = 0)]
public class ItemImages : ScriptableObject
{
    public List<Sprite> List;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitModel : MonoBehaviour
{
    public SpriteRenderer sprender;
    [HideInInspector]
    public int handIndex;

    public void SetSprite(Sprite sprite){
        sprender.sprite = sprite;
    }

    public void SetHandIndex(int index){
        handIndex = index;
    }
}

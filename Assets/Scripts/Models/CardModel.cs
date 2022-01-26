using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour
{
    public SpriteRenderer sprender;
    public Sprite backSprite;
    public Sprite frontSprite;
    public bool shouldBeConcealed = true;
    public int value;
    public int denomination;

    public void SetCard(Sprite sprite, int value){
        frontSprite = sprite;
        sprender.sprite = frontSprite;
        denomination = value;
        this.value = NumToVal(value);
    }

    public void SetShouldBeConcealed(bool value){
        shouldBeConcealed = value;
        sprender.sprite = (shouldBeConcealed) ? backSprite : frontSprite;
    }

    public void SetOrder(int order){
        sprender.sortingOrder = order;
    }

    public Sprite GetFrontSprite(){
        return frontSprite;
    }

    private int NumToVal(int num) {
        if (num < 9) {//0 -> 8 are 2 -> 10
            return num + 2;
        } else if (num < 12) {//a face card
            return 10;
        }
        return -1;//ace is a flag
    }
}

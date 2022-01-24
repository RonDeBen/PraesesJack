using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour
{
    public SpriteRenderer sprender;
    public Sprite backSprite;
    private Sprite frontSprite;
    private bool shouldBeConcealed = true;
    public int value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCard(Sprite sprite, int value){
        frontSprite = sprite;
        sprender.sprite = frontSprite;
        this.value = value;
    }

    public void SetShouldBeConcealed(bool value){
        shouldBeConcealed = value;
        sprender.sprite = (shouldBeConcealed) ? backSprite : frontSprite;
    }

    public void SetOrder(int order){
        sprender.sortingOrder = order;
    }
}

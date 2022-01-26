using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModel : MonoBehaviour
{
    private List<CardModel> cards = new List<CardModel>();
    private List<int> values = new List<int>();
    private bool hasAnAce = false;

    public int Count(){
        return cards.Count;
    }

    public void Add(CardModel newCard, Vector3 startPos, Vector3 offset, bool isDoubleDown){
        cards.Add(newCard);
        newCard.SetOrder(cards.Count);
        newCard.transform.position = startPos + ((cards.Count - 1) * offset);
        if (isDoubleDown) {
            newCard.transform.Rotate(0f, 0f, 90f);
        }
        CalculateValues();
    }

    public void Clear(){
        foreach(CardModel card in cards){
            Destroy(card.gameObject);
        }
        hasAnAce = false;
        cards.Clear();
        values.Clear();
    }

    public CardModel GetCard(int cardIndex){
        return cards[cardIndex];
    }

    public List<int> CalculateValues(){
        values = new List<int>();
        int totalValue = 0;
        foreach(CardModel card in cards){
            if(card.value == -1){//an ace
                totalValue++;
                hasAnAce = true;
            }else{
                totalValue += card.value;
            }
        }
        values.Add(totalValue);
        if(hasAnAce && (totalValue + 10) <= 21){
            values.Add(totalValue + 10);
        }
        return values;
    }

    public int HighestValue(){
        return (values.Count == 1) ? values[0] : values[1];
    }

    public int LowestValue(){
        return values[0];
    }

    public void FlipHole(){//my favorite function name
        cards[1].SetShouldBeConcealed(false);
    }

    public bool HasAPair(){
        return ((cards.Count == 2) && (cards[0].denomination == cards[1].denomination));
    }

    public List<CardModel> GetCopyOfPair(){
        GameObject firstCardObj = Instantiate(cards[0].gameObject, Vector3.zero, Quaternion.identity);
        GameObject secondCardObj = Instantiate(cards[1].gameObject, Vector3.zero, Quaternion.identity);
        CardModel firstCard = firstCardObj.GetComponent<CardModel>();
        CardModel secondCard = secondCardObj.GetComponent<CardModel>();
        return new List<CardModel>{firstCard, secondCard};
    }

    public Sprite GetCardSprite(int cardIndex){
        return cards[cardIndex].GetFrontSprite();
    }

    public bool HasValueToDoubleDown(){
        return ((LowestValue() >= 9) && (LowestValue() <= 11));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModel : MonoBehaviour
{
    private List<CardModel> cards = new List<CardModel>();
    private List<int> values = new List<int>();
    private bool hasAnAce = false;

    public void AddCard(CardModel newCard){

    }

    public int Count(){
        return cards.Count;
    }

    public void Add(CardModel newCard, Vector3 startPos, Vector3 offset){
        cards.Add(newCard);
        newCard.SetOrder(cards.Count);
        newCard.transform.position = startPos + (cards.Count * offset);
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

    public void FlipHouseCard(){
        cards[1].SetShouldBeConcealed(false);
    }
}
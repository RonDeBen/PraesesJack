using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public DeckController dc;
    public PlayerController pc;
    public Vector3 houseStartPos, offset;
    // private List<CardModel> houseHand = new List<CardModel>();
    private HandModel houseHand = new HandModel();

    public void Deal(){
        houseHand.Clear();
        DealToPlayer(0);
        DealToHouse(false);
        DealToPlayer(0);
        DealToHouse(true);
    }

    public void DealToHouse(bool shouldConceal){
        CardModel newCard = dc.GetNextCard(shouldConceal);
        houseHand.Add(newCard, houseStartPos, offset);
        newCard.transform.position = houseStartPos + (houseHand.Count() * offset);
    }

    public void DealToPlayer(int hand){
        pc.ReceiveCard(dc.GetNextCard(false), hand);
    }

    public void DealersTurn(){
        houseHand.FlipHouseCard();
        houseHand.CalculateValues();
        while(houseHand.HighestValue() < 17){
            DealToHouse(false);
            houseHand.CalculateValues();
        }
        if(houseHand.LowestValue() > 21){
            Debug.Log("House busted, player wins");
        }else{
            pc.DetermineWinner();
        }
    }

    public int HouseValue(){
        return houseHand.HighestValue();
    }
}

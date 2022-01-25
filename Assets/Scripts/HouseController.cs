using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public DeckController deckCont;
    public PlayerController playerCont;
    public TextController textCont;
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
        CardModel newCard = deckCont.GetNextCard(shouldConceal);
        houseHand.Add(newCard, houseStartPos, offset);
        newCard.transform.position = houseStartPos + (houseHand.Count() * offset);
    }

    public void DealToPlayer(int hand){
        playerCont.ReceiveCard(deckCont.GetNextCard(false), hand);
    }

    public void DealersTurn(){
        houseHand.FlipHouseCard();
        houseHand.CalculateValues();
        while(houseHand.HighestValue() < 17){
            DealToHouse(false);
            houseHand.CalculateValues();
        }
        if(houseHand.LowestValue() > 21){
            textCont.SetOutcomeText("House busted, player wins");
        }else{
            playerCont.DetermineWinner();
        }
    }

    public int HouseValue(){
        return houseHand.HighestValue();
    }
}

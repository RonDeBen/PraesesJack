using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public DeckController deckCont;
    public PlayerController playerCont;
    public TextController textCont;
    public BettingController betCont;
    public Vector3 houseStartPos, offset;
    // private List<CardModel> houseHand = new List<CardModel>();
    private HandModel houseHand = new HandModel();

    public void Deal(){
        houseHand.Clear();
        DealToPlayer(0, false);
        DealToHouse(false);
        DealToPlayer(0, false);
        DealToHouse(true);
    }

    public void DealToHouse(bool shouldConceal){
        CardModel newCard = deckCont.GetNextCard(shouldConceal);
        houseHand.Add(newCard, houseStartPos, offset, false);
    }

    public void DealToPlayer(int hand, bool isDoubleDown){
        playerCont.ReceiveCard(deckCont.GetNextCard(false), hand, isDoubleDown);
    }

    public void DealersTurn(bool isDoubleDown){
        houseHand.FlipHouseCard();
        houseHand.CalculateValues();
        while(houseHand.HighestValue() < 17){
            DealToHouse(false);
            houseHand.CalculateValues();
        }
        if(houseHand.LowestValue() > 21){
            textCont.SetOutcomeText("House busted, player wins");
            betCont.WinBet(false, isDoubleDown);
        }else{
            playerCont.DetermineWinner(isDoubleDown);
        }
    }

    public int HouseValue(){
        return houseHand.HighestValue();
    }
}

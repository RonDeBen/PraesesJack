using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private List<HandModel> hands = new List<HandModel>();
    public Vector3 playerStartPos, offset, doubleDownOffset;
    public HouseController houseCont; 
    public BettingController betCont;
    public TextController textCont;
    public GameObject hitMePrefab, doubleDownPrefab;
    public Button clearBtn, betStandBtn;
    private List<GameObject> hitMeObjs = new List<GameObject>();
    private List<GameObject> doubleDownObjs = new List<GameObject>();
    private RaycastHit2D hit;
    private Vector2 touchPosWorld2D;
    private bool placedABet = false;
    private bool isDoublingDown;

    void Start(){
        HandModel newHand = new HandModel();
        hands.Add(newHand);
    }

    void FixedUpdate(){
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
            touchPosWorld2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            if (hit.collider != null) {
                if(hit.collider.gameObject.tag == "DoubleDown"){
                    DoubleDownModel dmm = hit.collider.gameObject.GetComponent<DoubleDownModel>();
                    HitMe(dmm.handIndex, true);
                }
                if(hit.collider.gameObject.tag == "HitMe"){
                    HitMeModel hmm = hit.collider.gameObject.GetComponent<HitMeModel>();
                    HitMe(hmm.handIndex, false);
                }
                if(hit.collider.gameObject.tag == "Insurance"){
                    //do insurance stuff
                    textCont.SetOutcomeText("Insure up to half your bet");
                    SetCanBetButton(false);
                    betCont.SetBetButtons(true);
                    betCont.SetIsInsuring(true);
                }
            }
        }
    }

    public void DetermineWinner(bool isDoubleDown){
        foreach(HandModel hand in hands){
            if(hand.HighestValue() == houseCont.HouseValue()){
                textCont.SetOutcomeText("It's a tie");
            }else if(hand.HighestValue() > houseCont.HouseValue()){
                textCont.SetOutcomeText("Player Wins");
                betCont.WinBet(false, isDoubleDown);
            }else{
                betCont.LoseBet(false, isDoubleDown);
                textCont.SetOutcomeText("Player Loses");
            }
        }
    }

    public void HitMe(int handIndex, bool isDoubleDown){
        houseCont.DealToPlayer(handIndex, isDoubleDown);
        CheckHand(handIndex, isDoubleDown);
    }

    public void OnBetStandButtonPressed(){
        if(placedABet){//is the stand button
            Stand(false);
        }else{//is the place bet button
            placedABet = true;
            betCont.SetBetButtons(false);
            clearBtn.interactable = false;
            textCont.SetOutcomeText("");
            textCont.SetBetStandText("STAND");
            ClearHands();
            houseCont.Deal();
        }
    }
    
    public void Stand(bool isDoubleDown){
        houseCont.DealersTurn(isDoubleDown);
        clearBtn.interactable = true;
        placedABet = false;
        textCont.SetBetStandText("PLACE BET");
    }

    public void ReceiveCard(CardModel newCard, int handIndex, bool isDoubleDown){
        hands[handIndex].Add(newCard, playerStartPos, offset, isDoubleDown); 
        if(hitMeObjs.Count < hands.Count){
            GameObject go = Instantiate(hitMePrefab, Vector3.zero, Quaternion.identity);
            HitMeModel hmm = go.GetComponent<HitMeModel>();
            hmm.handIndex = handIndex;
            hitMeObjs.Add(go);
        }
        if(!isDoubleDown && CanHit(handIndex)){
            hitMeObjs[handIndex].transform.position = playerStartPos + (hands[handIndex].Count() * offset);
        }else{
            hitMeObjs[handIndex].transform.position = new Vector3(100f, 100f, 0f);
            if(!HasABust(handIndex) && isDoubleDown){
                Stand(isDoubleDown);
            }
        }
    }

    public void ClearHands(){
        foreach(HandModel hand in hands){
            hand.Clear();
        }
        hands.Clear();
        HandModel newHand = new HandModel();
        hands.Add(newHand);
    }

    public void CheckHand(int handIndex, bool isDoubleDown){
        if(HasABust(handIndex)){
            Busted(isDoubleDown);
        }
        if(HasABlackJack(handIndex)){
            GotABlackJack(isDoubleDown);
        }
        CheckForDoubleDown(handIndex);
    }

    public bool HasABust(int handIndex){
        return hands[handIndex].LowestValue() > 21;
    }

    public bool HasABlackJack(int handIndex){
        return hands[handIndex].HighestValue() == 21;
    }

    public void CheckForDoubleDown(int handIndex){
        if (doubleDownObjs.Count < hands.Count) {
            GameObject go = Instantiate(doubleDownPrefab, Vector3.zero, Quaternion.identity);
            DoubleDownModel ddm = go.GetComponent<DoubleDownModel>();
            ddm.handIndex = handIndex;
            doubleDownObjs.Add(go);
        }
        if(CanDoubleDown(handIndex)){  
            doubleDownObjs[handIndex].transform.position = playerStartPos + doubleDownOffset;
        }else{
            doubleDownObjs[handIndex].transform.position = new Vector3(100f, 100f, 0f);
        }
    }

    public bool CanDoubleDown(int handIndex){
        return (!HasABlackJack(handIndex) && (hands[handIndex].Count() == 2) && betCont.CanAffordToDoubleDown());
    }

    public void GotABlackJack(bool isDoubleDown){
        FinishedBeforeHouse();
        houseCont.CheckForStandOff();
        // betCont.WinBet(true, isDoubleDown);
        // textCont.SetOutcomeText("You Got a BlackJack!!");
    }

    public void Busted(bool isDoubleDown){
        FinishedBeforeHouse();
        betCont.LoseBet(false, isDoubleDown);
        textCont.SetOutcomeText("You Busted");
    }

    public void FinishedBeforeHouse(){
        clearBtn.interactable = true;
        placedABet = false;
        textCont.SetBetStandText("PLACE BET");
        if(betCont.isInsuring){
            houseCont.CheckInsuranceBet();
        }
    }

    private bool CanHit(int handIndex){
        return (hands[handIndex].LowestValue() < 21);
    }

    public void SetCanBetButton(bool truth){
        betStandBtn.interactable = truth;
    }

    public bool CanClearBet(){
        return !placedABet;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // private List<CardModel> hand = new List<CardModel>();
    private List<HandModel> hands = new List<HandModel>();
    public Vector3 playerStartPos, offset;
    public HouseController houseCont; 
    public BettingController betCont;
    public TextController textCont;
    public GameObject hitMePrefab;
    public Button clearBtn, betStandBtn;
    private List<GameObject> hitMeObjs = new List<GameObject>();
    private RaycastHit hit;
    private bool placedABet = false;

    void Start(){
        HandModel newHand = new HandModel();
        hands.Add(newHand);
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown("space")) {
            ClearHands();
            houseCont.Deal();
        }
        if(Input.GetKeyDown("z")){
            HitMe(0);
        }
        if(Input.GetKeyDown("x")){
            Stand();
        }
    }

    void FixedUpdate(){
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast (ray, out hit, 100)){
                if (hit.collider != null && hit.collider.gameObject.tag == "HitMe") {
                    HitMeModel hmm = hit.collider.gameObject.GetComponent<HitMeModel>();
                    HitMe(hmm.handIndex);
                }
            }
        }
    }

    public void DetermineWinner(){
        foreach(HandModel hand in hands){
            if(hand.HighestValue() == houseCont.HouseValue()){
                textCont.SetOutcomeText("It's a tie");
            }else if(hand.HighestValue() > houseCont.HouseValue()){
                textCont.SetOutcomeText("Player Wins");
                betCont.WinBet(false);
            }else{
                betCont.LoseBet(false);
                textCont.SetOutcomeText("Player Loses");
            }
        }
    }

    public void HitMe(int handIndex){
        houseCont.DealToPlayer(handIndex);
    }

    public void OnBetStandButtonPressed(){
        if(placedABet){//is the stand button
            clearBtn.interactable = true;
            placedABet = false;
            textCont.SetBetStandText("PLACE BET");
            Stand();
        }else{//is the place bet button
            clearBtn.interactable = false;
            textCont.SetOutcomeText("");
            placedABet = true;
            textCont.SetBetStandText("STAND");
            ClearHands();
            houseCont.Deal();
        }
    }
    
    public void Stand(){
        houseCont.DealersTurn();
    }

    public void ReceiveCard(CardModel newCard, int handIndex){
        hands[handIndex].Add(newCard, playerStartPos, offset); 
        CheckHand(handIndex); 
        if(hitMeObjs.Count < hands.Count){
            GameObject go = Instantiate(hitMePrefab, Vector3.zero, Quaternion.identity);
            HitMeModel hmm = go.GetComponent<HitMeModel>();
            hmm.handIndex = handIndex;
            hitMeObjs.Add(go);
        }
        if(CanHit(handIndex)){
            hitMeObjs[handIndex].transform.position = playerStartPos + ((hands[handIndex].Count() + 1) * offset);
        }else{
            hitMeObjs[handIndex].transform.position = new Vector3(100f, 100f, 0f);
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

    public void CheckHand(int handIndex){
        CheckForBust(handIndex);
        CheckForBlackJack(handIndex);
    }

    public void CheckForBust(int handIndex){
        if(hands[handIndex].LowestValue() > 21){
            Busted();
        }
    }

    public void CheckForBlackJack(int handIndex){
        if(hands[handIndex].HighestValue() == 21){
            GotABlackJack();
        }
    }

    public void GotABlackJack(){
        FinishedBeforeHouse();
        betCont.WinBet(true);
        textCont.SetOutcomeText("You Got a BlackJack!!");
    }

    public void Busted(){
        FinishedBeforeHouse();
        betCont.LoseBet(false);
        textCont.SetOutcomeText("You Busted");
    }

    public void FinishedBeforeHouse(){
        clearBtn.interactable = true;
        placedABet = false;
        textCont.SetBetStandText("PLACE BET");
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

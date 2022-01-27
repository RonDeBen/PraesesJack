using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutController : MonoBehaviour
{
    public float gameWidthCent, gameHeightCent, chipsWidthCent, chipsHeightCent;
    public float playerXOffsetCent, playerYOffsetCent, houseXOffsetCent, houseYOffsetCent, outcomeXOffsetCent, outcomeYOffsetCent, outcomeWidthCent;
    private float screenWidth, screenHeight;
    public RectTransform gameRect, chipsRect;
    public PlayerController playerCont;
    public HouseController houseCont;
    public TextController textCont;
    // Start is called before the first frame update
    void Start(){
         Camera cam = Camera.main;

        screenWidth = cam.pixelWidth;
        screenHeight = cam.pixelHeight;

        float width = gameWidthCent * screenWidth;
        float height = gameHeightCent * screenHeight;

        gameRect.sizeDelta = new Vector2(width, height);
        gameRect.anchoredPosition = new Vector2(width / 2f, -(height / 2f));

        width = chipsWidthCent * screenWidth;
        height = chipsHeightCent * screenHeight;

        chipsRect.sizeDelta = new Vector2(width, height);
        chipsRect.anchoredPosition = new Vector2(width / 2f, (height / 2f));

        Vector3 topLeft = cam.ScreenToWorldPoint(new Vector3(0f, cam.pixelHeight, cam.nearClipPlane));
        Vector3 bottomRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0f, cam.nearClipPlane));

        float boxWidth = bottomRight.x - topLeft.x;
        float boxHeight = topLeft.y - bottomRight.y;
        
        float playerX = topLeft.x + boxWidth * playerXOffsetCent;
        float playerY = topLeft.y - boxHeight * playerXOffsetCent;

        float houseX = topLeft.x + boxWidth * houseXOffsetCent;
        float houseY = topLeft.y - boxHeight * houseYOffsetCent;

        float outcomeX = topLeft.x + boxWidth * outcomeXOffsetCent;
        float outcomeY = topLeft.y - boxHeight * outcomeYOffsetCent;

        playerCont.SetStartPosition(new Vector3(playerX, playerY, 0f));
        houseCont.SetStartPosition(new Vector3(houseX, houseY, 0f));
        textCont.SetOutcomeTextConstraints(new Vector3(outcomeX, outcomeY, 0f), (outcomeWidthCent * boxWidth));
    }
}

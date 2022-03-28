using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    [SerializeField] private RectTransform mainCanvasRT;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private int reelId;
    private int finalScreenNumber = 0;
    private int currentFinalSymbol = 0;

    [SerializeField] private RectTransform[] reelSymbols;
    [SerializeField] private float endPosition;

    private float mainCanvasScale;

    private float symbolHeigth;

    internal bool isFinalSpin = false;

    internal RectTransform[] ReelSymbols { get => reelSymbols; }

    private void Start()
    {
        symbolHeigth = reelSymbols[0].rect.height;
        mainCanvasScale = mainCanvasRT.lossyScale.y;
        foreach (var symbol in reelSymbols)
        {
            ChangeSprite(symbol);
        }
    }

    private void Update()
    {
        for (var i = 0; i < reelSymbols.Length; i++)
        {
            var reelT = reelSymbols[i].transform;

            if (reelT.position.y <= -endPosition * mainCanvasScale)
            {
                MoveTop(reelT);
                ChangeSprite(reelT);
            }
        }
    }

    private void MoveTop(Transform reelT)
    {
        var topSymbolPosition = reelT.localPosition.y + symbolHeigth * reelSymbols.Length;
        var topPosition = new Vector3(reelT.localPosition.x, topSymbolPosition);
        reelT.localPosition = topPosition;
    }

    private void ChangeSprite(Transform reelT)
    {
        if (isFinalSpin)
        {
            reelT.GetComponent<Image>().sprite = GetFinalSprite();
        }
        else
        {
            reelT.GetComponent<Image>().sprite = GetRandomSprite();
        }
    }

    private Sprite GetFinalSprite()
    {
        var finalScreenItemIndex = currentFinalSymbol + (reelId - 1) * 3;
        var currentFinalScreen = gameConfig.FinalScreens[finalScreenNumber].FinalScreenData;
        if (finalScreenItemIndex >= currentFinalScreen.Length)
        {
            finalScreenItemIndex = 0;
        }
        var newSymbol = gameConfig.GameSprites[currentFinalScreen[finalScreenItemIndex]];
        currentFinalSymbol++;
        return newSymbol.SpriteImage;
    }

    private Sprite GetRandomSprite()
    {
        int randomSymbol = Random.Range(0, gameConfig.GameSprites.Length);
        var sprite = gameConfig.GameSprites[randomSymbol].SpriteImage;
        return sprite;
    }

    public void ResetPosition(float spinnerPosition)
    {
        ResetValues();
        foreach (var symbol in reelSymbols)
        {
            var reelPos = symbol.transform.localPosition;
            var correction = Mathf.Round(reelPos.y - spinnerPosition);
            var correctedPos = new Vector3(reelPos.x, correction);
            symbol.transform.localPosition = correctedPos;
        }
    }

    private void ResetValues()
    {
        currentFinalSymbol = 0;
        if (finalScreenNumber < gameConfig.FinalScreens.Length - 1)
        {
            finalScreenNumber++;
        }
        else
        {
            finalScreenNumber = 0;
        }
    }
}

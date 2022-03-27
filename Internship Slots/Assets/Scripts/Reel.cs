using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    [SerializeField] private RectTransform mainCanvasRT;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] private RectTransform[] reelSymbols;
    [SerializeField] private float endPosition;

    private float mainCanvasScale;

    private float symbolHeigth;

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
            var reelRT = reelSymbols[i].transform;

            if (reelRT.position.y <= -endPosition * mainCanvasScale)
            {
                MoveTop(reelRT);
                ChangeSprite(reelRT);
            }
        }
    }

    private void MoveTop(Transform reelRT)
    {
        var topSymbolPosition = reelRT.localPosition.y + symbolHeigth * reelSymbols.Length;
        var topPosition = new Vector3(reelRT.localPosition.x, topSymbolPosition);
        reelRT.localPosition = topPosition;
    }

    private void ChangeSprite(Transform reelRT)
    {
        reelRT.GetComponent<Image>().sprite = GetRandomSprite();
    }

    private Sprite GetRandomSprite()
    {
        int randomSymbol = Random.Range(0, sprites.Length);
        var sprite = sprites[randomSymbol];
        return sprite;
    }

    public void ResetPosition(float spinnerPosition)
    {
        foreach (var symbol in reelSymbols)
        {
            var reelPos = symbol.transform.localPosition;
            var correction = Mathf.Round(reelPos.y - spinnerPosition);
            var correctedPos = new Vector3(reelPos.x, correction);
            symbol.transform.localPosition = correctedPos;
        }
    }
}

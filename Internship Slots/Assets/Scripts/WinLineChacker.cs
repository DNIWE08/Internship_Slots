using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class WinLineChacker : MonoBehaviour
{
    [SerializeField] private GameConfig gameConfig;

    [SerializeField] private Reel[] reels;
    private WinLineConfig[] winLinesData;

    public static event Action OnReelsStop;

    private void Start()
    {
        winLinesData = gameConfig.WinLines;
        OnReelsStop += WinLinesAnimation;
    }

    public List<RectTransform> CheckWinLines()
    {
        List<RectTransform> winItems = new List<RectTransform>();
        RectTransform[] checkWinLine = new RectTransform[3];
        foreach (var line in winLinesData)
        {
            for(var i = 0; i < line.WinLine.Length; i++)
            {
                var currentReelSymbol = reels[i].ReelSymbols[line.WinLine[i]];
                checkWinLine[i] = currentReelSymbol;
                //print(line.WinLine[i]);
                //winItems.Add(currentReelSymbol);
                //print(currentReelSymbol.GetComponent<Image>().name);
                //var winSymbol = 0;
            }
            if(checkWinLine[0].GetComponent<Image>().sprite == checkWinLine[1].GetComponent<Image>().sprite &&
                checkWinLine[1].GetComponent<Image>().sprite == checkWinLine[2].GetComponent<Image>().sprite)
            {
                winItems.Add(checkWinLine[0]);
                winItems.Add(checkWinLine[1]);
                winItems.Add(checkWinLine[2]);
            }
        }

        return winItems;
    }

    public void WinLinesAnimation()
    {
        var winSymbols = CheckWinLines();
        if(winSymbols.Count > 0)
        {
            foreach(var symbol in winSymbols)
            {
                symbol.DOScale(1.2f, 0.4f)
                    .SetLoops(4, LoopType.Yoyo)
                    .OnComplete(() =>
                    {
                        FillSymbols(winSymbols, Color.white);
                    });
                FillSymbols(winSymbols, Color.grey);
            }
        }
    }

    private void FillSymbols(List<RectTransform> winSymbols, Color color)
    {
        for (var i = 0; i < reels.Length; i++)
        {
            foreach (var reelSymbol in reels[i].ReelSymbols)
            {
                if (reelSymbol != winSymbols[i])
                {
                    reelSymbol.GetComponent<Image>().color = color;
                }
            }
        }
    }

    public static void StartCheckAnimation()
    {
        OnReelsStop?.Invoke();
    }
}

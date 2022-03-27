using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReelSpinner : MonoBehaviour
{
    [SerializeField] private Reel[] reelsSpinner;

    private int spinIteration;

    private void Start()
    {
        spinIteration = -200 * 4;
    }

    public void Spin()
    {
        for (int i = 0; i < reelsSpinner.Length; i++)
        {
            var reelSpinner = reelsSpinner[i].transform;
            reelSpinner.DOLocalMoveY(spinIteration * 5, 2f)
                .SetEase(Ease.InOutCubic)
                .SetDelay(i * 0.5f)
                .OnComplete(() =>
                {
                    PrepareReel(reelSpinner);
                });
        }
    }

    private void PrepareReel(Transform reelT)
    {
        var prevReelPosY = reelT.localPosition.y;
        var traveledReelDistance = -(0 + prevReelPosY);
        reelT.localPosition = new Vector3(reelT.localPosition.x, 0);
        reelT.GetComponent<Reel>().ResetPosition(traveledReelDistance);
    }
}

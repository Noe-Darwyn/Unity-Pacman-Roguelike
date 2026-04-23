using System;
using System.Collections.Generic;
using UnityEngine;
using TemporaryUpgradeCardSpace;

public class UpgradeChoiceUI : MonoBehaviour
{
    [SerializeField] private UpgradeCardUI[] cards;

    private Action<TemporaryUpgradeCard> onSelected;

    public void Show(List<TemporaryUpgradeCard> choices, Action<TemporaryUpgradeCard> callback)
    {
        gameObject.SetActive(true);
        onSelected = callback;

        for (int i = 0; i < cards.Length; i++)
        {
            if (i < choices.Count)
            {
                cards[i].gameObject.SetActive(true);
                cards[i].Setup(choices[i], OnCardClicked);
            }
            else
            {
                cards[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnCardClicked(TemporaryUpgradeCard card)
    {
        onSelected?.Invoke(card);
        gameObject.SetActive(false);
    }
}
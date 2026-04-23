using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using TemporaryUpgradeCardSpace;

public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button button;

    private TemporaryUpgradeCard currentCard;

    public void Setup(TemporaryUpgradeCard card, Action<TemporaryUpgradeCard> onClick)
    {
        if (card == null)
        {
            Debug.LogError("UpgradeCardUI received null card!");
            return;
        }
        currentCard = card;

        icon.sprite = card?.temporaryUpgradeSprite;
        title.text = card?.temporaryUpgradeName;
        description.text = card?.temporaryUpgradeDescription;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick(card));
    }
}
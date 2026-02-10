using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using System.Collections.Generic;

public class MenuUI : MonoBehaviour
{
    //[SerializeField] private Image MenuLogo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Transform PacmanTransform;
    public float pacAnimationDuration = 1f;
    [SerializeField] private List<GameObject> MainMenuPanels;
    public float beginPosition;
    void Start()
    {
        /*if (MenuLogo != null)
        {
            MenuLogo.gameObject.transform.DOLocalMoveY(50f, 1f).SetLoops(-1, LoopType.Yoyo);//.SetEase(Ease.InOutSine);
        }*/
        if (PacmanTransform == null)
        {
            Debug.LogError("PacmanTransform is not assigned in the inspector!");
        } else
        {
            PacmanTransform.localPosition = new Vector3(beginPosition, PacmanTransform.localPosition.y, PacmanTransform.localPosition.z);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton(Transform buttonTransform)
    {
        Debug.Log("Button Clicked");
        if (buttonTransform != null)
        {
            PacmanTransform.localPosition = new Vector3(beginPosition, PacmanTransform.localPosition.y, PacmanTransform.localPosition.z);
            
            Sequence mainSequence = DOTween.Sequence();
            mainSequence.Append(buttonTransform.DOScale(0.9f, 0.1f));
            mainSequence.Append(PacmanTransform.DOMoveX(beginPosition/2, pacAnimationDuration, true).SetEase(Ease.Linear));
            mainSequence.AppendCallback(() =>
            {
                foreach (Transform child in buttonTransform.parent)
                {
                    child.DOScale(0f, 0f).SetEase(Ease.InBounce);
                    child.gameObject.SetActive(false);
                }
            });
            mainSequence.Append(PacmanTransform.DOMoveX(-beginPosition, pacAnimationDuration, true).SetEase(Ease.Linear));

            if (buttonTransform.name == "BTN_LaunchGame")
            {
                mainSequence.AppendCallback(() =>
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Pacman");
                });
            }
            else if (buttonTransform.name == "BTN_Quit")
            {
                mainSequence.AppendCallback(() =>
                {
                    Application.Quit();
                });
            } else if (buttonTransform.name == "BTN_Options")
            {
                mainSequence.AppendCallback(() =>
                {
                    DisplayOptionsMenu();
                });
            } else if (buttonTransform.name == "BTN_PermaUpgrade")
            {
                mainSequence.AppendCallback(() =>
                {
                    DisplayPermaUpgradeMenu();
                });
            } else if (buttonTransform.name == "BTN_MainMenu")
            {
                mainSequence.AppendCallback(() =>
                {
                    DisplayMainMenu();
                });
            }else
            {
                mainSequence.AppendCallback(() =>
                {
                    Debug.LogWarning("Button clicked with unrecognized name: " + buttonTransform.name);
                });
            }
        }
    }

    private void HideAllPanels()
    {
        foreach (GameObject panel in MainMenuPanels)
        {
            panel.SetActive(false);
        }
    }

    private void DisplayPanel(GameObject panel)
    {
        if (panel == null)
        {
            Debug.LogError("Panel is null. Cannot display.");
            return;
        }
        HideAllPanels();
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        panel.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        //also display every child of the panel
        foreach (Transform child in panel.transform)
        {
            Debug.Log("Animating child: " + child.name);
            child.gameObject.SetActive(true);
            child.localScale = Vector3.zero;
            child.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }
    }

    private void DisplayMainMenu()
    {
        HideAllPanels();
        DisplayPanel(MainMenuPanels.Find(panel => panel.name == "UI_MainMenuPanel"));
    }

    private void DisplayOptionsMenu()
    {
        HideAllPanels();
        DisplayPanel(MainMenuPanels.Find(panel => panel.name == "UI_OptionsPanel"));
    }

    private void DisplayPermaUpgradeMenu()
    {
        HideAllPanels();
        DisplayPanel(MainMenuPanels.Find(panel => panel.name == "UI_PermaUpgradePanel"));
    }

    public void OnHoverButton(Transform buttonTransform)
    {
        //Debug.Log("Button Hovered");
        buttonTransform.DOScale(1.1f, 0.2f);
    }

    public void OnHoverButtonExit(Transform buttonTransform)
    {
        //Debug.Log("Button Hover Exited");
        buttonTransform.DOScale(1f, 0.2f);
    }

    public void OnMusicVolumeChange(Slider slider)
    {
        float volume = slider.value;
        Debug.Log("Music Volume Changed: " + volume);
        //TODO: Implement music volume change logic here
    }

    public void OnSFXVolumeChange(Slider slider)
    {
        float volume = slider.value;
        Debug.Log("SFX Volume Changed: " + volume);
        //TODO : Implement SFX volume change logic here
    }

    public void OnLanguageChange()
    {
        Debug.Log("Language Changed");
        //TODO: Implement language change logic here
    }

    public void OnResolutionChange()
    {
        Debug.Log("Resolution Changed");
        //TODO: Implement resolution change logic here
    }

    public void OnWindowModeChange()
    {
        Debug.Log("Window Mode Changed");
        //TODO : Implement window mode change logic here
    }
}

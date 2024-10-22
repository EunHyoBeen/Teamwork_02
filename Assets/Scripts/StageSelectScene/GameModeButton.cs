using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameModeButton : MonoBehaviour
{
    [SerializeField] private RectTransform soloModeBtn;
    [SerializeField] private RectTransform multiCoopModeBtn;
    [SerializeField] private RectTransform multiIndiModeBtn;

    private void Start()
    {
        if (GameManager.Instance.gameData.GameMode == InGameManager.GameMode.Alone)
        {
            HighlightSoloButton();
        }
        else if (GameManager.Instance.gameData.GameMode == InGameManager.GameMode.Duo_community)
        {
            HighlightMultiCoopButton();
        }
        else
        {
            HighlightMultiIndiButton();
        }
    }

    public void SelectSolo()
    {
        if (GameManager.Instance.gameData.GameMode == InGameManager.GameMode.Alone) return;

        GameManager.Instance.gameData.GameMode = InGameManager.GameMode.Alone;

        HighlightSoloButton();
    }

    public void SelectMultiCoop()
    {
        if (GameManager.Instance.gameData.GameMode == InGameManager.GameMode.Duo_community) return;

        GameManager.Instance.gameData.GameMode = InGameManager.GameMode.Duo_community;

        HighlightMultiCoopButton();
    }

    public void SelectMultiIndi()
    {
        if (GameManager.Instance.gameData.GameMode == InGameManager.GameMode.Duo_individual) return;

        GameManager.Instance.gameData.GameMode = InGameManager.GameMode.Duo_individual;

        HighlightMultiIndiButton();
    }


    private void HighlightSoloButton()
    {
        HighlightButton(soloModeBtn, true);
        HighlightButton(multiCoopModeBtn, false);
        HighlightButton(multiIndiModeBtn, false);
    }

    private void HighlightMultiCoopButton()
    {
        HighlightButton(soloModeBtn, false);
        HighlightButton(multiCoopModeBtn, true);
        HighlightButton(multiIndiModeBtn, false);
    }

    private void HighlightMultiIndiButton()
    {
        HighlightButton(soloModeBtn, false);
        HighlightButton(multiCoopModeBtn, false);
        HighlightButton(multiIndiModeBtn, true);
    }


    private void HighlightButton(RectTransform buttonOgject, bool highlight)
    {
        Button btn;
        Outline outline;
        ColorBlock colorBlock;
        Color color;

        btn = buttonOgject.GetComponent<Button>();
        colorBlock = btn.colors;
        color = colorBlock.normalColor;
        color.a = highlight ? 50 / 255f : 5 / 255f;
        colorBlock.normalColor = color;
        colorBlock.selectedColor = color;
        btn.colors = colorBlock;

        outline = buttonOgject.GetComponent<Outline>();
        color = outline.effectColor;
        color.a = highlight ? 150 / 255f : 50 / 255f;
        outline.effectColor = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeButton : MonoBehaviour
{
    [SerializeField] private RectTransform soloModeBtn;
    [SerializeField] private RectTransform multiModeBtn;

    private void Awake()
    {
        HighlightSoloButton();
    }

    public void SelectSolo()
    {
        if (GameManager.Instance.gameData.GameMode == InGameManager.GameMode.Alone) return;

        GameManager.Instance.gameData.GameMode = InGameManager.GameMode.Alone;

        HighlightSoloButton();
    }

    public void SelectMulti()
    {
        if (GameManager.Instance.gameData.GameMode == InGameManager.GameMode.Duo_community) return;

        GameManager.Instance.gameData.GameMode = InGameManager.GameMode.Duo_community;

        HighlightMultiButton();
    }


    private void HighlightSoloButton()
    {
        Button btn;
        Outline outline;
        ColorBlock colorBlock;
        Color color;

        btn = soloModeBtn.GetComponent<Button>();
        colorBlock = btn.colors;
        color = colorBlock.normalColor;
        color.a = 50 / 255f;
        colorBlock.normalColor = color;
        colorBlock.selectedColor = color;
        btn.colors = colorBlock;

        outline = soloModeBtn.GetComponent<Outline>();
        color = outline.effectColor;
        color.a = 150 / 255f;
        outline.effectColor = color;

        btn = multiModeBtn.GetComponent<Button>();
        colorBlock = btn.colors;
        color = colorBlock.normalColor;
        color.a = 5 / 255f;
        colorBlock.normalColor = color;
        colorBlock.selectedColor = color;
        btn.colors = colorBlock;

        outline = multiModeBtn.GetComponent<Outline>();
        color = outline.effectColor;
        color.a = 50 / 255f;
        outline.effectColor = color;
    }

    private void HighlightMultiButton()
    {
        Button btn;
        Outline outline;
        ColorBlock colorBlock;
        Color color;

        btn = soloModeBtn.GetComponent<Button>();
        colorBlock = btn.colors;
        color = colorBlock.normalColor;
        color.a = 5 / 255f;
        colorBlock.normalColor = color;
        colorBlock.selectedColor = color;
        btn.colors = colorBlock;

        outline = soloModeBtn.GetComponent<Outline>();
        color = outline.effectColor;
        color.a = 50 / 255f;
        outline.effectColor = color;

        btn = multiModeBtn.GetComponent<Button>();
        colorBlock = btn.colors;
        color = colorBlock.normalColor;
        color.a = 50 / 255f;
        colorBlock.normalColor = color;
        colorBlock.selectedColor = color;
        btn.colors = colorBlock;

        outline = multiModeBtn.GetComponent<Outline>();
        color = outline.effectColor;
        color.a = 150 / 255f;
        outline.effectColor = color;
    }
}

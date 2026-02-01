using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseMaskMenu : MonoBehaviour
{
    public void OnAnyButtonClicked()
    {
        GameObject clickedGameObject = EventSystem.current.currentSelectedGameObject;

        if (clickedGameObject != null)
        {
            if (clickedGameObject.TryGetComponent<Button>(out var clickedButton))
            {
                GameManager.Instance.SelectedMask = clickedButton.GetComponent<MenuMaskType>().MaskSO;
                Debug.Log(GameManager.Instance.SelectedMask);
                UIFade uiFade = gameObject.GetComponentInChildren<UIFade>();
                uiFade.FadeOut(() =>
                {
                    SceneManager.LoadScene(2);
                });
            }
        }
    }
}

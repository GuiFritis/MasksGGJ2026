using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseMaskMenu : MonoBehaviour
{
    public void OnAnyButtonClicked(MaskSO mask)
    {
        
        GameManager.Instance.SelectedMask = mask;
        UIFade uiFade = gameObject.GetComponentInChildren<UIFade>();
        uiFade.FadeOut(() =>
        {
            SceneManager.LoadScene(2);
        });
    }
}

using DG.Tweening;
using TMPro;
using UnityEngine;

public class MaskTutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;

    void Start()
    {
        DisplayTutorial();
    }

    private void DisplayTutorial()
    {
        _textMesh.text = GameManager.Instance.SelectedMask.description;
        _textMesh.DOColor(Color.white, .5f).SetDelay(5).OnComplete(
            () => _textMesh.DOColor(Color.clear, .5f).SetDelay(7f)
        );
    }
}

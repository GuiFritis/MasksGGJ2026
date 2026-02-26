using DG.Tweening;
using TMPro;
using UnityEngine;

public class MaskTutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;

    void Start()
    {
        Invoke(nameof(DisplayTutorial), 2);
    }

    private void DisplayTutorial()
    {
        _textMesh.text = GameManager.Instance.SelectedMask.description;
        _textMesh.DOColor(Color.white, .5f).OnComplete(
            () => _textMesh.DOColor(Color.clear, .5f).SetDelay(3f)
        );
    }
}

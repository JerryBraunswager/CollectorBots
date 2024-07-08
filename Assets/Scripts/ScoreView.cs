using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreView : MonoBehaviour
{
    [SerializeField] private Base _base;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _base.ScoreChanged += WriteScore;
    }

    private void OnDisable()
    {
        _base.ScoreChanged -= WriteScore;
    }

    private void WriteScore(float score)
    {
        _text.text = score.ToString();
    }
}

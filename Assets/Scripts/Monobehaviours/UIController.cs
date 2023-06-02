using UnityEngine;
using TMPro;

namespace Monobehaviours
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] TMP_Text ScoreNum;

        private void Start()
        {
            GameController.Instance.OnScoreChanged += OnScoreChanged;

        }

        private void OnScoreChanged(object sender, ObjectPoints points)
        {
            ScoreNum.text = (int.Parse(ScoreNum.text) + (int)points).ToString();
        }

    }
        
}
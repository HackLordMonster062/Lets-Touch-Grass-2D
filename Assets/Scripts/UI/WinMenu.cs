using TMPro;
using UnityEngine;

public class WinMenu : MonoBehaviour {
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text grade;

    public void Initialize(float time) {
        gameObject.SetActive(true);

        timeText.text = UIManager.FormatTime(time);
        grade.text = CalculateGrade(time);
    }

    public void Retry() {
        GameManager.instance.Retry();
    }

    public void Menu() {

    }

    static string CalculateGrade(float time) {
        switch (time) {
            case > 360:
                return "F";
            case > 300:
                return "D";
            case > 240:
                return "C";
            case > 200:
                return "B";
            case > 180:
                return "A";
            default:
                return "S";
        }
    }
}

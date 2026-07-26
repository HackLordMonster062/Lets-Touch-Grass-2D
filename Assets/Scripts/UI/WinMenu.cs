using TMPro;
using UnityEngine;

public class WinMenu : MonoBehaviour {
    [SerializeField] TMP_Text time;
    [SerializeField] TMP_Text grade;

    public void Initialize(float time) {
        
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

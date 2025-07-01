using UnityEngine;
using UnityEngine.TextCore.Text;


public class MngDialogs : MonoBehaviour
{

    [Header("Time Preferences")]
    public float _textTimeDuration = 6.0f;  // seconds
    [Header("Text Style")]
    public GUIStyle _textStyle;

    private static float startTime;
    private static string textToShowLeft = string.Empty;
    private static string textToShowRight = string.Empty;


    public void OnGUI()
    {
        //Debug.Log(Time.time);
        if (textToShowLeft != string.Empty && (Time.time - startTime) < _textTimeDuration)
        {
            GUI.Label(new Rect(Screen.width * 0.1f, Screen.height - 40, Screen.width * 0.4f, 30), textToShowLeft, _textStyle);
        }
        else
        {
            textToShowLeft = string.Empty;
        }
        if (textToShowRight != string.Empty && (Time.time - startTime) < _textTimeDuration)
        {
            GUI.Label(new Rect(Screen.width * 0.6f, Screen.height - 40, Screen.width * 0.4f, 30), textToShowRight, _textStyle);
        }
        else
        {
            textToShowRight = string.Empty;
        }

    }

    private static void InitTimer()
    {
        startTime = Time.time;
    }

    public static void SetText(string text, ushort side)
    {
        if (Time.time > 1.0)    // Avoid texting from aircraft's initialization
        {
            if (side == 0)
                textToShowLeft = "ATC: " + text;
            else
                textToShowRight = "Pilot: " + text;
            
            InitTimer();
        }
    }
    public static string GetText(ushort side)
    {
        if (side == 0)
            return textToShowLeft;
        else
            return textToShowRight;
    }

}

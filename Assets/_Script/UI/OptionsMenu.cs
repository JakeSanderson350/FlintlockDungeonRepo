using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] PlayerPreferences prefs;

    [SerializeField] Slider mouseSensitivity;

    private void OnEnable()
    {
        mouseSensitivity.value = prefs.sentitivityX;
    }

    private void OnDisable()
    {
        prefs.sentitivityX = mouseSensitivity.value;
        prefs.sentitivityY = mouseSensitivity.value * 0.8f;
    }
}

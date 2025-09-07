using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    Canvas hud;

    [SerializeField] Image screenWipe;

    [SerializeField] Image healthbar;
    [SerializeField] ShakeSettings shakeSettings;


    Color temp = Color.white;

    private void Start()
    {
        hud = GetComponent<Canvas>();
        temp.a = 0;
        healthbar.color = temp; 
    }

    private void OnEnable()
    {
        EventManager.onPlayerDied += PlayerDied;
    }

    private void OnDisable()
    {
        EventManager.onPlayerDied -= PlayerDied;
    }

    public void SetHealthBar(float num, float change, float percent)
    {
        if(change < 0)
        {
            shakeSettings.strength = -Vector3.one * 0.01f * change;
            Tween.PunchScale(healthbar.transform, shakeSettings);
        }

        temp.a = Mathf.Lerp(1, 0, percent);
        healthbar.color = temp;
    }

    void PlayerDied()
    {
        Tween.ScaleY(screenWipe.transform, 1f, 2f);
    }
}

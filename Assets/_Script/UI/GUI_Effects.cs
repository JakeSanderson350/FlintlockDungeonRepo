using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUI_Effects : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField] bool selectOnAwake;
    [SerializeField] AudioClip mouseDownSFX;
    [SerializeField] AudioClip mouseEnterSFX;

    private void Start()
    {
        if(selectOnAwake) GetComponent<Button>().Select();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioPlayer.PlaySFX(mouseDownSFX, Vector3.zero, 0.1f).spatialBlend = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioPlayer.PlaySFX(mouseEnterSFX, Vector3.zero, 0.1f).spatialBlend = 0;
    }


}

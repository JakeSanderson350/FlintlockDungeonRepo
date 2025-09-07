using TMPro;
using UnityEngine;
public class Health : Resource
{
    //basically a stub

    [SerializeField] TextMeshProUGUI healthUI;

    protected override void SetValue(float num)
    {
        base.SetValue(num);
        healthUI.text = ((int)num).ToString();
    }
}

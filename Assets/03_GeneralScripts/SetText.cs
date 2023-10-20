using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    
    public void SetInt(int value)
    {
        text.text = value.ToString();
    }
}

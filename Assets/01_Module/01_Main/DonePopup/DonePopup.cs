using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DonePopup : MonoBehaviour
{
    [SerializeField] Button _exit;
    [SerializeField] TMP_Text _text;


    internal void Init()
    {
        _exit.onClick.AddListener(() => { gameObject.SetActive(false); });
    }

    internal void On(string addr)
    {
        _text.text = addr;
        gameObject.SetActive(true);
    }
}

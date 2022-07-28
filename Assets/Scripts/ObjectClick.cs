using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject Popup;
    public GameObject Popup_Back;
    public GameObject X;

    //オブジェクトのクリックを検知
    public void OnPointerClick(PointerEventData eventData)
    {
        print($"{name}がクリックされた。");
        Popup.SetActive(true);
        Popup_Back.SetActive(true);
        X.SetActive(true);
    }
}

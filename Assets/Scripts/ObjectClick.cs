using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject Popup;
    public GameObject Popup_Back;
    public GameObject X;

    //�I�u�W�F�N�g�̃N���b�N�����m
    public void OnPointerClick(PointerEventData eventData)
    {
        print($"{name}���N���b�N���ꂽ�B");
        Popup.SetActive(true);
        Popup_Back.SetActive(true);
        X.SetActive(true);
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField, Tooltip("退出按钮")] private Button exitButton;

    private Vector3 m_Offset;//拖拽时的鼠标偏移量
    #region 预览事件
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowButtonAnim();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideButtonAnim();
    }
    #endregion

    #region 拖动事件
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_Offset = this.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("开始拖动");
        var tmpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = new Vector3(tmpPos.x + m_Offset.x, tmpPos.y + m_Offset.y, 0);
    }
    #endregion

    #region 点击事件
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
    #endregion

    #region UI动画
    
    
    private void ShowButtonAnim()
    {
        exitButton.gameObject.SetActive(true);
        
    }
    private void HideButtonAnim()
    {
        exitButton.gameObject.SetActive(false);
    }
    #endregion
}

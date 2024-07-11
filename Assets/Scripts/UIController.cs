using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField, Tooltip("桌宠")] private GameObject deskPetBody;
    [SerializeField, Tooltip("鼠标光标图片")] private Texture2D cursorTexture;
    [SerializeField, Tooltip("放大滑动条")] private Slider magnifySlider;
    [SerializeField, Tooltip("动画时长")] private float animTime;
    [SerializeField, Tooltip("UI")] private GameObject UI;

    [Header("拖拽设置")]
    [SerializeField, Tooltip("窗口设置")] private BackGroundSet backgroundSet;

    [Header("缩放设置")]
    [SerializeField, Tooltip("缩放极值")] private Vector2 scaleBoundary;

    private float scale;
    private Vector3 mOffset;//拖拽时的鼠标偏移量
    private bool clicked;
    
    [Header("随机动画播放设置")] 
    [SerializeField, Tooltip("随机动画勾选框")] private Toggle randomAnimToggle;
    
    private void Start()
    {
        magnifySlider.onValueChanged.AddListener(delegate {Magnify();});
        scale = PlayerPrefs.GetFloat("myKey", 0.5f);
        magnifySlider.value = AntiNormalization(scaleBoundary.x, scaleBoundary.y, scale);
        deskPetBody.transform.localScale = new Vector3(scale, scale, scale);
        randomAnimToggle.onValueChanged.AddListener((bool isOn) =>
        {
            AnimationController.Instance.SetIfPlayRandomAnim(isOn);
        });

        //Debug.Log("屏幕大小：" + Screen.currentResolution.width + "," + Screen.currentResolution.height);
    }

    private float Normalization(float left, float right, float num)
    {
        var target = 0f;
        target = num * (right - left) + left;
        return target;
    }
    
    private float AntiNormalization(float left, float right, float num)
    {
        var target = 0f;
        target = (num - left) / (right - left);
        return target;
    }
    
    #region 预览事件
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    #endregion

    #region 拖动事件
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Camera.main != null) mOffset = this.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        clicked = false;
        EndClick();
    }

    public void OnDrag(PointerEventData eventData)
    {
        clicked = false;
        EndClick();
        DoDrag();
    }

    private void DoDrag()
    {
        if (Camera.main == null) return;
        var tmpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pos = new Vector2Int(
            (int)Normalization(0,Screen.currentResolution.width/10f,tmpPos.x/30),
            -(int)Normalization(0,Screen.currentResolution.height/10f,tmpPos.y/30));
        
        backgroundSet.ChangeWindowPosition(pos);
    }
    #endregion

    #region 点击事件
    public void OnPointerClick(PointerEventData eventData)
    {
        clicked = !clicked;
        if (clicked)
        {
            ShowUIAnim();
        }
        else
        {
            HideUIAnim();
        }
    }
    private void EndClick()
    {
        HideUIAnim();
        magnifySlider.gameObject.SetActive(false);
    }
    #endregion

    #region UI动画
    private void ShowUIAnim()
    {
        UI.SetActive(true);
        magnifySlider.gameObject.SetActive(true);
    }
    private void HideUIAnim()
    {
        UI.gameObject.SetActive(false);
    }
    #endregion

    #region UI组件交互功能实现
    private void Magnify()
    {
        scale = Normalization(scaleBoundary.x, scaleBoundary.y, magnifySlider.value);
        deskPetBody.transform.localScale = new Vector3(scale, scale, scale);
        PlayerPrefs.SetFloat("myKey", scale);
    }
    #endregion
}

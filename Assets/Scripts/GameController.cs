using UnityEngine;
using System.Runtime.InteropServices;
public class GameController: MonoBehaviour
{
    
    private float lastX = -1;
    private float lastY = -1;
    private GameObject MainCamera;
    //Windows接口
    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);
    private const int VK_LBUTTON = 0x01; //鼠标左键
    private const int VK_RBUTTON = 0x02; //鼠标右键
    private int a;

    //动画辅助key
    string AniKey = "Begin";

    // Start is called before the first frame update
    void Awake()
    {
        MainCamera = GameObject.Find("Main Camera");
        if (MainCamera != null)
        {
            if (MainCamera.GetComponent<Camera>().orthographic)
            {
                //LAppLive2DManager.Instance.SetTouchMode2D(true);
            }
            else
            {
                Debug.Log("\"Main Camera\" Projection : Perspective");

                //LAppLive2DManager.Instance.SetTouchMode2D(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetAsyncKeyState(VK_LBUTTON) != 0)
        {
            if (AniKey == "Begin")
            {
                lastX = Input.mousePosition.x;
                lastY = Input.mousePosition.y;

                //LAppLive2DManager.Instance.TouchesBegan(Input.mousePosition);
                Debug.Log("TouchesBegan" + Input.mousePosition);
                AniKey = "Ani";
            }
            else
            {
                if (lastX == Input.mousePosition.x && lastY == Input.mousePosition.y)
                {
                    return;
                }
                lastX = Input.mousePosition.x;
                lastY = Input.mousePosition.y;
                //LAppLive2DManager.Instance.TouchesMoved(Input.mousePosition);
                Debug.Log("TouchesMoved" + Input.mousePosition);
                AniKey = "Begin";
            }
        }
        else
        {
            if (AniKey == "Ani")
            {
                lastX = -1;
                lastY = -1;
                //LAppLive2DManager.Instance.TouchesEnded(Input.mousePosition);
                Debug.Log("TouchesEnded" + Input.mousePosition);
                AniKey = "Begin";
            }
        }
    }
}

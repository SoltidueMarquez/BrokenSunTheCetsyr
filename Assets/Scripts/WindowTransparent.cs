using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowTransparent : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWind, string text, string caption, uint type);

    /// <summary>
    /// 窗体边框结构体，存放的是四个顶点的坐标
    /// </summary>
    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    /// <summary>
    /// 获取当前活动窗口
    /// </summary>
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    
    /// <summary>
    /// 将窗口边框扩展到客户区域(玻璃片效果，详见官网的API)
    /// </summary>
    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    /// <summary>
    /// 修改窗口的样式属性实现穿透点击
    /// </summary>
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    /// <summary>
    /// 设置窗口位置函数
    /// </summary>
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
        uint uFlags);

    /// <summary>
    /// 设置分层窗口透明度函数
    /// </summary>
    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);
    
    //设置新的窗口样式为分层和透明
    const int GWL_EXSTYLE = -20;
    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;
    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);//插入位置始终置顶
    const uint LWA_COLORKEY = 0x00000001;//颜色键标志

    private IntPtr hWnd;
    private void Start()
    {
        //MessageBox(new IntPtr(0), "Hello World", "HelloDialog", 0);

        //保证在编辑器状态以及编译后都能运行
#if !UNITY_EDITOR_
        //初始化活动窗口句柄
        hWnd = GetActiveWindow();
        
        //初始化边框结构体并调用扩展函数
        var margins = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);
        //同时设置双属性
        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
        //窗体中所有颜色为0的地方将变为透明
        SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY);
        //设置窗口位置始终置顶
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
#endif
        //让程序在后台执行
        Application.runInBackground = true;
    }
}

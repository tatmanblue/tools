/*
 ******************************************************************************
 This file is part of BigWoo.NET.

    BigWoo.NET is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    BigWoo.NET is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BigWoo.NET; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA


    architected and written by 
    matt raffel 
    matt.raffel@gmail.com

       copyright (c) 2010 by matt raffel unless noted otherwise

 ******************************************************************************
*/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BigWoo.SDK
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class User32
    {   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool EnableWindow(IntPtr hWnd, bool enable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="modeNum"></param>
        /// <param name="devMode"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int EnumDisplaySettings (string deviceName, int modeNum, ref DEVMODE devMode );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWndParent"></param>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern Boolean EnumChildWindows(int hWndParent, Delegate lpEnumFunc, int lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="s"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern Int32 GetWindowText(int hWnd, StringBuilder s, int nMaxCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern Int32 GetWindowTextLength(int hwnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devMode"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="newLong"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int newLong);
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiAction"></param>
        /// <param name="uiParam"></param>
        /// <param name="bRetValue"></param>
        /// <param name="fWinINI"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int bRetValue, uint fWinINI);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="dwTime"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool AnimateWindow(IntPtr hWnd, uint dwTime, uint dwFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="rect"></param>
        /// <param name="erase"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool InvalidateRect(IntPtr hWnd, ref RECT rect, bool erase);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hInstance"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, uint cursor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hCursor"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetCursor(IntPtr hCursor);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetFocus();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        /// <summary>
        /// Used in conjunction with getting mouse capture.  Releases the capture
        /// </summary>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool WaitMessage();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool TranslateMessage(ref MSG msg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool DispatchMessage(ref MSG msg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="hWnd"></param>
        /// <param name="wFilterMin"></param>
        /// <param name="wFilterMax"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax);
	
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="hWnd"></param>
        /// <param name="wFilterMin"></param>
        /// <param name="wFilterMax"></param>
        /// <param name="wFlag"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PeekMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax, uint wFlag);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hDC"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="cmdShow"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hWnd, short cmdShow);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="repaint"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndAfter"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int X, int Y, int Width, int Height, uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hdcDst"></param>
        /// <param name="pptDst"></param>
        /// <param name="psize"></param>
        /// <param name="hdcSrc"></param>
        /// <param name="pprSrc"></param>
        /// <param name="crKey"></param>
        /// <param name="pblend"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT pt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref POINT pt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tme"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENTS tme);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hRgn"></param>
        /// <param name="redraw"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtKey"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern ushort GetKeyState(int virtKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool DrawFocusRect(IntPtr hWnd, ref RECT rect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool HideCaret(IntPtr hWnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ShowCaret(IntPtr hWnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="pdwRecipients"></param>
        /// <param name="uiMessage"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL", EntryPoint="BroadcastSystemMessageA", SetLastError=true, CharSet=CharSet.Unicode, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
        public static extern int BroadcastSystemMessage(Int32 dwFlags, ref Int32 pdwRecipients, int uiMessage, int wParam, int lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
        public static extern int GetUserProfileString();
    }
}
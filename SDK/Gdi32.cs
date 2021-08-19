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

namespace BigWoo.SDK
{
    /// <summary>
    /// class for Gdi API calls
    /// </summary>
    public sealed class Gdi32
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int CombineRgn(IntPtr dest, IntPtr src1, IntPtr src2, int flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateRectRgnIndirect(ref RECT rect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDC"></param>
        /// <param name="rectBox"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClipBox(IntPtr hDC, ref RECT rectBox);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDC"></param>
        /// <param name="hRgn"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brush"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateBrushIndirect(ref LOGBRUSH brush);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hdcDest"></param>
        /// <param name="nXDest"></param>
        /// <param name="nYDest"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <param name="hdcSrc"></param>
        /// <param name="nXSrc"></param>
        /// <param name="nYSrc"></param>
        /// <param name="dwRop"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(
              IntPtr hdcDest,     // handle to destination DC (device context)
              int nXDest,         // x-coord of destination upper-left corner
              int nYDest,         // y-coord of destination upper-left corner
              int nWidth,         // width of destination rectangle
              int nHeight,        // height of destination rectangle
              IntPtr hdcSrc,      // handle to source DC
              int nXSrc,          // x-coordinate of source upper-left corner
              int nYSrc,          // y-coordinate of source upper-left corner
              System.Int32 dwRop  // raster operation code
              );

        /// <summary>
        /// Used for getting the SDK DC handle for the specified device
        /// typically used for drawing on the device2
        /// </summary>
        /// <param name="driverName"></param>
        /// <param name="deviceName"></param>
        /// <param name="output"></param>
        /// <param name="lpInitData"></param>
        /// <returns>IntPtr, the handle to the DC</returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(
              String driverName,
              String deviceName,
              String output,
              IntPtr lpInitData
              );

        /// <summary>
        /// SDK Function CreateCompatibleBitmap
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool DeleteDC(IntPtr dc);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDC"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool PatBlt(IntPtr hDC, int x, int y, int width, int height, uint flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr DeleteObject(IntPtr hObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDC"></param>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDC"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
    }
}
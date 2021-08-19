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
using System.Drawing;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace BigWoo.SDK
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG 
    {
        public IntPtr hwnd;
        public int message;
        public IntPtr wParam;
        public IntPtr lParam;
        public int time;
        public int pt_x;
        public int pt_y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public int fErase;
        public Rectangle rcPaint;
        public int fRestore;
        public int fIncUpdate;
        public int Reserved1;
        public int Reserved2;
        public int Reserved3;
        public int Reserved4;
        public int Reserved5;
        public int Reserved6;
        public int Reserved7;
        public int Reserved8;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;
    }

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TRACKMOUSEEVENTS
    {
        public uint cbSize;
        public uint dwFlags;
        public IntPtr hWnd;
        public uint dwHoverTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LOGBRUSH
    {
        public uint lbStyle; 
        public uint lbColor; 
        public uint lbHatch; 
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DEVMODE 
    {
        [MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)] public string dmDeviceName;
        public short  dmSpecVersion;
		public short  dmDriverVersion;
		public short  dmSize;
		public short  dmDriverExtra;
		public int    dmFields;

		public short dmOrientation;
		public short dmPaperSize;
		public short dmPaperLength;
		public short dmPaperWidth;

		public short dmScale;
		public short dmCopies;
		public short dmDefaultSource;
		public short dmPrintQuality;
		public short dmColor;
		public short dmDuplex;
		public short dmYResolution;
		public short dmTTOption;
		public short dmCollate;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmFormName;
		public short dmLogPixels;
		public short dmBitsPerPel;
		public int   dmPelsWidth;
		public int   dmPelsHeight;

		public int   dmDisplayFlags;
		public int   dmDisplayFrequency;

		public int   dmICMMethod;
		public int   dmICMIntent;
		public int   dmMediaType;
		public int   dmDitherType;
		public int   dmReserved1;
		public int   dmReserved2;

		public int   dmPanningWidth;
		public int   dmPanningHeight;
	};

}

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

namespace BigWoo.SDK
{
	/// <summary>
	/// Class containing wrapper functions for various button messages and SDK function
	/// calls, intent is to give simple interfaces for interacting with buttons
	/// </summary>
	public sealed class ButtonControls
	{
        private ButtonControls() {}

        /// <summary>
        /// Sets a check box to the state input
        /// </summary>
        /// <param name="hwnd">int, handle to the check box</param>
        /// <param name="state">ButtonStates, state of the check box, usually checked or unchecked</param>
        public static void SetCheckState(int hwnd, ButtonStates state)
        {
            User32.SendMessage((IntPtr) hwnd, (int) CheckBoxControlMessages.BM_SETCHECK, (uint) state, 0);
        }
	}
}

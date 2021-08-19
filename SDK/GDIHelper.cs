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

       copyright (c) 2009 by matt raffel unless noted otherwise

 ******************************************************************************
*/
#region using statements
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
#endregion

namespace BigWoo.SDK
{
    /// <summary>
    /// 
    /// </summary>
    internal class GDIHelper
    {
        private GDIHelper() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath GetRoundRectPath(RectangleF rect, float radius)
        {
            return GetRoundRectPath(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath GetRoundRectPath(float X, float Y, float width, float height, float radius)
        {

            GraphicsPath gp = new GraphicsPath();

            gp.AddLine(X + radius, Y, X + width - (radius * 2), Y);

            gp.AddArc(X + width - (radius * 2), Y, radius * 2, radius * 2, 270, 90);

            gp.AddLine(X + width, Y + radius, X + width, Y + height - (radius * 2));

            gp.AddArc(X + width - (radius * 2), Y + height - (radius * 2), radius * 2, radius * 2, 0, 90);

            gp.AddLine(X + width - (radius * 2), Y + height, X + radius, Y + height);

            gp.AddArc(X, Y + height - (radius * 2), radius * 2, radius * 2, 90, 90);

            gp.AddLine(X, Y + height - (radius * 2), X, Y + radius);

            gp.AddArc(X, Y, radius * 2, radius * 2, 180, 90);

            gp.CloseFigure();

            return gp;
        }
    }
}

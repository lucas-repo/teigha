/////////////////////////////////////////////////////////////////////////////// 
// Copyright (C) 2002-2016, Open Design Alliance (the "Alliance"). 
// All rights reserved. 
// 
// This software and its documentation and related materials are owned by 
// the Alliance. The software may only be incorporated into application 
// programs owned by members of the Alliance, subject to a signed 
// Membership Agreement and Supplemental Software License Agreement with the
// Alliance. The structure and organization of this software are the valuable  
// trade secrets of the Alliance and its suppliers. The software is also 
// protected by copyright law and international treaty provisions. Application  
// programs incorporating this software must include the following statement 
// with their copyright notices:
//   
//   This application incorporates Teigha(R) software pursuant to a license 
//   agreement with Open Design Alliance.
//   Teigha(R) Copyright (C) 2002-2016 by Open Design Alliance. 
//   All rights reserved.
//
// By use of this software, its documentation or related materials, you 
// acknowledge and accept the above terms.
///////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Teigha.Core;
using Teigha.TG;

namespace ExDgnCreate
{
  class EntityBoxes
  {
    public const double WIDTH_BOX = 2.25;
    public const double HEIGHT_BOX = 3.25;

    public const double HOR_SPACE = 0.625;
    public const double VER_SPACE = 0.375;

    public const int HOR_BOXES = 11;
    public const int VER_BOXES = 7;

    /************************************************************************/
    /* Define the entity box widths                                         */
    /************************************************************************/
    static int[,] BoxSizes = {
  {1,1,1,1,2,1,1,1,1,1,0},
  {1,3,2,1,1,1,2,0,0,0,0},
  {2,3,3,1,2,0,0,0,0,0,0},
  {1,1,1,2,1,1,1,1,1,1,0},
  {2,2,2,1,1,2,1,0,0,0,0},
  {3,2,1,1,1,1,1,1,0,0,0},
  {1,1,1,1,1,1,1,1,1,1,1}
};

    /**********************************************************************/
    /* Return the width of the specified box                              */
    /**********************************************************************/
    static public double getWidth(int row, int col)
    {
      return BoxSizes[row, col] * WIDTH_BOX + (BoxSizes[row, col] - 1) * HOR_SPACE;
    }
    /**********************************************************************/
    /* Return the height of specified box                                 */
    /**********************************************************************/
    static public double getHeight()
    {
      return HEIGHT_BOX;
    }
    /**********************************************************************/
    /* Return true if and only if the specified box is a box              */
    /**********************************************************************/
    static public bool isBox(int row, int col)
    {
      return BoxSizes[row, col] > 0 ? true : false;
    }

    /**********************************************************************/
    /* Return the upper-left corner of the specified box                  */
    /**********************************************************************/
    static public OdGePoint3d getBox(int row, int col)
    {
      OdGePoint3d point = OdGePoint3d.kOrigin;
      if (col > HOR_BOXES - 1)
        return point;

      point = new OdGePoint3d(0, HEIGHT_BOX * VER_BOXES + VER_SPACE * (VER_BOXES - 1), 0);

      for (int i = 0; i < col; i++)
      {
        point.x += BoxSizes[row, i] * WIDTH_BOX;
        point.x += (BoxSizes[row, i]) * HOR_SPACE;
      }
      point.y -= row * HEIGHT_BOX;
      point.y -= row * VER_SPACE;
      return point;
    }

    /**********************************************************************/
    /* Return the center of the specified box                             */
    /**********************************************************************/
    static public OdGePoint3d getBoxCenter(int row, int col)
    {
      OdGePoint3d point = getBox(row, col);
      double w = getWidth(row, col);
      point.x += w / 2.0;
      point.y -= HEIGHT_BOX / 2.0;
      return point;
    }

    /**********************************************************************/
    /* Return the size of the box array                                   */
    /**********************************************************************/
    static public OdGeVector3d getArraySize()
    {
      return new OdGeVector3d(WIDTH_BOX * HOR_BOXES + HOR_SPACE * (HOR_BOXES - 1),
                         -(HEIGHT_BOX * VER_BOXES + VER_SPACE * (VER_BOXES - 1)),
                         0);
    }

    /**********************************************************************/
    /* Return the center of the box array                                 */
    /**********************************************************************/
    static public OdGePoint3d getArrayCenter()
    {
      return new OdGePoint3d(0.5 * (WIDTH_BOX * HOR_BOXES + HOR_SPACE * (HOR_BOXES - 1)),
                         0.5 * (HEIGHT_BOX * VER_BOXES + VER_SPACE * (VER_BOXES - 1)),
                         0);
    }
  }
}

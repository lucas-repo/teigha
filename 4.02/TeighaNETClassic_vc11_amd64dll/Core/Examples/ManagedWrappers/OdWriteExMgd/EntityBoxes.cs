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
using System.Text;
using Teigha.Geometry;

namespace OdWriteExMgd
{
  class EntityBoxes
  {
    public const double WIDTH_BOX = 2.25;
    public const double HEIGHT_BOX = 3.25;

    public const double HOR_SPACE = 0.625;
    public const double VER_SPACE = 0.375;

    public const int HOR_BOXES = 11;
    public const int VER_BOXES = 8;

    static readonly int[,] BoxSizes = new int[VER_BOXES,HOR_BOXES]
      {
        {1,1,1,1,2,1,1,1,1,1,0},
        {1,3,2,1,1,1,2,0,0,0,0},
        {2,3,3,1,2,0,0,0,0,0,0},
        {1,1,1,2,1,1,1,1,1,1,0},
        {2,2,2,1,1,2,1,0,0,0,0},
        {3,2,1,1,1,1,1,1,0,0,0},
        {1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,2,0}
      };

    public EntityBoxes()
    {
    }

    /**********************************************************************/
    /* Return the width of the specified box                              */
    /**********************************************************************/
    public double getWidth(int row, int col)
    {
      return BoxSizes[row, col]*WIDTH_BOX + (BoxSizes[row, col] - 1 )*HOR_SPACE;
    }
    /**********************************************************************/
    /* Return the height of specified box                                 */
    /**********************************************************************/
    public double getHeight()
    {
      return HEIGHT_BOX;
    }
    /**********************************************************************/
    /* Return true if and only if the specified box is a box              */
    /**********************************************************************/
    public bool isBox(int row, int col)
    {
      return BoxSizes[row, col] > 0 ? true : false;
    }
    
    /**********************************************************************/
    /* Return the upper-left corner of the specified box                  */
    /**********************************************************************/
    public Point3d getBox(int row, int col)
    {
      Point3d point = new Point3d();
      if ( col > HOR_BOXES-1 )
        return point;

      point = new Point3d(0, HEIGHT_BOX * VER_BOXES + VER_SPACE * (VER_BOXES-1), 0);
      for (int i=0; i < col;  i++ )
      {
        double dTmp = (BoxSizes[row, i] * WIDTH_BOX + BoxSizes[row, i] * HOR_SPACE);
        Vector3d vec1 = Vector3d.XAxis;
        Vector3d vec = vec1 * dTmp;
        Matrix3d mat = Matrix3d.Displacement(vec);
        point = point.TransformBy(mat);
      }
      point = point.TransformBy( Matrix3d.Displacement( -Vector3d.YAxis*( row*HEIGHT_BOX + row*VER_SPACE ) ) );
      return point;
    }

    /**********************************************************************/
    /* Return the center of the specified box                             */
    /**********************************************************************/
    public Point3d getBoxCenter(int row, int col)
    {
      Point3d point = getBox(row,col);
      double w = getWidth(row,col);

      point = point.TransformBy( Matrix3d.Displacement( Vector3d.XAxis*w/2.0 - Vector3d.YAxis*HEIGHT_BOX/2.0 ) );
      return point;
    }

    /**********************************************************************/
    /* Return the size of the box array                                   */
    /**********************************************************************/
    public Vector3d getArraySize()
    {
      return new Vector3d(WIDTH_BOX * HOR_BOXES + HOR_SPACE * (HOR_BOXES-1), 
                         -(HEIGHT_BOX * VER_BOXES + VER_SPACE * (VER_BOXES-1)),
                         0);
    }

    /**********************************************************************/
    /* Return the center of the box array                                 */
    /**********************************************************************/
    public Point3d getArrayCenter()
    {
      return new Point3d(0.5 * (WIDTH_BOX * HOR_BOXES + HOR_SPACE * (HOR_BOXES-1)), 
                         0.5 * (HEIGHT_BOX * VER_BOXES + VER_SPACE * (VER_BOXES-1)),
                         0);
    }
  }
}

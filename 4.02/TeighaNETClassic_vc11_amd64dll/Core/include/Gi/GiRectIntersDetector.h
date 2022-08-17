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




#ifndef __ODGIRECTINTERSDETECTOR__
#define __ODGIRECTINTERSDETECTOR__


#include "Gi/GiConveyorNode.h"
#include "Ge/GeDoubleArray.h"
#include "Ge/GePoint2dArray.h"

class OdGiDeviation;
class OdGiConveyorContext;

#include "TD_PackPush.h"

/** \details

    <group OdGi_Classes> 
    Conveyor node detecting if given primitive intersects with rectangular prism.
    If intersects - something is passed through. In other case - primitive is eaten
    by this conveyor node.
*/
class ODGI_EXPORT OdGiRectIntersDetector : public OdGiConveyorNode
{
public:
  ODRX_DECLARE_MEMBERS(OdGiRectIntersDetector);

  virtual void set(const OdGePoint2d* points, // points defining sides of clipping prism perpendicular to XY
                   bool   bClipLowerZ = false, // number of points is always two
                   double dLowerZ = 0.0,
                   bool   bClipUpperZ = false,
                   double dUpperZ = 0.0) = 0;

  virtual void set(const OdGePoint2dArray& points, // points defining sides of clipping prism perpendicular to XY
                   bool   bClipLowerZ = false, // number of points is always two
                   double dLowerZ = 0.0,
                   bool   bClipUpperZ = false,
                   double dUpperZ = 0.0) = 0;

  virtual void get(OdGePoint2dArray& points,
                   bool&   bClipLowerZ,
                   double& dLowerZ,
                   bool&   bClipUpperZ,
                   double& dUpperZ) const = 0;

  /** Sets max deviation for curve tesselation.
  */
  virtual void setDeviation(const OdGeDoubleArray& deviations) = 0;

  /** Sets deviation object to obtain max deviation for curve tesselation.
  */
  virtual void setDeviation(const OdGiDeviation* pDeviation) = 0;

  /** Sets the draw context object (to access to traits, etc).
  */
  virtual void setDrawContext(OdGiConveyorContext* pDrawCtx) = 0;
};

typedef OdSmartPtr<OdGiRectIntersDetector> OdGiRectIntersDetectorPtr;

#include "TD_PackPop.h"

#endif //#ifndef __ODGIRECTINTERSDETECTOR__

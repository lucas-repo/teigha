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

#ifndef OD_MODGEOMCREATORPE_H
#define OD_MODGEOMCREATORPE_H

#include "TD_PackPush.h"
#include "Ge/GeNurbSurface.h"
#include "Ge/GeNurbCurve3d.h"
#include "ModelerGeometry.h"

class TOOLKIT_EXPORT OdModelerGeometryCreatorPE : public OdRxObject
{
public:
  OdModelerGeometryCreatorPE() {};
  virtual ~OdModelerGeometryCreatorPE() {};
  ODRX_DECLARE_MEMBERS(OdModelerGeometryCreatorPE);

  virtual OdResult createNurbSurface(const OdGeNurbSurface &surface, const OdArray<OdArray< OdGeNurbCurve3d*> > &arrLoopsPtr, 
                                     const OdArray<OdArray< OdGeNurbCurve2d*> > &arrLoopsProjPtr, const OdArray<OdGePoint3d> *arrApex, OdModelerGeometryPtr &pSurface, bool bVaidateInput = true) = 0;
};

/** \details
  This template class is a specialization of the OdSmartPtr class for OdModelerGeometryCreatorPE object pointers.
*/
typedef OdSmartPtr<OdModelerGeometryCreatorPE> OdModelerGeometryCreatorPEPtr;

#include "TD_PackPop.h"

#endif //OD_MODGEOMCREATORPE_H

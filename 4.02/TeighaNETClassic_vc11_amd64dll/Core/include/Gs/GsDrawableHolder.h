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
#ifndef ODGSDRAWABLEHOLDER_INC
#define ODGSDRAWABLEHOLDER_INC

#include "TD_PackPush.h"
#include "Ge/GeExtents3d.h"
#include "IntArray.h"
#include "Gi/GiDrawable.h"

class OdGsNode;
class OdGsBaseModel;
class OdDbStub;

/** \details
  <group OdGs_Classes> 
    
  Corresponding C++ library: TD_Gs
*/
struct DrawableHolder
{
  DrawableHolder(): m_drawableId(0), m_pGsRoot(0)
  {
  }
  OdDbStub* m_drawableId;
  OdGiDrawablePtr m_pDrawable;
  OdSmartPtr<OdGsBaseModel> m_pGsModel;
  OdGsNode* m_pGsRoot;
  OdRxObjectPtr m_pMetafile;
  OdGeExtents3d m_lastExt;
};
typedef OdArray<DrawableHolder> DrawableHolderArray;

#include "TD_PackPop.h"

#endif // ODGSDRAWABLEHOLDER_INC

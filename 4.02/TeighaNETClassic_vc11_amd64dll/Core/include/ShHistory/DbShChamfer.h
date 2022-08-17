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

#ifndef ODDB_DBSHCHAMFER_H
#define ODDB_DBSHCHAMFER_H

#include "TD_PackPush.h"

#include "DbShHistoryNode.h"

/** \details

    \sa
    TD_Db
    <group OdDbSh_Classes> 
*/
class DB3DSOLIDHISTORY_EXPORT OdDbShChamfer : public OdDbShHistoryNode
{
public:
  ODDB_DECLARE_MEMBERS(OdDbShChamfer);
  OdDbShChamfer();

  virtual OdResult dwgInFields(OdDbDwgFiler* pFiler);  
  virtual void dwgOutFields(OdDbDwgFiler* pFiler) const;  
  virtual OdResult dxfInFields(OdDbDxfFiler* pFiler);  
  virtual void dxfOutFields(OdDbDxfFiler* pFiler) const;

  int numEdges();
  OdResult setBaseDistance(double dDist);
  double getBaseDistance();
  double getOtherDistance();
  OdResult setOtherDistance(double dDist);
};

/** \details
    This template class is a specialization of the OdSmartPtr class for OdDbShChamfer object pointers.
*/
typedef OdSmartPtr<OdDbShChamfer> OdDbShChamferPtr;

#include "TD_PackPop.h"

#endif


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

#ifndef __OD_DB_CAMERA__
#define __OD_DB_CAMERA__

#include "DbEntity.h"

#include "TD_PackPush.h"

class OdDbViewTableRecord;
typedef OdSmartPtr<OdDbViewTableRecord> OdDbViewTableRecordPtr;

/** \details

    <group OdDb_Classes>
*/
class TOOLKIT_EXPORT OdDbCamera : public OdDbEntity
{
public:
  ODDB_DECLARE_MEMBERS(OdDbCamera);

  OdDbCamera();

  virtual OdResult dwgInFields(OdDbDwgFiler* pFiler);
  virtual void dwgOutFields(OdDbDwgFiler* pFiler) const;
  virtual OdResult dxfInFields(OdDbDxfFiler* pFiler);
  virtual void dxfOutFields(OdDbDxfFiler* pFiler) const;

  virtual OdResult subErase(bool erasing);
  virtual void subClose();

  OdDbObjectId view() const;
  void setView(const OdDbObjectId &viewId);

  // Some helpful methods for NDBRO editing (required for grip points implementation for example)
  // For internal use
  OdDbViewTableRecordPtr openView(OdDb::OpenMode openMode = OdDb::kForRead) const;
  // For internal use
  void updateView();

protected:

  virtual OdResult subTransformBy(const OdGeMatrix3d& xfm);

  virtual OdUInt32 subSetAttributes(OdGiDrawableTraits* pTraits) const;
  virtual bool subWorldDraw(OdGiWorldDraw* pWd) const;
  virtual void subViewportDraw(OdGiViewportDraw* pVd) const;

  virtual OdResult subGetClassID(void* pClsid) const;

  virtual OdResult subGetGeomExtents(OdGeExtents3d& extents) const;

  virtual OdDbObjectPtr subDeepClone(OdDbIdMapping& ownerIdMap, OdDbObject*, bool bPrimary) const ODRX_OVERRIDE;
  virtual OdDbObjectPtr subWblockClone(OdDbIdMapping& ownerIdMap, OdDbObject*, bool bPrimary) const ODRX_OVERRIDE;

  virtual void subHighlight(bool bDoIt = true, const OdDbFullSubentPath* pSubId = 0, bool highlightAll = false) const;
};

typedef OdSmartPtr<OdDbCamera> OdDbCameraPtr;

#include "TD_PackPop.h"

#endif // __OD_DB_CAMERA__

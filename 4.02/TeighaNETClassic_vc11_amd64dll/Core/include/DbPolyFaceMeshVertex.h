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




#ifndef _OD_DB_POLYFACEMESH_VERTEX_
#define _OD_DB_POLYFACEMESH_VERTEX_

#include "TD_PackPush.h"

#include "DbVertex.h"

/** \details
    This class represents OdDbPolyFaceMesh vertices in an OdDbDatabase instance.
  
    \sa
    TD_Db

    <group OdDb_Classes>
*/
class TOOLKIT_EXPORT OdDbPolyFaceMeshVertex : public OdDbVertex
{
public:
  ODDB_DECLARE_MEMBERS(OdDbPolyFaceMeshVertex);

  OdDbPolyFaceMeshVertex();
  
  /** \details
    Returns the WCS position of this entity (DXF 10).
  */
  OdGePoint3d position() const;

  /** \details
    Sets the WCS position of this entity (DXF 10).
    \param position [in]  Position.
  */
  void setPosition(
    const OdGePoint3d& position);

  virtual OdResult dwgInFields(
    OdDbDwgFiler* pFiler);

  virtual void dwgOutFields(
    OdDbDwgFiler* pFiler) const;

  virtual OdResult dxfInFields_R12(
    OdDbDxfFiler* pFiler);

  virtual void dxfOutFields_R12(
    OdDbDxfFiler* pFiler) const;

  virtual OdResult subErase(
    bool erasing);
};
/** \details
    This template class is a specialization of the OdSmartPtr class for OdDbPolyFaceMeshVertex object pointers.
*/
typedef OdSmartPtr<OdDbPolyFaceMeshVertex> OdDbPolyFaceMeshVertexPtr;

#include "TD_PackPop.h"

#endif

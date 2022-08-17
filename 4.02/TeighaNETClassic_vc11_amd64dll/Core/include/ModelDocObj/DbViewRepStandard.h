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

#ifndef OD_VIEWREPSTANDARD_H
#define OD_VIEWREPSTANDARD_H

#include "DbObject.h"
#include "ModelDocObjExports.h"

#include "TD_PackPush.h"

/** \details

    Corresponding C++ library: AcModelDocObj
    <group OdModelDocObj_Classes> 
*/
class MODELDOCOBJ_EXPORT OdDbViewRepStandard : public OdDbObject
{
public:
  //enum DpiResolutionType {};
  //enum ProjectionType {};
  //enum SectionThreadEndDisplayType {};
  //enum CircularThreadEdgeDisplayType {};
  //enum PreviewDisplayType {};
  //enum SynergyVersion {};

  ODDB_DECLARE_MEMBERS(OdDbViewRepStandard);
  //OdDbViewRepStandard(enum OdDbViewRepStandard::DpiResolutionType,
  //                    enum OdDbViewRepStandard::ProjectionType,
  //                    enum OdDbViewRepStandard::SectionThreadEndDisplayType,
  //                    enum OdDbViewRepStandard::CircularThreadEdgeDisplayType,
  //                    enum OdDbViewRepStandard::PreviewDisplayType,
  //                    enum OdDbViewRepStandard::SynergyVersion);
  OdDbViewRepStandard();
  virtual ~OdDbViewRepStandard();

  //enum OdDbViewRepStandard::SynergyVersion synergyVersion() const;
  //enum OdDbViewRepStandard::PreviewDisplayType previewDisplayType() const;
  //enum OdDbViewRepStandard::CircularThreadEdgeDisplayType circularThreadEdgeDisplayType() const;
  //enum OdDbViewRepStandard::SectionThreadEndDisplayType sectionThreadEndDisplayType() const;
  //enum OdDbViewRepStandard::ProjectionType projectionType() const;
  //enum OdDbViewRepStandard::DpiResolutionType dpiResolutionType() const;

protected:
  // OdDbObject methods :
  virtual OdResult dwgInFields(OdDbDwgFiler* pFiler);
  virtual void dwgOutFields(OdDbDwgFiler* pFiler) const;
  virtual OdResult dxfInFields(OdDbDxfFiler* pFiler);
  virtual void dxfOutFields(OdDbDxfFiler* pFiler) const;

//private:
//  OdResult initialize();
//  void setPreviewDisplayType(enum OdDbViewRepStandard::PreviewDisplayType);
//  void setCircularThreadEdgeDisplayType(enum OdDbViewRepStandard::CircularThreadEdgeDisplayType);
//  void setSectionThreadEndDisplayType(enum OdDbViewRepStandard::SectionThreadEndDisplayType);
//  void setProjectionType(enum OdDbViewRepStandard::ProjectionType);
//  void setDpiResolutionType(enum OdDbViewRepStandard::DpiResolutionType);
};

/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbViewRepStandard object pointers.
*/
typedef OdSmartPtr<OdDbViewRepStandard> OdDbViewRepStandardPtr;

#include "TD_PackPop.h"

#endif // OD_VIEWREPSTANDARD_H

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




#ifndef _OD_DB_ALIGNED_DIMENSION_
#define _OD_DB_ALIGNED_DIMENSION_

#include "TD_PackPush.h"

#include "DbDimension.h"

/** \details
    This class represents Aligned Dimension entities in an OdDbDatabase instance.

    \remarks
    An Aligned Dimension entity dimensions the distance between between any two points in space.
    The normal of the Dimension entity must be perpendicular to the line between said points. 

    \sa
    TD_Db

    <group OdDb_Classes>
*/
class TOOLKIT_EXPORT OdDbAlignedDimension : public OdDbDimension
{
public:
  ODDB_DECLARE_MEMBERS(OdDbAlignedDimension);
  
  OdDbAlignedDimension();
  
  /** \details
    Returns the WCS start point of the first extension line of this Dimension entity (DXF 13).
  */
  OdGePoint3d xLine1Point() const;

  /** \details
    Sets the WCS start point of the first extension line of this Dimension entity (DXF 13).
  
    \param xLine1Point [in]  Start point.
  */
  void setXLine1Point(
    const OdGePoint3d& xLine1Point);

  /** \details
    Returns the WCS start point of the second extension line of this Dimension entity (DXF 14).
  */
  OdGePoint3d xLine2Point() const;

  /** \details
    Sets the WCS start point of the second extension line of this Dimension entity (DXF 14).
  
    \param xLine2Point [in]  Start point.
  */
  void setXLine2Point(
    const OdGePoint3d& xLine2Point);
  
  /** \details
    Returns the WCS point defining the location of dimension line for this Dimension entity (DXF 10).
  */
  OdGePoint3d dimLinePoint() const;

  /** \details
    Sets the WCS point defining the location of dimension line for this Dimension entity (DXF 10).
    
    \param dimLinePoint [in]  Dimension line point.
  */
  void setDimLinePoint(
    const OdGePoint3d& dimLinePoint);
  
  /** \details
    Returns the obliquing angle for this Dimension entity (DXF 52).
    
    \remarks
    All angles are expressed in radians.
  */
  double oblique() const;

  /** \details
    Sets the obliquing angle for this Dimension entity (DXF 52).
    
    \param oblique [in]  Obliquing angle.
    
    \remarks
    All angles are expressed in radians.
  */
  void setOblique(
    double oblique);

  bool jogSymbolOn() const;
  void setJogSymbolOn(bool value);

  OdGePoint3d jogSymbolPosition() const;
  void setJogSymbolPosition(const OdGePoint3d& pt);

  double jogSymbolHeight();
  void setJogSymbolHeight(double value);

  virtual OdResult dwgInFields(
    OdDbDwgFiler* pFiler);

  virtual void dwgOutFields(
    OdDbDwgFiler* pFiler) const;

  virtual OdResult dxfInFields(
    OdDbDxfFiler* pFiler);

  virtual void dxfOutFields(
    OdDbDxfFiler* pFiler) const;

  virtual OdResult dxfInFields_R12(
    OdDbDxfFiler* pFiler);

  virtual void dxfOutFields_R12(
    OdDbDxfFiler* pFiler) const;
  
  virtual OdResult subGetClassID(
    void* pClsid) const;
};

/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbAlignedDimension object pointers.
*/
typedef OdSmartPtr<OdDbAlignedDimension> OdDbAlignedDimensionPtr;

#include "TD_PackPop.h"

#endif

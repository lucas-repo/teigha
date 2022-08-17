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

#ifndef OD_DBABSTRACTVIEWPORTDATA_H
#define OD_DBABSTRACTVIEWPORTDATA_H

#include "AbstractViewPE.h"
#include "Gi/GiViewportTraits.h"

class OdGsView;

/** \details
    This class is the base class for Protocol Extension classes for OdDbViewport and OdDbViewportTableRecord objects.
    
    Corresponding C++ library: TD_Db
    
    \sa
    OdDbAbstractViewTableRecord

    \remarks
    Only the following objects are supported:
    
    *  OdDbViewport
    *  OdDbViewportTableRecord
    *  Derivatives of one of the above.

    <group OdDb_Classes> 
*/
class TOOLKIT_EXPORT OdDbAbstractViewportData : public OdAbstractViewPE
{
public:
  ODRX_DECLARE_MEMBERS(OdDbAbstractViewportData);

  /** \details
    Sets the parameters for the specified Viewport object according to the arguments.
    \param pSourceView [in]  Pointer to the source View object.
    \param pDestinationView [in]  Pointer to the destination View object
  */
  virtual void setProps(OdRxObject* pViewport, const OdRxObject* pSourceView) const;

  // OdAbstractViewPE-inherited methods

  TD_USING(OdAbstractViewPE::setUcs);
  /** \param pSourceView [in]  Pointer to the source Viewport object.
    \param pDestinationView [in]  Pointer to the destination Viewport object
  */
  virtual void setUcs(OdRxObject* pDestinationView, const OdRxObject* pSourceView) const;
  /** \details
  Returns true if and only if there is a view offset associated with the specified Viewport object.
  \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool hasViewOffset(const OdRxObject* pViewport) const;
  /** \details
  Applies plot settings to view.
  \param pSourceView [in]  Pointer to the source Viewport object.
  \param pDestinationView [in]  Pointer to the destination Viewport object
  \returns
  Returns true if plot settings was applied successfully.
  */
  virtual bool applyPlotSettings(OdRxObject* pDestinationView, const OdRxObject* pSourceView) const;

  //

  /** \details
    Returns true if and only if the UCS that is associated with the specified Viewport object will become active
    with activation of the Viewport object.

    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool isUcsSavedWithViewport(const OdRxObject* pViewport) const = 0;

  /** \details
    Controls the activation of the UCS that is associated with the specified Viewport object 
    with the activation of the Viewport object.
    
    \param pViewport [in]  Pointer to the Viewport object.
    \param ucsPerViewport [in]  Controls activation of the UCS.
  */
  virtual void setUcsPerViewport( OdRxObject* pViewport, bool ucsPerViewport) const = 0;

  /** \details
    Returns true if and only if UCS follow mode is on for the specified Viewport object.
    
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool isUcsFollowModeOn(const OdRxObject* pViewport) const = 0;

  /** \details
    Controls UCS follow mode for the specified Viewport object.
    
    \param pViewport [in]  Pointer to the Viewport object.
    \param ucsFollowMode [in]  Controls UCS follow mode.
  */
  virtual void setUcsFollowModeOn(OdRxObject* pViewport, bool ucsFollowMode) const = 0;

  /** \details
    Returns the circle zoom percent for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \returns
    Returns a value in the range [1..20000]
  */
  virtual OdUInt16 circleSides(const OdRxObject* pViewport) const = 0;
  /** \details
    Sets circle zoom percent for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param circleSides [in]  Circle zoom percent. [1,20000]
  */
  virtual void setCircleSides(OdRxObject* pViewport, OdUInt16 circleSides) const = 0;

  /** \details
    Returns true if and only if the grid is on for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool isGridOn(const OdRxObject* pViewport) const = 0;
  /** \details
    Controls the grid for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param gridOn [in]  Controls the grid.
  */
  virtual void setGridOn(OdRxObject* pViewport, bool gridOn) const = 0;

  /** \details
    Returns the grid increment for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual OdGeVector2d gridIncrement(const OdRxObject* pViewport) const = 0;
  /** \details
    Sets the grid increment for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param gridIncrement [in]  Grid increment.
  */
  virtual void setGridIncrement(OdRxObject* pViewport, const OdGeVector2d& gridIncrement) const = 0;

  /** \details
    Returns the grid bound to limits flag for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool isGridBoundToLimits(const OdRxObject* pViewport) const = 0;
  /** \details
    Controls the grid bound to limits flag for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param gridDispFlag [in]  Controls the grid bound to limits flag.
  */
  virtual void setGridBoundToLimits(OdRxObject* pViewport, bool gridDispFlag) const = 0;

  /** \details
    Returns the adaptive grid flag for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool isGridAdaptive(const OdRxObject* pViewport) const = 0;
  /** \details
    Controls the adaptive grid flag for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param gridDispFlag [in]  Controls the adaptive grid flag.
  */
  virtual void setGridAdaptive(OdRxObject* pViewport, bool gridDispFlag) const = 0;

  /** \details
    Returns the grid subdivision restricted flag for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool isGridSubdivisionRestricted(const OdRxObject* pViewport) const = 0;
  /** \details
    Controls the grid subdivision restricted flag for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param gridDispFlag [in]  Controls the subdivision restricted grid flag.
  */
  virtual void setGridSubdivisionRestricted(OdRxObject* pViewport, bool gridDispFlag) const = 0;

  /** \details
    Returns the grid follow flag for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool isGridFollow(const OdRxObject* pViewport) const = 0;
  /** \details
    Controls the grid follow flag for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param gridDispFlag [in]  Controls the grid follow flag.
  */
  virtual void setGridFollow(OdRxObject* pViewport, bool gridDispFlag) const = 0;

  /** \details
    Returns the major grid lines frequency for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual OdInt16 gridMajor(const OdRxObject* pViewport) const = 0;
  /** \details
    Controls the major grid lines frequency for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param nMajor [in]  Controls the major grid lines frequency.
  */
  virtual void setGridMajor(OdRxObject* pViewport, OdInt16 nMajor) const = 0;

  /** \details
    Returns true if and only if the UCS icon is visible for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool isUcsIconVisible(const OdRxObject* pViewport) const = 0;
  /** \details
    Controls the UCS icon visibility for the specified Viewport object.
    
    \param pViewport [in]  Pointer to the Viewport object.
    \param iconVisible [in]  Controls the visibility.
  */
  virtual void setUcsIconVisible(OdRxObject* pViewport, bool iconVisible) const = 0;

  /** \details
    Returns true if and only if the UCS icon is at the UCS orgin for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool isUcsIconAtOrigin(const OdRxObject* pViewport) const = 0;
  /** \details
    Controls the UCS icon display at the UCS orgin for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param atOrigin [in]  Controls the UCS icon.
    
    \remarks
    If atOrigin is true, the UCS icon is at the UCS origin for this Viewport object. If false, it is at the 
    corner for this Viewport object.
    
  */
  virtual void setUcsIconAtOrigin(OdRxObject* pViewport, bool atOrigin) const = 0;

  /** \details
    Returns true if and only if the snap mode is on for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool isSnapOn(const OdRxObject* pViewport) const = 0;
  /** \details
    Controls the snap mode for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param snapOn [in]  Controls snap mode.
  */
  virtual void setSnapOn(OdRxObject* pViewport, bool snapOn) const = 0;

  /** \details
    Returns true if and only if isometric snap style is on for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual bool isSnapIsometric(const OdRxObject* pViewport) const = 0;
  /** \details
    Controls the isometric snap style for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param snapIsometric [in]  Controls isometric snap style.
  */
  virtual void setSnapIsometric(OdRxObject* pViewport, bool snapIsometric) const = 0;

  /** \details
    Returns the UCS snap angle for the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual double snapAngle(const OdRxObject* pViewport) const = 0;
  /** \details
    Returns the UCS snap angle for the specified Viewport object (DXF 50).
    \param pViewport [in]  Pointer to the Viewport object.
    \param snapAngle [in]  Snap angle.
    \remarks
    All angles are expressed in radians.
  */
  virtual void setSnapAngle(OdRxObject* pViewport, double snapAngle) const = 0;

  /** \details
    Returns the UCS snap base point of the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual OdGePoint2d snapBase(const OdRxObject* pViewport) const = 0;
  /** \details
    Sets the UCS snap base point of the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param snapBase [in]  Snap base.
  */
  virtual void setSnapBase(OdRxObject* pViewport, const OdGePoint2d& snapBase) const = 0; 

  /** \details
    Returns the snap increment of the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
  */
  virtual OdGeVector2d snapIncrement(const OdRxObject* pViewport) const = 0;
  /** \details
    Returns the snap increment of the specified Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param snapIncrement [in]  Snap increment.
  */
  virtual void setSnapIncrement(OdRxObject* pViewport, const OdGeVector2d& snapIncrement) const = 0;

  /** \details
    Returns the snap IsoPair of this Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.

    \remarks
    snapIsoPair() returns one of the following:
    
    <table>
    Value    Description
    0        Left isoplane
    1        Top isoplane
    2        Right isoplane
    </table>
  */
  virtual OdUInt16 snapIsoPair(const OdRxObject* pViewport) const = 0;
  /** \details
    Sets the snap IsoPair of this Viewport object.
    \param pViewport [in]  Pointer to the Viewport object.
    \param snapIsoPair [in]  Snap IsoPair
    \remarks
    snapIsoPair must be one of the following:
    
    <table>
    Value    Description
    0        Left isoplane
    1        Top isoplane
    2        Right isoplane
    </table>
  */
  virtual void setSnapIsoPair(OdRxObject* pViewport, OdUInt16 snapIsoPair) const = 0;

  virtual bool isDefaultLightingOn(const OdRxObject* pViewport) const = 0;
  virtual void setDefaultLightingOn(OdRxObject* pViewport, bool isOn) const = 0;

  virtual OdGiViewportTraits::DefaultLightingType defaultLightingType(const OdRxObject* pViewport) const = 0;
  virtual void setDefaultLightingType(OdRxObject* pViewport, OdGiViewportTraits::DefaultLightingType lightingType) const = 0;

  virtual double brightness(const OdRxObject* pViewport) const = 0;
  virtual void setBrightness(OdRxObject* pViewport, double brightness) const = 0;

  virtual double contrast(const OdRxObject* pViewport) const = 0;
  virtual void setContrast(OdRxObject* pViewport, double contrast) const = 0;

  virtual OdCmColor ambientLightColor(const OdRxObject* pViewport) const = 0;
  virtual void setAmbientLightColor(OdRxObject* pViewport, const OdCmColor& color) const = 0;

  virtual OdDbStub *sunId(const OdRxObject* pViewport) const = 0;
  virtual OdDbStub *setSun(OdRxObject* pViewport, OdRxObject* pSun) const = 0;

  virtual void toneOperatorParameters(const OdRxObject* pViewport, OdGiToneOperatorParameters &params) const = 0;
  virtual void setToneOperatorParameters(OdRxObject* pViewport, const OdGiToneOperatorParameters &params) const = 0;

  virtual OdGsView* gsView(const OdRxObject* pViewport) const = 0;
  virtual void setGsView(OdRxObject* pViewport, OdGsView* pGsView) const = 0;

  virtual int navvcubedisplay(const OdRxObject* pViewport) const;
  virtual OdResult setNavvcubedisplay(OdRxObject* pViewport, int nVal) const;
};

/** \details
    This template class is a specialization of the OdSmartPtr class for OdDbAbstractViewportData object pointers.
*/
typedef OdSmartPtr<OdDbAbstractViewportData> OdDbAbstractViewportDataPtr;

#endif //#ifndef OD_DBABSTRACTVIEWPORTDATA_H

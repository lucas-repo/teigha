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

#ifndef OD_GESURF_H
#define OD_GESURF_H /*!DOM*/

#include "Ge/GeEntity3d.h"
#include "Ge/GeVector3dArray.h"
#include "Ge/GePoint2d.h"

class OdGePoint2d;
class OdGeCurve3d;
class OdGePointOnCurve3d;
class OdGePointOnSurface;
class OdGePointOnSurfaceData;
class OdGeInterval;

#include "TD_PackPush.h"

/** \details
    This class is the base class for all OdGe parametric surfaces.

    Corresponding C++ library: TD_Ge

    <group OdGe_Classes> 
*/
class GE_TOOLKIT_EXPORT OdGeSurface : public OdGeEntity3d
{
public:


  /** \details
    Returns the 2D pair of parameter values of a point on this surface.

    \param point [in]  Point to be evaluated.
    \param tol [in]  Geometric tolerance.

    \remarks
    The returned parameters specify a point within tol of point.
    If point is not on this surface, the results are unpredictable.
    If you are not sure the point is on this surface, use 
    isOn() instead of this function.

    \remarks
    By default this function throws exception "not Implemented". Should be implemented in derived classes.
  */
   OdGePoint2d paramOf(
    const OdGePoint3d& point,
    const OdGeTol& tol = OdGeContext::gTol) const;

  TD_USING(OdGeEntity3d::isOn);
  /** \param ParamPoint [out]  Receives the 2D pair of parameter values at the point. 
   
    \remarks
    By default this function throws exception "not Implemented". Should be implemented in derived classes.
  */
   bool isOn( 
    const OdGePoint3d& point, 
    OdGePoint2d& paramPoint,
    const OdGeTol& tol = OdGeContext::gTol) const;

  /** \details
    Returns the point on this surface closest to the specified point.

    \param point [in]  Any 3D point.
    \param tol [in]  Geometric tolerance.
  */
   OdGePoint3d closestPointTo(
    const OdGePoint3d& point,
    const OdGeTol& tol = OdGeContext::gTol) const;

  /** \details
    Returns the point on this surface closest to the specified point.

    \param point [in]  Any 3D point.
    \param pntOnSurface [out]  Receives the closest point on surface to specified point. 
    \param tol [in]  Geometric tolerance.

    \remarks
    By default this function throws exception "not Implemented". Should be implemented in derived classes.
  */
   void getClosestPointTo(
    const OdGePoint3d& point, 
    OdGePointOnSurface& pntOnSurface,
    const OdGeTol& tol = OdGeContext::gTol) const;

  /** \details
    Returns the distance to the point on this curve closest to the specified point.

    \param point [in]  Any 3D point.
    \param tol [in]  Geometric tolerance.

    \remarks
    By default this function throws exception "not Implemented". Should be implemented in derived classes.
  */
   double distanceTo(
    const OdGePoint3d& point, 
    const OdGeTol& tol = OdGeContext::gTol) const;

  /** \details
    Returns true if and only if the normal of this surface has been reversed an odd
    number of times.
  */
   bool isNormalReversed() const;

  /** \details
    Reverses the normal of this surface and reurns a reference to this surface.
  */
   OdGeSurface& reverseNormal();

  /** \details
    Returns the minimum rectangle in parameter space that contains the parameter
    domain of this surface.

    \param intrvlU [out]  Receives the u interval.
    \param intrvlV [out]  Receives the v interval.

    \remarks
    By default this function throws exception "not Implemented". Should be implemented in derived classes.
  */
   void getEnvelope(
    OdGeInterval& intrvlU, 
    OdGeInterval& intrvlV) const;

  /** \details
    Returns true if and only if this surface is closed in the U direction.

    \param tol [in]  Geometric tolerance.

    \remarks
    By default this function throws exception "not Implemented". Should be implemented in derived classes.
  */
     bool isClosedInU(
    const OdGeTol& tol = OdGeContext::gTol) const;

  /** \details
    Returns true if and only if this surface is closed in the V direction.

    \param tol [in]  Geometric tolerance.

    \remarks
    By default this function throws exception "not Implemented". Should be implemented in derived classes.
  */
     bool isClosedInV(
    const OdGeTol& tol = OdGeContext::gTol) const;

  /** \details
    Returns the point corresponding to the parameter pair, as well as the
    derivatives and the normal at that point.

    \param param [in]  The parameter pair to be evaluated.
    \param numDeriv [in]  The number of derivatives to be computed.
    \param derivatives [out]  Receives an array of derivatives at the point corresponding to param.
    \param normal [out]  Receives the normal at the point corresponding to param.
    
    \remarks
    Derivatives are ordered as follows: du, dv, dudu, dvdv, dudv

    \remarks
    By default this function throws exception "not Implemented". Should be implemented in derived classes.
  */
   OdGePoint3d evalPoint(
    const OdGePoint2d& param) const;
   OdGePoint3d evalPoint(
    const OdGePoint2d& param, 
    int numDeriv,
    OdGeVector3dArray& derivatives) const;
   OdGePoint3d evalPoint(
    const OdGePoint2d& param,
    int numDeriv,
    OdGeVector3dArray& derivatives, 
    OdGeVector3d& normal) const;

  OdGeSurface& operator=(
    const OdGeSurface& surf);

  //////////////////////////////////////////////////////////////////////////
  // TD Special :

  /** \details
  Returns projP and true,
  if and only if there is a point on this surface, projP, where
  a normal to this surface passes through the point p.

  \param p [in]  Any 3D point.
  \param projP [out]  Receives the point on this surface. 
  */
   bool project(const OdGePoint3d& p, OdGePoint3d& projP, const OdGeTol& tol = OdGeContext::gTol) const;

protected:
  OdGeSurface();
  OdGeSurface(
    const OdGeSurface& surf);

};

#include "TD_PackPop.h"

#endif // OD_GESURF_H


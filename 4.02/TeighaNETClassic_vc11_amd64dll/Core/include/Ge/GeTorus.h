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

#ifndef OD_GETORUS_H
#define OD_GETORUS_H /*!DOM*/

#include "OdPlatform.h"
#include "Ge/GeSurface.h"
#include "Ge/GeCircArc3d.h"

#include "TD_PackPush.h"

/** \details
    This class represents toroidal segments.

    \remarks
    The torus is surface generated by revolving a circular arc
    about an axis of symmetry, where the plane of the circular arc 
    contains the axis of symmetry

    The torus is located in space by its center, which is a point on the axis of symmetry.  

    The center of the circular arc is at a distance of majorRadius from
    the center of the torus. The radius of the circular arc is the
    minorRadius.

    Parameter U is the longitude (about the axis of symmetry), which for a closed torus defaults
    to the range [-OdaPI, OdaPI).  Zero corresponds to the refAxis (which is
    a vector orthogonal to the axis of symmetry).  Applying the right
    hand rule along the symmetric axis defines the increasing direction
    for U.

    Parameter v parameterizes the circular tube, which
    for a closed circle defaults to the range [-OdaPI, OdaPI).  Applying the
    right hand rule along the refAxis X-axisOfSymmetry defines the
    increasing direction for v.

    The torus is periodic in U, v with a period of Oda2PI.
    [umin, umax] x [vmin, vmax] defines a four sided toroidal patch bounded
    by four circular arcs. Following constraints apply to the definition
    of a toroidal patch.

    * umin < umax and |umin - umax| <= Oda2PI.
    * vmin < vmax and |vmin - vmax| <= Oda2PI

    Corresponding C++ library: TD_Ge

   <group OdGe_Classes>

   <link ge_OdGeTorus.html, Working with Toruses>
*/
class GE_TOOLKIT_EXPORT OdGeTorus : public OdGeSurface
{
public:

  /** 
    \param majorRadius [in] The major *radius* of this *torus*.
    \param minorRadius [in] The minor *radius* of this *torus*.
    \param center [in] The origin of the this *torus*.
    \param axisOfSymmetry [in] Axis of symmetry (rotation).
    \param refAxis [in] defines thegle 0 about the axis of symmetry.
    \param startAngleU [in] Start angle about the axis of symmetry.
    \param endAngleU [in] End angle about the axis of symmetry.
    \param startAngleV [in] Start angle about the tube.
    \param endAngleV [in] End angle about the tube.
    \param source [in] Object to be cloned.
  */
  OdGeTorus();
  OdGeTorus(
    double majorRadius, 
    double minorRadius,
    const OdGePoint3d& center, 
    const OdGeVector3d& axisOfSymmetry);
  OdGeTorus(
    double majorRadius, 
    double minorRadius,
    const OdGePoint3d& center, 
    const OdGeVector3d& axisOfSymmetry,
    const OdGeVector3d& refAxis,
    double startAngleU, 
    double endAngleU,
    double startAngleV, 
    double endAngleV);
  OdGeTorus(
    const OdGeTorus& source);

  // Geometric properties.
  //

  /** \details
    Returns the major radius of this torus.
  */
   double majorRadius() const;

  /** \details
    Returns the minor radius of this torus.
  */
   double minorRadius() const;

  /** \details
    Returns the start and end angles about about the axis of symmetry.

    \param startAngleU [out]  Receives the angle about the axis of symmetry.
    \param endAngleU [out]  Receives the end angle about the axis of symmetry.
  */
   void getAnglesInU(
    double& startAngleU, 
    double& endAngleU) const;

  /** \details
    Returns the start and end angles about about the tube.

    \param startAngleV [out]  Receives the start angle about the tube.
    \param endAngleV [out]  Receives the end angle about the tube.
  */
   void getAnglesInV(
    double& startAngleV, 
    double& endAngleV) const;

  /** \details
    Returns the center of this torus.
  */
   OdGePoint3d center() const;

  /** \details
    Returns the Axis of symmetry (rotation).
  */
   OdGeVector3d axisOfSymmetry() const;

  /** \details
    Returns the reference axis.
  */
   OdGeVector3d refAxis() const;

  /** \details
    Returns true if and only if the normal to this surface
    is pointing outward.
  */
   bool isOuterNormal() const;

  /** \details
    Sets the major radius of this torus.

    \param majorRadius [in]  The major radius of this torus.
  */
   OdGeTorus& setMajorRadius(
    double radius);

  /** \details
    Sets the minor radius of this torus.

    \param minorRadius [in]  The minor radius of this torus.
  */
   OdGeTorus& setMinorRadius(
    double radius);

  /** \details
    Sets the start and end angles about about the axis of symmetry.

    \param startAngleU [in]  Start angle about the axis of symmetry.
    \param endAngleU [in]  End angle about the axis of symmetry.
  */
   OdGeTorus& setAnglesInU(
    double startAngleU, 
    double endAngleU);

  /** \details
    Sets the start and end angles about about the tube.

    \param startAngleV [in]  Start angle about the tube.
    \param endAngleV [in]  End angle about the tube.
  */
   OdGeTorus& setAnglesInV(
    double startAngleV, 
    double endAngleV);

  /** \details
    Sets the parameters for this torus according to the arguments. 

    \param majorRadius [in]  The major radius of this torus.
    \param minorRadius [in]  The minor radius of this torus.
    \param center [in]  The origin of the this torus.
    \param axisOfSymmetry [in]  Axis of symmetry (rotation).
    \param refAxis [in]  defines thegle 0 about the axis of symmetry.
    \param startAngleU [in]  Start angle about the axis of symmetry.
    \param endAngleU [in]  End angle about the axis of symmetry.
    \param startAngleV [in]  Start angle about the tube.
    \param endAngleV [in]  End angle about the tube.
    \returns
    Returns a reference to this torus.
  */
   OdGeTorus& set(
    double majorRadius, 
    double minorRadius,
    const OdGePoint3d& center,
    const OdGeVector3d& axisOfSymmetry);
   OdGeTorus& set(
    double majorRadius, 
    double minorRadius,
    const OdGePoint3d&  center,
    const OdGeVector3d& axisOfSymmetry,
    const OdGeVector3d& refAxis,
    double startAngleU, 
    double endAngleU,
    double startAngleV, 
    double endAngleV);

  OdGeTorus& operator =(
    const OdGeTorus& torus);

  /** \details
    Returns True if the torus intersects with the specified
    line entity, and returns the number of intersections and the
    points of intersection.

    \param lineEnt [in]  Any 3D line entity.
    \param numInt [out]  Receives the number of intersections.
    \param p1 [out]  Receives the first intersection point.
    \param p2 [out]  Receives the second intersection point.
    \param p3 [out]  Receives the third intersection point.
    \param p4 [out]  Receives the fourth intersection point.
    \param tol [in]  Geometric tolerance.

    \remarks
    * p1 is valid if and only if numInt > 0.
    * p2 is valid if and only if numInt > 1.
    * p3 is valid if and only if numInt > 2.
    * p4 is valid if and only if numInt > 3.
  */
   bool intersectWith(
    const OdGeLinearEnt3d& linEnt, 
    int& numInt,
    OdGePoint3d& p1, 
    OdGePoint3d& p2,
    OdGePoint3d& p3, 
    OdGePoint3d& p4,
    const OdGeTol& tol = OdGeContext::gTol) const;

  // Shape Classification Functions

  /** \details
    Returns true if and only if (majorRadius < 0) and (|majorRadius| < minorRadius), producing
    a solid with points along the axis of symmetry.

    \remarks
    Exactly one of the following functions will be true for a given torus:

    * isApple() 
    * isDoughnut() 
    * isLemon() 
    * isVortex()
  */
   bool isLemon() const;

  /** \details
    Returns true if and only if (0 < majorRadius < minorRadius), creating a solid with dimples at the
    axis of symmetry.

    \remarks
    Exactly one of the following functions will be true for a given torus:
    * isApple() 
    * isDoughnut() 
    * isLemon() 
    * isVortex()
  */
   bool isApple() const;

  /** \details
    Returns true if and only if (minorRadius == majorRadius), producing a donut.
    with a zero-radius hole.

    \remarks
    Exactly one of the following functions will be true for a given torus:
    * isApple() 
    * isDoughnut() 
    * isLemon() 
    * isVortex()
  */
   bool isVortex() const;

  /** \details
    Returns true if and only if (minorRadius <  majorRadius), creating a solid with a hole in the middle.

    \remarks
    Exactly one of the following functions will be true for a given torus:
    * isApple() 
    * isDoughnut() 
    * isLemon() 
    * isVortex()
  */
   bool isDoughnut() const;

  /** \details
    Returns true if and only if (minorRadius <= 0) OR (majorRadius <= -minorRadius) 
  */
   bool isDegenerate() const;

  /** \details
    Returns true if and only if there is a hole in the torus.

    \returns
    Returns true if and only if |majorRadius| > |minorRadius| + 1e-10
  */
   bool isHollow() const;

  //////////////////////////////////////////////////////////////////////////

};

#include "TD_PackPop.h"

#endif // OD_GETORUS_H


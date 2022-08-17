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

#ifndef __ODGICONTEXTUALCOLORS_H__
#define __ODGICONTEXTUALCOLORS_H__

#include "Gi/GiExport.h"
#include "RxObject.h"
#include "CmColor.h"

#include "TD_PackPush.h"

/** \details
    Define colors depend from vectorization context.

    \sa
    TD_Gi 
    <group OdGi_Classes> 
*/
class ODGI_EXPORT OdGiContextualColors : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdGiContextualColors);

  // Color accessors

  // Grid
  virtual OdCmEntityColor gridMajorLines() const;
  virtual OdCmEntityColor gridMinorLines() const;
  virtual OdCmEntityColor gridAxisLines() const;
  virtual bool gridMajorLineTintXYZ() const;
  virtual bool gridMinorLineTintXYZ() const;
  virtual bool gridAxisLineTintXYZ() const;

  // Light
  virtual OdCmEntityColor lightGlyphs() const;
  virtual OdCmEntityColor lightHotspot() const;
  virtual OdCmEntityColor lightFalloff() const;
  virtual OdCmEntityColor lightStartLimit() const;
  virtual OdCmEntityColor lightEndLimit() const;
  virtual OdCmEntityColor lightShapeColor() const;
  virtual OdCmEntityColor lightDistanceColor() const;
  virtual OdCmEntityColor webMeshColor() const;
  virtual OdCmEntityColor webMeshMissingColor() const;

  // Camera
  virtual OdCmEntityColor cameraGlyphs() const;
  virtual OdCmEntityColor cameraFrustrum() const;
  virtual OdCmEntityColor cameraClipping() const;

  // Runtime parameters
  virtual void setContextFlags(OdUInt32 nFlags, bool bSet = true) = 0;
  virtual bool flagsSet(OdUInt32 nFlags) const = 0;

  // This part is for internal use. Provides interface extensibility feature.

  enum ColorType
  {
    kGridMajorLinesColor = 0,
    kGridMinorLinesColor,
    kGridAxisLinesColor,
    kLightGlyphsColor,
    kLightHotspotColor,
    kLightFalloffColor,
    kLightStartLimitColor,
    kLightEndLimitColor,
    kLightShapeColor,
    kLightDistanceColor,
    kWebMeshColor,
    kWebMeshMissingColor,
    kCameraGlyphsColor,
    kCameraFrustrumColor,
    kCameraClippingColor,

    kNumColors
  };

  enum ColorTint
  {
    kGridMajorLineTint = 0,
    kGridMinorLineTint,
    kGridAxisLineTint,

    kNumTintFlags
  };

  virtual OdCmEntityColor contextualColor(ColorType /*type*/) const { return OdCmEntityColor(OdCmEntityColor::kForeground); }
  virtual bool contextualColorTint(ColorTint /*type*/) const { return false; }
};

typedef OdSmartPtr<OdGiContextualColors> OdGiContextualColorsPtr;

// Default implementations of OdGiContextualColors accessor methods

inline OdCmEntityColor OdGiContextualColors::gridMajorLines() const
{
  return contextualColor(kGridMajorLinesColor);
}

inline OdCmEntityColor OdGiContextualColors::gridMinorLines() const
{
  return contextualColor(kGridMinorLinesColor);
}

inline OdCmEntityColor OdGiContextualColors::gridAxisLines() const
{
  return contextualColor(kGridAxisLinesColor);
}

inline bool OdGiContextualColors::gridMajorLineTintXYZ() const
{
  return contextualColorTint(kGridMajorLineTint);
}

inline bool OdGiContextualColors::gridMinorLineTintXYZ() const
{
  return contextualColorTint(kGridMinorLineTint);
}

inline bool OdGiContextualColors::gridAxisLineTintXYZ() const
{
  return contextualColorTint(kGridAxisLineTint);
}

inline OdCmEntityColor OdGiContextualColors::lightGlyphs() const
{
  return contextualColor(kLightGlyphsColor);
}

inline OdCmEntityColor OdGiContextualColors::lightHotspot() const
{
  return contextualColor(kLightHotspotColor);
}

inline OdCmEntityColor OdGiContextualColors::lightFalloff() const
{
  return contextualColor(kLightFalloffColor);
}

inline OdCmEntityColor OdGiContextualColors::lightStartLimit() const
{
  return contextualColor(kLightStartLimitColor);
}

inline OdCmEntityColor OdGiContextualColors::lightEndLimit() const
{
  return contextualColor(kLightEndLimitColor);
}

inline OdCmEntityColor OdGiContextualColors::lightShapeColor() const
{
  return contextualColor(kLightShapeColor);
}

inline OdCmEntityColor OdGiContextualColors::lightDistanceColor() const
{
  return contextualColor(kLightDistanceColor);
}

inline OdCmEntityColor OdGiContextualColors::webMeshColor() const
{
  return contextualColor(kWebMeshColor);
}

inline OdCmEntityColor OdGiContextualColors::webMeshMissingColor() const
{
  return contextualColor(kWebMeshMissingColor);
}

inline OdCmEntityColor OdGiContextualColors::cameraGlyphs() const
{
  return contextualColor(kCameraGlyphsColor);
}

inline OdCmEntityColor OdGiContextualColors::cameraFrustrum() const
{
  return contextualColor(kCameraFrustrumColor);
}

inline OdCmEntityColor OdGiContextualColors::cameraClipping() const
{
  return contextualColor(kCameraClippingColor);
}

/** \details
    Provides default implementation for OdGiContextualColors.

    \sa
    TD_Gi 
    <group OdGi_Classes> 
*/
class ODGI_EXPORT OdGiContextualColorsImpl : public OdGiContextualColors
{
public:
  ODRX_DECLARE_MEMBERS(OdGiContextualColorsImpl);

  enum VisualType
  {
    kVisualTypeNotSet = -1,
    k2dModel = 0,
    kLayout,
    k3dParallel,
    k3dPerspective,
    kBlock,

    kNumVisualTypes
  };
  virtual void setVisualType(VisualType type) = 0;
  virtual VisualType visualType() const = 0;

  virtual void setContextualColor(ColorType type, const OdCmEntityColor &color) = 0;
  void setContextualColor(ColorType type, ODCOLORREF color) { setContextualColor(type, ODTOCMCOLOR(color)); }
  virtual void setContextualColorTint(ColorTint type, bool bSet) = 0;

  virtual void setDefaultForType() = 0;
};

typedef OdSmartPtr<OdGiContextualColorsImpl> OdGiContextualColorsImplPtr;

/** \details
    Provides redirection ability for OdGiContextualColors.

    \sa
    TD_Gi 
    <group OdGi_Classes> 
*/
class ODGI_EXPORT OdGiContextualColorsRedir : public OdGiContextualColors
{
public:
  ODRX_DECLARE_MEMBERS(OdGiContextualColorsRedir);

  virtual void setRedirectionObject(OdGiContextualColors *pObj) = 0;
  virtual const OdGiContextualColors *redirectionObject() const = 0;
  virtual OdGiContextualColors *redirectionObject() = 0;
};

typedef OdSmartPtr<OdGiContextualColorsRedir> OdGiContextualColorsRedirPtr;

#include "TD_PackPop.h"

#endif // __ODGICONTEXTUALCOLORS_H__

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

#ifndef __ODGIMATERIAL_H__
#define __ODGIMATERIAL_H__

#include "TD_PackPush.h"
#include "RootExport.h"
#include "GiExport.h"

#include "CmColor.h"
#include "Ge/GeMatrix3d.h"
#include "Gi/GiDrawable.h"
#include "Gi/GiVariant.h"

class OdGiMaterialColor;
class OdGiMaterialMap;
class OdGiRasterImage;

/** \details
    This class defines the interface to material traits for the vectorization process.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODRX_ABSTRACT FIRSTDLL_EXPORT OdGiMaterialTraits : public OdGiDrawableTraits
{
public:
  ODRX_DECLARE_MEMBERS(OdGiMaterialTraits);

  enum IlluminationModel {
    kBlinnShader = 0,
    kMetalShader
  };

  enum ChannelFlags
  {
    kNone           = 0x00000,
    kUseDiffuse     = 0x00001,
    kUseSpecular    = 0x00002,
    kUseReflection  = 0x00004,
    kUseOpacity     = 0x00008,
    kUseBump        = 0x00010,
    kUseRefraction  = 0x00020,
    kUseNormalMap   = 0x00040,
    kUseEmission    = 0x00080,

    kUseAll         = (kUseDiffuse | kUseSpecular | kUseReflection |
                       kUseOpacity | kUseBump | kUseRefraction | kUseNormalMap),
    kUseAllInternal = (kUseAll | kUseEmission)
  };

  enum Mode {
    kRealistic = 0,
    kAdvanced
  };

  // Returned via subSetAttributes()
  enum
  {
    kByBlock    = (OdGiDrawable::kLastFlag << 1),
    kByLayer    = (OdGiDrawable::kLastFlag << 2)
  };

  // Extended material traits enumerations

  enum LuminanceMode
  {
    kSelfIllumination = 0,
    kLuminance,
    kEmissionColor
  };

  enum NormalMapMethod
  {
    kTangentSpace
  };

  enum GlobalIlluminationMode
  {
    kGlobalIlluminationNone,
    kGlobalIlluminationCast,
    kGlobalIlluminationReceive,
    kGlobalIlluminationCastAndReceive
  };

  enum FinalGatherMode
  {
    kFinalGatherNone,
    kFinalGatherCast,
    kFinalGatherReceive,
    kFinalGatherCastAndReceive
  };

  /** \details
    Returns the ambient color component of this MaterialTraits object.

    \param ambientColor [out]  Receives the ambient color.

    \remarks
    The ambient component component is most apparent when there is no direct illumination on the entity.
  */
  virtual void ambient(OdGiMaterialColor& ambientColor) const = 0;

  /** \details
    Returns the diffuse component of this MaterialTraits object.

    \param diffuseColor [out]  Receives the diffuse color.
    \param diffuseMap [out]  Receives the diffuse map.

    \remarks
    The diffuse component is most apparent when there is direct illumination on the entity.
  */
  virtual void diffuse(OdGiMaterialColor& diffuseColor, OdGiMaterialMap& diffuseMap) const = 0;

  /** \details
    Returns the specular component of this MaterialTraits object.

    \param specularColor [out]  Receives the specular color.
    \param specularMap [out]  Receives the specular map.
    \param glossFactor [out]  Receives the gloss factor.

    \remarks
    The specular component is viewpoint dependent, and most apparent when the entity is highlighted.
  */
  virtual void specular(OdGiMaterialColor& specularColor, OdGiMaterialMap& specularMap, double& glossFactor) const = 0;

  /** \details
    Returns the reflection component of this MaterialTraits object.

    \param reflectionMap [out]  Receives the reflection map.

    \remarks
    The reflection component creates a mirror finish on the entity .
  */
  virtual void reflection(OdGiMaterialMap& reflectionMap) const = 0;

  /** \details
    Returns the opacity component of this MaterialTraits object.

    \param opacityPercentage [out]  Receives the opacity percentage.
    \param opacityMap [out]  Receives the opacity map.
  */
  virtual void opacity(double& opacityPercentage, OdGiMaterialMap& opacityMap) const = 0;

  /** \details
    Returns the bump component of this MaterialTraits object.

    \param bumpMap [out]  Receives the bump map.
  */
  virtual void bump(OdGiMaterialMap& bumpMap) const = 0;

  /** \details
    Returns the refraction component of this MaterialTraits object.

    \param refractionIndex [out]  Receives the refraction index.
    \param refractionMap [out]  Receives the refraction map.
  */
  virtual void refraction(double& refractionIndex, OdGiMaterialMap& refractionMap) const = 0;

  /** \details
    Returns the translucence of this MaterialTraits object.
  */
  virtual double translucence() const = 0;

  /** \details
    Returns the self illumination of this MaterialTraits object.
  */
  virtual double selfIllumination() const = 0;

  /** \details
    Returns the reflectivity of this MaterialTraits object.
  */
  virtual double reflectivity() const = 0;

  /** \details
    Returns the illumination model of this MaterialTraits object.
  */
  virtual IlluminationModel illuminationModel() const = 0;

  /** \details
    Returns the channel flags of this MaterialTraits object.
  */
  virtual ChannelFlags channelFlags() const = 0;

  /** \details
    Returns the mode of this MaterialTraits object.
  */
  virtual Mode mode() const = 0;

  /** \details
    Sets the ambient color component of this MaterialTraits object.

    \param ambientColor [in]  Ambient color.

    \remarks
    The ambient component component is most apparent when there is no direct illumination on the entity.
  */
  virtual void setAmbient(const OdGiMaterialColor& ambientColor) = 0;

  /** \details
    Sets the diffuse component of this MaterialTraits object.

    \param diffuseColor [in]  Diffuse color.
    \param diffuseMap [in]  Diffuse map.

    \remarks
    The diffuse component is most apparent when there is direct illumination on the entity.
  */
  virtual void setDiffuse(const OdGiMaterialColor& diffuseColor, const OdGiMaterialMap& diffuseMap) = 0;

  /** \details
    Sets the specular component of this MaterialTraits object.

    \param specularColor [in]  Specular color.
    \param specularMap [in]  Specular map.
    \param glossFactor [in]  Gloss factor.

    \remarks
    The specular component is viewpoint dependent, and most apparent when the entity is highlighted.
  */
  virtual void setSpecular(const OdGiMaterialColor& specularColor, const OdGiMaterialMap& specularMap, double glossFactor) = 0;

  /** \details
    Sets the reflection component of this MaterialTraits object.

    \param reflectionMap [in]  Reflection map.

    \remarks
    The reflection component creates a mirror finish on the entity.
  */
  virtual void setReflection(const OdGiMaterialMap& reflectionMap) = 0;

  /** \details
    Sets the opacity component of this MaterialTraits object.

    \param opacityPercentage [in]  Opacity percentage.
    \param opacityMap [in]  Opacity map.
  */
  virtual void setOpacity(double opacityPercentage, const OdGiMaterialMap& opacityMap) = 0;

  /** \details
    Sets the bump component of this MaterialTraits object.

    \param bumpMap [in]  Bump map.
  */
  virtual void setBump(const OdGiMaterialMap& bumpMap) = 0;

  /** \details
    Sets the refraction component of this MaterialTraits object.

    \param refractionIndex [in]  Refraction index.
    \param refractionMap [in]  Refraction map.
  */
  virtual void setRefraction(double refractionIndex, const OdGiMaterialMap& refractionMap) = 0;

  /** \details
    Sets the translucence of this MaterialTraits object.

    \param value [in]  Translucence value.
  */
  virtual void setTranslucence(double value) = 0;

  /** \details
    Sets the self illumination of this MaterialTraits object.

    \param value [in]  Self illumination level.
  */
  virtual void setSelfIllumination(double value) = 0;

  /** \details
    Sets the reflectivity of this MaterialTraits object.

    \param value [in]  Reflectivity value.
  */
  virtual void setReflectivity(double value) = 0;

  /** \details
    Sets the illumination model of this MaterialTraits object.

    \param model [in]  Illumination model.
  */
  virtual void setIlluminationModel(IlluminationModel model) = 0;

  /** \details
    Sets the channel flags of this MaterialTraits object.

    \param value [in]  Channel flags.
  */
  virtual void setChannelFlags(ChannelFlags flags) = 0;

  /** \details
    Sets the mode of this MaterialTraits object.

    \param value [in]  Mode value.
  */
  virtual void setMode(Mode value) = 0;

  // Extended material properties

  /** \details
    Sets the color bleed scale of this MaterialTraits object.

    \param scale [in]  Color bleed scale.
  */
  virtual void setColorBleedScale(double scale) = 0;
  /** \details
    Returns the color bleed scale of this MaterialTraits object.
  */
  virtual double colorBleedScale() const = 0;

  /** \details
    Sets the indirect bump scale of this MaterialTraits object.

    \param scale [in]  Indirect bump scale.
  */
  virtual void setIndirectBumpScale(double scale) = 0;
  /** \details
    Returns the indirect bump scale of this MaterialTraits object.
  */
  virtual double indirectBumpScale() const = 0;

  /** \details
    Sets the reflectance scale of this MaterialTraits object.

    \param scale [in]  Reflectance scale.
  */
  virtual void setReflectanceScale(double scale) = 0;
  /** \details
    Returns the reflectance scale of this MaterialTraits object.
  */
  virtual double reflectanceScale() const = 0;

  /** \details
    Sets the transmittance scale of this MaterialTraits object.

    \param scale [in]  Transmittance scale.
  */
  virtual void setTransmittanceScale(double scale) = 0;
  /** \details
    Returns the transmittance scale of this MaterialTraits object.
  */
  virtual double transmittanceScale() const = 0;

  /** \details
    Sets the two sided mode of this MaterialTraits object.

    \param flag [in]  Two sided mode flag.
  */
  virtual void setTwoSided(bool flag) = 0;
  /** \details
    Returns the two sided mode of this MaterialTraits object.
  */
  virtual bool twoSided() const = 0;

  /** \details
    Sets the luminance mode of this MaterialTraits object.

    \param mode [in]  Luminance mode.
  */
  virtual void setLuminanceMode(LuminanceMode mode) = 0;
  /** \details
    Returns the luminance mode of this MaterialTraits object.
  */
  virtual LuminanceMode luminanceMode() const = 0;

  /** \details
    Sets the luminance of this MaterialTraits object.

    \param value [in]  Luminance value.
  */
  virtual void setLuminance(double value) = 0;
  /** \details
    Returns the luminance of this MaterialTraits object.
  */
  virtual double luminance() const = 0;

  /** \details
    Sets the normalMap component of this MaterialTraits object.

    \param normalMap [in]  Normal map.
    \param method [in]  Normal map method.
    \param strength [in]  Strength factor.
  */
  virtual void setNormalMap(const OdGiMaterialMap &normalMap, NormalMapMethod method, double strength) = 0;
  /** \details
    Returns the normalMap component of this MaterialTraits object.

    \param normalMap [out]  Receives the normal map.
    \param method [out]  Receives the normal map method.
    \param strength [out]  Receives the normal map strength factor.
  */
  virtual void normalMap(OdGiMaterialMap &normalMap, NormalMapMethod &method, double &strength) const = 0;

  /** \details
    Sets the global illumination mode of this MaterialTraits object.

    \param mode [in]  Global illumination mode.
  */
  virtual void setGlobalIllumination(GlobalIlluminationMode mode) = 0;
  /** \details
    Returns the global illumination mode of this MaterialTraits object.
  */
  virtual GlobalIlluminationMode globalIllumination() const = 0;

  /** \details
    Sets the final gather mode of this MaterialTraits object.

    \param mode [in]  Final gather mode.
  */
  virtual void setFinalGather(FinalGatherMode mode) = 0;
  /** \details
    Returns the final gather mode of this MaterialTraits object.
  */
  virtual FinalGatherMode finalGather() const = 0;

  /** \details
    Sets the emission component of this MaterialTraits object.

    \param emissionColor [in]  Emission color.
    \param emissionMap [in]  Emission map.
  */
  virtual void setEmission(const OdGiMaterialColor& emissionColor, const OdGiMaterialMap& emissionMap) = 0;
  /** \details
    Returns the emission component of this MaterialTraits object.

    \param emissionColor [out]  Receives the emission color.
    \param emissionMap [out]  Receives the emission map.
  */
  virtual void emission(OdGiMaterialColor& emissionColor, OdGiMaterialMap& emissionMap) const = 0;
};

/** \details
    This template class is a specialization of the OdSmartPtr class for OdGiMaterialTraits object pointers.
*/
typedef OdSmartPtr<OdGiMaterialTraits> OdGiMaterialTraitsPtr;


/** \details
    This class implements material colors by color method and value.

    \sa
    TD_Gi

    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiMaterialColor
{
public:
  enum Method
  {
    kInherit  = 0, // Uses the current drawing *color*.
    kOverride = 1  // Uses the *color* set with setColor.
  };

  ODGI_EXPORT_STATIC static const OdGiMaterialColor kNull;

  OdGiMaterialColor();

  /** \details
    Sets the color method for this MaterialColor object.

    \param method [in]  Color method.

    \remarks
    method must be one of the following:

    <table>
    Name          Value    Description
    kInherit      0        Uses the current drawing color.
    kOverride     1        Uses the color set with setColor.
    </table>
  */
  void setMethod(Method method);

  /** \details
    Sets the color factor for this MaterialColor object.
    \param factor [in]  Color factor.
    \remarks
    A color factor of 0.0 makes all colors black; a color factor of 1.0 leaves them unchanged.
  */
  void setFactor(double factor);

  /** \details
    Returns a reference to, or a copy of, the color of this MaterialColor object as
    an OdCmEntityColor instance.
  */
  OdCmEntityColor& color();


  /** \details
    Returns the color method for this MaterialColor object.

    \remarks
    method() returns one of the following:

    <table>
    Name          Value    Description
    kInherit      0        Uses the current drawing color.
    kOverride     1        Uses the color set with setColor.
    </table>
  */
  Method method() const;

  /** \details
    Returns the color factor for this MaterialColor object.


    \remarks
    A color factor of 0.0 makes all colors black; a color factor of 1.0 leaves them unchanged.
  */
  double factor() const;

  /** \details
    Returns a constant reference to, or a copy of, the color of this MaterialColor object as
    an constant OdCmEntityColor instance.
  */
  const OdCmEntityColor& color() const;

  bool operator ==(const OdGiMaterialColor &other) const
  {
    return (m_method == other.m_method) &&
           (m_factor == other.m_factor) &&
           (m_color  == other.m_color);      
  }
  bool operator != (const OdGiMaterialColor &other) const
  {
    return (m_method != other.m_method) ||
           (m_factor != other.m_factor) ||
           (m_color  != other.m_color);      
  }

  /** \details
    Sets the color for this MaterialColor object.
    \param color [in]  New color for this MaterialColor object.
  */
  void setColor(const OdCmEntityColor &color);

private:
  Method          m_method;
  double          m_factor;
  OdCmEntityColor m_color;
};


/** \details
    This class defines material textures.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODRX_ABSTRACT FIRSTDLL_EXPORT OdGiMaterialTexture : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdGiMaterialTexture);
  virtual bool operator==(const OdGiMaterialTexture & texture)const;
protected:
  OdGiMaterialTexture() { }
  ~OdGiMaterialTexture() { }
};

inline bool OdGiMaterialTexture::operator==(const OdGiMaterialTexture & texture) const {
  return (texture.isA() == isA());
}

/** \details
This template class is a specialization of the OdSmartPtr class for OdGiMaterialTexture object pointers.
*/
typedef OdSmartPtr<OdGiMaterialTexture> OdGiMaterialTexturePtr;

/** \details
     This class defines image based material textures.
     Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODRX_ABSTRACT FIRSTDLL_EXPORT OdGiImageTexture : public OdGiMaterialTexture
{
public:
  ODRX_DECLARE_MEMBERS(OdGiImageTexture);
protected:
  OdGiImageTexture() { }
  ~OdGiImageTexture() { }
};

/** \details
This template class is a specialization of the OdSmartPtr class for OdGiImageTexture object pointers.
*/
typedef OdSmartPtr<OdGiImageTexture> OdGiImageTexturePtr;

/** \details
     This class defines image file based material texture.
     Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class FIRSTDLL_EXPORT OdGiImageFileTexture : public OdGiImageTexture
{
public:
  ODRX_DECLARE_MEMBERS(OdGiImageFileTexture);

  virtual void setSourceFileName(const OdString& fileName);
  virtual const OdString sourceFileName() const;

  virtual bool operator==(const OdGiMaterialTexture & texture) const;
  OdGiImageFileTexture &operator =(const OdGiImageFileTexture &texture);

  // OdRxObject overrides
  void copyFrom(const OdRxObject* pSource);
private:
  OdString m_sourceFileName;
};

/** \details
This template class is a specialization of the OdSmartPtr class for OdGiImageFileTexture object pointers.
*/
typedef OdSmartPtr<OdGiImageFileTexture> OdGiImageFileTexturePtr;

/** \details
     This class defines raster image based material texture.
     Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class FIRSTDLL_EXPORT OdGiRasterImageTexture : public OdGiImageTexture
{
public:
  ODRX_DECLARE_MEMBERS(OdGiRasterImageTexture);

  virtual void setRasterImage(const OdGiRasterImage *pRasterImage);
  virtual const OdGiRasterImage *rasterImage() const;

  virtual bool operator==(const OdGiMaterialTexture & texture) const;
  OdGiRasterImageTexture &operator =(const OdGiRasterImageTexture &texture);

  // OdRxObject overrides
  void copyFrom(const OdRxObject* pSource);
private:
  OdRxObjectPtr m_pRasterImage;
};

/** \details
This template class is a specialization of the OdSmartPtr class for OdGiImageFileTexture object pointers.
*/
typedef OdSmartPtr<OdGiRasterImageTexture> OdGiRasterImageTexturePtr;

/** \details
     This class defines procedural material textures.
     Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODRX_ABSTRACT FIRSTDLL_EXPORT OdGiProceduralTexture : public OdGiMaterialTexture
{
public:
  ODRX_DECLARE_MEMBERS(OdGiProceduralTexture);

  enum Type
  {
    kWood    = 0,
    kMarble  = 1,
    kGeneric = 2
  };

  virtual Type type() const = 0;
protected:
  OdGiProceduralTexture() { }
  ~OdGiProceduralTexture() { }
};

/** \details
This template class is a specialization of the OdSmartPtr class for OdGiProceduralTexture object pointers.
*/
typedef OdSmartPtr<OdGiProceduralTexture> OdGiProceduralTexturePtr;

/** \details
    This class defines wood procedural material textures.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class FIRSTDLL_EXPORT OdGiWoodTexture : public OdGiProceduralTexture
{
public:
  ODRX_DECLARE_MEMBERS(OdGiWoodTexture);

  OdGiWoodTexture();

  virtual Type type() const  {return kWood;}

  // wood properties

  virtual void setColor1(const OdGiMaterialColor &woodColor1);
  virtual const OdGiMaterialColor& color1(void) const;

  virtual void setColor2(const OdGiMaterialColor &woodColor2);
  virtual const OdGiMaterialColor& color2(void) const;

  virtual void setRadialNoise(double radialNoise);
  virtual double radialNoise(void) const;

  virtual void setAxialNoise(double axialNoise);
  virtual double axialNoise(void) const;

  virtual void setGrainThickness(double grainThickness);
  virtual double grainThickness(void) const;

  virtual bool operator==(const OdGiMaterialTexture & texture) const;
  OdGiWoodTexture &operator =(const OdGiWoodTexture &texture);

  // OdRxObject overrides
  void copyFrom(const OdRxObject* pSource);
private:
  OdGiMaterialColor m_color1;
  OdGiMaterialColor m_color2;
  double            m_radialNoise;
  double            m_axialNoise;
  double            m_grainThickness;
};

/** \details
This template class is a specialization of the OdSmartPtr class for OdGiWoodTexture object pointers.
*/
typedef OdSmartPtr<OdGiWoodTexture> OdGiWoodTexturePtr;

/** \details
    This class defines marble procedural material texture.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class FIRSTDLL_EXPORT OdGiMarbleTexture : public OdGiProceduralTexture
{
public:
  ODRX_DECLARE_MEMBERS(OdGiMarbleTexture);

  OdGiMarbleTexture();

  virtual Type type() const  {return kMarble;}

  virtual void setStoneColor(const OdGiMaterialColor &stoneColor);
  virtual const OdGiMaterialColor& stoneColor(void) const;

  virtual void setVeinColor(const OdGiMaterialColor &veinColor);
  virtual const OdGiMaterialColor& veinColor(void) const;

  virtual void setVeinSpacing(double veinSpacing);
  virtual double veinSpacing(void) const;

  virtual void setVeinWidth(double veinWidth);
  virtual double veinWidth(void) const;

  virtual bool operator==(const OdGiMaterialTexture & texture) const;
  OdGiMarbleTexture &operator =(const OdGiMarbleTexture &texture);

  // OdRxObject overrides
  void copyFrom(const OdRxObject* pSource);
private:
  OdGiMaterialColor m_stoneColor;
  OdGiMaterialColor m_veinColor;
  double            m_veinSpacing;
  double            m_veinWidth;
};

/** \details
This template class is a specialization of the OdSmartPtr class for OdGiMarbleTexture object pointers.
*/
typedef OdSmartPtr<OdGiMarbleTexture> OdGiMarbleTexturePtr;

/** \details
    This class defines generic procedural material texture.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class FIRSTDLL_EXPORT OdGiGenericTexture : public OdGiProceduralTexture
{
public:
  ODRX_DECLARE_MEMBERS(OdGiGenericTexture);

  OdGiGenericTexture();

  virtual Type type() const  {return kGeneric;}

  virtual void setDefinition(const OdGiVariant &definition);
  virtual OdGiVariantPtr definition() const;
  virtual void definition(OdGiVariantPtr &pDefinition) const;

  virtual bool operator==(const OdGiMaterialTexture & texture) const;
  OdGiGenericTexture &operator =(const OdGiGenericTexture &texture);

  // OdRxObject overrides
  void copyFrom(const OdRxObject* pSource);
private:
  OdGiVariantPtr    m_definition;
};

/** \details
This template class is a specialization of the OdSmartPtr class for OdGiGenericTexture object pointers.
*/
typedef OdSmartPtr<OdGiGenericTexture> OdGiGenericTexturePtr;


/** \details
    This class implements mappers.


    \remarks
    Mappers determine how an OdGiMaterialMap is mapped to an object surface.

    \sa
    TD_Gi

    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiMapper
{
public:
  enum Projection
  {
    kInheritProjection   = 0, // Inherits *projection* from the current material's mapper.
    kPlanar              = 1, // Maps directly to XY coordinates.
    kBox                 = 2, // Maps to planes perpendicular to major axes.
    kCylinder            = 3, // Maps to cylinder aligned with Z-axis.
    kSphere              = 4, // Maps to sphere aligned with Z-axis

    kDgnParametric       = 0x32, // Maps directly to surface coordinates. For dgn materials.
    kDgnPlanar           = 0x33, // Maps directly to surface coordinates. For dgn materials.
    kDgnCylinder         = 0x34, // Maps to cylinder aligned with Z-axis. For dgn materials.
    kDgnCylinderCapped   = 0x35, // Maps to cylinder aligned with Z-axis. If normal to surface same as Z-axis
                                 // then used planar mapping. For dgn materials.
    kDgnSphere           = 0x36  // Maps to sphere aligned with Z-axis. For dgn materials.
  };

  enum Tiling
  {
    kInheritTiling    = 0, // Inherits *tiling* from the current material's mapper.
    kTile             = 1, // Repeats map along image axes.
    kCrop             = 2, // Crops map < 0.0 or > 1.0 on image axes.
    kClamp            = 3, // Clamps (stretches) map between 0.0 and 1.0 on image axes.
    kMirror           = 4  // Mirror the material map at every integer boundary.
  };

  enum AutoTransform
  {
    kInheritAutoTransform = 0x0, // Inherits automatic *transform* from the current material/s mapper.
    kNone                 = 0x1, // No automatic *transform*.
    kObject               = 0x2, // Adjusts the mapper *transform* to align with and fit the current object.
    kModel                = 0x4  // Multiples the mapper *transform* by the current block *transform*.
  };

  ODGI_EXPORT_STATIC static const OdGiMapper kIdentity;

  OdGiMapper()
    : m_projection(kPlanar)
    , m_uTiling(kTile)
    , m_vTiling(kTile)
    , m_autoTransform(kNone)
  {
  }

  OdGiMapper (const OdGiMapper& mapper);
  OdGiMapper& operator=(const OdGiMapper& mapper);
  bool operator==(const OdGiMapper& mapper) const;
  bool operator!=(const OdGiMapper& mapper) const;

  /** \details
    Sets the type of projection for this Mapper object.

    \param projection [in]  Projection type.

    \remarks
    projection must be one of the following:

    <table>
    Name                  Value   Description
    kInheritProjection    0       Inherits projection from the current material's mapper.
    kPlanar               1       Maps directly to XY coordinates.
    kBox                  2       Maps to planes perpendicular to major axes.
    kCylinder             3       Maps to cylinder aligned with Z-axis.
    kSphere               4       Maps to sphere aligned with Z-axis
    </table>
  */
  void setProjection(Projection projection);

  /** \details
    Sets the type of X-axis tiling for this Mapper object.

    \param tiling [in]  Tiling type.

    \remarks
    tiling must be one of the following:

    <table>
    Name                  Value   Description
    kInheritTiling        0       Inherits tiling from the current material's mapper.
    kTile                 1       Repeats map along image axes.
    kCrop                 2       Crops map < 0.0 or > 1.0 on image axes.
    kClamp                3       Clamps (stretches) map between 0.0 and 1.0 on image axes.
    kMirror               4       Mirror the material map at every integer boundary.
    </table>
  */
  void setUTiling(Tiling tiling);

  /** \details
    Sets the type of Y-axis tiling for this Mapper object.

    \param tiling [in]  Tiling type.

    \remarks
    tiling must be one of the following:

    <table>
    Name                  Value   Description
    kInheritTiling        0       Inherits tiling from the current material's mapper.
    kTile                 1       Repeats map along image axes.
    kCrop                 2       Crops map < 0.0 or > 1.0 on image axes.
    kClamp                3       Clamps (stretches) map between 0.0 and 1.0 on image axes.
    kMirror               4       Mirror the material map at every integer boundary.
    </table>
  */
  void setVTiling(Tiling tiling);

  /** \details
    Sets the type of automatic transform for this Mapper object.

    \param autoTransform [in]  Automatic transform type.

    \remarks
    autoTransform must be a one of the following:

    <table>
    Name                      Value   Description
    kInheritAutoTransform     0x0     Inherits automatic transform from the current material/s mapper.
    kNone                     0x1     No automatic transform.
    kObject                   0x2     Adjusts the mapper transform to align with and fit the current object.
    kModel                    0x4     Multiples the mapper transform by the current block transform.
    </table>
  */
  void setAutoTransform(AutoTransform autoTransform);

  /** \details
    Returns a reference to the transformation matrix for this Mapper object.

    \remarks
    The transform matrix defines mapping of an OdGiMaterialMap image when applied to the surface of an object.
  */
  OdGeMatrix3d& transform();

  /** \details
    Returns the type of projection for this Mapper object.

    \remarks
    projection() returns one of the following:

    <table>
    Name                  Value   Description
    kInheritProjection    0       Inherits projection from the current material's mapper.
    kPlanar               1       Maps directly to XY coordinates.
    kBox                  2       Maps to planes perpendicular to major axes.
    kCylinder             3       Maps to cylinder aligned with Z-axis.
    kSphere               4       Maps to sphere aligned with Z-axis
    </table>
  */
  Projection projection() const;

  /** \details
    Returns the type of X-axis tiling for this Mapper object.

    \remarks
    tiling() returns one of the following:

    <table>
    Name                  Value   Description
    kInheritTiling        0       Inherits tiling from the current material's mapper.
    kTile                 1       Repeats map along image axes.
    kCrop                 2       Crops map < 0.0 or > 1.0 on image axes.
    kClamp                3       Clamps (stretches) map between 0.0 and 1.0 on image axes.
    kMirror               4       Mirror the material map at every integer boundary.
    </table>
  */
  Tiling uTiling() const;

  /** \details
    Returns the type of Y-axis tiling for this Mapper object.

    \remarks
    tiling() returns one of the following:

    <table>
    Name                  Value   Description
    kInheritTiling        0       Inherits tiling from the current material's mapper.
    kTile                 1       Repeats map along image axes.
    kCrop                 2       Crops map < 0.0 or > 1.0 on image axes.
    kClamp                3       Clamps (stretches) map between 0.0 and 1.0 on image axes.
    kMirror               4       Mirror the material map at every integer boundary.
    </table>
  */
  Tiling vTiling() const;

  /** \details
    Returns the type of automatic transform for this Mapper object.

    \remarks
    autoTransform() returns a combination of one or more of the following:

    <table>
    Name                      Value   Description
    kInheritAutoTransform     0x0     Inherits automatic transform from the current material's mapper.
    kNone                     0x1     No automatic transform.
    kObject                   0x2     Adjusts the mapper transform to align with and fit the current object.
    kModel                    0x4     Multiples the mapper transform by the current block transform.
    </table>
  */
  AutoTransform autoTransform() const;

  const OdGeMatrix3d& transform() const;

  void setTransform(const OdGeMatrix3d &tm);

private:
  Projection    m_projection;
  Tiling        m_uTiling;
  Tiling        m_vTiling;
  AutoTransform m_autoTransform;
  OdGeMatrix3d  m_transform;
};


/** \details
  This class implements material maps.

  \remarks
  Material maps define bitmapped images which are applied to object surfaces.

  <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiMaterialMap
{
public:
  enum Source
  {
    kScene      = 0,  // Map is created from the current scene.
    kFile       = 1,  // Map is from a file.
    kProcedural = 2   // Map is procedural.
  };

  ODGI_EXPORT_STATIC static const OdGiMaterialMap kNull;

  OdGiMaterialMap();

  /** \details
    Sets the image source for this MaterialMap object.

    \param source [in]  Image source.

    \remarks
    source must be one of the following:

    <table>
    Name      Value   Description
    kScene    0,      Image is created from the current scene.
    kFile     1       Image is from a file.
    </table>
  */
  void setSource(Source source);

  /** \details
    Sets the source filename for this MaterialMap object.
    \param filename [in]  Source filename.
  */
  void setSourceFileName(const OdString& filename);

  /** \details
    Sets the blend factor for this MaterialMap object.
    \param blendFactor [in]  Blend factor.
    \remarks
    A blend factor of 0.0 makes the MaterialMap invisible. A blend factor of 1.0 makes it opaque.
    In between, the MaterialMap is transparent.
  */
  void setBlendFactor(double blendFactor);

  /** \details
    Sets the Material texture for this MaterialMap object.
  */
  void setTexture(OdGiMaterialTexturePtr pTexture);

  /** \details
    Returns a reference to, or a copy of, the OdGiMapper for this MaterialMap object.

    \remarks
    The transform matrix defines mapping of an OdGiMaterialMap image when applied to the surface of an object.
  */
  OdGiMapper& mapper();

  /** \details
    Returns the image source for this MaterialMap object.

    \remarks
    source must be one of the following:

    <table>
    Name      Value   Description
    kScene    0,      Image is created from the current scene.
    kFile     1       Image is from a file.
    </table>
  */
  Source source() const;

  /** \details
    Returns the source filename for this MaterialMap object.
  */
  OdString sourceFileName() const;

  /** \details
    Returns the blend factor for this MaterialMap object.

    \remarks
    A blend factor of 0.0 makes the MaterialMap invisible. A blend factor of 1.0 makes it opaque.
    In between, the MaterialMap is transparent.
  */
  double blendFactor() const;

  /** \details
    Returns a reference to the OdGiMapper for this MaterialMap object.
  */
  const OdGiMapper& mapper() const;

  /** \details
    Sets the OdGiMapper for this MaterialMap object.
    \param mapper [in]  OdGiMapper reference.
  */
  void setMapper(const OdGiMapper &mapper);

  /** \details
  Returns the material texture for this MaterialMap object.
  */
  const OdGiMaterialTexturePtr  texture(void) const;

  bool operator ==(const OdGiMaterialMap &other) const
  {
    if ((m_source      == other.m_source)      &&
        (m_fileName    == other.m_fileName)    &&
        (m_blendFactor == other.m_blendFactor) &&
        (m_mapper      == other.m_mapper))
    {
      if (m_texture.isNull() && other.m_texture.isNull())
        return true;
      if (!m_texture.isNull() && !other.m_texture.isNull())
        return (*m_texture == *(other.m_texture));
    }
    return false;
  }
  bool operator !=(const OdGiMaterialMap &other) const
  {
    return !(*this == other);
  }

  OdGiMaterialMap &operator =(const OdGiMaterialMap& mmap)
  {
    m_source = mmap.m_source;
    m_fileName = mmap.m_fileName;
    m_blendFactor = mmap.m_blendFactor;
    m_mapper = mmap.m_mapper;
    m_texture = mmap.m_texture;
    return *this;
  }
private:
  Source     m_source;
  OdString   m_fileName;
  double     m_blendFactor;
  OdGiMapper m_mapper;
  OdGiMaterialTexturePtr m_texture;
};

// OdGiMaterialColor inline implementations

inline
OdGiMaterialColor::OdGiMaterialColor()
  : m_method(kInherit)
  , m_factor(1.0)
{
}

inline void
OdGiMaterialColor::setMethod(Method method)
{
  m_method = method;
}

inline void
OdGiMaterialColor::setFactor(double factor)
{
  m_factor = factor;
}

inline OdCmEntityColor&
OdGiMaterialColor::color()
{
  return m_color;
}

inline OdGiMaterialColor::Method
OdGiMaterialColor::method() const
{
  return m_method;
}

inline double
OdGiMaterialColor::factor() const
{
  return m_factor;
}

inline const OdCmEntityColor&
OdGiMaterialColor::color() const
{
  return m_color;
}

inline void
OdGiMaterialColor::setColor(const OdCmEntityColor &color)
{
  m_color = color;
}

// OdGiMaterialMap inline implementations

inline
OdGiMaterialMap::OdGiMaterialMap()
  : m_source(kFile)
  , m_blendFactor(1.0)
{
}

inline void
OdGiMaterialMap::setSource(Source source)
{
  m_source = source;
}

inline void
OdGiMaterialMap::setSourceFileName(const OdString& filename)
{
  m_fileName = filename;
}

inline void
OdGiMaterialMap::setBlendFactor(double blendFactor)
{
  m_blendFactor = blendFactor;
}

inline OdGiMapper&
OdGiMaterialMap::mapper()
{
  return m_mapper;
}

inline OdGiMaterialMap::Source
OdGiMaterialMap::source() const
{
  return m_source;
}

inline OdString
OdGiMaterialMap::sourceFileName() const
{
  return m_fileName;
}

inline double
OdGiMaterialMap::blendFactor() const
{
  return m_blendFactor;
}

inline const OdGiMapper&
OdGiMaterialMap::mapper() const
{
  return m_mapper;
}

inline void
OdGiMaterialMap::setMapper(const OdGiMapper &mapper)
{
  m_mapper = mapper;
}

inline void
OdGiMaterialMap::setTexture(OdGiMaterialTexturePtr pTexture)
{
  m_texture = pTexture;
}

inline const OdGiMaterialTexturePtr
OdGiMaterialMap::texture(void) const
{
  return m_texture;
}

// OdGiMapper inline implementations

inline
OdGiMapper::OdGiMapper(const OdGiMapper& mapper)
  : m_projection(mapper.m_projection)
  , m_uTiling(mapper.m_uTiling)
  , m_vTiling(mapper.m_vTiling)
  , m_autoTransform(mapper.m_autoTransform)
  , m_transform(mapper.m_transform)
{
}

inline OdGiMapper&
OdGiMapper::operator=(const OdGiMapper& mapper)
{
  if (&mapper != this)
  {
    m_projection = mapper.m_projection;
    m_uTiling = mapper.m_uTiling;
    m_vTiling = mapper.m_vTiling;
    m_autoTransform = mapper.m_autoTransform;
    m_transform = mapper.m_transform;
  }
  return *this;
}

inline bool
OdGiMapper::operator==(const OdGiMapper& mapper) const
{
  return m_projection == mapper.m_projection
    && m_uTiling == mapper.m_uTiling
    && m_vTiling == mapper.m_vTiling
    && m_autoTransform == mapper.m_autoTransform
    && m_transform == mapper.m_transform;
}

inline bool
OdGiMapper::operator!=(const OdGiMapper& mapper) const
{
  return !(*this == mapper);
}

inline void
OdGiMapper::setProjection(Projection projection)
{
  m_projection = projection;
}

inline void
OdGiMapper::setUTiling(Tiling tiling)
{
  m_uTiling = tiling;
}

inline void
OdGiMapper::setVTiling(Tiling tiling)
{
  m_vTiling = tiling;
}

inline void
OdGiMapper::setAutoTransform(AutoTransform autoTransform)
{
  m_autoTransform = autoTransform;
}

inline OdGiMapper::Projection
OdGiMapper::projection() const
{
  return m_projection;
}

inline OdGiMapper::Tiling
OdGiMapper::uTiling() const
{
  return m_uTiling;
}

inline OdGiMapper::Tiling
OdGiMapper::vTiling() const
{
  return m_vTiling;
}

inline OdGiMapper::AutoTransform
OdGiMapper::autoTransform() const
{
  return m_autoTransform;
}

inline const OdGeMatrix3d&
OdGiMapper::transform() const
{
  return m_transform;
}

inline OdGeMatrix3d&
OdGiMapper::transform()
{
  return m_transform;
}

inline void
OdGiMapper::setTransform(const OdGeMatrix3d &tm)
{
  transform() = tm;
}

// OdGiImageFileTexture inline implementations

inline void
OdGiImageFileTexture::setSourceFileName(const OdString& fileName)
{
  m_sourceFileName = fileName;
}

inline const OdString
OdGiImageFileTexture::sourceFileName() const
{
  return m_sourceFileName;
}

inline bool
OdGiImageFileTexture::operator==(const OdGiMaterialTexture & texture) const
{
  if (!(texture.isA() == isA()))
    return false;
  const OdGiImageFileTexture &refTexture = static_cast<const OdGiImageFileTexture&>(texture);
  return m_sourceFileName == refTexture.m_sourceFileName;
}

inline OdGiImageFileTexture &
OdGiImageFileTexture::operator =(const OdGiImageFileTexture &texture)
{
  m_sourceFileName = texture.m_sourceFileName;
  return *this;
}

inline void OdGiImageFileTexture::copyFrom(const OdRxObject* pSource)
{
  OdGiImageFileTexturePtr pSrcTex = OdGiImageFileTexture::cast(pSource);
  if (!pSrcTex.isNull())
  {
    setSourceFileName(pSrcTex->sourceFileName());
  }
  else
  {
    throw OdError(eNotApplicable);
  }
}

// OdGiRasterImageTexture inline implementations

inline void
OdGiRasterImageTexture::setRasterImage(const OdGiRasterImage *pRasterImage)
{
  m_pRasterImage = reinterpret_cast<const OdRxObject*>(pRasterImage);
}

inline const OdGiRasterImage *
OdGiRasterImageTexture::rasterImage() const
{
  return reinterpret_cast<const OdGiRasterImage*>(m_pRasterImage.get());
}

inline bool
OdGiRasterImageTexture::operator==(const OdGiMaterialTexture & texture) const
{
  if (!(texture.isA() == isA()))
    return false;
  const OdGiRasterImageTexture &refTexture = static_cast<const OdGiRasterImageTexture&>(texture);
  return m_pRasterImage.get() == refTexture.m_pRasterImage.get();
}

inline OdGiRasterImageTexture &
OdGiRasterImageTexture::operator =(const OdGiRasterImageTexture &texture)
{
  m_pRasterImage = texture.m_pRasterImage;
  return *this;
}

inline void OdGiRasterImageTexture::copyFrom(const OdRxObject* pSource)
{
  OdGiRasterImageTexturePtr pSrcTex = OdGiRasterImageTexture::cast(pSource);
  if (!pSrcTex.isNull())
  {
    setRasterImage(pSrcTex->rasterImage());
  }
  else
  {
    throw OdError(eNotApplicable);
  }
}

// OdGiWoodTexture inline implementations

inline
OdGiWoodTexture::OdGiWoodTexture()
  : m_radialNoise(0.)
  , m_axialNoise(0.)
  , m_grainThickness(0.)
{
}

inline void
OdGiWoodTexture::setColor1(const OdGiMaterialColor &woodColor1)
{
  m_color1 = woodColor1;
}

inline const OdGiMaterialColor&
OdGiWoodTexture::color1(void) const
{
  return m_color1;
}

inline void
OdGiWoodTexture::setColor2(const OdGiMaterialColor &woodColor2)
{
  m_color2 = woodColor2;
}

inline const OdGiMaterialColor&
OdGiWoodTexture::color2(void) const
{
  return m_color2;
}

inline void
OdGiWoodTexture::setRadialNoise(double radialNoise)
{
  m_radialNoise = radialNoise;
}

inline double
OdGiWoodTexture::radialNoise(void) const
{
  return m_radialNoise;
}

inline void
OdGiWoodTexture::setAxialNoise(double axialNoise)
{
  m_axialNoise = axialNoise;
}

inline double
OdGiWoodTexture::axialNoise(void) const
{
  return m_axialNoise;
}

inline void
OdGiWoodTexture::setGrainThickness(double grainThickness)
{
  m_grainThickness = grainThickness;
}

inline double
OdGiWoodTexture::grainThickness(void) const
{
  return m_grainThickness;
}

inline bool
OdGiWoodTexture::operator==(const OdGiMaterialTexture & texture) const
{
  if (!(texture.isA() == isA()))
    return false;
  const OdGiWoodTexture &refTexture = static_cast<const OdGiWoodTexture&>(texture);
  return (m_color1 == refTexture.m_color1) &&
         (m_color2 == refTexture.m_color2) &&
         (m_radialNoise == refTexture.m_radialNoise) &&
         (m_axialNoise == refTexture.m_axialNoise) &&
         (m_grainThickness == refTexture.m_grainThickness);
}

inline OdGiWoodTexture &
OdGiWoodTexture::operator =(const OdGiWoodTexture &texture)
{
  m_color1 = texture.m_color1;
  m_color2 = texture.m_color2;
  m_radialNoise = texture.m_radialNoise;
  m_axialNoise = texture.m_axialNoise;
  m_grainThickness = texture.m_grainThickness;
  return *this;
}

inline void OdGiWoodTexture::copyFrom(const OdRxObject* pSource)
{
  OdGiWoodTexturePtr pSrcTex = OdGiWoodTexture::cast(pSource);
  if (!pSrcTex.isNull())
  {
    setColor1(pSrcTex->color1());
    setColor2(pSrcTex->color2());
    setRadialNoise(pSrcTex->radialNoise());
    setAxialNoise(pSrcTex->axialNoise());
    setGrainThickness(pSrcTex->grainThickness());
  }
  else
  {
    throw OdError(eNotApplicable);
  }
}

// OdGiMarbleTexture inline implementations

inline
OdGiMarbleTexture::OdGiMarbleTexture()
  : m_veinSpacing(0.)
  , m_veinWidth(0.)
{
}

inline void
OdGiMarbleTexture::setStoneColor(const OdGiMaterialColor &stoneColor)
{
  m_stoneColor = stoneColor;
}

inline const OdGiMaterialColor&
OdGiMarbleTexture::stoneColor(void) const
{
  return m_stoneColor;
}

inline void
OdGiMarbleTexture::setVeinColor(const OdGiMaterialColor &veinColor)
{
  m_veinColor = veinColor;
}

inline const OdGiMaterialColor&
OdGiMarbleTexture::veinColor(void) const
{
  return m_veinColor;
}

inline void
OdGiMarbleTexture::setVeinSpacing(double veinSpacing)
{
  m_veinSpacing = veinSpacing;
}

inline double
OdGiMarbleTexture::veinSpacing(void) const
{
  return m_veinSpacing;
}

inline void
OdGiMarbleTexture::setVeinWidth(double veinWidth)
{
  m_veinWidth = veinWidth;
}

inline double
OdGiMarbleTexture::veinWidth(void) const
{
  return m_veinWidth;
}

inline bool
OdGiMarbleTexture::operator==(const OdGiMaterialTexture & texture) const
{
  if (!(texture.isA() == isA()))
    return false;
  const OdGiMarbleTexture &refTexture = static_cast<const OdGiMarbleTexture&>(texture);
  return (m_stoneColor == refTexture.m_stoneColor) &&
         (m_veinColor == refTexture.m_veinColor) &&
         (m_veinSpacing == refTexture.m_veinSpacing) &&
         (m_veinWidth == refTexture.m_veinWidth);
}

inline OdGiMarbleTexture &
OdGiMarbleTexture::operator =(const OdGiMarbleTexture &texture)
{
  m_stoneColor = texture.m_stoneColor;
  m_veinColor = texture.m_veinColor;
  m_veinSpacing = texture.m_veinSpacing;
  m_veinWidth = texture.m_veinWidth;
  return *this;
}

inline void OdGiMarbleTexture::copyFrom(const OdRxObject* pSource)
{
  OdGiMarbleTexturePtr pSrcTex = OdGiMarbleTexture::cast(pSource);
  if (!pSrcTex.isNull())
  {
    setStoneColor(pSrcTex->stoneColor());
    setVeinColor(pSrcTex->veinColor());
    setVeinSpacing(pSrcTex->veinSpacing());
    setVeinWidth(pSrcTex->veinWidth());
  }
  else
  {
    throw OdError(eNotApplicable);
  }
}

// OdGiGenericTexture inline implementations

inline
OdGiGenericTexture::OdGiGenericTexture()
{
}

inline void
OdGiGenericTexture::setDefinition(const OdGiVariant &definition)
{
  if (m_definition.isNull())
  {
    m_definition = OdGiVariant::createObject(definition);
  }
  else
  {
    *m_definition = definition;
  }
}

inline OdGiVariantPtr
OdGiGenericTexture::definition() const
{
  return m_definition;
}

inline void
OdGiGenericTexture::definition(OdGiVariantPtr &pDefinition) const
{
  if (m_definition.isNull())
  {
    pDefinition = OdGiVariant::createObject();
  }
  else
  {
    pDefinition = OdGiVariant::createObject(*m_definition);
  }
}

inline bool
OdGiGenericTexture::operator==(const OdGiMaterialTexture & texture) const
{
  if (!(texture.isA() == isA()))
    return false;
  const OdGiGenericTexture &refTexture = static_cast<const OdGiGenericTexture&>(texture);
  if (m_definition.isNull() && refTexture.m_definition.isNull())
    return true;
  if (!m_definition.isNull() && !refTexture.m_definition.isNull())
    return (*m_definition == *(refTexture.m_definition));
  return false;
}

inline OdGiGenericTexture &
OdGiGenericTexture::operator =(const OdGiGenericTexture &texture)
{
  if (!texture.m_definition.isNull())
    m_definition = OdGiVariant::createObject(*(texture.m_definition));
  else
    m_definition.release();
  return *this;
}

inline void OdGiGenericTexture::copyFrom(const OdRxObject* pSource)
{
  OdGiGenericTexturePtr pSrcTex = OdGiGenericTexture::cast(pSource);
  if (!pSrcTex.isNull())
  {
    setDefinition(*(pSrcTex->definition()));
  }
  else
  {
    throw OdError(eNotApplicable);
  }
}

#include "TD_PackPop.h"

#endif //#ifndef __ODGIMATERIAL_H__

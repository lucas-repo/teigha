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

#ifndef __ODGIVISUALSTYLEDATA_H__
#define __ODGIVISUALSTYLEDATA_H__

#include "Gi/GiVisualStyle.h"
#include "StaticRxObject.h"
#include "CmColor.h"

#include "TD_PackPush.h"

// OdGiVisualStyleDataContainer

/** \details
  <group OdGi_Classes>

  This class defines the container for visual style data.

  Corresponding C++ library: TD_Gi
*/
class OdGiVisualStyleDataContainer : public OdStaticRxObject<OdGiVisualStyle>
{
  public:
    // OdCmEntityColor wrapper for OdCmColorBase interface
    struct OdCmColorBaseAdapt : public OdCmColorBase
    {
      OdGiVariant *m_pVar;

      OdCmColorBaseAdapt() : m_pVar(NULL) { }

      void setBase(OdGiVariant *pVar) { m_pVar = pVar; }
      OdGiVariant *base() { return m_pVar; }

      OdCmEntityColor &entColor() { return const_cast<OdCmEntityColor&>(m_pVar->asColor()); }
      const OdCmEntityColor &entColor() const { return m_pVar->asColor(); }

      OdCmEntityColor::ColorMethod colorMethod() const { return entColor().colorMethod(); }
      void setColorMethod(OdCmEntityColor::ColorMethod colorMethod) { entColor().setColorMethod(colorMethod); }
      bool isByColor() const { return entColor().isByColor(); }
      bool isByLayer() const { return entColor().isByLayer(); }
      bool isByBlock() const { return entColor().isByBlock(); }
      bool isByACI() const { return entColor().isByACI(); }
      bool isForeground() const { return entColor().isForeground(); }
      bool isByDgnIndex() const { return entColor().isByDgnIndex(); }
      OdUInt32 color() const { return entColor().color(); }
      void setColor(OdUInt32 color) { entColor().setColor(color); }
      void setRGB(OdUInt8 red, OdUInt8 green, OdUInt8 blue) { entColor().setRGB(red, green, blue); }
      void setRed(OdUInt8 red) { entColor().setRed(red); }
      void setGreen(OdUInt8 green) { entColor().setGreen(green); }
      void setBlue(OdUInt8 blue) { entColor().setBlue(blue); }
      OdUInt8 red() const { return entColor().red(); }
      OdUInt8 green() const { return entColor().green(); }
      OdUInt8 blue() const { return entColor().blue(); }
      OdUInt16 colorIndex() const { return entColor().colorIndex(); }
      void setColorIndex(OdUInt16 colorIndex) { entColor().setColorIndex(colorIndex); }
#if 0
      OdCmEntityColor m_entColor;

      OdCmColorBaseAdapt() : m_entColor() { }
      OdCmColorBaseAdapt(OdUInt8 red, OdUInt8 green, OdUInt8 blue) : m_entColor(red, green, blue) { }
      OdCmColorBaseAdapt(const OdCmEntityColor &cColor) : m_entColor(cColor) { }
      OdCmColorBaseAdapt(const OdCmColorBase &cColor) { m_entColor.setColor(cColor.color()); }
      OdCmColorBaseAdapt(OdCmEntityColor::ColorMethod cm) : m_entColor(cm) { }

      OdCmEntityColor::ColorMethod colorMethod() const { return m_entColor.colorMethod(); }
      void setColorMethod(OdCmEntityColor::ColorMethod colorMethod) { m_entColor.setColorMethod(colorMethod); }
      bool isByColor() const { return m_entColor.isByColor(); }
      bool isByLayer() const { return m_entColor.isByLayer(); }
      bool isByBlock() const { return m_entColor.isByBlock(); }
      bool isByACI() const { return m_entColor.isByACI(); }
      bool isForeground() const { return m_entColor.isForeground(); }
      bool isByDgnIndex() const { return m_entColor.isByDgnIndex(); }
      OdUInt32 color() const { return m_entColor.color(); }
      void setColor(OdUInt32 color) { m_entColor.setColor(color); }
      void setRGB(OdUInt8 red, OdUInt8 green, OdUInt8 blue) { m_entColor.setRGB(red, green, blue); }
      void setRed(OdUInt8 red) { m_entColor.setRed(red); }
      void setGreen(OdUInt8 green) { m_entColor.setGreen(green); }
      void setBlue(OdUInt8 blue) { m_entColor.setBlue(blue); }
      OdUInt8 red() const { return m_entColor.red(); }
      OdUInt8 green() const { return m_entColor.green(); }
      OdUInt8 blue() const { return m_entColor.blue(); }
      OdUInt16 colorIndex() const { return m_entColor.colorIndex(); }
      void setColorIndex(OdUInt16 colorIndex) { m_entColor.setColorIndex(colorIndex); }
#endif
      // Unnecessary methods
      bool setNames(const OdString& /*colorName*/, const OdString& /*bookName*/ = OdString::kEmpty) { return false; }
      OdString colorName() const { return OdString::kEmpty; }
      OdString bookName() const { return OdString::kEmpty; }
      OdString colorNameForDisplay() const { return OdString::kEmpty; }
    };
    // Style subclasses
    class OdGiFaceStyleDataContainer : public OdStaticRxObject<OdGiFaceStyle>
    {
      protected:
        OdGiVisualStyle           *m_pBase;
        mutable OdCmColorBaseAdapt m_cmMonoColor;
      public:
        OdGiFaceStyleDataContainer()
          : m_pBase(NULL)
        {
        }
        ~OdGiFaceStyleDataContainer()
        {
        }

        void setBase(OdGiVisualStyle *pBase)
        {
          m_pBase = pBase;
          m_cmMonoColor.setBase(m_pBase->trait(OdGiVisualStyleProperties::kFaceMonoColor).get());
        }
        OdGiVisualStyle *base() const
        {
          return m_pBase;
        }

        virtual void setLightingModel(LightingModel lightingModel)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kFaceLightingModel, (OdInt32)lightingModel);
        }
        virtual LightingModel lightingModel() const
        {
          return (LightingModel)m_pBase->trait(OdGiVisualStyleProperties::kFaceLightingModel)->asInt();
        }

        virtual void setLightingQuality(LightingQuality lightingQuality)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kFaceLightingQuality, (OdInt32)lightingQuality);
        }
        virtual LightingQuality lightingQuality() const
        {
          return (LightingQuality)m_pBase->trait(OdGiVisualStyleProperties::kFaceLightingQuality)->asInt();
        }

        virtual void setFaceColorMode(FaceColorMode mode)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kFaceColorMode, (OdInt32)mode);
        }
        virtual FaceColorMode faceColorMode() const
        {
          return (FaceColorMode)m_pBase->trait(OdGiVisualStyleProperties::kFaceColorMode)->asInt();
        }

        virtual void setFaceModifiers(unsigned long nModifiers)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kFaceModifiers, (OdInt32)nModifiers);
        }
        virtual void setFaceModifierFlag(FaceModifier flag, bool bEnable)
        {
          m_pBase->setTraitFlag(OdGiVisualStyleProperties::kFaceModifiers, flag, bEnable);
        }
        virtual unsigned long faceModifiers() const
        {
          return (unsigned long)m_pBase->trait(OdGiVisualStyleProperties::kFaceModifiers)->asInt();
        }
        virtual bool isFaceModifierFlagSet(FaceModifier flag) const
        {
          return m_pBase->traitFlag(OdGiVisualStyleProperties::kFaceModifiers, flag);
        }

        virtual void setOpacityLevel(double nLevel, bool bEnableModifier)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kFaceOpacity, nLevel);
          if (bEnableModifier)
            setFaceModifierFlag(kOpacity, bEnableModifier);
        }
        virtual double opacityLevel() const
        {
          return m_pBase->trait(OdGiVisualStyleProperties::kFaceOpacity)->asDouble();
        }

        virtual void setSpecularAmount(double nAmount, bool bEnableModifier)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kFaceSpecular, nAmount);
          if (bEnableModifier)
            setFaceModifierFlag(kSpecular, bEnableModifier);
        }
        virtual double specularAmount() const
        {
          return m_pBase->trait(OdGiVisualStyleProperties::kFaceSpecular)->asDouble();
        }

        virtual void setMonoColor(const OdCmColorBase& color, bool bEnableMode)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kFaceMonoColor, color);
          if (bEnableMode)
            setFaceColorMode(kMono);
        }
        virtual const OdCmColorBase& monoColor() const
        {
          return m_cmMonoColor;
        }
        virtual OdCmColorBase& monoColor()
        {
          return m_cmMonoColor;
        }
    };
    class OdGiEdgeStyleDataContainer : public OdStaticRxObject<OdGiEdgeStyle>
    {
      protected:
        OdGiVisualStyle           *m_pBase;
        mutable OdCmColorBaseAdapt m_cmIntColor;
        mutable OdCmColorBaseAdapt m_cmObColor;
        mutable OdCmColorBaseAdapt m_cmEdgeColor;
        mutable OdCmColorBaseAdapt m_cmSilColor;
        EdgeStyleApply             m_esApply;
      public:
        OdGiEdgeStyleDataContainer()
          : m_pBase(NULL)
          , m_esApply(kDefault)
        {
        }
        ~OdGiEdgeStyleDataContainer()
        {
        }

        void setBase(OdGiVisualStyle *pBase)
        {
          m_pBase = pBase;
          m_cmIntColor.setBase(m_pBase->trait(OdGiVisualStyleProperties::kEdgeIntersectionColor).get());
          m_cmObColor.setBase(m_pBase->trait(OdGiVisualStyleProperties::kEdgeObscuredColor).get());
          m_cmEdgeColor.setBase(m_pBase->trait(OdGiVisualStyleProperties::kEdgeColor).get());
          m_cmSilColor.setBase(m_pBase->trait(OdGiVisualStyleProperties::kEdgeSilhouetteColor).get());
        }
        OdGiVisualStyle *base() const
        {
          return m_pBase;
        }

        virtual void setEdgeModel(EdgeModel model)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeModel, (OdInt32)model);
        }
        virtual EdgeModel edgeModel() const
        {
          return (EdgeModel)m_pBase->trait(OdGiVisualStyleProperties::kEdgeModel)->asInt();
        }

        virtual void setEdgeStyles(unsigned long nStyles)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeStyles, (OdInt32)nStyles);
        }
        virtual void setEdgeStyleFlag(EdgeStyle flag, bool bEnable)
        {
          m_pBase->setTraitFlag(OdGiVisualStyleProperties::kEdgeStyles, flag, bEnable);
        }
        virtual unsigned long edgeStyles() const
        {
          return (unsigned long)m_pBase->trait(OdGiVisualStyleProperties::kEdgeStyles)->asInt();
        }
        virtual bool isEdgeStyleFlagSet(EdgeStyle flag) const
        {
          return m_pBase->traitFlag(OdGiVisualStyleProperties::kEdgeStyles, flag);
        }

        virtual void setIntersectionColor(const OdCmColorBase& color)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeIntersectionColor, color);
        }
        virtual const OdCmColorBase& intersectionColor() const
        {
          return m_cmIntColor;
        }
        virtual OdCmColorBase& intersectionColor()
        {
          return m_cmIntColor;
        }

        virtual void setObscuredColor(const OdCmColorBase& color)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeObscuredColor, color);
        }
        virtual const OdCmColorBase& obscuredColor() const
        {
          return m_cmObColor;
        }
        virtual OdCmColorBase& obscuredColor()
        {
          return m_cmObColor;
        }

        virtual void setObscuredLinetype(LineType ltype)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeObscuredLinePattern, (OdInt32)ltype);
        }
        virtual LineType obscuredLinetype() const
        {
          return (LineType)m_pBase->trait(OdGiVisualStyleProperties::kEdgeObscuredLinePattern)->asInt();
        }

        virtual void setIntersectionLinetype(LineType ltype)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeIntersectionLinePattern, (OdInt32)ltype);
        }
        virtual LineType intersectionLinetype() const
        {
          return (LineType)m_pBase->trait(OdGiVisualStyleProperties::kEdgeIntersectionLinePattern)->asInt();
        }

        virtual void setCreaseAngle(double nAngle)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeCreaseAngle, nAngle);
        }
        virtual double creaseAngle() const
        {
          return m_pBase->trait(OdGiVisualStyleProperties::kEdgeCreaseAngle)->asDouble();
        }

        virtual void setEdgeModifiers(unsigned long nModifiers)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeModifiers, (OdInt32)nModifiers);
        }
        virtual void setEdgeModifierFlag(EdgeModifier flag, bool bEnable)
        {
          m_pBase->setTraitFlag(OdGiVisualStyleProperties::kEdgeModifiers, flag, bEnable);
        }
        virtual unsigned long edgeModifiers() const
        {
          return (unsigned long)m_pBase->trait(OdGiVisualStyleProperties::kEdgeModifiers)->asInt();
        }
        virtual bool isEdgeModifierFlagSet(EdgeModifier flag) const
        {
          return m_pBase->traitFlag(OdGiVisualStyleProperties::kEdgeModifiers, flag);
        }

        virtual void setEdgeColor(const OdCmColorBase& color, bool bEnableModifier)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeColor, color);
        }
        virtual const OdCmColorBase& edgeColor() const
        {
          return m_cmEdgeColor;
        }
        virtual OdCmColorBase& edgeColor()
        {
          return m_cmEdgeColor;
        }

        virtual void setOpacityLevel(double nLevel, bool bEnableModifier)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeOpacity, nLevel);
          if (bEnableModifier)
            setEdgeModifierFlag(kOpacity, bEnableModifier);
        }
        virtual double opacityLevel() const
        {
          return m_pBase->trait(OdGiVisualStyleProperties::kEdgeOpacity)->asDouble();
        }

        virtual void setEdgeWidth(int nWidth, bool bEnableModifier)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeWidth, (OdInt32)nWidth);
          if (bEnableModifier)
            setEdgeModifierFlag(kWidth, bEnableModifier);
        }
        virtual int edgeWidth() const
        {
          return (int)m_pBase->trait(OdGiVisualStyleProperties::kEdgeWidth)->asInt();
        }

        virtual void setOverhangAmount(int nAmount, bool bEnableModifier)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeOverhang, (OdInt32)nAmount);
          if (bEnableModifier)
            setEdgeModifierFlag(kOverhang, bEnableModifier);
        }
        virtual int overhangAmount() const
        {
          return (int)m_pBase->trait(OdGiVisualStyleProperties::kEdgeOverhang)->asInt();
        }

        virtual void setJitterAmount(JitterAmount amount, bool bEnableModifier)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeJitterAmount, (OdInt32)amount);
          if (bEnableModifier)
            setEdgeModifierFlag(kJitter, bEnableModifier);
        }
        virtual JitterAmount jitterAmount() const
        {
          return (JitterAmount)m_pBase->trait(OdGiVisualStyleProperties::kEdgeJitterAmount)->asInt();
        }

        virtual void setWiggleAmount(WiggleAmount amount, bool bEnableModifier)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeWiggleAmount, (OdInt32)amount);
          if (bEnableModifier)
            setEdgeModifierFlag(kWiggle, bEnableModifier);
        }
        virtual WiggleAmount wiggleAmount() const
        {
          return (WiggleAmount)m_pBase->trait(OdGiVisualStyleProperties::kEdgeWiggleAmount)->asInt();
        }

        virtual void setSilhouetteColor(const OdCmColorBase& color)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeSilhouetteColor, color);
        }
        virtual const OdCmColorBase& silhouetteColor() const
        {
          return m_cmSilColor;
        }
        virtual OdCmColorBase& silhouetteColor()
        {
          return m_cmSilColor;
        }

        virtual void setSilhouetteWidth(short nWidth)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeSilhouetteWidth, (OdInt32)nWidth);
        }
        virtual short silhouetteWidth() const
        {
          return (short)m_pBase->trait(OdGiVisualStyleProperties::kEdgeSilhouetteWidth)->asInt();
        }

        virtual void setHaloGap(int nHaloGap, bool bEnableModifier)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeHaloGap, (OdInt32)nHaloGap);
          if (bEnableModifier)
            setEdgeModifierFlag(kHaloGap, bEnableModifier);
        }
        virtual int haloGap() const
        {
          return (int)m_pBase->trait(OdGiVisualStyleProperties::kEdgeHaloGap)->asInt();
        }

        virtual void setIsolines(unsigned short nIsolines)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeIsolines, (OdInt32)nIsolines);
        }
        virtual unsigned short isolines() const
        {
          return (unsigned short)m_pBase->trait(OdGiVisualStyleProperties::kEdgeIsolines)->asInt();
        }

        virtual void setHidePrecision(bool bHidePrecision)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kEdgeHidePrecision, bHidePrecision);
        }
        virtual bool hidePrecision() const
        {
          return (unsigned short)m_pBase->trait(OdGiVisualStyleProperties::kEdgeHidePrecision)->asBoolean();
        }

        virtual void setEdgeStyleApply(EdgeStyleApply apply)
        {
          m_esApply = apply;
        }
        virtual EdgeStyleApply edgeStyleApply() const
        {
          return m_esApply;
        }
    };
    class OdGiDisplayStyleDataContainer : public OdStaticRxObject<OdGiDisplayStyle>
    {
      protected:
        OdGiVisualStyle           *m_pBase;
      public:
        OdGiDisplayStyleDataContainer()
          : m_pBase(NULL)
        {
        }
        ~OdGiDisplayStyleDataContainer()
        {
        }

        void setBase(OdGiVisualStyle *pBase)
        {
          m_pBase = pBase;
        }
        OdGiVisualStyle *base() const
        {
          return m_pBase;
        }

        virtual void setDisplaySettings(unsigned long nSettings)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kDisplayStyles, (OdInt32)nSettings);
        }
        virtual void setDisplaySettingsFlag(DisplaySettings flag, bool bEnable)
        {
          m_pBase->setTraitFlag(OdGiVisualStyleProperties::kDisplayStyles, flag, bEnable);
        }
        virtual unsigned long displaySettings() const
        {
          return (unsigned long)m_pBase->trait(OdGiVisualStyleProperties::kDisplayStyles)->asInt();
        }
        virtual bool isDisplaySettingsFlagSet(DisplaySettings flag) const
        {
          return m_pBase->traitFlag(OdGiVisualStyleProperties::kDisplayStyles, flag);
        }

        virtual void setBrightness(double value)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kDisplayBrightness, value);
        }
        virtual double brightness() const
        {
          return m_pBase->trait(OdGiVisualStyleProperties::kDisplayBrightness)->asDouble();
        }

        virtual void setShadowType(ShadowType type)
        {
          m_pBase->setTrait(OdGiVisualStyleProperties::kDisplayShadowType, (OdInt32)type);
        }
        virtual ShadowType shadowType() const
        {
          return (ShadowType)m_pBase->trait(OdGiVisualStyleProperties::kDisplayBrightness)->asInt();
        }
    };
  protected:
    OdGiFaceStyleDataContainer    m_faceStyle;
    OdGiEdgeStyleDataContainer    m_edgeStyle;
    OdGiDisplayStyleDataContainer m_displayStyle;
    Type                          m_type;
    OdStaticRxObject<OdGiVariant> m_props[OdGiVisualStyleProperties::kPropertyCount];
    Operation                     m_ops[OdGiVisualStyleProperties::kPropertyCount];
  public:
    OdGiVisualStyleDataContainer()
      : m_faceStyle()
      , m_edgeStyle()
      , m_displayStyle()
      , m_type(kCustom)
    {
      // Setup redirections
      m_faceStyle.setBase(this);
      m_edgeStyle.setBase(this);
      m_displayStyle.setBase(this);
      // Face properties
      m_props[OdGiVisualStyleProperties::kFaceLightingModel].set((OdInt32)OdGiVisualStyleProperties::kPhong);
      m_props[OdGiVisualStyleProperties::kFaceLightingQuality].set((OdInt32)OdGiVisualStyleProperties::kPerVertexLighting);
      m_props[OdGiVisualStyleProperties::kFaceColorMode].set((OdInt32)OdGiVisualStyleProperties::kNoColorMode);
      m_props[OdGiVisualStyleProperties::kFaceModifiers].set((OdInt32)OdGiVisualStyleProperties::kNoFaceModifiers);
      m_props[OdGiVisualStyleProperties::kFaceOpacity].set(0.6);
      m_props[OdGiVisualStyleProperties::kFaceSpecular].set(30.0);
      m_props[OdGiVisualStyleProperties::kFaceMonoColor].set(OdCmEntityColor(255, 255, 255));
      // Edge properties
      m_props[OdGiVisualStyleProperties::kEdgeModel].set((OdInt32)OdGiVisualStyleProperties::kIsolines);
      m_props[OdGiVisualStyleProperties::kEdgeStyles].set((OdInt32)OdGiVisualStyleProperties::kObscuredFlag);
      m_props[OdGiVisualStyleProperties::kEdgeIntersectionColor].set(OdCmEntityColor(OdCmEntityColor::kForeground));
      m_props[OdGiVisualStyleProperties::kEdgeObscuredColor].set(OdCmEntityColor(OdCmEntityColor::kNone));
      m_props[OdGiVisualStyleProperties::kEdgeObscuredLinePattern].set((OdInt32)OdGiVisualStyleProperties::kSolid);
      m_props[OdGiVisualStyleProperties::kEdgeIntersectionLinePattern].set((OdInt32)OdGiVisualStyleProperties::kSolid);
      m_props[OdGiVisualStyleProperties::kEdgeCreaseAngle].set(1.0);
      m_props[OdGiVisualStyleProperties::kEdgeModifiers].set((OdInt32)OdGiVisualStyleProperties::kEdgeColorFlag);
      m_props[OdGiVisualStyleProperties::kEdgeColor].set(OdCmEntityColor(OdCmEntityColor::kForeground));
      m_props[OdGiVisualStyleProperties::kEdgeOpacity].set(1.0);
      m_props[OdGiVisualStyleProperties::kEdgeWidth].set((OdInt32)1);
      m_props[OdGiVisualStyleProperties::kEdgeOverhang].set((OdInt32)6);
      m_props[OdGiVisualStyleProperties::kEdgeJitterAmount].set((OdInt32)OdGiVisualStyleProperties::kJitterMedium);
      m_props[OdGiVisualStyleProperties::kEdgeSilhouetteColor].set(OdCmEntityColor(OdCmEntityColor::kForeground));
      m_props[OdGiVisualStyleProperties::kEdgeSilhouetteWidth].set((OdInt32)5);
      m_props[OdGiVisualStyleProperties::kEdgeHaloGap].set((OdInt32)0);
      m_props[OdGiVisualStyleProperties::kEdgeIsolines].set((OdInt32)0);
      m_props[OdGiVisualStyleProperties::kEdgeHidePrecision].set(false);
      // Display properties
      m_props[OdGiVisualStyleProperties::kDisplayStyles].set((OdInt32)OdGiVisualStyleProperties::kBackgroundsFlag);
      m_props[OdGiVisualStyleProperties::kDisplayBrightness].set(0.0);
      m_props[OdGiVisualStyleProperties::kDisplayShadowType].set((OdInt32)OdGiVisualStyleProperties::kShadowsNone);
      // New in AC2011, 2013
      m_props[OdGiVisualStyleProperties::kUseDrawOrder].set(false);
      m_props[OdGiVisualStyleProperties::kViewportTransparency].set(true);
      m_props[OdGiVisualStyleProperties::kLightingEnabled].set(true);
      m_props[OdGiVisualStyleProperties::kPosterizeEffect].set(false);
      m_props[OdGiVisualStyleProperties::kMonoEffect].set(false);
      // New in 2013
      m_props[OdGiVisualStyleProperties::kBlurEffect].set(false);
      m_props[OdGiVisualStyleProperties::kPencilEffect].set(false);
      m_props[OdGiVisualStyleProperties::kBloomEffect].set(false);
      m_props[OdGiVisualStyleProperties::kPastelEffect].set(false);
      m_props[OdGiVisualStyleProperties::kBlurAmount].set((OdInt32)50);
      m_props[OdGiVisualStyleProperties::kPencilAngle].set(0.0);
      m_props[OdGiVisualStyleProperties::kPencilScale].set(1.0);
      m_props[OdGiVisualStyleProperties::kPencilPattern].set((OdInt32)0);
      m_props[OdGiVisualStyleProperties::kPencilColor].set(OdCmEntityColor(0, 0, 0));
      m_props[OdGiVisualStyleProperties::kBloomThreshold].set((OdInt32)50);
      m_props[OdGiVisualStyleProperties::kBloomRadius].set((OdInt32)3);
      m_props[OdGiVisualStyleProperties::kTintColor].set(OdCmEntityColor(0, 0, 255));
      m_props[OdGiVisualStyleProperties::kFaceAdjustment].set(false);
      m_props[OdGiVisualStyleProperties::kPostContrast].set((OdInt32)50);
      m_props[OdGiVisualStyleProperties::kPostBrightness].set((OdInt32)50);
      m_props[OdGiVisualStyleProperties::kPostPower].set((OdInt32)50);
      m_props[OdGiVisualStyleProperties::kTintEffect].set(false);
      m_props[OdGiVisualStyleProperties::kBloomIntensity].set((OdInt32)50);
      m_props[OdGiVisualStyleProperties::kColor].set(OdCmEntityColor(OdCmEntityColor::kByLayer));
      m_props[OdGiVisualStyleProperties::kTransparency].set(1.0);
      m_props[OdGiVisualStyleProperties::kEdgeWiggleAmount].set((OdInt32)OdGiVisualStyleProperties::kWiggleMedium);
      m_props[OdGiVisualStyleProperties::kEdgeTexturePath].set(OdString(OD_T("strokes_ogs.tif")));
      m_props[OdGiVisualStyleProperties::kDepthOfField].set(false);
      m_props[OdGiVisualStyleProperties::kFocusDistance].set(1.0);
      m_props[OdGiVisualStyleProperties::kFocusWidth].set(1.0);
    }
    ~OdGiVisualStyleDataContainer()
    {
    }

    virtual OdGiFaceStyle& faceStyle()
    {
      return m_faceStyle;
    }
    virtual OdGiEdgeStyle& edgeStyle()
    {
      return m_edgeStyle;
    }
    virtual OdGiDisplayStyle& displayStyle()
    {
      return m_displayStyle;
    }

    virtual const OdGiFaceStyle& faceStyle() const
    {
      return m_faceStyle;
    }
    virtual const OdGiEdgeStyle& edgeStyle() const
    {
      return m_edgeStyle;
    }
    virtual const OdGiDisplayStyle& displayStyle() const
    {
      return m_displayStyle;
    }

    virtual void setFaceStyle(const OdGiFaceStyle& style)
    {
      m_faceStyle.set(style);
    }
    virtual void setEdgeStyle(const OdGiEdgeStyle& style)
    {
      m_edgeStyle.set(style);
    }
    virtual void setDisplayStyle(const OdGiDisplayStyle& style)
    {
      m_displayStyle.set(style);
    }

    // New interface

    virtual bool setType(Type type)
    {
      try
      {
        configureForType(type);
      }
      catch (const OdError &)
      {
        return false;
      }
      m_type = type;
      return true;
    }
    virtual Type type() const
    {
      return m_type;
    }

    virtual bool setTrait(Property prop, Operation op)
    {
      if ((prop > OdGiVisualStyleProperties::kInvalidProperty) && (prop < OdGiVisualStyleProperties::kPropertyCount))
      {
        m_ops[prop] = op;
        return true;
      }
      return false;
    }
    virtual bool setTrait(Property prop, const OdGiVariant *pVal,
                          Operation op = OdGiVisualStyleOperations::kSet)
    {
      if ((prop > OdGiVisualStyleProperties::kInvalidProperty) && (prop < OdGiVisualStyleProperties::kPropertyCount) &&
          pVal && (pVal->type() == propertyType(prop)))
      {
        static_cast<OdGiVariant&>(m_props[prop]) = *pVal;
        m_ops[prop] = op;
        return true;
      }
      return false;
    }

    virtual OdGiVariantPtr trait(Property prop, Operation *pOp = NULL) const
    {
      if ((prop > OdGiVisualStyleProperties::kInvalidProperty) && (prop < OdGiVisualStyleProperties::kPropertyCount))
      {
        if (pOp)
          *pOp = m_ops[prop];
        return OdGiVariantPtr(m_props + prop);
      }
      return OdGiVariant::createObject();
    }
    virtual Operation operation(Property prop) const
    {
      if ((prop > OdGiVisualStyleProperties::kInvalidProperty) && (prop < OdGiVisualStyleProperties::kPropertyCount))
      {
        return m_ops[prop];
      }
      return OdGiVisualStyleOperations::kInvalidOperation;
    }
};

#include "TD_PackPop.h"

#endif //__ODGIVISUALSTYLEDATA_H__

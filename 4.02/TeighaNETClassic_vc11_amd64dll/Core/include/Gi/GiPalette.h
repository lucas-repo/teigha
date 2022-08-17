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

#ifndef __ODGIPALETTE_H__
#define __ODGIPALETTE_H__

#include "TD_PackPush.h"

#include "GiExport.h"
#include "OdArray.h"
#include "SharedPtr.h"

/** \details
    This class represents RGB color components as set of 32bit integers.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
struct OdGiIntRGB
{
  OdInt32 r, g, b;

  OdGiIntRGB(OdInt32 _r, OdInt32 _g, OdInt32 _b)
    : r(_r), g(_g), b(_b)
  { }
  OdGiIntRGB() { }
  OdGiIntRGB(ODCOLORREF cref)
  {
    setColor(cref);
  }

  /** \details
      Returns red color component.
  */
  OdInt32 red() const { return r; }
  /** \details
      Returns green color component.
  */
  OdInt32 green() const { return g; }
  /** \details
      Returns blue color component.
  */
  OdInt32 blue() const { return b; }

  /** \details
      Resets red color component.
      \param _r [in]  New value for red color component.
  */
  void setRed(OdInt32 _r) { r = _r; }
  /** \details
      Resets green color component.
      \param _g [in]  New value for green color component.
  */
  void setGreen(OdInt32 _g) { g = _g; }
  /** \details
      Resets blue color component.
      \param _b [in]  New value for blue color component.
  */
  void setBlue(OdInt32 _b) { b = _b; }
  /** \details
      Resets red, green and blue color components together.
      \param _r [in]  New value for red color component.
      \param _g [in]  New value for green color component.
      \param _b [in]  New value for blue color component.
  */
  void setRGB(OdInt32 _r, OdInt32 _g, OdInt32 _b) { r = _r; g = _g; b = _b; }

  /** \details
      Resets all color components from ODCOLORREF.
      \param cref [in]  Input color.
  */
  void setColor(ODCOLORREF cref)
  {
    setRGB((OdInt32)ODGETRED(cref), (OdInt32)ODGETGREEN(cref), (OdInt32)ODGETBLUE(cref));
  }
  /** \details
      Returns color as ODCOLORREF.
  */
  ODCOLORREF color() const
  {
    return ODRGB(r, g, b);
  }
};

/** \details
    This class represents RGB color cube.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiColorCube
{
  protected:
    OdInt32    m_nBaseOffset;
    OdGiIntRGB m_nGridDivs;
    float      m_fIntensity;
    OdInt32    m_nGridSize;
    OdGiIntRGB m_nOffsets;
    OdGiIntRGB m_nDims;
  public:
    OdGiColorCube(OdGiIntRGB nGridDivs = OdGiIntRGB(2, 2, 2), float fIntensity = 1.0f, OdInt32 nBaseOffset = 0)
      : m_nBaseOffset(nBaseOffset)
      , m_nGridDivs(nGridDivs)
      , m_fIntensity(fIntensity)
    {
      validate();
    }

    /** \details
        Returns base offset for color entries.
    */
    OdInt32 baseOffset() const { return m_nBaseOffset; }
    /** \details
        Returns color cube grid divisions count.
    */
    const OdGiIntRGB &gridDivisions() const { return m_nGridDivs; }
    /** \details
        Returns color cube intensity.
    */
    float intensity() const { return m_fIntensity; }
    /** \details
        Returns total number of color cube grid knots.
    */
    OdInt32 gridSize() const { return m_nGridSize; }
    /** \details
        Returns offsets of color components.
    */
    const OdGiIntRGB &offsets() const { return m_nOffsets; }
    /** \details
        Returns dimensions of color cube components.
    */
    const OdGiIntRGB &dimensions() const { return m_nDims; }

    /** \details
        Returns color for specified color cube knot.
        \param nColor [in]  Index of color cube knot.
    */
    ODCOLORREF color(OdInt32 nColor) const;

    /** \details
        Compute closest color cube knot index.
        \param cref [in]  Input color.
    */
    OdInt32 closestMatch(ODCOLORREF cref) const;

    struct DtMatchResult
    {
      OdInt32 m_fitColors[4];
      OdInt32 m_pattern[4];
    };

    /** \details
        Compute set of closest color cube knot indexes.
        \param cref [in]  Input color.
        \param results [out]  Set of output colors and pattern offsets.
        \returns
        Returns number of fit colors.
    */
    OdInt32 ditheredMatch(ODCOLORREF cref, DtMatchResult &results) const;
  protected:
    void validate();
};

/** \details
    This class represents shades of gray ramp.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiGrayRamp
{
  protected:
    OdInt32 m_nBaseOffset;
    OdInt32 m_nGridDivs;
    float m_fIntensity;
    OdInt32 m_nDim;
  public:
    OdGiGrayRamp(OdInt32 nGridDivs = 2, float fIntensity = 1.0f, OdInt32 nBaseOffset = 0)
      : m_nBaseOffset(nBaseOffset)
      , m_nGridDivs(nGridDivs)
      , m_fIntensity(fIntensity)
    {
      validate();
    }

    /** \details
        Returns base offset for color entries.
    */
    OdInt32 baseOffset() const { return m_nBaseOffset; }
    /** \details
        Returns gray ramp divisions count.
    */
    OdInt32 gridDivisions() const { return m_nGridDivs; }
    /** \details
        Returns gray ramp intensity.
    */
    float intensity() const { return m_fIntensity; }
    /** \details
        Returns total number of gray ramp subdivisions.
    */
    OdInt32 dimension() const { return m_nDim; }

    /** \details
        Returns color for specified gray ramp subdivision.
        \param nColor [in]  Index of gray ramp subdivision.
    */
    ODCOLORREF color(OdInt32 nColor) const;

    /** \details
        Compute closest gray ramp subdivision index.
        \param cref [in]  Input color.
    */
    OdInt32 closestMatch(ODCOLORREF cref) const;
  protected:
    void validate();
};

/** \details
    This class represents 256 colors palette.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiPalette
{
  protected:
    struct PalEntry
    {
      ODCOLORREF m_color;
      OdInt32 m_entryHelper;
    };
    typedef OdArray<PalEntry, OdMemoryAllocator<PalEntry> > PalEntryArray;
  protected:
    PalEntryArray m_palette;
    OdSharedPtr<OdGiColorCube> m_pColorCube;
    OdSharedPtr<OdGiGrayRamp> m_pGrayRamp;
  public:
    OdGiPalette()
      : m_palette(256, 1)
    {
      initPalette();
    }

    /** \details
        Returns palette color.
        \param nColor [in]  Color index.
    */
    ODCOLORREF color(OdUInt32 nColor) const
    {
      return m_palette[nColor].m_color;
    }
    /** \details
        Returns palette entry helper.
        \param nColor [in]  Color index.
    */
    OdInt32 entryHelper(OdUInt32 nColor) const
    {
      return m_palette[nColor].m_entryHelper;
    }

    /** \details
        Sets palette color.
        \param nColor [in]  Color index.
        \param color [in]  Input color.
    */
    void setColor(OdUInt32 nColor, ODCOLORREF color)
    {
      m_palette[nColor].m_color = color;
    }
    /** \details
        Sets palette entry helper.
        \param nColor [in]  Color index.
        \param entryHelper [in]  New palette entry helper value.
    */
    void setEntryHelper(OdUInt32 nColor, OdInt32 entryHelper)
    {
      m_palette[nColor].m_entryHelper = entryHelper;
    }

    /** \details
        Returns color cube linked with this palette.
    */
    const OdGiColorCube *colorCube() const
    {
      return m_pColorCube.get();
    }
    /** \details
        Link color cube with this palette.
    */
    void setColorCube(const OdGiColorCube &colorCube)
    {
      resetColorCube(new OdGiColorCube(colorCube));
    }
    /** \details
        Reset linkage of color cube with this palette.
    */
    void resetColorCube()
    {
      resetColorCube(NULL);
    }

    /** \details
        Returns gray ramp linked with this palette.
    */
    const OdGiGrayRamp *grayRamp() const
    {
      return m_pGrayRamp.get();
    }
    /** \details
        Link gray ramp with this palette.
    */
    void setGrayRamp(const OdGiGrayRamp &grayRamp)
    {
      resetGrayRamp(new OdGiGrayRamp(grayRamp));
    }
    /** \details
        Reset linkage of gray ramp with this palette.
    */
    void resetGrayRamp()
    {
      resetGrayRamp(NULL);
    }

    /** \details
        Setup palette for specified color cube.
        \param cb [in]  Input color cube.
    */
    bool install(const OdGiColorCube &cb);
    /** \details
        Setup palette for specified gray ramp.
        \param gr [in]  Input gray ramp.
    */
    bool install(const OdGiGrayRamp &gr);
    /** \details
        Setup palette as a clone of another palette.
        \param pal2 [in]  Input palette.
    */
    bool install(const OdGiPalette &pal2);

    /** \details
        Compute closest palette color index.
        \param cref [in]  Input color.
        \param bThroughPal [in]  Skip usage of linked color cube and gray ramp in calculations.
    */
    OdInt32 closestMatch(ODCOLORREF cref, bool bThroughPal = false) const;

    /** \details
        Find start index of first available free block in palette.
        \param blockSize [in]  Size of free block for search.
    */
    OdInt32 firstAvailableBlock(OdInt32 blockSize) const;

    /** \details
        Check does this palette is equal with another one.
        \param pal2 [in]  Palette for check.
    */
    bool isEqualTo(const OdGiPalette &pal2) const;
    /** \details
        Remove equal sub-palette from current palette.
        \param pal2 [in]  Palette for remove.
    */
    bool remove(const OdGiPalette &pal2);

    /** \details
        Clear palette.
    */
    void clear();
  protected:
    void initPalette();

    void resetColorCube(OdGiColorCube *pColorCube);
    void resetGrayRamp(OdGiGrayRamp *pGrayRamp);

    void setColor(OdInt32 nColor, ODCOLORREF color);
};

#include "TD_PackPop.h"

#endif //#ifndef __ODGIPALETTE_H__

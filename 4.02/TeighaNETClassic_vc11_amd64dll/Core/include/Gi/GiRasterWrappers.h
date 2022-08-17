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

#ifndef __OD_GI_RASTER_WRAPPERS__
#define __OD_GI_RASTER_WRAPPERS__

#include "Gi/GiExport.h"
#include "Gi/GiRasterImage.h"
#include "Gi/GiImage.h"
#include "UInt8Array.h"

#include "TD_PackPush.h"

/** \details
    This class is a dummy implementation of the OdGiRasterImage interface.

    \remarks
    This class is intended to be used to preset image parameters for 
    OdDbRasterImageDef objects without the actual loading of raster image files.

    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiRasterImageDesc : public OdGiRasterImageParam
{
  OdUInt32                          m_pixelWidth, m_pixelHeight, m_colorDepth, m_palSize, m_alignment;
  OdGiRasterImage::PixelFormatInfo  m_pixFmt;
  Units                             m_units;
  double                            m_xPelsPerUnit, m_yPelsPerUnit;
  OdUInt8*                          m_pPal;
  OdGiRasterImage::ImageSource      m_imageSource;
  OdGiRasterImage::TransparencyMode m_transparencyMode;
public:
  OdGiRasterImageDesc();
  virtual ~OdGiRasterImageDesc();

  /** \details
    Creates an OdGiRasterImage object with the specified parameters. 
    \param pixelWidth [in]  Image width in pixels.
    \param pixelHeight [in]  Image height in pixels.
    \param colorDepth [in]  Number of bits per pixel used for colors.
    \param units [in]  Units / pixel.
    \param xPelsPerUnit [in]  Pixels per unit value ( x direction ).
    \param yPelsPerUnit [in]  Pixels per unit value ( y direction ).
    \param pSourceImage [in]  Source raster image to copy parameters from.
    \returns
    Returns a SmartPointer to the newly created object.
  */
  static OdGiRasterImagePtr createObject(OdUInt32 pixelWidth, OdUInt32 pixelHeight, Units units = kNone, double xPelsPerUnit = 0.0, double yPelsPerUnit = 0.0);
  static OdGiRasterImagePtr createObject(OdUInt32 pixelWidth, OdUInt32 pixelHeight, OdUInt32 colorDepth, Units units = kNone, double xPelsPerUnit = 0.0, double yPelsPerUnit = 0.0);
  static OdGiRasterImagePtr createObject(const OdGiRasterImage *pSourceImage);

  /** \details
    Copy parameters from original OdGiRasterImage object.
    \param pSourceImage [in]  Pointer to the original image object.
  */
  void setFrom(const OdGiRasterImage *pSourceImage);

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);

  OdUInt32 pixelWidth() const;
  OdUInt32 pixelHeight() const;
  OdUInt32 colorDepth() const;
  OdUInt32 numColors() const;
  ODCOLORREF color(OdUInt32 colorIndex) const;
  OdUInt32 paletteDataSize() const;
  void paletteData(OdUInt8* bytes) const;
  OdUInt32 scanLineSize() const;
  const OdUInt8* scanLines() const;
  void scanLines(OdUInt8* scnLines, OdUInt32 firstScanline, OdUInt32 numLines = 1) const;
  PixelFormatInfo pixelFormat() const;
  OdUInt32 scanLinesAlignment() const;
  Units defaultResolution(double& xPelsPerUnit, double& yPelsPerUnit) const;

  /** \details
    Sets the image width in pixels of this RasterImageDesc object.
    \param pixelWidth [in]  Pixel width.
  */
  void setPixelWidth(OdUInt32 pixelWidth);
  /** \details
    Sets the image height in pixels of this RasterImageDesc object.
    \param pixelHeight [in]  Pixel height.
  */
  void setPixelHeight(OdUInt32 pixelHeight);

  /** \details
    Sets the number of bits per pixel used for colors by this RasterImageDesc object.
    \param colorDepth [in]  Color depth.
  */
  void setColorDepth(OdUInt32 colorDepth);

  OdGiRasterImage::PixelFormatInfo& pixelFormat();

  /** \details
    Sets the palette in BMP format for this RasterImageDesc object.
    \param paletteData [in]  Palette data.
    \param paletteSize [in]  Palette size in bytes.
  */
  void setPalette(OdUInt32 paletteSize, OdUInt8* paletteData);

  /** \details
    Sets the scan lines alignment, in bytes, for this RasterImage object.
    \param alignment [in]  Scan line alignment.
    Example:
    Alignment is 4 for Windows BMP.
  */
  void setScanLinesAlignment(OdUInt32 alignment);

  /** \details
    Sets the default raster image resolution for this RasterImage object.
    \param units [in]  Units / pixel.
    \param xPelsPerUnit [in]  Pixels per unit value ( x direction ).
    \param yPelsPerUnit [in]  Pixels per unit value ( y direction ).
  */
  void setDefaultResolution(Units units, double xPelsPerUnit, double yPelsPerUnit);

  OdUInt32 supportedParams() const;
  OdGiRasterImage::ImageSource imageSource() const;
  void setImageSource(OdGiRasterImage::ImageSource source);
  OdGiRasterImage::TransparencyMode transparencyMode() const;
  void setTransparencyMode(OdGiRasterImage::TransparencyMode mode);
};

class OdGiImageBGRA32;

/** \details
    Emulates OdGiImageBGRA32 within OdGiRasterImage interface.
    \remarks
    Stores only pointer to original image, not a SmartPointer, so deletion of original image before
    this class may cause access violation error.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiRasterImageBGRA32 : public OdGiRasterImageParam
{
protected:
  OdGiImageBGRA32 *m_pBGRAImage;
  OdGiRasterImage::TransparencyMode m_transparencyMode;
public:
  OdGiRasterImageBGRA32();
  virtual ~OdGiRasterImageBGRA32();
  
  /** \details
    Creates an OdGiRasterImage object with the specified parameters.
    \param pImage [in]  Input BGRA32 image.
    \param transparencyMode [in]  Transparency mode.
    \returns
    Returns a SmartPointer to the newly created object.
  */
  static OdGiRasterImagePtr createObject(OdGiImageBGRA32 *pImage, OdGiRasterImage::TransparencyMode transparencyMode = OdGiRasterImage::kTransparency8Bit);

  OdUInt32 pixelWidth() const;
  OdUInt32 pixelHeight() const;
  OdUInt32 colorDepth() const;
  OdUInt32 numColors() const;
  ODCOLORREF color(OdUInt32 colorIndex) const;
  OdUInt32 paletteDataSize() const;
  void paletteData(OdUInt8* bytes) const;
  OdUInt32 scanLineSize() const;
  const OdUInt8* scanLines() const;
  void scanLines(OdUInt8* scnLines, OdUInt32 firstScanline, OdUInt32 numLines = 1) const;
  OdGiRasterImage::PixelFormatInfo pixelFormat() const;
  OdUInt32 scanLinesAlignment() const;
  OdGiRasterImage::Units defaultResolution(double& xPelsPerUnit, double& yPelsPerUnit) const;
  OdUInt32 supportedParams() const;
  OdGiRasterImage::ImageSource imageSource() const;
  OdGiRasterImage::TransparencyMode transparencyMode() const;
  void setTransparencyMode(OdGiRasterImage::TransparencyMode mode);

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);
};

/** \details
    Extends OdGiRasterImageBGRA32 class to keep a copy of OdGiImageBGRA32 inside.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiRasterImageBGRA32Holder : public OdGiRasterImageBGRA32
{
protected:
  OdGiImageBGRA32 m_ImageCopy;
public:
  OdGiRasterImageBGRA32Holder();
  ~OdGiRasterImageBGRA32Holder();
  
  /** \details
    Creates an OdGiRasterImage object with the specified parameters.
    \param pImage [in]  Input BGRA32 image.
    \param transparencyMode [in]  Transparency mode.
    \returns
    Returns a SmartPointer to the newly created object.
  */
  static OdGiRasterImagePtr createObject(OdGiImageBGRA32 *pImage, OdGiRasterImage::TransparencyMode transparencyMode = OdGiRasterImage::kTransparency8Bit);

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);
};

/** \details
    Extends OdGiRasterImageBGRA32 class to keep a copy of OdGiImageBGRA32 and pixels inside.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiRasterImageBGRA32Copy : public OdGiRasterImageBGRA32Holder
{
private:
  OdGiPixelBGRA32Array m_pxArray;
public:
  OdGiRasterImageBGRA32Copy();
  ~OdGiRasterImageBGRA32Copy();
  
  /** \details
    Creates an OdGiRasterImage object with the specified parameters.
    \param pImage [in]  Input BGRA32 image.
    \param transparencyMode [in]  Transparency mode.
    \returns
    Returns a SmartPointer to the newly created object.
  */
  static OdGiRasterImagePtr createObject(OdGiImageBGRA32 *pImage, OdGiRasterImage::TransparencyMode transparencyMode = OdGiRasterImage::kTransparency8Bit);

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);
};

/** \details
    This class stores full copy of original raster image data.
    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiRasterImageHolder : public OdGiRasterImageParam
{
  OdUInt32                          m_pixelWidth, m_pixelHeight, m_colorDepth, m_alignment;
  OdGiRasterImage::PixelFormatInfo  m_pixFmt;
  Units                             m_units;
  double                            m_xPelsPerUnit, m_yPelsPerUnit;
  OdGiRasterImage::ImageSource      m_imageSource;
  OdGiRasterImage::TransparencyMode m_transparencyMode;
  int                               m_transparentColor;
  OdUInt8Array                      m_palData;
  OdUInt8Array                      m_pixData;
public:
  OdGiRasterImageHolder();
  virtual ~OdGiRasterImageHolder();

  /** \details
    Store data from original OdGiRasterImage object.
    \param pOrig [in]  Pointer to the original image object.
  */
  void setFrom(const OdGiRasterImage* pOrig);

  /** \details
    Creates an OdGiRasterImage object from original OdGiRasterImage object.
    \param pImage [in]  Pointer to the original image object.
    \returns
    Returns a SmartPointer to the newly created object.
  */
  static OdGiRasterImagePtr createObject(const OdGiRasterImage* pImage);

  OdUInt32 pixelWidth() const;
  OdUInt32 pixelHeight() const;
  Units defaultResolution(double& xPelsPerUnit, double& yPelsPerUnit) const;
  OdUInt32 colorDepth() const;
  OdUInt32 numColors() const;
  ODCOLORREF color(OdUInt32 colorIndex) const;
  OdUInt32 paletteDataSize() const;
  void paletteData(OdUInt8* bytes) const;
  OdUInt32 scanLineSize() const;
  void scanLines(OdUInt8* scnLines, OdUInt32 firstScanline, OdUInt32 numLines = 1) const;
  const OdUInt8* scanLines() const;
  PixelFormatInfo pixelFormat() const;
  OdUInt32 scanLinesAlignment() const;
  int transparentColor() const;
  ImageSource imageSource() const;
  TransparencyMode transparencyMode() const;
  OdUInt32 supportedParams() const;
  void setImageSource(ImageSource source);
  void setTransparencyMode(TransparencyMode mode);

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);
};

/** \details
    This class is the base class for raster image transformer objects.

    Corresponding C++ library: TD_Gi

    \remarks
    Transforming an image does not effect the orignal.

    The default methods for this class do nothing but return the
    corresponding values from the original object.

    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiRasterImageWrapper : public OdGiRasterImageParam
{
  OdGiRasterImagePtr m_pOrig;
public:
  OdGiRasterImageWrapper();
  virtual ~OdGiRasterImageWrapper();

  /** \details
    Sets the original OdGiRasterImage object associated with this RasterImageWrapper object.
    \param pOrig [in]  Pointer to the original image object.
  */
  void setOriginal(const OdGiRasterImage* pOrig);
  const OdGiRasterImage *original() const;
  OdGiRasterImagePtr cloneOriginal() const;

  OdUInt32 pixelWidth() const;
  OdUInt32 pixelHeight() const;
  Units defaultResolution(double& xPelsPerUnit, double& yPelsPerUnit) const;
  OdUInt32 colorDepth() const;
  OdUInt32 numColors() const;
  ODCOLORREF color(OdUInt32 colorIndex) const;
  OdUInt32 paletteDataSize() const;
  void paletteData(OdUInt8* bytes) const;
  OdUInt32 scanLineSize() const;
  void scanLines(OdUInt8* scnLines, OdUInt32 firstScanline, OdUInt32 numLines = 1) const;
  const OdUInt8* scanLines() const;
  PixelFormatInfo pixelFormat() const;
  OdUInt32 scanLinesAlignment() const;
  int transparentColor() const;
  ImageSource imageSource() const;
  TransparencyMode transparencyMode() const;
  OdUInt32 supportedParams() const;
  void setImageSource(ImageSource source);
  void setTransparencyMode(TransparencyMode mode);
  void* imp() const;

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);
};

/** \details
    This raster image transformer class transforms OdGiRasterImage objects to Bitonal images.

    Corresponding C++ library: TD_Gi

    \remarks
    Transforming an image does not effect the orignal.

    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiBitonalRasterTransformer : public OdGiRasterImageWrapper
{
  RGBQUAD m_palette[2];
  int m_transpColor;
public:
  OdGiBitonalRasterTransformer();
  virtual ~OdGiBitonalRasterTransformer();

  /** \details
      Creates bitonal raster transformer.
      \param pOrig [in]  Original raster image pointer.
      \param foregroundColor [in]  Foreground color.
      \param backgroundColor [in]  Background color.
      \param transparent [in]  Image is transparent if and only if true.
  */
  static OdGiRasterImagePtr createObject(const OdGiRasterImage* pOrig, ODCOLORREF foregroundColor, ODCOLORREF backgroundColor, bool transparent);

  /** \details
      Sets bitonal raster transformer parameters.
      \param pOrig [in]  Original raster image pointer.
      \param foregroundColor [in]  Foreground color.
      \param backgroundColor [in]  Background color.
      \param transparent [in]  Image is transparent if and only if true.
  */
  void setOriginal(const OdGiRasterImage* pOrig, ODCOLORREF foregroundColor, ODCOLORREF backgroundColor, bool transparent);

  OdUInt32 numColors() const;
  ODCOLORREF color(OdUInt32 colorIndex) const;
  OdUInt32 paletteDataSize() const;
  void paletteData(OdUInt8* bytes) const;
  PixelFormatInfo pixelFormat() const;
  int transparentColor() const;

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);
};

/** \details
    This raster image transformer class mirrors image upside down.

    Corresponding C++ library: TD_Gi

    \remarks
    Transforming an image does not effect the orignal.

    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiUpsideDownRasterTransformer : public OdGiRasterImageWrapper
{
public:
  OdGiUpsideDownRasterTransformer();
  virtual ~OdGiUpsideDownRasterTransformer();

  /** \details
      Create new image upside down transformer.
      \param pOrig [in]  Original raster image pointer.
  */
  static OdGiRasterImagePtr createObject(const OdGiRasterImage* pOrig);

  void scanLines(OdUInt8* scnLines, OdUInt32 firstScanline, OdUInt32 numLines = 1) const;
  const OdUInt8* scanLines() const;

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);
};

/** \details
    This raster image transformer class mirrors image right to left.

    Corresponding C++ library: TD_Gi

    \remarks
    Transforming an image does not effect the orignal.

    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiLeftToRightRasterTransformer : public OdGiRasterImageWrapper
{
public:
  OdGiLeftToRightRasterTransformer();
  virtual ~OdGiLeftToRightRasterTransformer();

  /** \details
      Create new image left to right transformer.
      \param pOrig [in]  Original raster image pointer.
  */
  static OdGiRasterImagePtr createObject(const OdGiRasterImage* pOrig);

  void scanLines(OdUInt8* scnLines, OdUInt32 firstScanline, OdUInt32 numLines = 1) const;
  const OdUInt8* scanLines() const;

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);
};

/** \details
    This raster image transformer class negates image colors.

    Corresponding C++ library: TD_Gi

    \remarks
    Transforming an image does not effect the orignal.

    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiInversionRasterTransformer : public OdGiRasterImageWrapper
{
public:
  OdGiInversionRasterTransformer();
  virtual ~OdGiInversionRasterTransformer();

  /** \details
      Create new image inversion transformer.
      \param pOrig [in]  Original raster image pointer.
  */
  static OdGiRasterImagePtr createObject(const OdGiRasterImage* pOrig);

  ODCOLORREF color(OdUInt32 colorIndex) const;
  void paletteData(OdUInt8* bytes) const;

  void scanLines(OdUInt8* scnLines, OdUInt32 firstScanline, OdUInt32 numLines = 1) const;
  const OdUInt8* scanLines() const;

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);
protected:
  virtual ODCOLORREF colorXform(ODCOLORREF color) const;
};

/** \details
    This raster image transformer class converts image colors into grayscale.

    Corresponding C++ library: TD_Gi

    \remarks
    Transforming an image does not effect the orignal.

    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiGrayscaleRasterTransformer : public OdGiInversionRasterTransformer
{
public:
  OdGiGrayscaleRasterTransformer();
  virtual ~OdGiGrayscaleRasterTransformer();

  /** \details
      Create new image grayscale transformer.
      \param pOrig [in]  Original raster image pointer.
  */
  static OdGiRasterImagePtr createObject(const OdGiRasterImage* pOrig);

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);
protected:
  virtual ODCOLORREF colorXform(ODCOLORREF color) const;
};

/** \details
    This raster image transformer class converts image colors into monochrome.

    Corresponding C++ library: TD_Gi

    \remarks
    Transforming an image does not effect the orignal.

    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiMonochromaticRasterTransformer : public OdGiGrayscaleRasterTransformer
{
  int m_threshold;
public:
  OdGiMonochromaticRasterTransformer();
  virtual ~OdGiMonochromaticRasterTransformer();

  /** \details
      Create new image monochromatic transformer.
      \param pOrig [in]  Original raster image pointer.
      \param threshold [in]  Threshold between light and dark image components.
  */
  static OdGiRasterImagePtr createObject(const OdGiRasterImage* pOrig, int threshold = 127);

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);

  void setThreshold(OdUInt8 treshold) { m_threshold = treshold; if (m_threshold > 254) m_threshold = 254; };
  int threshold() const { return m_threshold; }
protected:
  virtual ODCOLORREF colorXform(ODCOLORREF color) const;
};

#if 0 // @@@TODO: implement at least nearest, bilinear and bicubic resamplers
/** \details
    This raster image resampler class with simplest nearest interpolation filter.

    Corresponding C++ library: TD_Gi

    \remarks
    Resampling an image does not effect the orignal.

    <group OdGi_Classes>
*/
class ODGI_EXPORT OdGiNearestRasterResampler : public OdGiRasterImageWrapper
{
  OdUInt32 m_newPixWidth, m_newPixHeight;
public:
  OdGiNearestRasterResampler();
  virtual ~OdGiNearestRasterResampler();

  /** \details
      Create new image nearest interpolation resampler.
      \param pOrig [in]  Original raster image pointer.
      \param newPixWidth [in]  New image width.
      \param newPixHeight [in]  New image width.
  */
  static OdGiRasterImagePtr createObject(const OdGiRasterImage* pOrig, OdUInt32 newPixWidth, OdUInt32 newPixHeight);

  void scanLines(OdUInt8* scnLines, OdUInt32 firstScanline, OdUInt32 numLines = 1) const;
  const OdUInt8* scanLines() const;

  OdRxObjectPtr clone() const;
  void copyFrom(const OdRxObject *pSource);
};
#endif

#include "TD_PackPop.h"

#endif //  __OD_GI_RASTER_WRAPPERS__

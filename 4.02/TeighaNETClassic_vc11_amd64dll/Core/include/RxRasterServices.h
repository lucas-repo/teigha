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


// RasterModule.h - interface of module, performing different operations on raster images

#ifndef __OD_RASTER_MODULE__
#define __OD_RASTER_MODULE__

#include "RxModule.h"
#include "DbRootExport.h"
#include "UInt32Array.h"
#include "TDVersion.h"

class OdGiRasterImage;
typedef OdSmartPtr<OdGiRasterImage> OdGiRasterImagePtr;

#include "TD_PackPush.h"

#ifndef OD_FOURCC
#ifndef ODA_BIGENDIAN
#if defined(OD_SWIGCSHARP)
#define OD_FOURCC(a, b, c, d) (((uint)(d) << 24) | ((uint)(c) << 16) | ((uint)(b) << 8) | ((uint)(a)))
#elif defined(OD_SWIGJAVA)
#define OD_FOURCC(a, b, c, d) (((long)(d) << 24) | ((long)(c) << 16) | ((long)(b) << 8) | ((long)(a)))
#else
#define OD_FOURCC(a, b, c, d) (((OdUInt32)(d) << 24) | ((OdUInt32)(c) << 16) | ((OdUInt32)(b) << 8) | ((OdUInt32)(a)))
#endif
#else
#if defined(OD_SWIGCSHARP)
#define OD_FOURCC(a, b, c, d) (((uint)(a) << 24) | ((uint)(b) << 16) | ((uint)(c) << 8) | ((uint)(d)))
#elif defined(OD_SWIGJAVA)
#define OD_FOURCC(a, b, c, d) (((long)(a) << 24) | ((long)(b) << 16) | ((long)(c) << 8) | ((long)(d)))
#else
#define OD_FOURCC(a, b, c, d) (((OdUInt32)(a) << 24) | ((OdUInt32)(b) << 16) | ((OdUInt32)(c) << 8) | ((OdUInt32)(d)))
#endif
#endif
#endif // OD_FOURCC

/** \details
    This class is the base class for classes that provide platform-dependent loading and saving
    of Raster Image files for Teigha.
    Corresponding C++ library: TD_Db
    <group OdRx_Classes> 
*/
class ODRX_ABSTRACT FIRSTDLL_EXPORT OdRxRasterServices : public OdRxModule
{
public:
  // Predefined image types
  enum ImageType
  {
#if defined(OD_SWIGCSHARP) || defined(OD_SWIGJAVA)
    kUnknown  = 0xFFFFFFFF,
    kBMP      = OD_FOURCC(0x0042, 0x004D, 0x0050, 0x0020),
    kICO      = OD_FOURCC(0x0049, 0x0043, 0x004F, 0x0020),
    kJPEG     = OD_FOURCC(0x004A, 0x0050, 0x0045, 0x0047),
    kJNG      = OD_FOURCC(0x004A, 0x004E, 0x0047, 0x0020),
    kKOALA    = OD_FOURCC(0x004B, 0x004F, 0x0041, 0x004C),
    kLBM      = OD_FOURCC(0x004C, 0x0042, 0x004D, 0x0020),
    kIFF      = kLBM,
    kMNG      = OD_FOURCC(0x004D, 0x004E, 0x0047, 0x0020),
    kPBM      = OD_FOURCC(0x0050, 0x0042, 0x004D, 0x0020),
    kPBMRAW   = OD_FOURCC(0x0050, 0x0042, 0x004D, 0x0052),
    kPCD      = OD_FOURCC(0x0050, 0x0043, 0x0044, 0x0020),
    kPCX      = OD_FOURCC(0x0050, 0x0043, 0x0058, 0x0020),
    kPGM      = OD_FOURCC(0x0050, 0x0047, 0x004D, 0x0020),
    kPGMRAW   = OD_FOURCC(0x0050, 0x0047, 0x004D, 0x0052),
    kPNG      = OD_FOURCC(0x0050, 0x004E, 0x0047, 0x0020),
    kPPM      = OD_FOURCC(0x0050, 0x0050, 0x004D, 0x0020),
    kPPMRAW   = OD_FOURCC(0x0050, 0x0050, 0x004D, 0x0052),
    kRAS      = OD_FOURCC(0x0052, 0x0041, 0x0053, 0x0020),
    kTARGA    = OD_FOURCC(0x0054, 0x0047, 0x0041, 0x0020),
    kTIFF     = OD_FOURCC(0x0054, 0x0049, 0x0046, 0x0046),
    kWBMP     = OD_FOURCC(0x0057, 0x0042, 0x004DF, 0x0050),
    kPSD      = OD_FOURCC(0x0050, 0x0053, 0x0044, 0x0020),
    kCUT      = OD_FOURCC(0x0043, 0x0055, 0x0054, 0x0020),
    kXBM      = OD_FOURCC(0x0058, 0x0042, 0x004D, 0x0020),
    kXPM      = OD_FOURCC(0x0058, 0x0050, 0x004D, 0x0020),
    kDDS      = OD_FOURCC(0x0044, 0x0044, 0x0053, 0x0020),
    kGIF      = OD_FOURCC(0x0047, 0x0049, 0x0046, 0x0020),
    kHDR      = OD_FOURCC(0x0048, 0x0044, 0x0052, 0x0020),
    kFAXG3    = OD_FOURCC(0x0046, 0x0058, 0x0047, 0x0033),
    kSGI      = OD_FOURCC(0x0053, 0x0047, 0x0049, 0x0020),
    kEXR      = OD_FOURCC(0x0045, 0x0058, 0x0052, 0x0020),
    kJ2K      = OD_FOURCC(0x004A, 0x0032, 0x004B, 0x0020),
    kJP2      = OD_FOURCC(0x004A, 0x0050, 0x0032, 0x0020),
    kPFM      = OD_FOURCC(0x0050, 0x0046, 0x004D, 0x0020),
    kPICT     = OD_FOURCC(0x0050, 0x0049, 0x0043, 0x0054),
    kRAW      = OD_FOURCC(0x0052, 0x0041, 0x0057, 0x0020)
#else
    kUnknown  = -1,
    kBMP      = OD_FOURCC('B', 'M', 'P', ' '),
    kICO      = OD_FOURCC('I', 'C', 'O', ' '),
    kJPEG     = OD_FOURCC('J', 'P', 'E', 'G'),
    kJNG      = OD_FOURCC('J', 'N', 'G', ' '),
    kKOALA    = OD_FOURCC('K', 'O', 'A', 'L'),
    kLBM      = OD_FOURCC('L', 'B', 'M', ' '),
    kIFF      = kLBM,
    kMNG      = OD_FOURCC('M', 'N', 'G', ' '),
    kPBM      = OD_FOURCC('P', 'B', 'M', ' '),
    kPBMRAW   = OD_FOURCC('P', 'B', 'M', 'R'),
    kPCD      = OD_FOURCC('P', 'C', 'D', ' '),
    kPCX      = OD_FOURCC('P', 'C', 'X', ' '),
    kPGM      = OD_FOURCC('P', 'G', 'M', ' '),
    kPGMRAW   = OD_FOURCC('P', 'G', 'M', 'R'),
    kPNG      = OD_FOURCC('P', 'N', 'G', ' '),
    kPPM      = OD_FOURCC('P', 'P', 'M', ' '),
    kPPMRAW   = OD_FOURCC('P', 'P', 'M', 'R'),
    kRAS      = OD_FOURCC('R', 'A', 'S', ' '),
    kTARGA    = OD_FOURCC('T', 'G', 'A', ' '),
    kTIFF     = OD_FOURCC('T', 'I', 'F', 'F'),
    kWBMP     = OD_FOURCC('W', 'B', 'M', 'P'),
    kPSD      = OD_FOURCC('P', 'S', 'D', ' '),
    kCUT      = OD_FOURCC('C', 'U', 'T', ' '),
    kXBM      = OD_FOURCC('X', 'B', 'M', ' '),
    kXPM      = OD_FOURCC('X', 'P', 'M', ' '),
    kDDS      = OD_FOURCC('D', 'D', 'S', ' '),
    kGIF      = OD_FOURCC('G', 'I', 'F', ' '),
    kHDR      = OD_FOURCC('H', 'D', 'R', ' '),
    kFAXG3    = OD_FOURCC('F', 'X', 'G', '3'),
    kSGI      = OD_FOURCC('S', 'G', 'I', ' '),
    kEXR      = OD_FOURCC('E', 'X', 'R', ' '),
    kJ2K      = OD_FOURCC('J', '2', 'K', ' '),
    kJP2      = OD_FOURCC('J', 'P', '2', ' '),
    kPFM      = OD_FOURCC('P', 'F', 'M', ' '),
    kPICT     = OD_FOURCC('P', 'I', 'C', 'T'),
    kRAW      = OD_FOURCC('R', 'A', 'W', ' ')
#endif
  }; 

  // Loading flags
  enum LoadFlags
  {
#if defined(OD_SWIGCSHARP) || defined(OD_SWIGJAVA)
    // Specify loading format explicitly
    kLoadFmt        = OD_FOURCC(0x0046, 0x004D, 0x0054, 0x0020),
    // Avoids post-reorientation of TIFF format images
    kNoTIFFRotation = OD_FOURCC(0x004E, 0x0054, 0x0046, 0x0052)
#else
    // Specify loading format explicitly
    kLoadFmt        = OD_FOURCC('F', 'M', 'T', ' '),
    // Avoids post-reorientation of TIFF format images
    kNoTIFFRotation = OD_FOURCC('N', 'T', 'F', 'R')
#endif
  };

  // Saving flags
  enum SaveFlags
  {
#if defined(OD_SWIGCSHARP) || defined(OD_SWIGJAVA)
    // Specify palette index of transparent color (-1 - by default)
    kTransparentColor = OD_FOURCC(0x0054, 0x0043, 0x004C, 0x0052),

    // Jpeg compression quality (default depends from implementation)
    kJpegQuality = OD_FOURCC(0x004A, 0x0051, 0x0054, 0x0059),

    // Tiff compression format (no compression by default)
    kTiffCompression = OD_FOURCC(0x0054, 0x0043, 0x004D, 0x0050),
    kTiffCompressionDeflate = OD_FOURCC(0x005A, 0x0049, 0x0050, 0x0020),
    kTiffCompressionLzw = OD_FOURCC(0x004C, 0x005A, 0x0057, 0x0020),
    kTiffCompressionJpeg = OD_FOURCC(0x004A, 0x0050, 0x0045, 0x0047),
    kTiffCompressionCCITTFax3 = OD_FOURCC(0x0046, 0x0041, 0x0058, 0x0033),
    kTiffCompressionCCITTFax4 = OD_FOURCC(0x0046, 0x0041, 0x0058, 0x0034),
    kTiffCompressionEmbedded = OD_FOURCC(0x0045, 0x004D, 0x0042, 0x0044), // ODA extension for IbEnabler, actually it is CCITFax4 compression

    //dithering flag
    kDithering = OD_FOURCC(0x0044, 0x0049, 0x0054, 0x0048),
    kDitheringFS = OD_FOURCC(0x0044, 0x0054, 0x0046, 0x0053), // Floyd & Steinberg error diffusion
    kDitheringBayer4x4 = OD_FOURCC(0x0042, 0x0059, 0x0052, 0x0034), // Bayer ordered dispersed dot dithering (order 2 dithering matrix)
    kDitheringBayer8x8 = OD_FOURCC(0x0042, 0x0059, 0x0052, 0x0038), // Bayer ordered dispersed dot dithering (order 3 dithering matrix)
    kDitheringBayer16x16 = OD_FOURCC(0x0042, 0x0052, 0x0031, 0x0036), // Bayer ordered dispersed dot dithering (order 4 dithering matrix)
    kDitheringCluster6x6 = OD_FOURCC(0x0043, 0x004C, 0x0052, 0x0036), // Ordered clustered dot dithering (order 3 - 6x6 matrix)
    kDitheringCluster8x8 = OD_FOURCC(0x0043, 0x004C, 0x0052, 0x0038), // Ordered clustered dot dithering (order 4 - 8x8 matrix)
    kDitheringCluster16x16 = OD_FOURCC(0x0043, 0x004C, 0x0031, 0x0036), // Ordered clustered dot dithering (order 8 - 16x16 matrix)

    //rescale flags
    kRescale = OD_FOURCC(0x0052, 0x0053, 0x0043, 0x004C),
    kRescaleBox = OD_FOURCC(0x0042, 0x004F, 0x0058, 0x0020),             // Box, pulse, Fourier window, 1st order (constant) b-spline
    kRescaleBicubic = OD_FOURCC(0x0042, 0x0043, 0x0042, 0x0043),         // Mitchell & Netravali's two-param cubic filter
    kRescaleBilinear = OD_FOURCC(0x0042, 0x004C, 0x004E, 0x0052),        // Bilinear filter
    kRescaleBspline = OD_FOURCC(0x0042, 0x0053, 0x0050, 0x004C),         // 4th order (cubic) b-spline
    kRescaleCatmullrom = OD_FOURCC(0x0043, 0x0054, 0x004D, 0x004C),      // Catmull-Rom spline, Overhauser spline
    kRescaleLanczos3 = OD_FOURCC(0x004C, 0x004E, 0x0043, 0x005A),        // Lanczos3 filter
    kRescaleWidth = OD_FOURCC(0x0057, 0x0044, 0x0054, 0x0048),
    kRescaleHeight = OD_FOURCC(0x0048, 0x0047, 0x0048, 0x0054)
    //Note: rescaling converts bitional images to 8-bit images, so to keep the image bitional after rescale, the dithering flags must be in flag chain.
#else
    // Specify palette index of transparent color (-1 - by default)
    kTransparentColor = OD_FOURCC('T', 'C', 'L', 'R'),

    // Jpeg compression quality (default depends from implementation)
    kJpegQuality = OD_FOURCC('J', 'Q', 'T', 'Y'),

    // Tiff compression format (no compression by default)
    kTiffCompression = OD_FOURCC('T', 'C', 'M', 'P'),
    kTiffCompressionDeflate = OD_FOURCC('Z', 'I', 'P', ' '),
    kTiffCompressionLzw = OD_FOURCC('L', 'Z', 'W', ' '),
    kTiffCompressionJpeg = OD_FOURCC('J', 'P', 'E', 'G'),
    kTiffCompressionCCITTFax3 = OD_FOURCC('F', 'A', 'X', '3'),
    kTiffCompressionCCITTFax4 = OD_FOURCC('F', 'A', 'X', '4'),
    kTiffCompressionEmbedded = OD_FOURCC('E', 'M', 'B', 'D'), // ODA extension for IbEnabler, actually it is CCITFax4 compression

    //dithering flag
    kDithering = OD_FOURCC('D', 'I', 'T', 'H'),
    kDitheringFS = OD_FOURCC('D', 'T', 'F', 'S'), // Floyd & Steinberg error diffusion
    kDitheringBayer4x4 = OD_FOURCC('B', 'Y', 'R', '4'), // Bayer ordered dispersed dot dithering (order 2 dithering matrix)
    kDitheringBayer8x8 = OD_FOURCC('B', 'Y', 'R', '8'), // Bayer ordered dispersed dot dithering (order 3 dithering matrix)
    kDitheringBayer16x16 = OD_FOURCC('B', 'R', '1', '6'), // Bayer ordered dispersed dot dithering (order 4 dithering matrix)
    kDitheringCluster6x6 = OD_FOURCC('C', 'L', 'R', '6'), // Ordered clustered dot dithering (order 3 - 6x6 matrix)
    kDitheringCluster8x8 = OD_FOURCC('C', 'L', 'R', '8'), // Ordered clustered dot dithering (order 4 - 8x8 matrix)
    kDitheringCluster16x16 = OD_FOURCC('C', 'L', '1', '6'), // Ordered clustered dot dithering (order 8 - 16x16 matrix)

    //rescale flags
    kRescale = OD_FOURCC('R', 'S', 'C', 'L'),
    kRescaleBox = OD_FOURCC('B', 'O', 'X', ' '),             // Box, pulse, Fourier window, 1st order (constant) b-spline
    kRescaleBicubic = OD_FOURCC('B', 'C', 'B', 'C'),         // Mitchell & Netravali's two-param cubic filter
    kRescaleBilinear = OD_FOURCC('B', 'L', 'N', 'R'),        // Bilinear filter
    kRescaleBspline = OD_FOURCC('B', 'S', 'P', 'L'),         // 4th order (cubic) b-spline
    kRescaleCatmullrom = OD_FOURCC('C', 'T', 'M', 'L'),      // Catmull-Rom spline, Overhauser spline
    kRescaleLanczos3 = OD_FOURCC('L', 'N', 'C', 'Z'),        // Lanczos3 filter
    kRescaleWidth = OD_FOURCC('W', 'D', 'T', 'H'),
    kRescaleHeight = OD_FOURCC('H', 'G', 'H', 'T')
    //Note: rescaling converts bitional images to 8-bit images, so to keep the image bitional after rescale, the dithering flags must be in flag chain.
#endif
  };

  ODRX_DECLARE_MEMBERS(OdRxRasterServices);

  /** \details
      Loads the specified Raster Image file.
      \param filename [in]  Filename of the Raster Image file to be read.
      \param pStreamBuf [in]  Pointer to the StreamBuf object from which the data are to be read.
      \param pFlagsChain [in]  Optional zero-terminated loading flag pairs array.

      \remarks
      The returned pointer is expected to be passed to OdGiViewportGeometry::rasterImageDc().
  */
  virtual OdGiRasterImagePtr loadRasterImage(const OdString &filename, const OdUInt32 *pFlagsChain = NULL) = 0;
  virtual OdGiRasterImagePtr loadRasterImage(OdStreamBuf *pStreamBuf, const OdUInt32 *pFlagsChain = NULL) = 0;

  /** \details
      Create raster image using user-defined image implementation.
      \param pImp [in]  User-defined raster image implementation.

      \remarks
      For default implementation always return null.
  */
  virtual OdGiRasterImagePtr createRasterImage(void *pImp);

  /** \details
      Saves specified Raster Image to the specified file.
      \param rasterImage [in]  Raster image to be saved.
      \param filename [in]  Filename of the Raster Image file to be written.
      \param type [in]  Image format type to be written.
      \param pFlagsChain [in]  Optional zero-terminated saving flag pairs array.
  */
  virtual bool saveRasterImage(const OdGiRasterImage* rasterImage, const OdString& filename,
                               const OdUInt32 *pFlagsChain = NULL) = 0;
  virtual bool saveRasterImage(const OdGiRasterImage* rasterImage, const OdString& filename,
                               OdUInt32 type, const OdUInt32 *pFlagsChain = NULL) = 0;

  /** \details
      Try to convert raster image (RGB) to JPEG or other type.
      \param pRaster [in]  Raster image to be converted.
      \param type [in]  Image format type to be converted.
      \param pStreamBuf [in]  Pointer to the StreamBuf object to which the data are to be stored.
      \param pFlagsChain [in]  Optional zero-terminated saving flag pairs array.
  */
  virtual bool convertRasterImage(const OdGiRasterImage* pRaster, OdUInt32 type,
                                  OdStreamBuf* pStreamBuf, const OdUInt32 *pFlagsChain = NULL) = 0;

  /** \details
      Try to convert raster image to other type.
      \param pSrcStream [in]  Pointer to the StreamBuf object from which the data are to be converted.
      \param pDstStream [in]  Pointer to the StreamBuf object to which the data are to be converted.
      \param type [in]  Image format type to be converted.
      \param pFlagsChainSrc [in]  Optional zero-terminated loading flag pairs array.
      \param pFlagsChainDst [in]  Optional zero-terminated saving flag pairs array.
  */
  virtual bool convertRasterImage(OdStreamBuf* pSrcStream, OdStreamBuf* pDstStream, OdUInt32 type,
                                  const OdUInt32 *pFlagsChainSrc = NULL, const OdUInt32 *pFlagsChainDst = NULL) = 0;

  /** \details
      Returns array of supported image format types.
  */
  virtual OdUInt32Array getRasterImageTypes() const = 0;

  /** \details
      Checks does image format type is supported.
      \param type [in]  Image format type to be checked.
  */
  virtual bool isRasterImageTypeSupported(OdUInt32 type) const;

  /** \details
      Get file extension and filter name by type.
      \param type [in]  Image format type to be formatted.
      \param psFilterName [out]  Output filter name (can be Null).
  */
  virtual OdString mapTypeToExtension(OdUInt32 type, OdString* psFilterName) const = 0;

  /** \details
      Get image format type by file extension.
      \param extension [in]  File extension.
  */
  virtual OdUInt32 mapExtensionToType(const OdString& extension) const = 0;

  /** \details
      Try to detect image format type from input stream.
      \param filename [in]  Filename of the Raster Image file to be checked.
      \param pStreamBuf [in]  Pointer to the StreamBuf object from which the data are to be checked.
  */
  virtual OdUInt32 getImageFormat(const OdString &filename) const = 0;
  virtual OdUInt32 getImageFormat(OdStreamBuf* pStreamBuf) const = 0;
};

/** \details
  This template class is a specialization of the OdSmartPtr class for OdRxRasterServices object pointers.
*/
typedef OdSmartPtr<OdRxRasterServices> OdRxRasterServicesPtr;

#include "TD_PackPop.h"

#endif // __OD_RASTER_MODULE__

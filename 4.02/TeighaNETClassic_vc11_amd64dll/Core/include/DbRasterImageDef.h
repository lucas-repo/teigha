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




#ifndef __DBRASTERIMAGEDEF_H
#define __DBRASTERIMAGEDEF_H

#include "TD_PackPush.h"

// Forward declarations
//

class OdDbDictionary;

#include "DbObjectReactor.h"
#include "DbObject.h"
#include "Gi/GiRasterImage.h"
#include "Ge/GeVector2d.h"
#include "RxObjectImpl.h"

/** \details
    This virtual base class defines Raster Image Definition objects in an OdDbDatabase instance.
    
    \sa
    TD_Db

    \remarks
    Raster Image Definitions (OdDbRasterImageDef objects) work with Raster Image (OdDbRasterImage) entities) 
    in much the same way that Block Table Records (OdDbBlockTableRecord objects) work with Block References
    (OdDbBlockReference entities).

    <group OdDb_Classes>
*/
class TOOLKIT_EXPORT OdDbRasterImageDef : public OdDbObject
{
public:
  ODDB_DECLARE_MEMBERS(OdDbRasterImageDef);

  typedef OdGiRasterImage::Units Units;

  OdDbRasterImageDef();
  
  OdResult subErase(
    bool erasing);
  virtual void subHandOverTo (
    OdDbObject* pNewObject);
  void subClose();

  OdResult dwgInFields(
    OdDbDwgFiler* pFiler);

  void dwgOutFields(
    OdDbDwgFiler* pFiler) const;

  OdResult dxfInFields(
    OdDbDxfFiler* pFiler);

  void dxfOutFields(
    OdDbDxfFiler* pFiler) const;

  /** \details
    Sets the name of the source file containing the raster image for this Raster Image Definition object (DXF 1).
    
    \param pathName [in]  Path name.
  */
  OdResult setSourceFileName(const OdString& pathName);

  /** \details
    Returns the name of the source file containing the raster image for this Raster Image Definition object (DXF 1).
    
    \remarks
    This function calls searchForActivePath() to determine the active path.
    
    \remarks
    This RasterImageDef object must be open for reading.
  */
  OdString sourceFileName() const;

  /** \details
    Loads the source image file for this Raster Image Definition object.
    
    \param modifyDatabase [in]  If and only if true, undo recording will be done for this object.

    \remarks
    This function calls searchForActivePath() to determine the active path.
    
    If the image file is currently loaded, the file will not be read.
    
    "Lazy loading" implies that the image file will not be loaded until it is required.
    
  */
  virtual OdResult load(
    bool modifyDatabase = true);

  /** \details
    Unloads the image for this Raster Image Definition object. 
    
    \param modifyDatabase [in]  If and only if true, undo recording will be done for this object.

    \remarks
    This RasterImageDef object must be open for writing.

  */
  virtual void unload(
    bool modifyDatabase = true);

  /** \details
    Returns true if and only if the image file for this Raster Image Definition object is loaded (DXF 280).
  */
  virtual bool isLoaded() const;

  /** \details
    Returns the XY pixel size of the image for this Raster Image Definition (DXF 10).
  */
  virtual OdGeVector2d size() const;


  /** \details
    Returns the default physical pixel size, in mm/pixel, of the image for this Raster Image Definition object (DXF 10).
    
    \remarks
    If the image has no default pixel size, 
    this function returns 1.0/(image width in pixels) for XY resolutions.
  */
  virtual OdGeVector2d resolutionMMPerPixel() const;

  /** \details
    Sets the default physical pixel size, in mm/pixel, of the image for this Raster Image Definition object (DXF 10).
    
    \remarks
    Loading the actual image file resets this value if the image has default pixel size.
  */
  virtual OdResult setResolutionMMPerPixel(const OdGeVector2d&);


  int entityCount(bool* pbLocked = NULL) const;
  void updateEntities() const;

  /** \details
    Returns the resolution units for the image for this Raster Image Definition object (DXF 281).
  */
  virtual OdGiRasterImage::Units resolutionUnits() const;

  /** \details
    Sets the resolution units for the image for this Raster Image Definition object (DXF 281).

    \remarks
    Loading the actual image file resets this value.
  */
  virtual void setResolutionUnits(enum OdGiRasterImage::Units);

  /** \details
    Returns the OdGiRasterImage object associated with this RasterImageDef object 
  */
  virtual OdGiRasterImagePtr image(bool load = true);

  /** \details
    Creates an image from the specified OdGiRasterImage object.
    \param pImage [in]  Pointer to the RasterImage object.
    \param modifyDatabase [in]  If and only if true, marks the associated OdDbDatabase instance as modified.
    
    \remarks
    isLoaded() returns false if pImage is NULL. Otherwise, it is returns true..
  */
  virtual void setImage(OdGiRasterImage* pImage, bool modifyDatabase = true);

  /** \details
    Creates an image dictionary, if one is not already present, in the specified OdDbDatabase instance.

    \param pDb [in]  Pointer to the database.
    
    \returns
    Returns the Object ID of the image dictionary.
  */
  static OdDbObjectId createImageDictionary(OdDbDatabase* pDb);

  /** \details
    Returns the Object ID of the image dictionary in the specified OdDbDatabase instance.
    \param pDb [in]  Pointer to the database.  
  */
  static OdDbObjectId imageDictionary(OdDbDatabase* pDb);

  enum 
  { 
    kMaxSuggestNameSize = 2049 
  };

  /** \details
    Massages the original image filename to return a new
    image name suitable for an image dictionary.
    \param pImageDictionary [in]  Pointer to the image dictionary.
    \param strFilePath [in]  new image file name. 
    \param nMaxLength [in]  max length of the name returned
  */
  static OdString suggestName(const OdDbDictionary* pImageDictionary,
                  const OdString& strFilePath, int nMaxLength = kMaxSuggestNameSize);

  OdString activeFileName() const;
  OdResult setActiveFileName(const OdString& pPathName);
  /*   comment this out for a while

  int colorDepth() const;
  OdGiSentScanLines* makeScanLines(
    const OdGiRequestScanLines* pReq,
    const OdGeMatrix2d& pixToScreen,
    OdGePoint2dArray* pActiveClipBndy = 0, // Data will be modified!
    bool draftQuality = false,
    bool isTransparent = false,
    const double brightness = 50.0,
    const double contrast = 50.0,
    const double fade = 0.0
    ) const;
  OdString searchForActivePath();
  void embed(); // this function is not implemented in ARX
  bool isEmbedded() const;
  OdString fileType() const;
  void setUndoStoreSize(unsigned int maxImages = 10);
  unsigned int undoStoreSize() const;
  bool imageModified() const;
  void setImageModified(bool modified);
  void loadThumbnail(OdUInt16& maxWidth, OdUInt16& maxHeight,
    OdUInt8* pPalette = 0, int nPaletteEntries = 0);
  void unloadThumbnail();
  void* createThumbnailBitmap(BITMAPINFO*& pBitmapInfo,
    OdUInt8 brightness = 50, OdUInt8 contrast = 50, OdUInt8 fade = 0);
  IeFileDesc* fileDescCopy() const;
  void getScanLines(OdGiBitmap& bitmap,
    double brightness = 50.0,
    double contrast = 50.0,
    double fade = 0.0) const;
  void openImage(IeImg*& pImage);
  void closeImage();

  */
  static int classVersion();
};




/** \details
    This class implements Raster Image Definition Reactor objects in an OdDbDatabase instance.
    
    \sa
    TD_Db
    
    \remarks
    Raster Image Definition Reactor (OdDbRasterImageDefReactor) objects are used 
    to notify Raster Image (OdDbRasterImage) objects 
    of changes to their associated Raster Image Definition (OdDbRasterImage) objects.
    
    Modifications of Image Definition objects redraw their dependent Raster Image entities. 
    Deletion of Image Definition objects delete their dependent Raster Image entities.

    <group OdDb_Classes>
*/
class TOOLKIT_EXPORT OdDbRasterImageDefReactor : public OdDbObject
{
public:
  ODDB_DECLARE_MEMBERS(OdDbRasterImageDefReactor);

  OdDbRasterImageDefReactor();

  OdResult dwgInFields(
    OdDbDwgFiler* pFiler);

  void dwgOutFields(
    OdDbDwgFiler* pFiler) const;

  OdResult dxfInFields(
    OdDbDxfFiler* pFiler);

  void dxfOutFields(
    OdDbDxfFiler* pFiler) const;

  void erased(
    const OdDbObject* pObject, 
    bool erasing = true);

  void modified(
    const OdDbObject* pObject);

  enum DeleteImageEvent
  {
    kUnload = 1,
    kErase = 2
  };
  /** \details
    Controls notifications of OdDbRasterImage object events.
    
    \param enable [in]  If and only if true, enables notifications.
  */
  static void setEnable(
    bool enable);

  /** \details
    Notification function called whenever an OdDbRasterImageDef object is about to be unloaded or erased.
    
    \param pImageDef [in]  Pointer to the OdDbRasterImageDef object sending this notification.
    \param event [in]  Event triggering the notification.
    \param cancelAllowed [in]  True to enable user cancellation, false to disable.
    
    \returns
    Returns true if and only if not cancelled. 
    \remarks
    event must be one of the following:
    
    <table>
    Name      Value
    kUnload   1
    kErase    2
    </table>
    
    
    \remarks
    Use imageModified() to determine if the Image Definition has been modified.
  */
  virtual bool onDeleteImage( 
    const OdDbRasterImageDef* pImageDef,
    DeleteImageEvent event,
    bool cancelAllowed);

  static int classVersion();
};

/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbRasterImageDefReactor object pointers.
*/
typedef OdSmartPtr<OdDbRasterImageDefReactor> OdDbRasterImageDefReactorPtr;

/** \details
    This class implements Raster Image Definition Transient Reactor objects in an OdDbDatabase instance.
    Corresponding C++ library: TD_Db
    <group OdDb_Classes>
*/
class TOOLKIT_EXPORT OdDbRasterImageDefTransReactor : public OdDbObjectReactor
{
protected:
  OdDbRasterImageDefTransReactor() {}
public:
  ODRX_DECLARE_MEMBERS(OdDbRasterImageDefTransReactor);

  /** \details
    Notification function called whenever an OdDbRasterImageDef object is about to be unloaded or erased.
    
    \param pImageDef [in]  Pointer to the OdDbRasterImageDef object sending this notification.
    \param event [in]  Event triggering the notification.
    \param cancelAllowed [in]  True to enable user cancellation, false to disable.
    
    \returns
    Returns true if and only if not cancelled. 
    \remarks
    event must be one of the following:
    
    <table>
    Name                                 Value
    OdDbRasterImageDefReactor::kUnload   1
    OdDbRasterImageDefReactor::kErase    2
    </table>
  */
  virtual bool onDeleteImage( const OdDbRasterImageDef* pImageDef,
                              OdDbRasterImageDefReactor::DeleteImageEvent event,
                              bool cancelAllowed ) = 0;
};

/*   comment this for a while */

#if 0 /*!DOM*/


/** \details
  <group OdDb_Classes>
*/
class TOOLKIT_EXPORT OdDbRasterImageDefFileAccessReactor : public OdDbObjectReactor
{
protected:
  OdDbRasterImageDefFileAccessReactor() {}
public:
  ODRX_DECLARE_MEMBERS(OdDbRasterImageDefFileAccessReactor);

  virtual void onAttach(const OdDbRasterImageDef*, const OdString& pPath) = 0;
  virtual void onDetach(const OdDbRasterImageDef*, const OdString& pPath) = 0;
  virtual bool onOpen(const OdDbRasterImageDef*, const OdString& pPath,
    const OdString& pActivePath, bool& replacePath, OdString& replacementPath) = 0;

  virtual bool onPathChange(const OdDbRasterImageDef*,
    const OdString& pPath, const OdString& pActivePath,
    bool& replacePath, OdString& replacementPath) = 0;

  virtual void onClose(const OdDbRasterImageDef*, const OdString& pPath) = 0;

  virtual void onDialog(OdDbRasterImageDef*,
    const OdString& pCaption, const OdString& pFileExtensions) = 0;
};

#endif

/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbRasterImageDef object pointers.
*/
typedef OdSmartPtr<OdDbRasterImageDef> OdDbRasterImageDefPtr;

#include "TD_PackPop.h"

#endif // __DBRASTERIMAGEDEF_H


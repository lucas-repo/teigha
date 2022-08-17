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

#ifndef _ODDB_BASEDATABASE_INCLUDED_
#define _ODDB_BASEDATABASE_INCLUDED_

#include "RxObject.h"
#include "DbRootExport.h"
#include "OdFont.h"
#include "GiDefaultContext.h"
#include "Gs/Gs.h"
#include "Gi/GiLinetype.h"
#include "Ge/GeCurve2d.h"
#include "Ge/GeDoubleArray.h"
#include "Ge/GePoint2dArray.h"
#include "DbDate.h" // dgn dwf export
#include "OdUnitsFormatter.h"
#include "StringArray.h"
#include "DbHandle.h"

class OdDbBaseHostAppServices;
class OdGiAnnoScaleSet;

typedef OdRxObject OdDbBaseDatabase;
typedef OdRxObjectPtr OdDbBaseDatabasePtr;

/** \details
    This class is the base Protocol Extension class for database classes.

    \sa
    TD_DbRoot
    <group OdDbRoot_Classes> 
*/
class DBROOT_EXPORT OdDbBaseDatabasePE : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdDbBaseDatabasePE);

  enum ExtentsFlags
  {
    kZeroFlags      = 0,
    kExactExtents   = 1, //Zoom to extents exact to device/paper size w/o any margins if this flag is set
    kUseViewExtents = 2  //Use view extents instead of plot extents (default) for calculation of drawing extents if this flag is set
  };

  /** \details
      Returns the OdDbBaseHostAppServices object associated with this database object.
  */
  virtual OdDbBaseHostAppServices* appServices(OdDbBaseDatabase* pDb) const = 0;

  /** \details
      Returns the name of the file associated with this database object.
  */
  virtual OdString getFilename(OdDbBaseDatabase* pDb) const = 0;

  virtual void startTransaction(OdDbBaseDatabase* pDb) = 0;
  virtual void abortTransaction(OdDbBaseDatabase* pDb) = 0;

  virtual OdGiDefaultContextPtr createGiContext(OdDbBaseDatabase* pDb) const = 0;

  virtual void putNamedViewInfo(OdDbBaseDatabase* pDb, OdDbStub* layoutId, OdStringArray& names, OdGePoint3dArray& points) = 0;
  /** \details
  Creates a TextIterator object for the specified data.

  \param textString [in]  Text string to be parsed.
  \param length [in]  Length of string in bytes.
  \param raw [in]  If and only if true, character sequences in the form of %%c will be treated as raw text.
  \param codePageId [in]  Object ID of the Code Page of the text.
  \param pTextStyle [in]  Pointer to the TextStyle object associated with the specified text.

  \returns
  Returns a SmartPointer to the new OdDbTextIterator object.
  */
  virtual OdBaseTextIteratorPtr createTextIterator(OdDbBaseDatabase* db, const OdChar* textString, int length, bool raw, const OdGiTextStyle* pTextStyle) const = 0;
  
  // setup Gs views in the device, according to the active database layout data
  virtual OdGsDevicePtr setupActiveLayoutViews(OdGsDevice* pDevice, OdGiDefaultContext* pGiCtx) = 0;

  // setup Gs views in the device, according to the specified database layout data
  virtual OdGsDevicePtr setupLayoutView(OdGsDevice* pDevice, OdGiDefaultContext* pGiCtx, OdDbStub* layoutId) = 0;
  
  // setup palette associated with the specified layout in the device.
  // palBg -- optionally specifies background color to write into palette
  // to overwrite default DB-palette color use non-zero value for alpha component of RGBA color
  virtual void setupPalette(OdGsDevice* device, OdGiDefaultContext* giContext, OdDbStub* layoutId = 0, ODCOLORREF palBg = ODRGBA(0,0,0,0)) = 0;

  // get next view according to the active layout
  virtual OdDbStub* getNextViewForActiveLayout(OdGiDefaultContext* pGiCtx, OdDbStub* /*objectId*/) = 0;
  
  // setup Gs device to display current layout using plot settings stored in it (paper rotation, scale etc.)
  // returns visible rectangle (measured in device units)
  virtual void applyLayoutSettings(OdGsDCRect& clipBox, OdGsDevice* pDevice, OdDbBaseDatabase* db, OdUInt32 extentsFlags = kZeroFlags) = 0;
  
  // setup Gs device to draw current layout zoomed to extents
  // warning: this function may throw exception, if overall viewport is not found
  // outputRect is desired printable area (in device units)
  virtual void zoomToExtents(const OdGsDCRect& outputRect, OdGsDevice* pDevice, OdDbBaseDatabase* db,
                             OdUInt32 extentsFlags = kZeroFlags, OdDbStub* objectId = NULL) = 0;
  
  virtual void loadPlotstyleTableForActiveLayout(OdGiDefaultContext* pDwgContext, OdDbBaseDatabase* db) = 0;

  virtual OdRxIteratorPtr layers(OdDbBaseDatabase* db) const = 0;
  virtual OdRxIteratorPtr visualStyles(OdDbBaseDatabase* db) const = 0;

  // Layout here is an abstraction that corresponds to "sheet" in sheet set manager, 
  //   "page" in PDF and DWF, "layout"+"block table record" in DWG
  inline OdRxObjectPtr currentLayout(OdDbBaseDatabase* pDb)
  {
    return openObject(currentLayoutId(pDb));
  }

  virtual void setCurrentLayout(OdDbBaseDatabase* db, const OdString& name) = 0;

  virtual OdRxIteratorPtr layouts(OdDbBaseDatabase* db) const = 0;

  // TODO do it inline (is special for DGN now)
  virtual OdRxObjectPtr getLayout(OdDbBaseDatabase* pDb, const OdString& name)
  {
    return openObject(findLayoutNamed(pDb, name));
  }

  inline OdRxObjectPtr findLayoutByViewport(OdDbBaseDatabase* db, OdDbStub* pViewportId)
  {
    return openObject(findLayoutIdByViewport(db, pViewportId));
  }
  virtual OdDbStub* findLayoutIdByViewport(OdDbBaseDatabase* db, OdDbStub* pViewportId) = 0;

  virtual OdDbStub* findLayoutNamed(OdDbBaseDatabase* db, const OdString& name) = 0; // dgn dwf export

  inline OdRxObjectPtr getFirstLayout(OdDbBaseDatabase* db)
  {
    return openObject(getFirstLayoutId(db));
  }
  virtual OdDbStub* getFirstLayoutId(OdDbBaseDatabase* db) = 0; // dgn dwf export

  virtual OdTimeStamp getCreationTime(OdDbBaseDatabase* db) = 0; // dgn dwf export
  virtual OdTimeStamp getUpdateTime(OdDbBaseDatabase* db) = 0; // dgn dwf export
  virtual OdString getFingerPrintGuid(OdDbBaseDatabase* db) = 0; // dgn dwf export
  virtual OdString getVersionGuid(OdDbBaseDatabase* db) = 0; // dgn dwf export

  virtual int getUnits(OdDbBaseDatabase* db) = 0; // dgn dwf export
  virtual OdUnitsFormatter* baseFormatter(OdDbBaseDatabase* db) = 0; // dgn dwg common formatter base class
  virtual int getMeasurement(OdDbBaseDatabase* db) = 0;

  virtual bool getLineTypeById( OdDbBaseDatabase* db, OdDbStub* pLTypeId, OdGiLinetype& LType ) = 0;
  virtual bool getTextStyleById( OdDbBaseDatabase* db, OdDbStub* idStyle, OdGiTextStyle& shapeInfo ) = 0;   

  virtual OdDbStub* getObject(OdDbBaseDatabase* db, OdUInt64 handle) = 0;
  virtual OdDbHandle getHandle(OdDbStub* id);
  virtual OdDbBaseDatabase* getDatabase(OdDbStub *id);
  virtual OdDbStub* getOwner(OdDbStub *id);

  virtual OdCodePageId getCodePage(OdDbBaseDatabase* db) = 0;

  virtual OdDbStub* getModelBlockId(OdDbBaseDatabase* pDb) = 0;
  virtual OdDbStub* getPaperBlockId(OdDbBaseDatabase* pDb) = 0;
  virtual OdDbStub* currentLayoutId(OdDbBaseDatabase* pDb) = 0;
  virtual OdDbStub* xrefBlockId(OdDbBaseDatabase* pDb) = 0;

  virtual void setMultiThreadedRender(OdDbBaseDatabase* pDb, bool bOn) = 0;

  virtual bool isAProxy(OdRxObject* pDrw) = 0;
  virtual OdRxObjectPtr openObject(OdDbStub* pId) = 0;
  virtual bool getAnnoScaleSet(OdDbStub* drawableId, OdGiAnnoScaleSet& res) = 0;
  virtual OdDbStub *getCurrentLongTransation(const OdDbBaseDatabase *pDb) = 0;

  class DatabaseUnloadReactor
  {
    public:
      virtual ~DatabaseUnloadReactor() { }
      virtual void goodbye(const OdDbBaseDatabase* pDb) = 0;
  };
  virtual OdRxObjectPtr addDatabaseUnloadReactor(OdDbBaseDatabase* pDb, OdRxObject *pPrevReactor, DatabaseUnloadReactor *pReactorRedirect) = 0;
  virtual void removeDatabaseUnloadReactor(OdDbBaseDatabase* pDb, OdRxObject *pReactor) = 0;
};

/** \details
    This template class is a specialization of the OdSmartPtr class for OdDbBaseDatabasePE object pointers.
*/
typedef OdSmartPtr<OdDbBaseDatabasePE> OdDbBaseDatabasePEPtr;


/** \details
  This class is the protocol extension to be used as a layer abstraction, 
  independent of the underlying database (OdDbDatabase or OdDgDatabase).

  \sa
  TD_DbRoot

  <group OdDbRoot_Classes> 
*/
class DBROOT_EXPORT OdDbBaseLayerPE : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdDbBaseLayerPE);

  /** \details
    Returns name of this Layer object.

    \param obj [in]  Layer object
  */
  virtual OdString name(const OdRxObject* obj) const = 0;

  /** \details
    Returns true if and only if this Layer is off.
    
    \param obj [in]  Layer object
  */
  virtual bool isOff(const OdRxObject* obj) const = 0;
  /** \details
    Returns true if and only if this Layer is frozen.
    
    \param obj [in]  Layer object
  */
  virtual bool isFrozen(const OdRxObject* obj) const = 0;

  /** \details
    Sets the on / off status of this Layer

    \param obj [in]  Layer object
    \param off [in]  True for off, false for on.
  */
  virtual void setIsOff(OdRxObject* obj, bool off) const = 0;
  /** \details
    Sets the frozen status of this Layer.

    \param obj [in]  Layer object
    \param frozen [in]  True to freeze, false to thaw.
  */
  virtual void setIsFrozen(OdRxObject* obj, bool frozen) const = 0;

  /** \details
  Return true if function set line type description of this Layer.

  \param obj [in]  Layer object
  \param LType [out]  Description of layer line type.
  */
  virtual bool getLineType(OdRxObject* obj, OdGiLinetype& LType) const = 0;
};

typedef OdSmartPtr<OdDbBaseLayerPE> OdDbBaseLayerPEPtr;


/** \details
  This class is the protocol extension to be used as a visual style abstraction, 
  independent of the underlying database (OdDbDatabase or OdDgDatabase).

  \sa
  TD_DbRoot

  <group OdDbRoot_Classes> 
*/
class DBROOT_EXPORT OdDbBaseVisualStylePE : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdDbBaseVisualStylePE);

  /** \details
    Returns name of this Visual Style.

    \param obj [in]  Visual Style object
  */
  virtual OdString name(const OdRxObject* obj) const = 0;

  /** \details
    Returns true if and only if this Visual Style was marked as internal.
    
    \param obj [in]  Visual Style object
  */
  virtual bool isInternal(const OdRxObject* obj) const = 0;
};

typedef OdSmartPtr<OdDbBaseVisualStylePE> OdDbBaseVisualStylePEPtr;


/** \details
  This class is the protocol extension to be used as a layout abstraction, 
  independent of the underlying database (OdDbDatabase or OdDgDatabase).

  \sa
  TD_DbRoot

  <group OdDbRoot_Classes> 
*/
class DBROOT_EXPORT OdDbBaseLayoutPE : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdDbBaseLayoutPE);
  enum PlotRotation
  {
    k0degrees       = 0,   // No rotation
    k90degrees      = 1,   // 90° CCW
    k180degrees     = 2,   // Inverted
    k270degrees     = 3    // 90° CW
  };
  virtual OdString name(const OdRxObject*) = 0;
  virtual bool isModelLayout(const OdRxObject*) = 0;
  virtual bool printLineweights(const OdRxObject*) = 0;
  virtual bool showPlotStyles(const OdRxObject*) = 0;
  virtual OdString paperName(const OdRxObject*) = 0;
  virtual void getPaperSize(const OdRxObject*, double& paperWidth, double& paperHeight) const = 0;
  virtual PlotRotation plotRotation(const OdRxObject*) = 0;
  virtual double getTopMargin(const OdRxObject*) = 0;
  virtual double getRightMargin(const OdRxObject*) = 0;
  virtual double getBottomMargin(const OdRxObject*) = 0;
  virtual double getLeftMargin(const OdRxObject*) = 0;
  virtual OdDbStub* getId(const OdRxObject*) = 0;
  virtual bool isOverallVPortErased(const OdRxObject*) = 0;
  virtual OdResult getGeomExtents(const OdRxObject*,OdGeExtents3d& ext) = 0;

  // methods inherited by OdDbLayoutImpl class // dgn dwf export
  virtual bool useStandardScale(const OdRxObject*) = 0;
  virtual void getStdScale(const OdRxObject*, double& scale) = 0;
  virtual void getCustomPrintScale(const OdRxObject*, double& numerator, double& denominator) = 0;
  virtual int plotType(const OdRxObject*) = 0;
  virtual OdString getPlotViewName(const OdRxObject*) = 0;
  virtual void getPlotWindowArea(const OdRxObject*, double& xmin, double& ymin, double& xmax, double& ymax) = 0;
  virtual void getPlotOrigin(const OdRxObject*, double& x, double& y) = 0;
  virtual void getPlotPaperSize(const OdRxObject*, double& paperWidth, double& paperHeight) = 0;
  virtual int plotPaperUnits(const OdRxObject*) = 0;

  // for Gs
  virtual OdDbStub* getBlockId(const OdRxObject*) const = 0;
  virtual bool scalePSLinetypes(const OdRxObject*) const = 0;
  virtual bool getApproxExtents(const OdRxObject*, OdGePoint3d& extMin, OdGePoint3d& extMax) const = 0;
};

typedef OdSmartPtr<OdDbBaseLayoutPE> OdDbBaseLayoutPEPtr;


/** \details
  This class is the protocol extension to be used as a block table record abstraction,
  independent of the underlying database (OdDbDatabase or OdDgDatabase).

  \sa
  TD_DbRoot

  <group OdDbRoot_Classes>
*/
class DBROOT_EXPORT OdDbBaseBlockPE : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdDbBaseBlockPE);

  virtual bool isBlockReferenceAdded(OdRxObject *pBlock) const = 0;

  virtual bool isFromExternalReference(const OdRxObject *pBlock) const = 0;
  virtual OdDbBaseDatabase *xrefDatabase(const OdRxObject *pBlock) const = 0;

  virtual bool isLayout(const OdRxObject *pBlock) const = 0;
};

typedef OdSmartPtr<OdDbBaseBlockPE> OdDbBaseBlockPEPtr;


/** \details
  This class is the protocol extension to be used as a block reference abstraction,
  independent of the underlying database (OdDbDatabase or OdDgDatabase).

  \sa
  TD_DbRoot

  <group OdDbRoot_Classes>
*/
class DBROOT_EXPORT OdDbBaseBlockRefPE : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdDbBaseBlockRefPE);

  virtual OdDbStub *blockId(const OdRxObject *pBlockRef) const = 0;
  virtual OdGeMatrix3d blockTransform(const OdRxObject *pBlockRef) const = 0;

  virtual OdRxIteratorPtr newAttribIterator(const OdRxObject *pBlockRef, bool bSkipErased = true) const = 0;
  virtual bool isAttribute(const OdRxObject *pAttrib) const = 0;
  virtual OdDbStub *getId(const OdRxObject *pObj) const = 0;

  // is generic BlockRef (Gs willn't treat drawable as block reference if this method return false)
  // this method can be useful to filter out incompatible entities inherited from BlockRef.
  virtual bool isGeneric(const OdRxObject *pBlockRef) const = 0;
  // is MInsert block
  virtual bool isMInsert(const OdRxObject *pBlockRef) const = 0;
  // is basic BlockRef
  virtual bool isBasic(const OdRxObject *pBlockRef) const = 0;
};

typedef OdSmartPtr<OdDbBaseBlockRefPE> OdDbBaseBlockRefPEPtr;


/** \details
  This class is the protocol extension to be used as a entities sorting object abstraction,
  independent of the underlying database (OdDbDatabase or OdDgDatabase).

  \sa
  TD_DbRoot

  <group OdDbRoot_Classes>
*/
class DBROOT_EXPORT OdDbBaseSortEntsPE : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdDbBaseSortEntsPE);

  virtual OdDbStub *blockId(const OdRxObject *pSortents) const = 0;
};

typedef OdSmartPtr<OdDbBaseSortEntsPE> OdDbBaseSortEntsPEPtr;


/** \details
  This class is the protocol extension to be used as a long transaction abstraction,
  independent of the underlying database (OdDbDatabase or OdDgDatabase).

  \sa
  TD_DbRoot

  <group OdDbRoot_Classes>
*/
class DBROOT_EXPORT OdDbBaseLongTransactionPE : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdDbBaseLongTransactionPE);

  virtual OdDbStub *destinationBlock(const OdRxObject *pLT) const = 0;

  virtual bool workSetHas(const OdRxObject *pLT, OdDbStub *pId) const = 0;
  virtual OdRxIteratorPtr newWorkSetIterator(const OdRxObject *pLT, bool incRemoved = false, bool incSecondary = false) const = 0;

  virtual OdDbStub *getOwner(OdDbStub *pId) const = 0;
  virtual OdDbStub *getId(const OdRxObject *pObj) const = 0;
};

typedef OdSmartPtr<OdDbBaseLongTransactionPE> OdDbBaseLongTransactionPEPtr;

/** \details
This class defines the interface for the Hatch Protocol Extension classes.
<group OdDbRoot_Classes>
*/
class DBROOT_EXPORT OdDbBaseHatchPE : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdDbBaseHatchPE);
  enum HatchStyle
  {
    kNormal = 0, // Hatch toggles on each boundary.
    kOuter  = 1, // Hatch turns off after first inner loop.
    kIgnore = 2  // Hatch ignores inner loop
  };

  enum HatchLoopType
  {
    kDefault = 0,        // Not yet specified.
    kExternal = 1,        // Defined by external entities.
    kPolyline = 2,        // Defined by OdGe polyline.
    kDerived = 4,        // Derived from a picked point.
    kTextbox = 8,        // Defined by text.
    kOutermost = 0x10,     // Outermost loop.
    kNotClosed = 0x20,     // Open loop.
    kSelfIntersecting = 0x40,     // Self-intersecting loop.
    kTextIsland = 0x80,     // Text loop surrounded by an even number of loops.
    kDuplicate = 0x100,    // Duplicate loop.
    kIsAnnotative = 0x200,    // The bounding area is an annotative block.
    kDoesNotSupportScale = 0x400,    // The bounding type does not support scaling
    kForceAnnoAllVisible = 0x800,    // Forces all annotatives to be visible
    kOrientToPaper = 0x1000,   // Orients hatch loop to paper
    kIsAnnotativeBlock = 0x2000    // Describes if the hatch is an annotative block.
  };

  virtual int numLoops(const OdRxObject* pHatch) const = 0;
  virtual OdInt32 loopTypeAt(const OdRxObject* pHatch, int loopIndex) const = 0;
  virtual void getLoopAt(const OdRxObject* pHatch, int loopIndex, OdArray<OdGeCurve2d*>& edgePtrs) const = 0;
  virtual void getLoopAt(const OdRxObject* pHatch, int loopIndex, OdGePoint2dArray& vertices, OdGeDoubleArray& bulges) const = 0;
  virtual HatchStyle hatchStyle(const OdRxObject* pHatch) const = 0;
  virtual bool isGradient(const OdRxObject* pHatch) const = 0;
  virtual bool isSolidFill(const OdRxObject* pHatch) const = 0;

  //In TD hatch is always hatchPE, but in TG hatchPE can be added to any entity
  virtual bool isReallyHatch(const OdRxObject* pHatch) const = 0;
};

/** \details
This template class is a specialization of the OdSmartPtr class for OdDbHatchPE object pointers.
*/
typedef OdSmartPtr<OdDbBaseHatchPE> OdDbBaseHatchPEPtr;


#endif // _ODDB_BASEDATABASE_INCLUDED_

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




#ifndef _ODDBDATABASE_INCLUDED_
#define _ODDBDATABASE_INCLUDED_

#include "TD_PackPush.h"

#include "RxObject.h"
#include "DbObjectId.h"
#include "RxObjectImpl.h"
#include "OdString.h"
#include "DbDate.h"
#include "OdCodePage.h"
#include "OdToolKit.h"
#include "DbObject.h"
#include "CmColor.h"
#include "DbSecurity.h"
#include "DbSystemServices.h"
#include "ViewportDefs.h"
#include "DbAnnotationScale.h"

class OdDbObject;

class OdDbSymbolTable;
class OdDbBlockTable;
class OdDbTextStyleTable;
class OdDbLinetypeTable;
class OdDbLayerTable;
class OdDbViewTable;
class OdDbUCSTable;
class OdDbUCSTable;
class OdDbViewportTable;
class OdDbRegAppTable;
class OdDbDimStyleTable;
class OdDbDimStyleTableRecord;
class OdDbBlockTableRecord;
class OdDbTextStyleTableRecord;
class OdDbRegAppTableRecord;
class OdDbLinetypeTableRecord;
class OdDbHostAppServices;
class OdDbLayout;
class OdDbLayoutManagerReactor;
class OdGsView;
class OdGsDevice;
class OdGsDCRect;

class OdDbDictionary;
class OdDbDictionaryWithDefault;
class OdDbFiler;
class OdDbIdMapping;
class OdDbDatabaseReactor;
class OdDbSpatialFilter;
class OdDbLayerFilter;
class OdDbAuditInfo;
class OdDbUndoController;
class OdDbTransactionReactor;
class OdDbEntity;

class OdGePoint2d;
class OdGePoint3d;
class OdGeMatrix3d;
class OdGeVector3d;
class OdDbDwgFiler;

class OdGsModel;

class OdStreamBuf;
class OdThumbnailImage;

class OdDbDatabaseImpl;
class OdResBuf;
class OdDbUnitsFormatter;
class OdDbLayerStateManager;
class OdDbObjectIdGraph;
class OdDbHistoryManager;

/** \details
  This template class is a specialization of the OdSmartPtr class for OdResBuf object pointers.
*/
typedef OdSmartPtr<OdResBuf> OdResBufPtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbDictionary object pointers.
*/
typedef OdSmartPtr<OdDbDictionary> OdDbDictionaryPtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbDictionaryWithDefault object pointers.
*/
typedef OdSmartPtr<OdDbDictionaryWithDefault> OdDbDictionaryWithDefaultPtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbSymbolTable object pointers.
*/
typedef OdSmartPtr<OdDbSymbolTable> OdDbSymbolTablePtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbBlockTable object pointers.
*/
typedef OdSmartPtr<OdDbBlockTable> OdDbBlockTablePtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbLayerTable object pointers.
*/
typedef OdSmartPtr<OdDbLayerTable> OdDbLayerTablePtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbTextStyleTable object pointers.
*/
typedef OdSmartPtr<OdDbTextStyleTable> OdDbTextStyleTablePtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbViewTable object pointers.
*/
typedef OdSmartPtr<OdDbViewTable> OdDbViewTablePtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbUCSTable object pointers.
*/
typedef OdSmartPtr<OdDbUCSTable> OdDbUCSTablePtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbViewportTable object pointers.
*/
typedef OdSmartPtr<OdDbViewportTable> OdDbViewportTablePtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbRegAppTable object pointers.
*/
typedef OdSmartPtr<OdDbRegAppTable> OdDbRegAppTablePtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbDimStyleTable object pointers.
*/
typedef OdSmartPtr<OdDbDimStyleTable> OdDbDimStyleTablePtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbLinetypeTable object pointers.
*/
typedef OdSmartPtr<OdDbLinetypeTable> OdDbLinetypeTablePtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbBlockTableRecord object pointers.
*/
typedef OdSmartPtr<OdDbBlockTableRecord> OdDbBlockTableRecordPtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbDimStyleTableRecord object pointers.
*/
typedef OdSmartPtr<OdDbDimStyleTableRecord> OdDbDimStyleTableRecordPtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbTextStyleTableRecord object pointers.
*/
typedef OdSmartPtr<OdDbTextStyleTableRecord> OdDbTextStyleTableRecordPtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbRegAppTableRecord object pointers.
*/
typedef OdSmartPtr<OdDbRegAppTableRecord> OdDbRegAppTableRecordPtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbDimStyleTableRecord object pointers.
*/
typedef OdSmartPtr<OdDbDimStyleTableRecord> OdDbDimStyleTableRecordPtr;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbLinetypeTableRecord object pointers.
*/
typedef OdSmartPtr<OdDbLinetypeTableRecord> OdDbLinetypeTableRecordPtr;

class OdSecurityParams;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdSecurityParams object pointers.
*/
typedef OdSmartPtr<OdSecurityParams> OdSecurityParamsPtr;

class OdFileDependencyManager;
/** \details
  This template class is a specialization of the OdSmartPtr class for OdFileDependencyManager object pointers.
*/
typedef OdSmartPtr<OdFileDependencyManager> OdFileDependencyManagerPtr;

class OdDbObjectContextManager;
/** \details
This template class is a specialization of the OdSmartPtr class for OdDbObjectContextManager object pointers.
*/
typedef OdSmartPtr<OdDbObjectContextManager> OdDbObjectContextManagerPtr;

/** \details
  This template class is a specialization of the OdSmartPtr class for OdDbDatabase object pointers.
*/
typedef OdSmartPtr<OdDbDatabase> OdDbDatabasePtr;

/** \details

    <group TD_Namespaces>
*/
namespace OdDb
{
  enum UnitsValue
  {
    kUnitsUndefined    = 0,
    kUnitsInches       = 1,
    kUnitsFeet         = 2,
    kUnitsMiles        = 3,
    kUnitsMillimeters  = 4,
    kUnitsCentimeters  = 5,
    kUnitsMeters       = 6,
    kUnitsKilometers   = 7,
    kUnitsMicroinches  = 8,
    kUnitsMils         = 9,
    kUnitsYards        = 10,
    kUnitsAngstroms    = 11,
    kUnitsNanometers   = 12,
    kUnitsMicrons      = 13,
    kUnitsDecimeters   = 14,
    kUnitsDekameters   = 15,
    kUnitsHectometers  = 16,
    kUnitsGigameters   = 17,
    kUnitsAstronomical = 18,
    kUnitsLightYears   = 19,
    kUnitsParsecs      = 20,
    kUnitsMax          = kUnitsParsecs
  };
  enum EndCaps
  {
    kEndCapNone       =  0,
    kEndCapRound      =  1,
    kEndCapAngle      =  2,
    kEndCapSquare     =  3
  };

  enum JoinStyle
  {
    kJnStylNone       =  0,
    kJnStylRound      =  1,
    kJnStylAngle      =  2,
    kJnStylFlat       =  3
  };

  enum DuplicateLinetypeLoading
  {
    kDltNotApplicable = 0, // Not Applicable.
    kDltIgnore        = 1, // Ignore duplicate LineTypes.
    kDltReplace       = 2  // Replace duplicate LineTypes.
  };

  // Old vesrion of Microsoft timezone index (see http://support.microsoft.com/kb/973627) somewhat reordered 
  enum TimeZone
  {
    kInternationalDateLine      = -12000,   //(GMT-12:00) International Date Line West
    kMidwayIsland               = -11000,   //(GMT-11:00) Midway Island, Samoa
    kHawaii                     = -10000,   //(GMT-10:00) Hawaii
    kAlaska                     = -9000,    //(GMT-09:00) Alaska
    kPacific                    = -8000,    //(GMT-08:00) Pacific Time (US & Canada); Tijuana
    kMountain                   = -7000,    //(GMT-07:00) Mountain Time (US & Canada)
    kArizona                    = -7001,    //(GMT-07:00) Arizona
    kMazatlan                   = -7002,    //(GMT-07:00) Chihuahua, La Paz, Mazatlan
    kCentral                    = -6000,    //(GMT-06:00) Central Time (US & Canada)
    kCentralAmerica             = -6001,    //(GMT-06:00) Central America
    kMexicoCity                 = -6002,    //(GMT-06:00) Guadalajara, Mexico City, Monterrey
    kSaskatchewan               = -6003,    //(GMT-06:00) Saskatchewan
    kEastern                    = -5000,    //(GMT-05:00) Eastern Time (US & Canada)
    kIndiana                    = -5001,    //(GMT-05:00) Indiana (East)
    kBogota                     = -5002,    //(GMT-05:00) Bogota, Lima, Quito
    kAtlanticCanada             = -4000,    //(GMT-04:00) Atlantic Time (Canada)
    kCaracas                    = -4001,    //(GMT-04:00) Caracas, La Paz
    kSantiago                   = -4002,    //(GMT-04:00) Santiago
    kNewfoundland               = -3300,    //(GMT-03:30) Newfoundland and Labrador
    kBrasilia                   = -3000,    //(GMT-03:00) Brasilia
    kBuenosAires                = -3001,    //(GMT-03:00) Buenos Aires, Georgetown
    kGreenland                  = -3002,    //(GMT-03:00) Greenland
    kMidAtlantic                = -2000,    //(GMT-02:00) Mid-Atlantic
    kAzores                     = -1000,    //(GMT-01:00) Azores
    kCapeVerde                  = -1001,    //(GMT-01:00) Cape Verde Islands
    kUTC                        =     0,    //(UTC) Universal Coordinated Time
    kGMT                        =     1,    //(GMT) Greenwich Mean Time: Dublin, Edinburgh, Lisbon, London
    kMonrovia                   =     2,    //(GMT) Casablanca, Monrovia
    kBerlin                     =  1000,    //(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna
    kParis                      =  1001,    //(GMT+01:00) Brussels, Copenhagen, Madrid, Paris
    kPrague                     =  1002,    //(GMT+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague
    kSarajevo                   =  1003,    //(GMT+01:00) Sarajevo, Skopje, Warsaw, Zagreb
    kWestCentralAfrica          =  1004,    //(GMT+01:00) West Central Africa
    kAthens                     =  2000,    //(GMT+02:00) Athens, Beirut, Istanbul, Minsk
    kEasternEurope              =  2001,    //(GMT+02:00) Bucharest
    kCairo                      =  2002,    //(GMT+02:00) Cairo
    kHarare                     =  2003,    //(GMT+02:00) Harare, Pretoria
    kHelsinki                   =  2004,    //(GMT+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius
    kJerusalem                  =  2005,    //(GMT+02:00) Jerusalem
    kMoscow                     =  3000,    //(GMT+03:00) Moscow, St. Petersburg, Volgograd
    kRiyadh                     =  3001,    //(GMT+03:00) Kuwait, Riyadh
    kBaghdad                    =  3002,    //(GMT+03:00) Baghdad
    kEastAfrica                 =  3003,    //(GMT+03:00) Nairobi
    kTehran                     =  3300,    //(GMT+03:30) Tehran
    kAbuDhabi                   =  4000,    //(GMT+04:00) Abu Dhabi, Muscat
    kCaucasus                   =  4001,    //(GMT+04:00) Baku, Tbilisi, Yerevan
    kKabul                      =  4300,    //(GMT+04:30) Kabul
    kEkaterinburg               =  5000,    //(GMT+05:00) Ekaterinburg
    kIslamabad                  =  5001,    //(GMT+05:00) Islamabad, Karachi, Tashkent
    kKolkata                    =  5300,    //(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi
    kKathmandu                  =  5450,    //(GMT+05:45) Kathmandu
    kAlmaty                     =  6000,    //(GMT+06:00) Almaty, Novosibirsk
    kDhaka                      =  6001,    //(GMT+06:00) Astana, Dhaka
    kSriLanka                   =  6002,    //(GMT+06:00) Sri Jayawardenepura
    kRangoon                    =  6300,    //(GMT+06:30) Rangoon
    kBangkok                    =  7000,    //(GMT+07:00) Bangkok, Hanoi, Jakarta
    kKrasnoyarsk                =  7001,    //(GMT+07:00) Krasnoyarsk
    kBeijing                    =  8000,    //(GMT+08:00) Beijing, Chongqing, Hong Kong, Urumqi
    kSingapore                  =  8001,    //(GMT+08:00) Kuala Lumpur, Singapore
    kTaipei                     =  8002,    //(GMT+08:00) Taipei
    kIrkutsk                    =  8003,    //(GMT+08:00) Irkutsk, Ulaan Bataar
    kPerth                      =  8004,    //(GMT+08:00) Perth
    kTokyo                      =  9000,    //(GMT+09:00) Osaka, Sapporo, Tokyo
    kSeoul                      =  9001,    //(GMT+09:00) Seoul
    kYakutsk                    =  9002,    //(GMT+09:00) Yakutsk
    kAdelaide                   =  9300,    //(GMT+09:30) Adelaide
    kDarwin                     =  9301,    //(GMT+09:30) Darwin
    kSydney                     = 10000,    //(GMT+10:00) Canberra, Melbourne, Sydney
    kGuam                       = 10001,    //(GMT+10:00) Guam, Port Moresby
    kBrisbane                   = 10002,    //(GMT+10:00) Brisbane
    kHobart                     = 10003,    //(GMT+10:00) Hobart
    kVladivostock               = 10004,    //(GMT+10:00) Vladivostok
    kMagadan                    = 11000,    //(GMT+11:00) Magadan, Solomon Is., New Caledonia
    kWellington                 = 12000,    //(GMT+12:00) Auckland, Wellington
    kFiji                       = 12001,    //(GMT+12:00) Fiji, Kamchatka, Marshall Is.
    kTonga                      = 13000     //(GMT+13:00) Nuku'alofa
  };

  /** \details
    Specifies settings that affect the shape of the
    lofted solid or surface.
  */
  enum LoftParamType   
  {
    kLoftNoParam           = 0x00,
    kLoftNoTwist           = 0x01,
    kLoftAlignDirection    = 0x02,
    kLoftSimplify          = 0x04,
    kLoftClose             = 0x08,
    kLoftPeriodic          = 0x10,
    kLoftDefault           = kLoftNoTwist | kLoftAlignDirection | kLoftSimplify 
  };

  /** \details
    Specifies the normals of the lofted object where it
    passes through cross section curves.
  */
  enum LoftNormalsType  
  {
    kLoftRuled            = 0,
    kLoftSmooth           = 1,
    kLoftFirstNormal      = 2,
    kLoftLastNormal       = 3,
    kLoftEndsNormal       = 4,
    kLoftAllNormal        = 5,
    kLoftUseDraftAngles   = 6 
  };

    /** \details
      Specifies how the entity interacts with collision detection.
    */
    enum CollisionType     
    {
      kCollisionTypeNone  = 0,
      kCollisionTypeSolid = 1 
    };

  enum MeasurementValue
  {
    kEnglish = 0,  // English
    kMetric  = 1   // Metric
  };

  enum ProxyImage
  {
    kProxyNotShow	    = 0,
    kProxyShow	      = 1,
    kProxyBoundingBox	= 2
  };

  enum FilletTrimMode   
  { 
    kTrimNone      = 0,
    kTrimFirst     = 1,
    kTrimSecond    = 2,
    kTrimBoth      = 3 
  };

  /** \details
    Used in text file writing functions to force text file encoding.
  */
  enum TextFileEncoding
  {
    kTextFileEncodingDefault = 0, // Chooses current system encoding automaticly
    kTextFileEncodingANSI = 1, // Force ANSI character set encoding text writing
    kTextFileEncodingUTF8 = 11, // Use UTF8 encoding for write file
    kTextFileEncodingUTF16 = 21, // Chooses UTF16 endian automaticly
    kTextFileEncodingUTF16LE = 22, // UTF16 LittleEndian
    kTextFileEncodingUTF16BE = 23, // UTF16 BigEndian
    kTextFileEncodingUTF32 = 31, // Chooses UTF32 endian automaticly
    kTextFileEncodingUTF32LE = 32, // UTF32 LittleEndian
    kTextFileEncodingUTF32BE = 33 // UTF32 BigEndian
  };

  enum LayerAdskType
  {
    kLayerSystemLights = 0,
    kLayerConstraints  = 1
  };
}

/** \details
    This class represents a complete drawing file.

    \sa
    TD_Db

    <group OdDb_Classes>
*/
class TOOLKIT_EXPORT OdDbDatabase : public OdDbObject
{
protected:

  OdDbDatabase();

public:

  OdResult subGetClassID(
    void* pClsid) const;

  ODRX_DECLARE_MEMBERS(OdDbDatabase);

  void addRef();

  void release();

  /** \details
    Returns the OdDbHostAppServices object associated with this database object.
  */
  OdDbHostAppServices* appServices() const;

  /** \details
    Adds the default set of objects and settings to this database object.

    \remarks
    This includes the ModelSpace and PaperSpace blocks, default table entries, system variable settings, etc.

    \remarks
    measurement must be one the following:

    <table>
    Name              Value
    OdDb::kEnglish    0
    OdDb::kMetric     1
    </table>

    \param measurement [in]  Units of measurement.
  */
  void initialize(
    OdDb::MeasurementValue measurement = OdDb::kEnglish);

  virtual ~OdDbDatabase();

  /** \details
    Adds an object to this database object, and returns its Object ID.

    \param pObject [in]  Pointer to the object.
    \param ownerId [in]  Object ID of the owner of pObject.
    \param handle [in]  Handle for pObject

    \remarks
    A unique handle will be generated if handle is not supplied or is 0.

  */
  OdDbObjectId addOdDbObject(
    OdDbObject* pObject,
    OdDbObjectId ownerId = OdDbObjectId::kNull,
    OdDbHandle handle = 0);

  /** \details
    Adds the specified name to the APPID table of this database object.

    \param regAppName [in]  Application name.

    \remarks
    True if and only if the specified name had not been, and is now, in the APPID table.
  */
  bool newRegApp(
    const OdString& regAppName);

  /** \details
    Returns the Object ID of the BlockTable of this database object.
  */
  OdDbObjectId getBlockTableId() const;

  /** \details
    Returns the Object ID of the LayerTable of this database object.
  */
  OdDbObjectId getLayerTableId() const;

  /** \details
    Returns the Object ID of the TextStyleTable of this database object.
  */
  OdDbObjectId getTextStyleTableId() const;

  /** \details
    Returns the Object ID of the LinetypeTable of this database object.
  */
  OdDbObjectId getLinetypeTableId() const;

  /** \details
    Returns the Object ID of the ViewTable of this database object.
  */
  OdDbObjectId getViewTableId() const;

  /** \details
    Returns the Object ID of the UCSTable of this database object.
  */
  OdDbObjectId getUCSTableId() const;

  /** \details
    Returns the Object ID of the ViewportTable of this database object.
  */
  OdDbObjectId getViewportTableId() const;

  /** \details
    Returns the Object ID of the RegAppTable of this database object.
  */
  OdDbObjectId getRegAppTableId() const;

  /** \details
    Returns the Object ID of the DimStyleTable of this database object.
  */
  OdDbObjectId getDimStyleTableId() const;

  /** \details
    Returns the Object ID of the MLineStyle dictionary of this database object.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getMLStyleDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
    Returns the Object ID of the Group dictionary of this database object.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getGroupDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
    Returns the Object ID of the Layout dictionary of this database object.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getLayoutDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
    Returns the Object ID of the PlotStyleName dictionary of this database object.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getPlotStyleNameDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
    Returns the Object ID of the NamedObjects dictionary of this database object.
  */
  OdDbObjectId getNamedObjectsDictionaryId() const;

  /** \details
    Returns the Object ID of the PlotSettings dictionary of this database object.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getPlotSettingsDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
    Returns the Object ID of the Color dictionary of this database object.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getColorDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
    Returns the Object ID of the Material dictionary of this database object.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getMaterialDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
    Returns the Object ID of the VisualStyle dictionary of this database object.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getVisualStyleDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
    Returns the Object ID of the TableStyle dictionary of this database object.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getTableStyleDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
  Returns the Object ID of the ScaleList dictionary of this database object.
  \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getScaleListDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
    Returns the Object ID of the current TableStyle of this database object.
  */
  OdDbObjectId tablestyle() const;

  /** \details
    Sets the Object ID of the current TableStyle of this database object.
    \param objectId [in]  Object ID.
  */
  void setTablestyle(
    OdDbObjectId objectId);

  /** \details
    Returns the Object ID of the MLeaderStyle dictionary of this database object.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getMLeaderStyleDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
    Returns the Object ID of the current MLeaderStyle of this database object.
  */
  OdDbObjectId mleaderstyle() const;

  /** \details
    Sets the Object ID of the current MLeaderStyle of this database object.
    \param objectId [in]  Object ID.
  */
  void setMLeaderstyle(
    OdDbObjectId objectId);

  /** \details
    Returns the Object ID of the DetailViewStyle dictionary of this database object.
    \param createIfNotFound [in]  Determines whether to create a DetailViewStyle dictionary if it does not exist.
  */
  OdDbObjectId getDetailViewStyleDictionaryId(
    bool createIfNotFound = true) const;
   
  /** \details
    Returns the Object ID of the current DetailViewStyle of this database object.
  */
  OdDbObjectId detailViewStyle() const;

  /** \details
    Sets the Object ID of the current DetailViewStyle of this database object.
    \param objectId [in]  Object ID.
  */
  void setDetailViewStyle(
    OdDbObjectId objectId);

  /** \details
    Returns the Object ID of the SectionViewStyle dictionary of this database object.
    \param createIfNotFound [in]  Determines whether to create a SectionViewStyle dictionary if it does not exist.
  */
  OdDbObjectId getSectionViewStyleDictionaryId(
    bool createIfNotFound = true) const;

  /** \details
    Returns the Object ID of the current SectionViewStyle of this database object.
  */
  OdDbObjectId sectionViewStyle() const;

  /** \details
    Sets the Object ID of the current SectionViewStyle of this database object.
    \param objectId [in]  Object ID.
  */
  void setSectionViewStyle(
    OdDbObjectId objectId);

  /** \details
    Returns the Object ID of the "ACAD" RegApp object of this database object.
  */
  OdDbObjectId getRegAppAcadId() const;

  /** \details
    Returns the Object ID of the "Continuous" Linetype object of this database object.
  */
  OdDbObjectId getLinetypeContinuousId() const;

  /** \details
    Returns the Object ID of the "ByLayer" Linetype object of this database object.
  */
  OdDbObjectId getLinetypeByLayerId() const;

  /** \details
    Returns the Object ID of the "ByBlock" Linetype object of this database object.
  */
  OdDbObjectId getLinetypeByBlockId() const;

  /** \details
    Returns the Object ID of the ModelSpace Block object of this database object.
  */
  OdDbObjectId getModelSpaceId() const;

  /** \details
    Returns the Object ID of the PaperSpace Block object of this database object.
  */
  OdDbObjectId getPaperSpaceId() const;

  /** \details
    Returns the Object ID of the "Standard" text style object of this database object.
  */
  OdDbObjectId getTextStyleStandardId() const;

  /** \details
    Returns the Object ID of the "Standard" dimension style object of this database object.
  */
  OdDbObjectId getDimStyleStandardId() const;

  /** \details
    Returns the Object ID of the "0" layer object of this database object.
  */
  OdDbObjectId getLayerZeroId() const;

  /** \details
    Returns the Object ID of the "DEFPOINTS" layer object of this database object.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
  */
  OdDbObjectId getLayerDefpointsId(
    bool createIfNotFound = false) const;

  /** \details
    Returns the Object ID of the specified system layer object of this database object.
    \param layerType [in]  Type of system layer.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.

    \remarks
    layerType must be one of the following:

    <table>
    Name                        Value       Layer name
    OdDb::kLayerSystemLights    0           *ADSK_SYSTEM_LIGHTS
    OdDb::kLayerConstraints     1           *ADSK_CONSTRAINTS
    </table>
  */
  OdDbObjectId getLayerAdskId(
    OdDb::LayerAdskType layerType,
    bool createIfNotFound = false) const;

  /** \details
    Returns the object ID of the section manager.

    \remarks
    The ID may be null if the database
    has never contained any section planes.
  */
  OdDbObjectId getSectionManager() const;

  /** \details
    Returns the object ID of the point cloud dictionary.

    \remarks
    The ID may be null if the database
    has never contained any point clouds.
  */
  OdDbObjectId getPointCloudDictionaryId() const;

  /** \details
    Returns the Class DXF Name for the specified class.

    \param pClass [in]  Pointer to the class object.

    \sa
    OdRxClass
  */
  /*!DOM*/
  const OdString classDxfName(
    const OdRxClass* pClass);

  /** \details
    Returns the Object ID corresponding to the specified handle object in this database.

    \param objHandle [in]  Database handle.
    \param createIfNotFound [in]  Controls the creation of the object if it does not exist.
    \param xRefId [in]  Not used.

    \remarks
    If objHandle is 0, this function will create a new unique handle and return a
    newly created Object ID corresponding to this handle.

    \remarks
    createIfNotFound is for Teigha internal use only, and should always be false for Teigha applications.

    \sa
    OdDbObjectId
  */
  OdDbObjectId getOdDbObjectId(
    const OdDbHandle& objHandle,
    bool createIfNotFound = false,
    OdUInt32 xRefId = 0);

  /** \details
    Writes the contents of this database object to the specified StreamBuf object.

    \param pStreamBuf [in]  Pointer to the StreamBuf object to which the data are to be written.
    \param fileType [in]  File type.
    \param fileVersion [in]  File version.
    \param saveThumbnailImage [in]  Controls the saving of a thumbnail image to the file.
    \param dxfPrecision [in]  DXF file decimal digits precision. [0 .. 16].

    \remarks
    fileType must be one of the following:

    <table>
    Name          Value   Description
    OdDb::kDwg    0       .dwg file.
    OdDb::kDxf    1       .dxf file.
    OdDb::kDxb    2       Binary .dxf file.
    </table>

    fileVersion must be one of the following:

    <table>
    OdDb::kDwg?    Name          Value  Description
    n      OdDb::vAC09   11     Release 9
    n      OdDb::vAC10   13     Release 10
    y      OdDb::vAC12   16     Release 11-12
    y      OdDb::vAC13   19     Release 13
    y      OdDb::vAC14   21     Release 14
    y      OdDb::vAC15   23     2000-2002
    y      OdDb::vAC18   25     2004-2006
    y      OdDb::vAC21   27     2007-2009
    y      OdDb::vAC24   29     2010-2012
    y      OdDb::vAC27   31     2013-2016
    </table>

    Throws:
    OdError if the write is unsuccessful.
  */
  void writeFile(
    OdStreamBuf* pStreamBuf,
    OdDb::SaveType fileType,
    OdDb::DwgVersion fileVersion,
    bool saveThumbnailImage = false,
    int dxfPrecision = 16);

  /** \param filename [in]  Name of the file to which the data are to be written.
  */
  void writeFile(
    const OdString& filename,
    OdDb::SaveType fileType,
    OdDb::DwgVersion fileVersion,
    bool saveThumbnailImage = false,
    int dxfPrecision = 16)
  {
    closeInput();
    OdStreamBufPtr pFile = odSystemServices()->createFile(filename, Oda::kFileWrite, Oda::kShareDenyReadWrite, Oda::kCreateAlways);
    writeFile(pFile, fileType, fileVersion, saveThumbnailImage, dxfPrecision);
  }
  
  /**
  */
  void save(
    OdStreamBuf* pStreamBuf,
    bool saveThumbnailImage = false);

  /**
  */
  void save(
    const OdString& filename,        
    bool saveThumbnailImage = false);
  
  /** \details
    Reads the contents of the specified StreamBuf object or file into this database object.

    \param pStreamBuf [in]  Pointer to the StreamBuf object from which the data are to be read.
    \param partialLoad [in]  Controls the partial loading of .dwg files.
    \param pAuditInfo [in]  Pointer to an OdDbAuditInfo object.
    \param password [in]  Password for file.
    \param allowCPConversion [in]  If and only if true, allows code page conversion.

    \remarks
    Using this function in .tx code is not recommended.
    In .tx modules use OdDbHostAppServices::readFile() or OdDbHostAppServices::recoverFile() instead.
    This provides more control for host application which may override these virtual methods.

    The specified OdDbAuditInfo object controls the audit, and receives the audit status.

    If pAuditInfo is non-zero, a recover will be performed instead of a load.

    Throws:
    OdError if the read is unsuccessful.
  */
  void readFile(
    OdStreamBuf* pStreamBuf,
    bool partialLoad = false,
    OdDbAuditInfo *pAuditInfo = 0,
    const OdPassword& password = OdPassword(),
    bool allowCPConversion = false );

  /** \param filename [in]  Name of the file from which the data are to be read.
    \param shareMode [in]  Share mode to use when opening the specified file.

    \remarks
    shareMode must be one of the following:

    <table>
    Name                         Value       Description
    Oda::kShareDenyReadWrite     0x10        deny read/write mode
    Oda::kShareDenyWrite         0x20        deny write mode
    Oda::kShareDenyRead          0x30        deny read mode
    Oda::kShareDenyNo            0x40        deny none mode
    </table>

  */
  void readFile(
    const OdString& filename,
    bool partialLoad = false,
    Oda::FileShareMode shareMode = Oda::kShareDenyWrite,
    const OdPassword& password = OdPassword(),
    bool allowCPConversion = false )
  {
    readFile(odSystemServices()->createFile(filename, Oda::kFileRead, shareMode), partialLoad, 0, password, allowCPConversion);
  }

  /** \details
    Forces all data to be loaded from the input file associated with this database object, and closes the file.

    \remarks
    Client applications will normally not need to call this function.
  */
  /*!DOM*/
  void closeInput();

  /** \details
    Returns the approximate number of objects in this database object.

    \remarks
    This function returns value greater than or equal to the number of objects
    in this database object.

    This number is used to construct an OdDbObjectIdArray that can hold all the objects in the database, avoiding
    memory fragmentation and increasing performance.
  */
  OdInt32 approxNumObjects() const;

  /** \details
    Returns the version of this database object.

    \remarks
    The maintenance release version is returned if and only if pMaintReleaseVer != 0.

    \param pMaintReleaseVer [in]  Pointer to the MaintReleaseVer object to receive the maintenance release version.
  */
  OdDb::DwgVersion version(
    OdDb::MaintReleaseVer* pMaintReleaseVer = 0) const;

  /** \details
    Returns the number of times this database object has been saved since it was opened.

    \remarks
    This number may be zero.
  */
  OdInt32 numberOfSaves() const;

  /** \details
    Returns the version to which this database object was last saved.

    \param pMaintReleaseVer [in]  Pointer to the MaintReleaseVer object to receive the maintenance release version.

    \remarks
    The maintenance release version is returned if and only if pMaintReleaseVer != 0.
  */
  OdDb::DwgVersion lastSavedAsVersion(
    OdDb::MaintReleaseVer* pMaintReleaseVer = 0) const;

  /** \details
    Returns the FileType of the file from which this database object was read.

    \remarks
    originalFileType() returns one of the following:

    <table>
    Name         Value   Description
    OdDb::kDwg    0       .dwg file.
    OdDb::kDxf    0       .dxf file.
    </table>
  */
  OdDb::SaveType originalFileType() const;

  /** \details
    Returns the version of the file from which this database object was read.

    \param pMaintReleaseVer [in]  Pointer to the MaintReleaseVer object to receive the maintenance release version.

    \remarks
    The maintenance release version is returned if and only if pMaintReleaseVer != 0.
  */
  OdDb::DwgVersion originalFileVersion(
    OdDb::MaintReleaseVer* pMaintReleaseVer = 0) const;

  /** \details
    Returns the version of the application that created the file from which this database object was read.

    \param pMaintReleaseVer [in]  Pointer to the MaintReleaseVer object to receive the maintenance release version.

    \remarks
    The maintenance release version is returned if and only if pMaintReleaseVer != 0.
  */
  OdDb::DwgVersion originalFileSavedByVersion(
    OdDb::MaintReleaseVer* pMaintReleaseVer = 0) const;

   /** \details
    Adds the specified reactor to this object's reactor list.

    \param pReactor [in]  Pointer to the reactor object.
   */
  void addReactor(
    OdDbDatabaseReactor* pReactor) const;

  /** \details
    Removes the specified reactor from this object's reactor list.

    \param pReactor [in]  Pointer to the reactor object.
  */
  void removeReactor(OdDbDatabaseReactor*
    pReactor) const;

  /** \details
    Returns the R14 DIMFIT system variable of this database object.

    \remarks
    The DIMATFIT and DIMTMOVE values will be converted to DIMFIT. The complete mapping is as follows:

    <table>
    DIMTMOVE    DIMAFIT   DIMFIT
    0           0         0
    0           1         1
    0           2         2
    0           3         3
    1           0         4
    1           1         4
    1           2         4
    1           3         4
    2           0         5
    2           1         5
    2           2         5
    2           3         5
    </table>
  */
  int dimfit() const;

  /** \details
    Returns the R14 DIMUNIT system variable of this database object.

    \remarks
    The DIMLUNIT and DIMFRAC values will be converted to DIMUNIT. The complete mapping is as follows:

    <table>
    DIMFRAC   DIMLUNIT  DIMUNIT
    0         1         1
    0         2         2
    0         3         3
    0         4         4
    0         5         5
    0         6         8
    1         1         1
    1         2         2
    1         3         3
    1         4         4
    1         5         5
    1         6         8
    2         1         1
    2         2         2
    2         3         3
    2         4         6
    2         5         7
    2         6         8
    </table>

    All other input combinations return 2.
  */
  int dimunit() const;

  /** \details
    Sets the R14 DIMFIT system variable of this database object.

    \param val [in]  New value for DIMFIT. [0..5]

    \remarks
    The DIMFIT value will be converted to DIMATFIT and DIMTMOVE values. The complete mapping is as follows:
  */
  void setDimfit(
    int val);

  /** \details
    Sets the R14 DIMUNIT system variable of this database object.

    \param val [in]  New value for DIMUNIT. [1..7]

    \remarks
    The DIMUNIT value will be converted to DIMLUNIT and DIMFRAC values.
  */
  void setDimunit(
    int val);

  /** \details
    Deep clones a set of objects, and appends the clones to the specified in owner object.

    \param objectIds [in]  Array of Object IDs of the objects to be cloned.
    \param ownerId [in]  Object ID of owner object.
    \param idMap [in/out] ID map.
    \param deferXlation [in] Defer translation.

    \remarks
    idMap associates the Object IDs of the original objects with the Object IDs of the newly created clones.

    When cloning objects with different owner IDs, a separate call to this function is required for each
    ownerId. In this case, all but the final call to this function should be made with deferXlation == true.
    This defers translation until all objects have been cloned.

    A deep clone is a clone of the specified objects and everything they own.

    \sa
    wblockCloneObjects()

  */
  void deepCloneObjects(
    const OdDbObjectIdArray& objectIds,
    OdDbObjectId ownerId,
    OdDbIdMapping& idMap,
    bool deferXlation = false);


  /** \details
    Shallow clones a set of objects, and appends the clones to the specified in owner object.

    \param objectIds [in]  Array of Object IDs of the objects to be cloned.
    \param ownerId [in]  Object ID of the owner object.
    \param idMap [in/out] ID map.
    \param deferXlation [in] Defer translation.

    \remarks
    idMap associates the Object IDs of the original objects with the Object IDs of the newly created clones.

    When cloning objects with different owner IDs, a separate call to this function is required for each
    ownerId. In this case, all but the final call to this function should be made with deferXlation == true.
    This defers translation until all objects have been cloned.

    A shallow clone is a clone of the specified objects, but not what they own.

	duplicateRecordCloning must be one of the following:

    <table>
    Name                        Value   Description
    OdDb::kDrcNotApplicable     0       Not applicable to the object.
    OdDb::kDrcIgnore            1       If a duplicate record exists, use the existing record in the database, and ignore the clone.
    OdDb::kDrcReplace           2       If a duplicate record exists, replace it with the cloned record.
    OdDb::kDrcXrefMangleName    3       Incoming record names are mangled with <Xref>$0$<name>
    OdDb::kDrcMangleName        4       Incoming record names are mangled with $0$<name>
    OdDb::kDrcUnmangleName      5       Unmangle the names mangled by OdDb::kDrcMangleName, then default to OdDb::kDrcIgnore. Typically used by RefEdit when checking records into the original database.
    </table>

    \sa
    deepCloneObjects()

  */
  void wblockCloneObjects(
    const OdDbObjectIdArray& objectIds,
    OdDbObjectId ownerId,
    OdDbIdMapping& idMap,
    OdDb::DuplicateRecordCloning duplicateRecordCloning,
    bool deferXlation = false);

  /** \details
    Terminates a call to deepCloneObjects() or wblockCloneObjects().

    \param idMap [in/out] ID map of the function call to be terminated.

    \remarks
    idMap associates the Object IDs of the original objects with the Object IDs of the newly created clones.

    This call is necessary only when a call to deepCloneObjects() or wblockCloneObjects() is made with deferXlation == true,
    and no call will be made with deferXlation == false.
  */
  void abortDeepClone(
    OdDbIdMapping& idMap);

  /** \details
    Performs an audit operation on the header of this database object.

    \param pAuditInfo [in]  Pointer to an OdDbAuditInfo object.

    \remarks
    The specified OdDbAuditInfo object controls the audit, and receives the audit status.
  */
  void audit(
    OdDbAuditInfo* pAuditInfo);

  /** \details
    Starts a new transaction of this database object.
  */
  virtual void startTransaction();

  /** \details
    Ends the current transaction associated with this database object.

    \remarks
    This function commits all changes to objects since the start of the current transaction.
  */
  virtual void endTransaction();

  /** \details
    Aborts the current transaction associated with this database object.

    \remarks
    This function initiates an immediate rollback of all changes to objects
    since the start of the current transaction. This rollback occurs for
    all transaction-resident objects.
  */
  virtual void abortTransaction();

  /** \details
    Returns the number of active transactions associated with this database object.
  */
  virtual int numActiveTransactions();

  /** \details
    Adds a transaction reactor to this database object.

    \param reactor [in]  Transaction reactor.
  */
  virtual void addTransactionReactor(
    OdDbTransactionReactor* reactor);

  /** \details
    Removes the specified transaction reactor from this database object.

    \param reactor [in]  Transaction reactor.
  */
  virtual void removeTransactionReactor(
    OdDbTransactionReactor* reactor);


  /** \details
    Returns the thumbnail bitmap associated with this database object.

    \remarks
    The thumbnail is in Windows BITMAPINFO* format.

    \param dataLength [out]  Receives the data length of the thumbnail.
  */
  const void* thumbnailBitmap(
    OdUInt32& dataLength) const;

  /** \details
    Sets the thumbnail bitmap associated with this database object.

    \remarks
    The thumbnail is in Windows BITMAPINFO* format.

    \param dataLength [in]  Data length of the thumbnail.
    \param pBMPData [in]  Pointer to the bitmap data.
  */
  void setThumbnailBitmap(
    const void* pBMPData,
    OdUInt32 dataLength);

  /** \details
    Returns the state of the RetainOriginalThumbnailBitmap flag.
  */
  bool retainOriginalThumbnailBitmap() const;

  /** \details
    Controls the state of the RetainOriginalThumbnailBitmap flag.

    \param retain [in]  Retain if and only if true.
  */
  void setRetainOriginalThumbnailBitmap(
    bool retain);

  void dwgOutFields(
    OdDbDwgFiler* pFiler) const;

  OdResult dwgInFields(
    OdDbDwgFiler* pFiler);

#define VAR_DEF(type, name, def_value, metric_def_value, reserve1, reserve2)\
    SVARS_FUNC_MODIFIER type get##name() const;

#include "SysVarDefs.h"

#undef VAR_DEF
#undef RO_VAR_DEF

#define RO_VAR_DEF(type, name, def_value, metric_def_value, reserve1, reserve2)
#define VAR_DEF(type, name, def_value, metric_def_value, reserve1, reserve2)\
    SVARS_FUNC_MODIFIER void set##name(type val);

#include "SysVarDefs.h"

#undef RO_VAR_DEF
#undef VAR_DEF

#define VAR_DEF(type, name, dxf, def_value, metric_def_value, reserve1, reserve2)\
    virtual type dim##name() const;\
    virtual void setDim##name(type val);

#include "DimVarDefs.h"

#undef VAR_DEF

  /** \details
    Returns true if current Dimension Style is annotative
  */
  bool getDIMANNO() const;

  /** \details
    Returns the TDCREATE system variable of this database object.
    \remarks
    TDUCREATE represents the time and date, in Local Time, that the database was created.
  */
  OdDbDate getTDCREATE() const;

  /** \details
    Returns the TDUPDATE system variable of this database object.
    \remarks
    TDUUPDATE represents the time and date, in Local Time, that the database was last saved.
  */
  OdDbDate getTDUPDATE() const;

  /** \details
    Resets TDCREATE, TDUPDATE, TDINDWG and TDUSRTIMER system variables to the settings of the new drawing.
    \remarks
    TDCREATE and TDUPDATE will be set to the current system time and data. TDINDWG and TDUSRTIMER will be set to zero.
  */
  void resetTimes();

  /** \details
    Returns the specified system variable of this database object.

    \param name [in]  Name of the system variable.

    \returns
    Returns a SmartPointer to an OdResBuf object that contains the value.

    System variable names are case-insensitive strings.

    \sa
    System Variables
  */
  OdResBufPtr getSysVar(
    const OdString& name) const;

  /** \details
    Sets the specified system variable of this database object.

    \param name [in]  Name of the system variable.
    \param pValue [in]  Pointer to an OdResBuf object that contains the new value.

    \remarks
    System variable names are case-insensitive strings.

    \sa
    System Variables
  */
  void setSysVar(
    const OdString& name,
    const OdResBuf* pValue);

  /** \details
    Returns the next available handle number of this database object.
    \remarks
    Use OdDbHandle::getIntoAsciiBuffer() to return the hex string version of the handle.
  */
  OdDbHandle handseed() const;

  /** \details
    Copies the dimension variables of this database object to the
    specified DimStyleTableRecord.

    \param pDestination [in]  Pointer to the destination record.
  */
  void getDimstyleData(
    OdDbDimStyleTableRecord* pDestination) const;


  OdResult getDimstyleChildData(const OdRxClass *pDimClass,
                                OdDbDimStyleTableRecord* pRec,
                                OdDbObjectId &style) const;

  OdDbObjectId getDimstyleChildId(const OdRxClass *pDimClass,
                                  const OdDbObjectId &parentStyle) const;

  OdDbObjectId getDimstyleParentId(const OdDbObjectId &childStyle) const;


  /** \details
    Copies the dimension style data from the specified DimStyleTableRecord
    to the dimension variables of this database object.
    \param pSource [in]  Pointer to the source record.
  */
  void setDimstyleData(
    const OdDbDimStyleTableRecord* pSource);

  /** \details
    \param objectId [in]  Object ID of the source record.
  */
  void setDimstyleData(
    OdDbObjectId objectId);

  /** \details
    Loads a linetype into this database object.

    \param ltName [in] Name of the linetype to load.
    \param filename [in] Name of the linetype file from which to load.
    \param encode [in] Encoding used in linetype text file (if known).

    \remarks
    Wildcards are permitted in ltName; if "*" is specified, all LineTypes from filename will be loaded.

    dlt must be one of the following:

    <table>
    Name                      Value   Description
    OdDb::kDltNotApplicable     0       Not Applicable.
    OdDb::kDltIgnore            1       Ignore duplicate LineTypes.
    OdDb::kDltReplace           2       Replace duplicate LineTypes.
    </table>

    Throws:
    An appropriate error if not successful.
  */
  void loadLineTypeFile(
    const OdString& ltName,
    const OdString& filename,
    OdDb::DuplicateLinetypeLoading dlt = OdDb::kDltNotApplicable,
    OdDb::TextFileEncoding encode = OdDb::kTextFileEncodingDefault);

  /** \details
      Returns the name of the file associated with this database object.
  */
  virtual OdString getFilename() const;

  /** \details
    Removes the Object IDs from the specified array that are hard referenced
    in this database object.

    \remarks
    Any remaining Object IDs can be safely erased.

    \remarks
    This function does not modify this database object.

    \param objectIds [in/out] Array of Object IDs.
  */
  virtual void purge(
    OdDbObjectIdArray& objectIds) const;

  virtual OdResult purge(
    OdDbObjectIdGraph& objectIds) const;

  /** \details
      Returns a count of hard references to each of the specified Object IDs.

      \param objectIds [in]  Array of Object IDs.
      \param counts [out]  Receives an array of hard reference counts. 

      \remarks
      counts must be the same size as objectIds.

      Each element of counts will be incremented for each hard reference of the corresponding objectId
      The caller must initialize the elements of count to zero or an appropriate value
      before calling this function.
  */
  virtual void countHardReferences(
    const OdDbObjectIdArray& objectIds,
    OdUInt32* counts) const;

  /** \details
      Returns the Object ID of the active layout of this database object.
  */
  OdDbObjectId currentLayoutId() const;

  /** \details
    Sets the current layout of this database object.

    \param layoutName [in]  Name of the layout.
  */
  virtual void setCurrentLayout(
    const OdString& layoutName);

  /** \param layoutId [in]  Object ID of layout.
  */
  virtual void setCurrentLayout(
    const OdDbObjectId& layoutId);

  /** \details
      Returns the name of the active layout of this database object.

      \param allowModel [in]  If and only if true, allows the ModelSpace layout name to be returned.

      \remarks
      If allowModel == false, an empty string will be returned if the active layout is ModelSpace.
  */
  virtual OdString findActiveLayout(
    bool allowModel) const;

  /** \details
    Returns the Object ID of the BlockTableRecord associated with the active
    layout of this database object.
  */
  virtual OdDbObjectId getActiveLayoutBTRId() const;

  /** \details
    Returns the Object ID of the specified layout in this database object.

    \param layoutName [in]  Layout name.

    \remarks
    Returns a null Object ID if the specified layout is not found.
  */
  virtual OdDbObjectId findLayoutNamed(
    const OdString& layoutName) const;

  /** \details
    Deletes the specified layout from this database object.

    \param layoutName [in]  Layout name.
  */
  virtual void deleteLayout(
    const OdString& layoutName);

  /** \details
    Creates a new layout with the specified name in this database object.

    \remarks
    The newly created layout is added to this database object along with its
    newly created, associated OdDbBlockTableRecord.

    \param layoutName [in]  Layout name.
    \param pBlockTableRecId [in]  Pointer to the BlockTableRecID to receive the Object ID of the new BlockTableRecord object.
  */
  virtual OdDbObjectId createLayout(
    const OdString& layoutName,
    OdDbObjectId* pBlockTableRecId = 0);

  /** \details
    Returns the number of layouts in this database object.
  */
  virtual int countLayouts() const;

  /** \details
    Renames the specified layout in this database object.
    \param oldName [in]  Old name.
    \param newName [in]  New name.
  */
  virtual void renameLayout(
    const OdString& oldName,
    const OdString& newName);

  /** \details
      Starts undo recording of this database object.
  */
  void startUndoRecord();

  /** \details
      Returns true if undo information exists of this database object.
  */
  bool hasUndo() const;

  /** \details
    Performs an undo operation on this database object.

    \remarks
    All operations performed since the
    last call to startUndoRecording will be undone.

    At least 1 undo step is always supported by Teigha for internal needs.
  */
  void undo();

  void blockUndoRecording(bool bBegin);

  bool isUndoBlockStarted();

  void setUndoMark();

  bool hasUndoMark() const;

  void undoBack();

  int getUNDOMARKS() const;

  void clearUndo();

  /** \details
    Returns true if redo information exists of this database object.
  */
  bool hasRedo() const;

  /** \details
    Performs a redo operation on this database object.

    \remarks
    Restores operations undone by the last undo.
  */
  void redo();

  /** \details
    Performs an audit operation on the this entire database object.

    \param pAuditInfo [in]  Pointer to an AuditInfo object.

    \remarks
    The specified AuditInfo object controls the audit, and receives the audit status.
  */
  void auditDatabase(
    OdDbAuditInfo *pAuditInfo);

  void applyPartialUndo(
    OdDbDwgFiler* pUndoFiler,
    OdRxClass* pClassObj);

  OdDbDwgFiler* undoFiler();

  /** \details
    Copies contents of pSource database to this database.  

    \param sourceBlockName [in]  Name of the OdDbBlockTableRecord in pSource.
    \param destinationBlockName [in]  Name for the new OdDbBlockTableRecord.
    \param xfm [in]  Transformation matrix.
    \param pSource [in]  Source database.
    \param preserveSourceDatabase [in]  Isn't used. Objects from the source database are always copied, not moved.
    
    \remarks
    The data that will be copied includes contents of the BlockTable, LayerTable, LinetypeTable, DimStyleTable, RegAppTable,
	TextStyleTable, PlotStyleNameDictionary, PlotSettingsDictionary, MLStyleDictionary, ColorDictionary, MaterialDictionary, 
	TableStyleDictionary, MLeaderStyleDictionary, ScaleListDictionary, VisualStyleDictionary, GroupDictionary. 
	Unlike wblockCloneObjects() method, insert() also copies the sortens table. 
	While copying duplicate objects, names will be ignored; a destination object that has the same name as the source object will be rewritten by the source one.

	The arguments that are passed to the method define the specifics of copying the layout block contents. 

	If destinationBlockName is specified and sourceBlockName is not specified, a new OdDbBlockTableRecord is created and all model space entities of pSource 
	are copied into the new block.
	
	If both destinationBlockName and sourceBlockName are specified, all entities in the BlockTableRecord specified by sourceBlockName in pSource
    are copied into the new OdDbBlockTableRecord and offset by the INSBASE value. Contents of layout blocks are copied only if sourceBlockName is a layout block name. 

    If xfm is specified, all model space entities in pSource are copied into model space of this database object and transformed by xfm.
  */
  OdDbObjectId insert(
    const OdString& destinationBlockName,
    OdDbDatabase* pSource,
    bool preserveSourceDatabase = true);

  OdDbObjectId insert(
    const OdString& sourceBlockName,
    const OdString& destinationBlockName,
    OdDbDatabase* pSource,
    bool preserveSourceDatabase = true);

  void insert(
    const OdGeMatrix3d& xfm,
    OdDbDatabase* pSource,
    bool preserveSourceDatabase = true);

  /** \details
     Clones specified objects of this database to a new database.

     \param outObjIds [in]  Array of Object IDs to be cloned.
     \param basePoint [in]  WCS base point for the insertion.
     \param blockId [in]  Object ID of the BlockTableRecord to be cloned.

     \remarks
     The method creates a new OdDbDatabase object, populates it with specified objects from this database using wblockClone() method and returns a SmartPointer 
	 to the new database. If called with no arguments, the entire contents of this database are cloned. All objects are copied with their dependencies: 
	 for example, if a text entity uses a specific textstyle, it will be copied as well; line entities will be copied with their linestyles, etc.
  */
  OdDbDatabasePtr wblock(
    const OdDbObjectIdArray& outObjIds,
    const OdGePoint3d& basePoint);

  OdDbDatabasePtr wblock(
    OdDbObjectId blockId);

  OdDbDatabasePtr wblock();

 /** \details
    As implemented, this function does nothing but return a null SmartPointer.
    It will be fully implemented in a future release.
 */
  OdDbObjectPtr subWblockClone(OdDbIdMapping& ownerIdMap, OdDbObject*) const;

  /** \details
    Sets the security parameters of this database object.

    \param secParams [in]  Security params.
    \param setDbMod [in]  If and only if true, the DBMOD variable will be set if the security settings modified.

    \remarks
    As implemented, this function ignores setDbMod, and never modifies the DBMOD variable.
    It will be fully implemented in a future release.
  */
  void setSecurityParams(
    const OdSecurityParams& secParams,
    bool setDbMod = true);

  /** \details
    Returns the security parameters of this database object.
    \param secParams [in]  Security params.
    \remarks
    Returns true if and only if secParams.nFlags!=0 && secParams.password is not empty.
  */
  bool securityParams(
    OdSecurityParams& secParams) const;
  /** \details
    Returns the OdFileDependencyManager object of this database object.
  */
  OdFileDependencyManagerPtr fileDependencyManager() const;
  /** \details
  Returns the OdDbObjectContextManager object of this database object.
  */
  OdDbObjectContextManagerPtr objectContextManager() const;

  OdDbLayerStateManager* getLayerStateManager() const;

  /** \details
    Recalculates the extents of this database object.
    \param bExact [in]  Enables more slow but exact extents calculation algorithm.
  */
  void updateExt(bool bExact = false);

  /** \details
      Returns true if and only if this database object was created by an educational version of the application.
  */
  bool isEMR() const;
  /** \details
    Returns the Object ID of the OdDbBlockTableRecord that references this database object as an Xref.
  */
  OdDbObjectId xrefBlockId() const;

  /** \details
    Returns true if and only if this database object is partially opened.
  */
  bool isPartiallyOpened() const;

  /** \details
    Returns true if and only if this database object is being loaded from file.
  */
  bool isDatabaseLoading() const;

  /** \details
    Returns true if and only if this database object is being converted after loading from file or before saving to file.
  */
  bool isDatabaseConverting() const;

  /** \details
    Returns pointer to OdDbAuditInfo if database is being loaded from file in Recover mode else returns Null.
  */
  OdDbAuditInfo* auditInfo() const;

  /** \details
    Sets the current UCS of this database object.

    \param origin [in]  The WCS origin of the UCS.
    \param xAxis [in]  The WCS X-axis of the UCS.
    \param yAxis [in]  The WCS Y-axis of the UCS.
    \param viewType [in]  Orthographic view type.
    \param ucsId [in]  Object ID of the UCS.

    \remarks
    viewType must be one of the following:

    <table>
    Name                  Value   View type
    OdDb::kNonOrthoView   0       Non-orthographic with respect to the UCS
    OdDb::kTopView        1       Top view with respect to the UCS
    OdDb::kBottomView     2       Bottom view with respect to the UCS
    OdDb::kFrontView      3       Front view with respect to the UCS
    OdDb::kBackView       4       Back view with respect to the UCS
    OdDb::kLeftView       5       Left view with respect to the UCS
    OdDb::kRightView      6       Right view with respect to the UCS
    </table>
  */
  void setCurrentUCS(
    OdDb::OrthographicView viewType);
  void setCurrentUCS(
    const OdDbObjectId& ucsId);
  void setCurrentUCS(
    const OdGePoint3d& origin,
    const OdGeVector3d& xAxis,
    const OdGeVector3d& yAxis);

  /** \details
    Returns the UCS origin and orthographic view type for the current UCS.

    \param viewType [in]  Orthographic view type.

    \remarks
    viewType must be one of the following:

    <table>
    Name            Value   View type
    OdDb::kNonOrthoView   0       Non-orthographic with respect to the UCS
    OdDb::kTopView        1       Top view with respect to the UCS
    OdDb::kBottomView     2       Bottom view with respect to the UCS
    OdDb::kFrontView      3       Front view with respect to the UCS
    OdDb::kBackView       4       Back view with respect to the UCS
    OdDb::kLeftView       5       Left view with respect to the UCS
    OdDb::kRightView      6       Right view with respect to the UCS
    </table>
  */
  OdGePoint3d getUCSBASEORG(
    OdDb::OrthographicView viewType) const;

  /** \details
    Sets the UCS origin and orthographic view type for the current UCS.

    \param origin [in]  The WCS origin of the UCS.
    \param viewType [in]  Orthographic view type.

    \remarks
    viewType must be one of the following:

    <table>
    Name            Value   View type
    OdDb::kNonOrthoView   0       Non-orthographic with respect to the UCS
    OdDb::kTopView        1       Top view with respect to the UCS
    OdDb::kBottomView     2       Bottom view with respect to the UCS
    OdDb::kFrontView      3       Front view with respect to the UCS
    OdDb::kBackView       4       Back view with respect to the UCS
    OdDb::kLeftView       5       Left view with respect to the UCS
    OdDb::kRightView      6       Right view with respect to the UCS
    </table>
  */
  void setUCSBASEORG(
    OdDb::OrthographicView viewType,
    const OdGePoint3d& origin);

  /** \details
    Returns the UCS origin and orthographic view type for the current PaperSpace UCS.

    \param viewType [in]  Orthographic view type.

    \remarks
    viewType must be one of the following:

    <table>
    Name                  Value   View type
    OdDb::kNonOrthoView   0       Non-orthographic with respect to the UCS
    OdDb::kTopView        1       Top view with respect to the UCS
    OdDb::kBottomView     2       Bottom view with respect to the UCS
    OdDb::kFrontView      3       Front view with respect to the UCS
    OdDb::kBackView       4       Back view with respect to the UCS
    OdDb::kLeftView       5       Left view with respect to the UCS
    OdDb::kRightView      6       Right view with respect to the UCS
    </table>
  */
   OdGePoint3d getPUCSBASEORG(
    OdDb::OrthographicView viewType) const;

 /** \details
    Sets the UCS origin and orthographic view type for the current PaperSpace UCS.

    \param origin [in]  The WCS origin of the UCS.
    \param viewType [in]  Orthographic view type.

    \remarks
    viewType must be one of the following:

    <table>
    Name                  Value   View type
    OdDb::kNonOrthoView   0       Non-orthographic with respect to the UCS
    OdDb::kTopView        1       Top view with respect to the UCS
    OdDb::kBottomView     2       Bottom view with respect to the UCS
    OdDb::kFrontView      3       Front view with respect to the UCS
    OdDb::kBackView       4       Back view with respect to the UCS
    OdDb::kLeftView       5       Left view with respect to the UCS
    OdDb::kRightView      6       Right view with respect to the UCS
    </table>
 */
  void setPUCSBASEORG(
    OdDb::OrthographicView viewType,
    const OdGePoint3d& origin);

  /** \details
  Returns the database to its original state (when the xref was first read) by undoing
  any modifications.
  \remarks
  This function should be used with restoreForwardingXrefSymbols when reading or
  writing any xref object. 
  */
  void restoreOriginalXrefSymbols();

  /** \details
  Returns the database to its modified state (before any original state restoration) by
  redoing modifications.
  \remarks
  This function should be used with restoreOriginalXrefSymbols when reading or
  writing any xref object. The database requires write-access to symbol tables and 
  records.
  */
  void restoreForwardingXrefSymbols();

  /** \details
    Requires a lineweight and returns True if the specified lineweight is valid, that is, a one of 
    predefined lineweights in this database, or False otherwise.

    \param weight [in]  The lineweight value.
  */
  static bool isValidLineWeight(int weight);

  /** \details
    Requires a lineweight and returns the nearest OdDb::LineWeight enum number for this database. For example, 
    when the passed value is 8, the returned value is kLnWt009.

    \param weight [in]  The lineweight value.
  */
  static OdDb::LineWeight getNearestLineWeight(int weight);

  /*
  bool plotStyleMode() const;


  void forceWblockDatabaseCopy();

  void auditXData(OdDbAuditInfo* pInfo);

  OdDbUndoController* undoController() const;

  void setDimblk(const OdChar*);
  void setDimblk1(const OdChar*);
  void setDimblk2(const OdChar*);
  void setDimldrblk(const OdChar*);

  void getDimstyleChildData(const OdRxClass *pDimClass,
                            OdDbDimStyleTableRecordPtr& pRec,
                            OdDbObjectId &style) const;

  OdDbObjectId getDimstyleChildId(const OdRxClass *pDimClass,
                                  OdDbObjectId &parentStyle) const;

  OdDbObjectId getDimstyleParentId(OdDbObjectId &childStyle) const;

  void getDimRecentStyleList(OdDbObjectIdArray& objIds) const;

  void applyPartialOpenFilters(const OdDbSpatialFilter* pSpatialFilter,
                               const OdDbLayerFilter* pLayerFilter);

  void disablePartialOpen();

  void newFingerprintGuid();

  void newVersionGuid();

  double viewportScaleDefault() const;

  void setViewportScaleDefault(double newDefaultVPScale);

  OdDbObjectId getPaperSpaceVportId() const;

  virtual void copyLayout(const OdChar* copyname, const OdChar* newname);

  virtual void cloneLayout(const OdDbLayout* pLBTR, const OdChar* newname, int newTabOrder);

  virtual OdDbObjectId getNonRectVPIdFromClipId(const OdDbObjectId& clipId);

  virtual bool isViewportClipped(short index);

  */

  OdDbObjectId byLayerMaterialId() const;
  OdDbObjectId byBlockMaterialId() const;
  OdDbObjectId globalMaterialId() const;

  OdDbObjectId activeViewportId() const;

  virtual OdDbUnitsFormatter& formatter();

  void enableGraphicsFlush(bool bEnable);
  void flushGraphics();

// ODA_MT_DB_BEGIN
  bool isMultiThreadedMode() const;
  OdDb::MultiThreadedMode multiThreadedMode() const;
  virtual void setMultiThreadedMode(OdDb::MultiThreadedMode);
// ODA_MT_DB_END
  
  void setCannoscale(OdDbAnnotationScale* val);
  OdDbAnnotationScalePtr cannoscale() const;

  OdDbHistoryManager* historyManager();

private:
  friend class OdDbDatabaseImpl;
  OdDbDatabaseImpl* m_pImpl;
};

TOOLKIT_EXPORT OdUInt32 getGapsAmount(OdDbDatabase* pDb);
TOOLKIT_EXPORT OdRxClass* getClassByName(OdDbDatabase* pDb, const OdString& className);

/** Fills in OdThumbnailImage object from stream. Throws appropriate exception if an error occurred.
*/

TOOLKIT_EXPORT void odDbGetPreviewBitmap(OdStreamBuf* pStreamBuf, OdThumbnailImage* pPreview);


// The functions below provide write access to "Read-Only" *database* variables.
// Actually they are a OdDb::kludge for bypassing non-implemented DD functionality
// or to repair invalid drawings.
// They should be used with care.

/** \details
    Sets the DWGCODEPAGE value in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for DWGCODEPAGE.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetDWGCODEPAGE(
  OdDbDatabase& db,
  OdCodePageId val);

/** \details
    Sets the TDUCREATE system variable in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for TDUCREATE.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetTDUCREATE(
  OdDbDatabase& db,
  OdDbDate val);

/** \details
    Sets the TDUUPDATE value in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for TDUUPDATE.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetTDUUPDATE(
  OdDbDatabase& db,
  OdDbDate val);

/** \details
    Sets the TDINDWG value in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for TDINDWG.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetTDINDWG(
  OdDbDatabase& db,
  OdDbDate val);

/** \details
    Sets the TDUSRTIMER value in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for TDUSRTIMER.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetTDUSRTIMER(
  OdDbDatabase& db,
  OdDbDate val);

/** \details
    Sets the PSTYLEMODE value in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for PSTYLEMODE.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetPSTYLEMODE(
  OdDbDatabase& db,
  bool val);

/** \details
    Sets the UCSORG system variable in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for UCSORG.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetUCSORG(
  OdDbDatabase& db,
  OdGePoint3d val);

/** \details
    Sets the UCSXDIR value in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for UCSXDIR.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetUCSXDIR(
  OdDbDatabase& db,
  OdGeVector3d val);

/** \details
    Sets the UCSYDIR value in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for UCSYDIR.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetUCSYDIR(
  OdDbDatabase& db,
  OdGeVector3d val);

/** \details
    Sets the PUCSORG value in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for PUCSORG.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetPUCSORG(
  OdDbDatabase& db,
  OdGePoint3d val);

/** \details
    Sets the PUCSXDIR value in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for PUCSXDIR.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetPUCSXDIR(
  OdDbDatabase& db,
  OdGeVector3d val);

/** \details
    Sets the PUCSYDIR value in the specified OdDbDatabase instance.

    \param db [in]  Database.
    \param val [in]  Value for PUCSYDIR.

    \remarks
    This function provides write access to "Read-Only" system variables.
    It may be used to bypass non-implemented Teigha functionality or to repair drawings,
    and should be used with care.
*/
TOOLKIT_EXPORT void odDbSetPUCSYDIR(
  OdDbDatabase& db,
  OdGeVector3d val);


 /** \details
    Saves an line types to the specified file.
    \param db [in]  OdDbDatabase object.
    \param filename [in]  Name of the line type file to save.
		\param encode [in]  Encoding used for writing linetype text file.
 */
extern TOOLKIT_EXPORT void odDbSaveLineTypeFile(OdDbDatabase &Db, OdStreamBuf &filename, OdDb::TextFileEncoding encode = OdDb::kTextFileEncodingDefault);

 /** \details
    Loads an MLineStyle into the specified database object.

    \param db [in]  OdDbDatabase object.
    \param patternName [in]  Name of the pattern to load.
    \param filename [in]  Name of the MlineStyle file from which to load.
    \param dlt [in]  Duplicate linetype loading.
    \remarks
    dlt must be one of the following:

    <table>
    Name                        Value   Description
    OdDb::kDltNotApplicable     0       Not Applicable.
    OdDb::kDltIgnore            1       Ignore duplicate LineTypes.
    OdDb::kDltReplace           2       Replace duplicate LineTypes.
    </table>

    Throws:
    An appropriate error if not successful.
 */
extern TOOLKIT_EXPORT void odDbLoadMlineStyleFile(
  OdDbDatabase &db,
  const OdString& patternName,
    OdStreamBuf &filename,
  OdDb::DuplicateLinetypeLoading dlt = OdDb::kDltNotApplicable );

 /** \details
    Saves an MLineStyle to the specified file.
    \param db [in]  OdDbDatabase object.
    \param filename [in]  Name of the MlineStyle file to save.
 */
extern TOOLKIT_EXPORT void odDbSaveMlineStyleFile(OdDbDatabase &Db, OdStreamBuf &filename );

namespace OdDb
{
  TOOLKIT_EXPORT const char* DwgVersionToStr(DwgVersion ver);
  TOOLKIT_EXPORT DwgVersion DwgVersionFromStr(const char* str);
}


#include "TD_PackPop.h"

#endif /* _ODDBDATABASE_INCLUDED_ */


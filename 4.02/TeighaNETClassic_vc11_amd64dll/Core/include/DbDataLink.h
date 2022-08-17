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



#ifndef OD_DBDATALINK_H
#define OD_DBDATALINK_H

#include "DbObject.h"
#include "OdValue.h"
#include "StringArray.h"

#include "TD_PackPush.h"

namespace OdDb
{
  enum DataLinkOption
  { 
    kDataLinkOptionNone                         = 0,
    kDataLinkOptionAnonymous                    = 0x1,
    kDataLinkOptionPersistCache                 = 0x2,
    kDataLinkOptionDisableInLongTransaction     = 0x4,
    kDataLinkHasCustomData                      = 0x8
  };

  enum PathOption
  { 
    kPathOptionNone = 1,
    kPathOptionRelative,
    kPathOptionAbsolute,
    kPathOptionPathAndFile
  };

  enum UpdateDirection   
  { 
    kUpdateDirectionSourceToData   = 0x1,
    kUpdateDirectionDataToSource   = 0x2
  };

  enum UpdateOption 
  { 
    kUpdateOptionNone                                  = 0,
    kUpdateOptionSkipFormat                            = 0x20000,
    kUpdateOptionUpdateRowHeight                       = 0x40000,
    kUpdateOptionUpdateColumnWidth                     = 0x80000,
    kUpdateOptionAllowSourceUpdate                     = 0x100000,
    kUpdateOptionForceFullSourceUpdate                 = 0x200000,
    kUpdateOptionOverwriteContentModifiedAfterUpdate   = 0x400000,
    kUpdateOptionOverwriteFormatModifiedAfterUpdate    = 0x800000,
    kUpdateOptionForPreview                            = 0x1000000,
    kUpdateOptionIncludeXrefs                          = 0x2000000,
    kUpdateOptionSkipFormatAfterFirstUpdate            = 0x4000000
  };

  enum DataLinkGetSourceContext
  { 
    kDataLinkGetSourceContextUnknown,
    kDataLinkGetSourceContextEtransmit,
    kDataLinkGetSourceContextXrefManager,
    kDataLinkGetSourceContextFileWatcher,
    kDataLinkGetSourceContextOther
  };

  enum TableFillOption   
  { 
    kTableFillOptionNone                     = 0,
    kTableFillOptionRow                      = 0x1,
    kTableFillOptionReverse                  = 0x2,
    kTableFillOptionGenerateSeries           = 0x4,
    kTableFillOptionCopyContent              = 0x8,
    kTableFillOptionCopyFormat               = 0x10,
    kTableFillOptionOverwriteReadOnlyContent = 0x20,
    kTableFillOptionOverwriteReadOnlyFormat  = 0x40
  };

  enum TableCopyOption   
  { 
    kTableCopyOptionNone                                 = 0,
    kTableCopyOptionExpandOrContractTable                = 0x1,
    kTableCopyOptionSkipContent                          = 0x2,
    kTableCopyOptionSkipValue                            = 0x4,
    kTableCopyOptionSkipField                            = 0x8,
    kTableCopyOptionSkipFormula                          = 0x10,
    kTableCopyOptionSkipBlock                            = 0x20,
    kTableCopyOptionSkipDataLink                         = 0x40,
    kTableCopyOptionSkipLabelCell                        = 0x80,
    kTableCopyOptionSkipDataCell                         = 0x100,
    kTableCopyOptionSkipFormat                           = 0x200,
    kTableCopyOptionSkipCellStyle                        = 0x400,
    kTableCopyOptionConvertFormatToOverrides             = 0x800,
    kTableCopyOptionSkipCellState                        = 0x1000,
    kTableCopyOptionSkipContentFormat                    = 0x2000,
    kTableCopyOptionSkipDissimilarContentFormat          = 0x4000,
    kTableCopyOptionSkipGeometry                         = 0x8000,
    kTableCopyOptionSkipMerges                           = 0x10000,
    kTableCopyOptionFillTarget                           = 0x20000,
    kTableCopyOptionOverwriteReadOnlyContent             = 0x40000,
    kTableCopyOptionOverwriteReadOnlyFormat              = 0x80000,
    kTableCopyOptionOverwriteContentModifiedAfterUpdate  = 0x100000,
    kTableCopyOptionOverwriteFormatModifiedAfterUpdate   = 0x200000,
    kTableCopyOptionOnlyContentModifiedAfterUpdate       = 0x400000,
    kTableCopyOptionOnlyFormatModifiedAfterUpdate        = 0x800000,
    kTableCopyOptionRowHeight                            = 0x1000000,
    kTableCopyOptionColumnWidth                          = 0x2000000,
    kTableCopyOptionFullCellState                        = 0x4000000,
    kTableCopyOptionForRountrip                          = 0x8000000,  
    kTableCopyOptionConvertFieldToValue                  = 0x10000000, 
    kTableCopyOptionSkipFieldTranslation                 = 0x20000000
  };

};

/** \details
    This class links a table to data in another file.

    \sa
    TD_Db
    
    <group OdDb_Classes> 

*/
class TOOLKIT_EXPORT OdDbDataLink : public OdDbObject
{
public:
  ODDB_DECLARE_MEMBERS(OdDbDataLink);

  OdDbDataLink(void);
  
  OdString name (void) const;
  void setName (const OdString& sName);
  OdString description (void) const;
  void setDescription (const OdString& sDescription);
  OdString getToolTip (void) const;
  void setToolTip (const OdString& sToolTip);
  OdString  dataAdapterId (void) const;
  void setDataAdapterId (const OdString& sAdapterId);
  OdString  connectionString (void) const;
  void setConnectionString(const OdString& sConnectionString);
  OdDb::DataLinkOption option (void) const;
  void setOption (OdDb::DataLinkOption nOption);
  OdInt32 updateOption (void) const;
  void setUpdateOption(OdInt32 nOption);
  // void update (OdDb::UpdateDirection nDir, 
  //  OdDb::UpdateOption nOption);
  void getUpdateStatus(OdDb::UpdateDirection* pDir, 
    OdTimeStamp* pTime, 
    OdString* pErrMessage) const;
  void setUpdateStatus(const OdString& sErrMessage);
  bool isValid (void) const;
  OdInt32 getTargets (OdDbObjectIdArray& targetIds) const;
  void getSourceFiles(OdDb::DataLinkGetSourceContext nContext, 
    OdStringArray& files) const;
  // void repathSourceFiles(OdString& sBasePath, 
  //   OdDb::PathOption nOption);
  OdValue getCustomData(const OdString& sKey) const;
  void setCustomData(const OdString& sKey, 
    const OdValue* pData);

  virtual OdResult dwgInFields(
    OdDbDwgFiler* pFiler);
  virtual void dwgOutFields(
    OdDbDwgFiler* pFiler) const;
  virtual OdResult dxfInFields(
    OdDbDxfFiler* pFiler);
  virtual void dxfOutFields(
    OdDbDxfFiler* pFiler) const;
};

typedef OdArray<OdDbDataLink*> OdDbDataLinkArray;

typedef OdSmartPtr<OdDbDataLink> OdDbDataLinkPtr;

#include "TD_PackPop.h"

#endif // OD_DBDATALINK_H

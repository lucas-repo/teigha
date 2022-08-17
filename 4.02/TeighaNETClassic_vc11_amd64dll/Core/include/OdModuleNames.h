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

#ifndef MODULE_NAMES_DEFINED
#define MODULE_NAMES_DEFINED

#define OdDbRootModuleName       L"TD_DbRoot.dll"

#define OdGsModuleName           L"TD_Gs.dll"
#define OdGiModuleName           L"TD_Gi.dll"
#define OdDbModuleName           L"TD_Db.dll"

#define OdWinGDIModuleName       L"WinGDI.txv"
#define OdWinOpenGLModuleName    L"WinOpenGL.txv"
#define OdWinDirectXModuleName   L"WinDirectX.txv"
#define OdWinBitmapModuleName    L"WinBitmap.txv"
#define OdWinGLES2ModuleName     L"WinGLES2.txv"

#define OdDynBlocksModuleName    L"TD_DynBlocks"
#define OdAveModuleName          L"TD_Ave"

#define OdDbCommandsModuleName   L"DbCommands"

#define OdDwf7ExportModuleName   L"TD_Dwf7Export"

#define OdDwf7ImportModuleName    L"TD_Dwf7Import"
#define OdPdfImportVIModuleName   L"TD_PdfImportVI"
#define OdDgnImportModuleName     L"TD_DgnImport"
#define OdColladaImportModuleName L"TD_ColladaImport"
#define OdPdfExportModuleName     L"TD_PdfExport"
#define OdSvgExportModuleName     L"TD_SvgExport"
#define OdSTLExportModuleName     L"TD_STLExport"
#define OdColladaExportModuleName L"TD_ColladaExport"
#define OdRasterExportModuleName  L"TD_RasterExport"

#define OdSmModuleName           L"TD_Sm"
#define OdDgnUnderlayModuleName  L"TD_DgnUnderlay"
#define OdDwfUnderlayModuleName  L"TD_DwfUnderlay"
#define OdDwfDbModuleName        L"TDwfDb"
#define OdSpatialIndexModuleName L"TD_SpatialIndex.dll"
#define OdUndoHistory            L"TD_UndoHistory"

#define OdFTFontEngineModuleName L"TD_FtFontEngine"
#define OdPdfModuleVIModuleName  L"PdfModuleVI"

#define ODPS_PLOTSTYLE_SERVICES_APPNAME L"PlotStyleServices"
#define RX_RASTER_SERVICES_APPNAME (L"RxRasterServices")

#define DbConstraintsModuleName           L"DbConstraints"
#define SynergyObjDPWModuleName           L"AcSynergyObjDPW"
#define OdModelerGeometryModuleName       L"ModelerGeometry"
#define OdRecomputeDimBlockModuleName     L"RecomputeDimBlock"
#define OdExFieldEvaluatorModuleName      L"ExFieldEvaluator"
#define OdRasterProcessorModuleName       L"RasterProcessor"
#define OdOleItemHandlerModuleName        L"OdOleItemHandler"
#define OdOleSsItemHandlerModuleName      L"OdOleSsItemHandler"
#define OdExDeepCloningModuleName         L"ExDeepCloning"
#define OdExDynamicBlocksModuleName       L"ExDynamicBlocks"
#define OdExEvalWatcherModuleName         L"ExEvalWatchers"
#define OdGripPointsModuleName            L"GripPoints"
#define OdModelerCommandsModuleName       L"ModelerCommands"
#define OdCurveFunctionsModuleName        L"OdCurveFunctions"
#define OdOverrulingSampleModuleName      L"OverrulingSample"
#define OdAsdkSmileyDbModuleName          L"AsdkSmileyDb"
#define OdAeciIbModuleName                L"AECIIB"
#define OdAutoSurfServicesModuleName      L"AutoSurfServices"
#define OdAcIdViewObjModuleName           L"AcIdViewObj"
#define OdPlotSettingsValidatorModuleName L"PlotSettingsValidator"
#define OdThreadPoolModuleName            L"ThreadPool"
#define OdLspModuleName                   L"OdLsp"
#define OdSpaModelerModuleName            L"SpaModeler"
#define OdC3dModelerModuleName            L"C3dModeler"
#define OdExCustObjsModuleName            L"ExCustObjs"
#define OdExCommandsModuleName            L"ExCommands"
#define OdOpenCadTxModuleName             L"OpenCadTx"
#define Od3DSolidHistoryTxModuleName      L"TD_3DSolidHistory"
#define OdDgnLSModuleName                 L"AcDgnLS"
#define DbPointCloudObjModuleName         L"AcDbPointCloudObj"
#define OdModelDocObjModuleName           L"AcModelDocObj"
#define OdJoinEntityPEModuleName          L"TD_DbJoinEntityPE"
#define ExDimAssocModuleName              L"ExDimAssoc"
#define OdGeolocationObjModuleName        L"AcGeolocationObj"
#define OdGeoDataModuleName               L"OdGeoData"
#define OdGeoMapPEModuleName              L"OdDbGeoMapPE"
#define OdGeoCommandsModuleName           L"GeoCommands"
//#define OdDwfExportModuleName             L"OdDwfExportModule"
//#define OdDwfExportModuleName             L"TD_Dwf7Export"

#define RX_RCS_FILE_SERVICES              L"RcsFileServices"

#define TfCoreModuleName                  L"TD_TfCore"
#define TfModuleName                 	  L"TD_Tf"

#define OdQtOpenGLModuleName              L"OdaQtOpenGL.txv"
#define OdQtGLES2ModuleName               L"OdaQtGLES2.txv"

#define ODDB_HOST_APP_SERVICES L"OdDbHostAppServices"
#define ODRP_RASTERPROCESSING_SERVICES_APPNAME L"RasterProcessingServices"


#if defined(ODA_WINDOWS) && !defined(_WINRT)
#  define OdOlePlatformItemHandlerModuleName OdOleItemHandlerModuleName
#else
#  define OdOlePlatformItemHandlerModuleName OdOleSsItemHandlerModuleName
#endif

#endif

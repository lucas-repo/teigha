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

////////////////////////////////////////////////////////////
// Error codes definition container

//        Code                Message string
TF_ERROR_DEF( eOk,                OD_T("No error"))
TF_ERROR_DEF( eNotImplementedYet, OD_T("Not implemented yet"))
TF_ERROR_DEF( eNotApplicable,     OD_T("Not applicable"))
TF_ERROR_DEF( eInvalidInput,      OD_T("Invalid input"))
TF_ERROR_DEF( eInvalidFiler,      OD_T("Invalid filer"))
TF_ERROR_DEF( eAmbiguousInput,    OD_T("Ambiguous input"))
TF_ERROR_DEF( eAmbiguousOutput,   OD_T("Ambiguous output"))
TF_ERROR_DEF( eOutOfMemory,       OD_T("Out of memory"))
TF_ERROR_DEF( eNoInterface,       OD_T("No interface"))
TF_ERROR_DEF( eBufferTooSmall,    OD_T("Buffer is too small"))
TF_ERROR_DEF( eInvalidOpenState,  OD_T("Invalid open state"))
TF_ERROR_DEF( eUnsupportedMethod, OD_T("Unsupported method"))
TF_ERROR_DEF( eDuplicateHandle,   OD_T("Handle already exists"))
TF_ERROR_DEF( eNullHandle,        OD_T("Null handle"))
TF_ERROR_DEF( eBrokenHandle,      OD_T("Broken handle"))
TF_ERROR_DEF( eUnknownHandle,     OD_T("Unknown handle"))
TF_ERROR_DEF( eHandleInUse,       OD_T("Handle in use"))
TF_ERROR_DEF( eNullObjectPointer, OD_T("Null object pointer"))
TF_ERROR_DEF( eNullObjectId,      OD_T("Null object Id"))
TF_ERROR_DEF( eNullBlockName,     OD_T("Null Block name"))
TF_ERROR_DEF( eContainerNotEmpty, OD_T("Container is not empty"))
TF_ERROR_DEF( eNullEntityPointer, OD_T("Null entity pointer"))
TF_ERROR_DEF( eIllegalEntityType, OD_T("Illegal entity type"))
TF_ERROR_DEF( eKeyNotFound,       OD_T("Key not found"))
TF_ERROR_DEF( eDuplicateKey,      OD_T("Duplicate key"))
TF_ERROR_DEF( eInvalidIndex,      OD_T("Invalid index"))
TF_ERROR_DEF( eCharacterNotFound, OD_T("Character not found"))
TF_ERROR_DEF( eDuplicateIndex,    OD_T("Duplicate index"))
TF_ERROR_DEF( eAlreadyInDb,       OD_T("Already in Database"))
TF_ERROR_DEF( eOutOfDisk,         OD_T("Out of disk"))
TF_ERROR_DEF( eDeletedEntry,      OD_T("Deleted entry"))
TF_ERROR_DEF( eInvalidExtents,    OD_T("Invalid extents"))
TF_ERROR_DEF( eInvalidKey,        OD_T("Invalid key"))
TF_ERROR_DEF( eWrongObjectType,   OD_T("Wrong object type"))
TF_ERROR_DEF( eWrongDatabase,     OD_T("Wrong Database"))
TF_ERROR_DEF( eEndOfObject,       OD_T("End of oject"))
TF_ERROR_DEF( eEndOfFile,         OD_T("Unexpected end of file"))
TF_ERROR_DEF( eCantOpenFile,      OD_T("Can't open file"))
TF_ERROR_DEF( eFileExists,        OD_T("File exists"))
TF_ERROR_DEF( eFileCloseError,    OD_T("File close error"))
TF_ERROR_DEF( eFileWriteError,    OD_T("File write error"))
TF_ERROR_DEF( eFileAccessErr,     OD_T("File access error"))
TF_ERROR_DEF( eFileSystemErr,     OD_T("File system error"))
TF_ERROR_DEF( eFileInternalErr,   OD_T("File internal error"))
TF_ERROR_DEF( eUnknownFileType,   OD_T("Unknown file type"))
TF_ERROR_DEF( eFilerError,        OD_T("Filer error"))
TF_ERROR_DEF( eFileNotFound,      OD_T("File not found"))
TF_ERROR_DEF( eNoInputFiler,      OD_T("No input filer"))
TF_ERROR_DEF( eWasErased,         OD_T("Object was erased"))
TF_ERROR_DEF( ePermanentlyErased, OD_T("Object was permanently erased"))
TF_ERROR_DEF( eAtMaxReaders,      OD_T("At max readers"))
TF_ERROR_DEF( eIsWriteProtected,  OD_T("Is write protected"))
TF_ERROR_DEF( eNotAnEntity,       OD_T("An object in entitiesToMove is not an entity"))
TF_ERROR_DEF( eIteratorDone,      OD_T("Iterator done"))
TF_ERROR_DEF( eNullIterator,      OD_T("Null iterator"))
TF_ERROR_DEF( eOutOfRange,        OD_T("Out of range"))
TF_ERROR_DEF( eStringTooLong,     OD_T("String too long"))
TF_ERROR_DEF( eRecoveryFailed,    OD_T("Recovery failed"))
TF_ERROR_DEF( eInvalidFix,        OD_T("Invalid fix"))
TF_ERROR_DEF( eOutOfPagerMemory,  OD_T("Out of pager memory"))
TF_ERROR_DEF( eHandleExists,      OD_T("Handle exists"))
TF_ERROR_DEF( eNullPtr,           OD_T("Null Ptr"))
TF_ERROR_DEF( eLoadFailed,        OD_T("Load failed"))
TF_ERROR_DEF( eInvalidTypeInfo,   OD_T("Invalid type info"))
TF_ERROR_DEF( eInvalidPropertyInfo,   OD_T("Invalid property info"))
TF_ERROR_DEF( ePropertyNotSetYet, OD_T("Property not set yet"))
TF_ERROR_DEF( eNotOpenForRead,    OD_T("Not opened for read"))
TF_ERROR_DEF( eNotOpenForWrite,   OD_T("Not opened for write"))
TF_ERROR_DEF( eNotThatKindOfClass, OD_T("Not that kind of class"))
TF_ERROR_DEF( eObjectImproperlyRead, OD_T("Object improperly read"))
#define eNotImplemented eNotImplementedYet
#define eCannotBeErased eCannotBeErasedByCaller

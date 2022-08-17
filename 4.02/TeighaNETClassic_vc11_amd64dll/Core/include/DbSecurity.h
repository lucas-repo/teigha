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

#ifndef _ODDBSECURITY_INCLUDED_
#define _ODDBSECURITY_INCLUDED_

#include "TD_PackPush.h"

#include "RxObject.h"
#include "OdArray.h"
#include "DbExport.h"
#include "OdString.h"

enum
{
  SECURITYPARAMS_ENCRYPT_DATA     = 0x00000001,
  SECURITYPARAMS_ENCRYPT_PROPS    = 0x00000002,

  SECURITYPARAMS_SIGN_DATA        = 0x00000010,
  SECURITYPARAMS_ADD_TIMESTAMP    = 0x00000020,

  SECURITYPARAMS_ALGID_RC4        = 0x00006801
};


typedef OdString OdPassword;

/** \details
    This class represents the security parameters used by OdCrypt classes.
    
    Corresponding C++ library: TD_Db
    <group Other_Classes>
*/
class OdSecurityParams
{
public:
  OdSecurityParams()
    : nFlags(0)
    , nProvType(0)
    , nAlgId (SECURITYPARAMS_ALGID_RC4)
    , nKeyLength(40)
  {}

  OdUInt32    nFlags;
  OdPassword  password;
  OdUInt32    nProvType;
  OdString   provName;
  OdUInt32    nAlgId;
  OdUInt32    nKeyLength;

  OdString   sCertSubject;
  OdString   sCertIssuer;
  OdString   sCertSerialNum;
  OdString   sComment;
  OdString   sTimeServer;

};

/** \details
    This class defines the interface for the 
    encription/decription of byte data.
    
    Corresponding C++ library: TD_Db
    <group Other_Classes>
*/
class TOOLKIT_EXPORT OdCrypt : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdCrypt);

  /** \details
    Initializes this OdCrypt object with the specified security parameters.
    \param securityParams [in]  Security parameters. 
    \returns
    Returns true if and only if successful.
  */
  virtual bool initialize(const OdSecurityParams& securityParams) = 0;
  /** \details
    Encrypts the specified buffer.
    
    \param bufferSize [in]  Number of bytes.
    \param buffer [in/out] Data to be encrypted.
    \returns
    Returns true if and only if successful.
  */
  virtual bool encryptData(OdUInt8* buffer, OdUInt32 bufferSize) = 0;

  /** \details
    Decrypts the specified buffer.
    
    \param bufferSize [in]  Number of bytes.
    \param buffer [in/out] Data to be decrypted.
    \returns
    Returns true if and only if successful.
  */
  virtual bool decryptData(OdUInt8* buffer, OdUInt32 bufferSize) = 0;
};

/** \details
    This template class is a specialization of the OdSmartPtr class for OdCrypt object pointers.
*/
typedef OdSmartPtr<OdCrypt> OdCryptPtr;

/** \details
    This class implements Iterator objects that traverse entries in OdPwdCache objects.

    <group Other_Classes>
*/
class TOOLKIT_EXPORT OdPwdIterator : public OdRxObject
{
public:
  /** \details
    Returns true if and only if the traversal by this Iterator object is complete.
  */
    virtual bool done() const = 0;
  /** \details
    Sets this Iterator object to reference the entry following the current entry.
  */
  virtual void next() = 0;
  /** \details
    Returns the Password object pointed to by this Iterator object. 

    \param password [out]  Receives the Password object.
  */
  virtual void get(OdPassword& password) const = 0;
};

/** \details
    This template class is a specialization of the OdSmartPtr class for OdPwdIterator object pointers.
*/
typedef OdSmartPtr<OdPwdIterator> OdPwdIteratorPtr;

/** \details
  This class implements and manages a Password Cache.
    <group Other_Classes>
*/
class TOOLKIT_EXPORT OdPwdCache : public OdRxObject
{
public:
  /** \details
    Adds the specified Password object to this Cache object.

    \param password [in]  Password object.
  */
  virtual void add(const OdPassword& password) = 0;
  /** \details
    Returns an Iterator object that can be 
    used to traverse the OdPassword objects in this Stack object.
  */
  virtual OdPwdIteratorPtr newIterator() = 0;
};

/** \details
    This template class is a specialization of the OdSmartPtr class for OdPwdCache object pointers.
*/
typedef OdSmartPtr<OdPwdCache> OdPwdCachePtr;

#include "TD_PackPop.h"

#endif  // _ODDBSECURITY_INCLUDED_

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

#include "PromptResults.h"
#include "SelectionSet.h"

namespace Teigha
{
  namespace EditorInput
  {
    PromptResult::PromptResult( PromptStatus status, String^ result )
    {
      m_status       = status;
      m_stringResult = result;
    }

    String^ PromptResult::ToString()
    {
      return ToString( nullptr );
    }

    String^ PromptResult::ToString(IFormatProvider^ provider)
    {
      array< Object^ >^ args = { m_status, m_stringResult };
      return String::Format( provider, "({0},{1})", args );
    }

    void PromptResult::SetStatus( PromptStatus value )
    {
      m_status = value;
    }

    void PromptResult::SetStringResult( String^ value )
    {
      m_stringResult = value;
    }

    void PromptResult::SetValue( Object^ value )
    {
      m_stringResult = (String^) value;
    }

    //
    PromptDoubleResult::PromptDoubleResult( PromptStatus status, String^ result, double value )
      : PromptResult( status, result )
    {
      m_value = value;
    }

    void PromptDoubleResult::SetValue( Object^ value )
    {
      m_value = (double) value;
    }

    String^ PromptDoubleResult::ToString()
    {
      return ToString( nullptr );
    }

    String^ PromptDoubleResult::ToString( IFormatProvider^ provider )
    {
      array< Object^ >^ args = {PromptResult::ToString( provider ), m_value };
      return String::Format( provider, "({0},{1})", args );
    }

    //
    PromptPointResult::PromptPointResult( PromptStatus status, String^ result, Geometry::Point3d value )
      : PromptResult( status, result )
    {
      m_value = value;
    }

    void PromptPointResult::SetValue( Object^ value )
    {
      m_value = (Geometry::Point3d) value;
    }

    String^ PromptPointResult::ToString()
    {
      return ToString( nullptr );
    }

    String^ PromptPointResult::ToString( IFormatProvider^ provider )
    {
      array< Object^ >^ args = gcnew array<Object^> { PromptResult::ToString(provider), m_value };
      return String::Format(provider, "({0},{1})", args);
    }

    Geometry::Point3d PromptPointResult::Value::get()
    {
      return m_value;
    }

    PromptSelectionResult::PromptSelectionResult( PromptStatus retcode, OdDbSelectionSetPtr pSSet )
    {
      m_status = retcode;
      m_value = gcnew SelectionSetDelayMarshalled( pSSet );
    }

    String^ PromptSelectionResult::ToString()
    {
      return ToString( nullptr );
    }

    String^ PromptSelectionResult::ToString( IFormatProvider^ provider )
    {
      array< Object^ >^ args = { m_status, m_value };
      return String::Format( provider, "({0},{1})", args );
    }

    PromptStatus PromptSelectionResult::Status::get()
    {
      return m_status;
    }

    SelectionSet^ PromptSelectionResult::Value::get()
    {
      return m_value;
    }

    PromptEntityResult::PromptEntityResult( PromptStatus status, String^ result, DatabaseServices::ObjectId value, Geometry::Point3d pickPoint )
      : PromptResult( status, result )
    {
        m_pickPoint = pickPoint;
        m_value = value;
    }

    String^ PromptEntityResult::ToString()
    {
      return ToString( nullptr );
    }

    String^ PromptEntityResult::ToString( IFormatProvider^ provider )
    {
      array< Object^ >^ args = gcnew array< Object^ > { PromptResult::ToString( provider ), m_value, m_pickPoint };
      return String::Format( provider, "({0},{1},{2})", args );
    }

    DatabaseServices::ObjectId PromptEntityResult::ObjectId::get()
    {
      return m_value;
    }

    Geometry::Point3d PromptEntityResult::PickedPoint::get()
    {
      return m_pickPoint;
    }

    PromptFileNameResult::PromptFileNameResult( PromptStatus eStatus, String^ strFile, bool readOnly )
      : PromptResult( eStatus, strFile )
    {
      m_readOnly = readOnly;
    }

    String^ PromptFileNameResult::ToString()
    {
      return ToString( nullptr );
    }

    String^ PromptFileNameResult::ToString( IFormatProvider^ )
    {
      return nullptr;
    }

    PromptIntegerResult::PromptIntegerResult( PromptStatus status, String^ result, int value )
      : PromptResult( status, result )
    {
      m_value = value;
    }

    String^ PromptIntegerResult::ToString()
    {
      return ToString( nullptr );
    }

    String^ PromptIntegerResult::ToString( IFormatProvider^ provider )
    {
      array< Object^ >^ args = { PromptResult::ToString( provider ), m_value };
      return String::Format(provider, "({0},{1})", args);
    }
  }
}

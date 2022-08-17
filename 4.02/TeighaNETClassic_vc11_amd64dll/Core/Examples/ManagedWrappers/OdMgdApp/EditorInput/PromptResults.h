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

#pragma once
#pragma warning(disable:4793)

#include "../stdafx.h"

namespace Teigha
{
  namespace EditorInput
  {
    public enum class PromptStatus
    {
      Cancel    = -5002,
      Error     = -5001,
      Keyword   = -5005,
      Modeless  = 5027,
      None      = 5000,
      OK        = 5100,
      Other     = 5028
    };

    public ref class PromptResult
    {
      internal:
        //PromptResult( PromptStatus retcode, const wchar_t* stringResult );

        PromptResult( PromptStatus status, String^ result );

        //PromptResult( int retcode, String^ stringResult );

        void SetStatus( PromptStatus value );

        void SetStringResult( String^ value );

        virtual void SetValue( Object^ value );

      public:
        virtual String^ ToString() override;

        String^ ToString(IFormatProvider^ provider);

        property PromptStatus Status { PromptStatus get(){ return m_status; } }

        property String^ StringResult { String^ get() { return m_stringResult; } }

      private:
        PromptStatus m_status;
        String^ m_stringResult;
    };

    public ref class PromptDoubleResult sealed : public PromptResult
    {
      internal:
        //PromptDoubleResult( PromptStatus retcode, double value, const wchar_t* stringResult );

        PromptDoubleResult( PromptStatus status, String^ result, double value );

        //PromptDoubleResult( int retcode, String^ stringResult, double value );

        virtual void SetValue( Object^ value ) sealed override;

      public:
        virtual String^ ToString() sealed override;

        String^ ToString( IFormatProvider^ provider ) new;

        property double Value
        { double get() { return m_value; } }

      private:
        double m_value;
    };

    public ref class PromptPointResult sealed : public PromptResult
    {
      internal:
        //PromptPointResult( PromptStatus retcode, const OdGePoint3d* pt, const wchar_t* stringResult );

        PromptPointResult( PromptStatus status, String^ result, Geometry::Point3d value );

        //PromptPointResult( int retcode, String^ stringResult, const const double* pt );

        virtual void SetValue( Object^ value ) sealed override;

      public:
        virtual String^ ToString() sealed override;

        String^ ToString( IFormatProvider^ provider ) new;

        DECLARE_PROP_GET( Geometry::Point3d, Value );
        
      private:
        Geometry::Point3d m_value;
    };

    ref class SelectionSet;
    public ref class PromptSelectionResult sealed
    {
      internal:
        PromptSelectionResult( PromptStatus retcode, OdDbSelectionSetPtr pSSet );

      public:
        virtual String^ ToString() sealed override;

        String^ ToString(IFormatProvider^ provider);

        DECLARE_PROP_GET( PromptStatus, Status );

        DECLARE_PROP_GET( SelectionSet^, Value );

      private:
        PromptStatus m_status;
        SelectionSet^ m_value;
    };

    public ref class PromptEntityResult : public PromptResult
    {
      internal:
        PromptEntityResult( PromptStatus status, String^ result, DatabaseServices::ObjectId value, Geometry::Point3d pickPoint );

      public:
        virtual String^ ToString() override;

        String^ ToString( IFormatProvider^ provider ) new;
        
        DECLARE_PROP_GET( DatabaseServices::ObjectId, ObjectId );

        DECLARE_PROP_GET( Geometry::Point3d, PickedPoint );

      private:
        Geometry::Point3d m_pickPoint;
        DatabaseServices::ObjectId m_value;
    };

    public ref class PromptFileNameResult : PromptResult
    {
      internal:
        PromptFileNameResult( PromptStatus eStatus, String^ strFile, bool readOnly );

      public:
        virtual String^ ToString() override;

        String^ ToString( IFormatProvider^ provider ) new;

        property bool ReadOnly
        { bool get(){ return m_readOnly; } }
    
      private:
        array< String^ >^ m_files;
        bool m_readOnly;
    };

    public ref class PromptIntegerResult sealed : public PromptResult
    {
      internal:
        PromptIntegerResult( PromptStatus status, String^ result, int value );
        
      public:
        virtual String^ ToString() sealed override;
        
        String^ ToString( IFormatProvider^ provider ) new;

        property int Value { int get(){ return m_value; } }

      private:
        int m_value;
    };
  }
}

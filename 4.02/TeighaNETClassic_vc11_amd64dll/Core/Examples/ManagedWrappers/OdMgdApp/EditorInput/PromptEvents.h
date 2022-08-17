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
    ref class PromptPointOptions;
    ref class PromptDistanceOptions;
    ref class PromptAngleOptions;
    ref class PromptPointResult;
    ref class PromptDoubleResult;
    ref class PromptStringOptions;
    ref class PromptStringResult;
    ref class PromptIntegerResult;
    ref class PromptEntityResult;
    ref class PromptDoubleOptions;
    ref class PromptIntegerOptions;
    ref class PromptEntityOptions;
    ref class PromptKeywordOptions;
    ref class PromptNestedEntityOptions;
    ref class PromptNestedEntityResult;
    ref class PromptSelectionOptions;
    ref class PromptSelectionResult;

    public ref class PromptPointOptionsEventArgs : public EventArgs
    {
      internal:
        PromptPointOptionsEventArgs( PromptPointOptions^ options ) : m_options( options ){}

      public:
        property PromptPointOptions^ Options { PromptPointOptions^ get(){ return m_options; } }

      private:
        PromptPointOptions^ m_options;
    };

    public ref class PromptDistanceOptionsEventArgs : public EventArgs
    {
      internal:
        PromptDistanceOptionsEventArgs( PromptDistanceOptions^ options ) : m_options( options ){}

      public:
        property PromptDistanceOptions^ Options { PromptDistanceOptions^ get(){ return m_options; } }

      private:
        PromptDistanceOptions^ m_options;
    };

    public ref class PromptAngleOptionsEventArgs : public EventArgs
    {
      internal:
        PromptAngleOptionsEventArgs( PromptAngleOptions^ options ) : m_options( options ){}

      public:
        property PromptAngleOptions^ Options { PromptAngleOptions^ get(){ return m_options; } }

      private:
        PromptAngleOptions^ m_options;
    };

    public ref class PromptPointResultEventArgs : public EventArgs
    {
      internal:
        PromptPointResultEventArgs( PromptPointResult^ result ) : m_result( result ){}

      public:
        property PromptPointResult^ Result { PromptPointResult^ get(){ return m_result; } }

      private:
        PromptPointResult^ m_result;
    };

    public ref class PromptDoubleResultEventArgs : public EventArgs
    {
      internal:
        PromptDoubleResultEventArgs( PromptDoubleResult^ result ) : m_result( result ){}

      public:
        property PromptDoubleResult^ Result { PromptDoubleResult^ get(){ return m_result; } }

      private:
        PromptDoubleResult^ m_result;
    };

    public ref class PromptStringOptionsEventArgs : public EventArgs
    {
      internal:
        PromptStringOptionsEventArgs( PromptStringOptions^ options ) : m_options( options ){}

      public:
        property PromptStringOptions^ Options { PromptStringOptions^ get(){ return m_options; } }

      private:
        PromptStringOptions^ m_options;
    };

    ref class PromptResult;
    public ref class PromptStringResultEventArgs : public EventArgs
    {
      internal:
        PromptStringResultEventArgs( PromptResult^ result ) : m_result( result ){}

      public:
        property PromptResult^ Result { PromptResult^ get(){ return m_result; } }

      private:
        PromptResult^ m_result;
    };

    public ref class PromptIntegerResultEventArgs : public EventArgs
    {
      internal:
        PromptIntegerResultEventArgs( PromptIntegerResult^ result ) : m_result( result ){}

      public:
        property PromptIntegerResult^ Result { PromptIntegerResult^ get(){ return m_result; } }

      private:
        PromptIntegerResult^ m_result;
    };

    public ref class PromptEntityResultEventArgs : public EventArgs
    {
      internal:
        PromptEntityResultEventArgs( PromptEntityResult^ result ) : m_result( result ){}

      public:
        property PromptEntityResult^ Result { PromptEntityResult^ get(){ return m_result; } }

      private:
        PromptEntityResult^ m_result;
    };

    public ref class PromptDoubleOptionsEventArgs : public EventArgs
    {
      internal:
        PromptDoubleOptionsEventArgs( PromptDoubleOptions^ options ) : m_options( options ){}

      public:
        property PromptDoubleOptions^ Options { PromptDoubleOptions^ get(){ return m_options; } }

      private:
        PromptDoubleOptions^ m_options;
    };

    public ref class PromptIntegerOptionsEventArgs : public EventArgs
    {
      internal:
        PromptIntegerOptionsEventArgs( PromptIntegerOptions^ options ) : m_options( options ){}

      public:
        property PromptIntegerOptions^ Options { PromptIntegerOptions^ get(){ return m_options; } }

      private:
        PromptIntegerOptions^ m_options;
    };

    public ref class PromptEntityOptionsEventArgs : public EventArgs
    {
      internal:
        PromptEntityOptionsEventArgs( PromptEntityOptions^ options ) : m_options( options ){}

      public:
        property PromptEntityOptions^ Options { PromptEntityOptions^ get(){ return m_options; } }

      private:
        PromptEntityOptions^ m_options;
    };

    public ref class PromptKeywordOptionsEventArgs : public EventArgs
    {
      internal:
        PromptKeywordOptionsEventArgs( PromptKeywordOptions^ options ) : m_options( options ){}

      public:
        property PromptKeywordOptions^ Options { PromptKeywordOptions^ get(){ return m_options; } }

      private:
        PromptKeywordOptions^ m_options;
    };

    public ref class PromptNestedEntityOptionsEventArgs : public EventArgs
    {
      internal:
        PromptNestedEntityOptionsEventArgs( PromptNestedEntityOptions^ options ) : m_options( options ){}

      public:
        property PromptNestedEntityOptions^ Options { PromptNestedEntityOptions^ get(){ return m_options; } }

      private:
        PromptNestedEntityOptions^ m_options;
    };

    public ref class PromptNestedEntityResultEventArgs : public EventArgs
    {
      internal:
        PromptNestedEntityResultEventArgs( PromptNestedEntityResult^ result ) : m_result( result ){}

      public:
        property PromptNestedEntityResult^ Result { PromptNestedEntityResult^ get(){ return m_result; } }

      private:
        PromptNestedEntityResult^ m_result;
    };

    public ref class PromptSelectionOptionsEventArgs : public EventArgs
    {
      internal:
        PromptSelectionOptionsEventArgs( PromptSelectionOptions^ options ) : m_options( options ){}

      public:
        property PromptSelectionOptions^ Options { PromptSelectionOptions^ get(){ return m_options; } }

      private:
        PromptSelectionOptions^ m_options;
    };

    public ref class PromptSelectionResultEventArgs : public EventArgs
    {
      internal:
        PromptSelectionResultEventArgs( PromptSelectionResult^ result ) : m_result( result ){}

      public:
        property PromptSelectionResult^ Result { PromptSelectionResult^ get(){ return m_result; } }

      private:
        PromptSelectionResult^ m_result;
    };

    public ref class SelectionTextInputEventArgs sealed : public EventArgs
    {
      internal:
        SelectionTextInputEventArgs( const wchar_t* input )
        {
          m_input = gcnew String( input );
        }

      public:
        void AddObjects( array< DatabaseServices::ObjectId >^ ids )
        {
          if ( m_msg != nullptr )
            throw gcnew InvalidOperationException();

          m_ids = ids;
        }

        void SetErrorMessage( String^ errorMessage )
        {
          if ( m_ids != nullptr )
            throw gcnew InvalidOperationException();

          m_msg = errorMessage;
        }

        property String^ Input
        {
          String^ get() { return  m_input; }
        }

      private:
        array< DatabaseServices::ObjectId >^ m_ids;
        String^ m_input;
        String^ m_msg;
    };

    public ref class SelectionAddedEventArgs : public EventArgs{}; // TODO:

    public ref class SelectionRemovedEventArgs : public EventArgs{};  // TODO:

    public ref class PromptForSelectionEndingEventArgs : public EventArgs{};

    public ref class PromptForEntityEndingEventArgs : public EventArgs{};

    public ref class RolloverEventArgs : public EventArgs{};

    public ref class DraggingEventArgs : public EventArgs{};

    public ref class DraggingEndedEventArgs : public EventArgs{};
/*
public ref class PointFilterEventArgs sealed : public IDisposable
{
    // Methods
    internal:
    PointFilterEventArgs(ProcessInputParams* pInputParams, FilterResultParams* pResultParams, AcMgPointFilter* pFilter)
    {
    }
    private:
    void ~PointFilterEventArgs()
    {
    }
    public:
    virtual void Dispose() sealed override
    {
    }
    protected:
    virtual void Dispose([MarshalAs(::U1)]bool )
    {
    }
    // Fields
    private:
    bool m_callNext;
    private:
    InputPointContext^ m_context;
    private:
    AcMgPointFilter* m_pFilter;
    private:
    ProcessInputParams* m_pInputParams;
    private:
    FilterResultParams* m_pResultParams;
    private:
    PointFilterResult^ m_result;
    // Properties
    property PointFilterEventArgs^ CallNext;
    property PointFilterEventArgs^ Context;
    property PointFilterEventArgs^ Result;
};

public ref class PointMonitorEventArgs sealed : public IDisposable
{
    // Methods
    internal:
    PointMonitorEventArgs(ProcessInputParams* pInputParams, MonitorResultParams* pResultParams, AcMgPointMonitor* pMonitor)
    {
    }
    private:
    void ~PointMonitorEventArgs()
    {
    }
    public:
    void AppendToolTipText(String^ value)
    {
    }
    public:
    virtual void Dispose() sealed override
    {
    }
    protected:
    virtual void Dispose([MarshalAs(::U1)]bool )
    {
    }
    // Fields
    private:
    InputPointContext^ m_context;
    private:
    ProcessInputParams* m_pInputParams;
    private:
    AcMgPointMonitor* m_pMonitor;
    private:
    MonitorResultParams* m_pResultParams;
    // Properties
    property PointMonitorEventArgs^ Context;
};
*/
    public delegate void PromptPointOptionsEventHandler( Object^ sender, PromptPointOptionsEventArgs^ e );
    public delegate void PromptDistanceOptionsEventHandler( Object^ sender, PromptDistanceOptionsEventArgs^ e );
    public delegate void PromptAngleOptionsEventHandler( Object^ sender, PromptAngleOptionsEventArgs^ e );
    public delegate void PromptPointResultEventHandler( Object^ sender, PromptPointResultEventArgs^ e );
    public delegate void PromptDoubleResultEventHandler( Object^ sender, PromptDoubleResultEventArgs^ e );
    public delegate void PromptStringOptionsEventHandler( Object^ sender, PromptStringOptionsEventArgs^ e );
    public delegate void PromptStringResultEventHandler( Object^ sender, PromptStringResultEventArgs^ e );
    public delegate void PromptIntegerResultEventHandler( Object^ sender, PromptIntegerResultEventArgs^ e );
    public delegate void PromptEntityResultEventHandler( Object^ sender, PromptEntityResultEventArgs^ e );
    public delegate void PromptDoubleOptionsEventHandler( Object^ sender, PromptDoubleOptionsEventArgs^ e );
    public delegate void PromptIntegerOptionsEventHandler( Object^ sender, PromptIntegerOptionsEventArgs^ e );
    public delegate void PromptEntityOptionsEventHandler( Object^ sender, PromptEntityOptionsEventArgs^ e );
    public delegate void PromptKeywordOptionsEventHandler( Object^ sender, PromptKeywordOptionsEventArgs^ e );
    public delegate void PromptNestedEntityOptionsEventHandler( Object^ sender, PromptNestedEntityOptionsEventArgs^ e );
    public delegate void PromptNestedEntityResultEventHandler( Object^ sender, PromptNestedEntityResultEventArgs^ e );
    public delegate void PromptSelectionOptionsEventHandler( Object^ sender, PromptSelectionOptionsEventArgs^ e );
    public delegate void SelectionAddedEventHandler( Object^ sender, SelectionAddedEventArgs^ e );
    public delegate void SelectionRemovedEventHandler( Object^ sender, SelectionRemovedEventArgs^ e );
    public delegate void PromptForSelectionEndingEventHandler( Object^ sender, PromptForSelectionEndingEventArgs^ e );
    public delegate void PromptForEntityEndingEventHandler( Object^ sender, PromptForEntityEndingEventArgs^ e );
    public delegate void PromptSelectionResultEventHandler( Object^ sender, PromptSelectionResultEventArgs^ e );
    public delegate void RolloverEventHandler( Object^ sender, RolloverEventArgs^ e );
    public delegate void DraggingEventHandler( Object^ sender, DraggingEventArgs^ e );
    public delegate void DraggingEndedEventHandler( Object^ sender, DraggingEndedEventArgs^ e );
    public delegate void SelectionTextInputEventHandler( Object^ sender, SelectionTextInputEventArgs^ e );

    //public delegate void PointMonitorEventHandler( Object^ sender, PointMonitorEventArgs^ e );
    //public delegate void PointFilterEventHandler( Object^ sender, PointFilterEventArgs^ e );
  }
}

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

#include "../Stdafx.h"

namespace Teigha
{
  namespace ApplicationServices
  {
    ref class Document;

    //public ref class TabbedDialogEventArgs sealed : public EventArgs
    //{
    //};

    public ref class CommandEventArgs sealed : public EventArgs
    {
      public:
        CommandEventArgs(){}

      internal:
        CommandEventArgs( String^ globalcommandname ) : m_globalcommandname( globalcommandname ) {}

      public:
        property String^ GlobalCommandName { String^ get(){ return m_globalcommandname; } }

      private:
        String^ m_globalcommandname;
    };

    public ref class DocumentBeginCloseEventArgs sealed : public EventArgs
    {
      public:
        DocumentBeginCloseEventArgs() : m_veto( false ) {}

      public:
        void Veto() { m_veto = true; }

      internal:
        property bool IsVetoed{ bool get(){ return m_veto; } }

      private:
        bool m_veto;
    };

    public ref class UnknownCommandEventArgs sealed : public EventArgs
    {
      internal:
        UnknownCommandEventArgs( String^ globalcommandname ) : m_globalcommandname( globalcommandname ){}

      public:
        property String^ GlobalCommandName { String^ get(){ return m_globalcommandname; } }

      private:
        String^ m_globalcommandname;
    };

    public ref class LispWillStartEventArgs sealed : public EventArgs
    {
    };

    //public ref class DocumentLockModeChangedEventArgs sealed : public EventArgs
    //{
    //};

    public ref class DocumentActivationChangedEventArgs sealed : public EventArgs
    {
      internal:
        DocumentActivationChangedEventArgs( bool newvalue ) : m_newvalue( newvalue ){}

      public:
        property bool NewValue { bool get() { return m_newvalue; } }

      private:
        bool m_newvalue;
    };

    //public ref class DocumentLockModeWillChangeEventArgs sealed : public EventArgs
    //{
    //};

    //public ref class DocumentLockModeChangeVetoedEventArgs sealed : public EventArgs
    //{
    //};

    public ref class DocumentDestroyedEventArgs sealed : public EventArgs
    {
      internal:
        DocumentDestroyedEventArgs( String^ filename ) : m_filename( filename ) {}

      public:
        property String^ FileName { String^ get(){ return m_filename; } }

      private:
        String^ m_filename;
    };

    public ref class SystemVariableChangedEventArgs sealed : public EventArgs
    {
      internal:
        SystemVariableChangedEventArgs( bool changed, String^ name )
        {
          m_changed = changed;
          m_name = name;
        }

      public:
        property bool Changed { bool get(){ return m_changed; } }

        property String^ Name { String^ get(){ return m_name; } }

      private:
        bool m_changed;
        String^ m_name;
    };

    public ref class SystemVariableChangingEventArgs sealed : public EventArgs
    {
      internal:
        SystemVariableChangingEventArgs( String^ name )
        {
          m_name = name;
        }

      public:
        property String^ Name { String^ get(){ return m_name; } }

      private:
        String^ m_name;
    };

    public ref class PreTranslateMessageEventArgs sealed : public EventArgs
    {
      internal:
        PreTranslateMessageEventArgs( Interop::MSG pMsg )
        {
          Handled = false;
          Message = pMsg;
        }

      public:
        property bool Handled;

        property Interop::MSG Message;
    };

    public ref class DocumentCollectionEventArgs sealed : public EventArgs
    {
      internal:
        DocumentCollectionEventArgs( ApplicationServices::Document^ doc ) : m_document( doc ) {}

      public:
        property ApplicationServices::Document^ Document { ApplicationServices::Document^ get(){ return m_document; } }

      private:
        ApplicationServices::Document^ m_document;
    };

    //public ref class ProfileEventArgs sealed : public EventArgs
    //{
    //};

    public delegate void DocumentDestroyedEventHandler( Object^ sender, DocumentDestroyedEventArgs^ e );

    public delegate void DocumentActivationChangedEventHandler( Object^ sender, DocumentActivationChangedEventArgs^ e );

    //public delegate void DocumentLockModeWillChangeEventHandler( Object^ sender, DocumentLockModeWillChangeEventArgs^ e );

    //public delegate void DocumentLockModeChangeVetoedEventHandler( Object^ sender, DocumentLockModeChangeVetoedEventArgs^ e );

    //public delegate void DocumentLockModeChangedEventHandler( Object^ sender, DocumentLockModeChangedEventArgs^ e );

    public delegate void CommandEventHandler( Object^ sender, CommandEventArgs^ e );

    public delegate void UnknownCommandEventHandler( Object^ sender, UnknownCommandEventArgs^ e );

    public delegate void DocumentBeginCloseEventHandler( Object^ sender, DocumentBeginCloseEventArgs^ e );

    public delegate void LispWillStartEventHandler( Object^ sender, LispWillStartEventArgs^ e );

    //public delegate void TabbedDialogEventHandler( Object^ sender, TabbedDialogEventArgs^ e );

    public delegate void SystemVariableChangedEventHandler( Object^ sender, SystemVariableChangedEventArgs^ e );

    public delegate void SystemVariableChangingEventHandler( Object^ sender, SystemVariableChangingEventArgs^ e );

    //public delegate void TabbedDialogAction();

    public delegate void PreTranslateMessageEventHandler( Object^ sender, PreTranslateMessageEventArgs^ e );

    public delegate void ExecuteInApplicationContextCallback( Object^ userData );

    public delegate void DocumentCollectionEventHandler( Object^ sender, DocumentCollectionEventArgs^ e );

    //public delegate void ProfileEventHandler( Object^ sender, ProfileEventArgs^ e );
  }
}

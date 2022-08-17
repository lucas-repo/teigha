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
    ref class KeywordCollection;

    public ref class Keyword sealed
    {
      internal:
        Keyword( KeywordCollection^ parent );

      public:
        DECLARE_PROP( String^, DisplayName );
        DECLARE_PROP( bool,    Enabled );
        DECLARE_PROP( String^, GlobalName );
        DECLARE_PROP( bool,    IsReadOnly );
        DECLARE_PROP( String^, LocalName );
        DECLARE_PROP( bool,    Visible );

      private:
        KeywordCollection^ m_col;
        String^ m_displayName;
        bool m_enabled;
        String^ m_globalName;
        String^ m_localName;
        bool m_readOnly;
        bool m_visible;
    };

    [DefaultMember("Item")]
    public ref class KeywordCollection sealed : public ICollection, public IEnumerable
    {
      internal:
        KeywordCollection( const wchar_t* interopString );

        String^ GetInteropString();

        Keyword^ GetDefaultKeyword();

      public:
        KeywordCollection();

      public:
        void Add( String^ globalName );
        void Add( String^ globalName, String^ localName );
        void Add( String^ globalName, String^ localName, String^ displayName );
        void Add( String^ globalName, String^ localName, String^ displayName, bool visible, bool enabled );

        void Clear();

        void CopyTo( array<Keyword^>^ arr, int index );

        String^ GetDisplayString( bool showNoDefault );

        virtual IEnumerator^ GetEnumerator();

        virtual property int Count
        { int get() { return m_imp->Count; } }

        DECLARE_PROP( String^, Default );

        property bool IsReadOnly
        { bool get() { return m_imp->IsReadOnly; } }

        property Keyword^ Item[int]
        { Keyword^ get( int index ); }

      private:
        virtual property bool IsSynchronized
        { bool get() sealed = ICollection::IsSynchronized::get { return false; } }

        virtual property Object^ SyncRoot
        { Object^ get() sealed = ICollection::SyncRoot::get { return nullptr; } }

        virtual void CopyTo( Array^ arr, int index ) sealed = ICollection::CopyTo;

      private:
        Keyword^ m_default;
        ArrayList^ m_imp;
    };
  }
}

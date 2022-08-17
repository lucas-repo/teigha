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
  namespace Windows
  {
    using namespace Teigha::Runtime;

    ref class MenuItemCollection;

    public ref class Menu abstract 
    {
      protected:
        Menu();

      public:
        DECLARE_PROP_GET( MenuItemCollection^, MenuItems );

      internal:
        ArrayList^ m_menuItems;
    };

    public ref class MenuItem : public Menu
    {
      internal:
        void OnCommand( unsigned int id );

      public:
        MenuItem( String^ value );

        MenuItem( String^ value, Drawing::Icon^ icon );

        DECLARE_PROP( bool, Checked );

        DECLARE_PROP( bool, Enabled );

        DECLARE_PROP( bool, Visible );

        DECLARE_PROP( Drawing::Icon^, Icon );

        DECLARE_PROP( String^, Text );

        event EventHandler^ Click
        {
          void add( EventHandler^ value );
          void remove( EventHandler^ value );
        }

      internal:
        property unsigned int ID
        {
          unsigned int get(){ return m_id; }
        }

      private:
        static unsigned int m_menuID = 34000;

        unsigned int m_id;

        bool m_checked;

        bool m_enabled;

        Drawing::Icon^ m_icon;

        EventHandler^ m_pClickEvent;

        String^ m_text;

        String^ m_value;

        bool m_visible;
    };

    [DefaultMember("Item")]
    public ref class MenuItemCollection sealed : public Collections::IList
    {
      public:
        MenuItemCollection( Menu^ owner );

        int Add( MenuItem^ value );

        bool Contains( MenuItem^ value );

        int IndexOf( MenuItem^ value );

        void Insert( int index, MenuItem^ value );

        void Remove( MenuItem^ value );

        void CopyTo( array< MenuItem^ >^ arr, int index );

        virtual void Clear();

        virtual void RemoveAt( int index );

        virtual Collections::IEnumerator^ GetEnumerator();

        virtual property int Count
        {
          int get();
        }

        virtual property bool IsFixedSize
        {
          bool get(){ return false; }
        }

        virtual property bool IsReadOnly
        {
          bool get(){ return false; }
        }

        property MenuItem^ Item[int]
        {
          MenuItem^ get( int index );

          void set( int index, MenuItem^ item );
        }

      private:
        virtual int Add( Object^ value ) sealed = Collections::IList::Add;

        virtual bool Contains( Object^ value ) sealed = Collections::IList::Contains;

        virtual void CopyTo( Array^ arr, int index ) sealed = Collections::IList::CopyTo;

        virtual int IndexOf(Object^ value) sealed = Collections::IList::IndexOf;

        virtual void Insert(int index, Object^ value) sealed = Collections::IList::Insert;

        virtual void Remove(Object^ value) sealed = Collections::IList::Remove;

        virtual property Object^ IListItem[int]
        {
          Object^ get( int index ) sealed = Collections::IList::default::get;
          void set( int index, Object^ value ) sealed = Collections::IList::default::set;
        }
        
        virtual property bool IsSynchronized
        {
          bool get() sealed = Collections::IList::IsSynchronized::get { return false; }
        }

        virtual property Object^ SyncRoot
        {
          Object^ get() sealed = Collections::IList::SyncRoot::get { return nullptr; }
        }

      private protected:
        Menu^ m_owner;
    };

    public ref class ContextMenuExtension : public Menu
    {
      public:
        ContextMenuExtension();

        property String^ Title
        {
          String^ get();
          void set( String^ value );
        }

        event EventHandler^ Popup
        {
          void add( EventHandler^ value );
          void remove( EventHandler^ value );
        }

      internal:
        void FirePopup( EventArgs^ e );

        ArrayList^ getMenuItems();

        void Register();

        void Register( RXClass^ runtimeClass );

        void Unregister();

        void Unregister( RXClass^ runtimeClass );

      private:

        EventHandler^ m_pPopupEvent;

        String^ m_title;
    };
  }
}

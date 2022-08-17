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

#include "Menu.h"

namespace Teigha
{
  namespace Windows
  {
    Menu::Menu()
    {
      m_menuItems = gcnew ArrayList();
    }

    MenuItemCollection^ Menu::MenuItems::get()
    {
      return gcnew MenuItemCollection( this );
    }

    MenuItem::MenuItem( String^ value )
    {
      m_text = value;
      m_visible = true;
      m_enabled = true;
      m_checked = false;
      m_value = String::Empty;
      m_id = ++m_menuID;
    }

    MenuItem::MenuItem( String^ value, Drawing::Icon^ icon )
    {
      m_text = value;
      m_visible = true;
      m_enabled = true;
      m_checked = false;
      m_value = String::Empty;
      m_id = ++m_menuID;
      Icon = icon;
    }

    void MenuItem::OnCommand( unsigned int id )
    {
      if ( m_menuItems->Count != 0 )
      {
        for each ( MenuItem^ item in m_menuItems )
        {
          item->OnCommand( id );
        }
      }
      else
      {
        if ( id == m_id )
        {
          if ( m_pClickEvent != nullptr )
            m_pClickEvent( this, EventArgs::Empty );
        }
      }
    }

    bool MenuItem::Checked::get()
    {
      return m_checked;
    }

    void MenuItem::Checked::set( bool value )
    {
      m_checked = value;
    }

    bool MenuItem::Enabled::get()
    {
      return m_enabled;
    }

    void MenuItem::Enabled::set( bool value )
    {
      m_enabled = value;
    }

    bool MenuItem::Visible::get()
    {
      return m_visible;
    }

    void MenuItem::Visible::set( bool value )
    {
      m_visible = value;
    }

    Drawing::Icon^ MenuItem::Icon::get()
    {
      return m_icon;
    }

    void MenuItem::Icon::set( Drawing::Icon^ value )
    {
      m_icon = value;
    }

    String^ MenuItem::Text::get()
    {
      return m_text;
    }

    void MenuItem::Text::set( String^ value )
    {
      m_text = value;
    }

    void MenuItem::Click::add( EventHandler^ value )
    {
      m_pClickEvent += value;
    }

    void MenuItem::Click::remove( EventHandler^ value )
    {
      m_pClickEvent -= value;
    }

    MenuItemCollection::MenuItemCollection( Menu^ owner )
    {
      m_owner = owner;
    }

    int MenuItemCollection::Add( MenuItem^ value )
    {
      return m_owner->m_menuItems->Add( value );
    }

    bool MenuItemCollection::Contains( MenuItem^ value )
    {
      return m_owner->m_menuItems->Contains( value );
    }

    int MenuItemCollection::IndexOf( MenuItem^ value )
    {
      return m_owner->m_menuItems->IndexOf( value );
    }

    void MenuItemCollection::Insert( int index, MenuItem^ value )
    {
      m_owner->m_menuItems->Insert( index, value );
    }

    void MenuItemCollection::Remove( MenuItem^ value )
    {
      m_owner->m_menuItems->Remove( value );
    }

    void MenuItemCollection::CopyTo( array< MenuItem^ >^ arr, int index )
    {
      m_owner->m_menuItems->CopyTo( arr, index );
    }

    void MenuItemCollection::Clear()
    {
      m_owner->m_menuItems->Clear();
    }

    void MenuItemCollection::RemoveAt( int index )
    {
      m_owner->m_menuItems->RemoveAt( index );
    }

    Collections::IEnumerator^ MenuItemCollection::GetEnumerator()
    {
      return m_owner->m_menuItems->GetEnumerator();
    }

    int MenuItemCollection::Count::get()
    {
      return m_owner->m_menuItems->Count;
    }

    MenuItem^ MenuItemCollection::Item::get( int index )
    {
      return (MenuItem^) m_owner->m_menuItems[ index ];
    }

    void MenuItemCollection::Item::set( int index, MenuItem^ item )
    {
      m_owner->m_menuItems[ index ] = item;
    }

    int MenuItemCollection::Add( Object^ value )
    {
      return m_owner->m_menuItems->Add( value );
    }

    bool MenuItemCollection::Contains( Object^ value )
    {
      return m_owner->m_menuItems->Contains( value );
    }

    void MenuItemCollection::CopyTo( Array^ arr, int index )
    {
      m_owner->m_menuItems->CopyTo( arr, index );
    }

    int MenuItemCollection::IndexOf( Object^ value )
    {
      return m_owner->m_menuItems->IndexOf( value );
    }

    void MenuItemCollection::Insert( int index, Object^ value )
    {
      m_owner->m_menuItems->Insert( index, value );
    }

    void MenuItemCollection::Remove(Object^ value)
    {
      m_owner->m_menuItems->Contains( value );
    }

    Object^ MenuItemCollection::IListItem::get( int index )
    {
      return m_owner->m_menuItems[ index ];
    }

    void MenuItemCollection::IListItem::set( int index, Object^ value )
    {
      m_owner->m_menuItems[ index ] = value;
    }

    ContextMenuExtension::ContextMenuExtension()
    {
      m_title = String::Empty;
    }

    String^ ContextMenuExtension::Title::get()
    {
      return m_title;
    }

    void ContextMenuExtension::Title::set( String^ value )
    {
      m_title = value;
    }

    void ContextMenuExtension::Popup::add( EventHandler^ value )
    {
      m_pPopupEvent += value;
    }

    void ContextMenuExtension::Popup::remove( EventHandler^ value )
    {
      m_pPopupEvent -= value;
    }

    void ContextMenuExtension::FirePopup( EventArgs^ e )
    {
      if ( m_pPopupEvent != nullptr )
        m_pPopupEvent( nullptr, e );
    }

    ArrayList^ ContextMenuExtension::getMenuItems()
    {
      return m_menuItems;
    }

    void ContextMenuExtension::Register()
    {
      throw gcnew NotSupportedException();
    }

    void ContextMenuExtension::Register( RXClass^ )
    {
      throw gcnew NotSupportedException();
    }

    void ContextMenuExtension::Unregister()
    {
      throw gcnew NotSupportedException();
    }

    void ContextMenuExtension::Unregister( RXClass^ )
    {
      throw gcnew NotSupportedException();
    }
  }
}

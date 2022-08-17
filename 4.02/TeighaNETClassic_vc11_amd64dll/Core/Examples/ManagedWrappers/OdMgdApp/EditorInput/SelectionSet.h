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
#pragma warning( disable:4793 )

#include "../stdafx.h"

namespace Teigha
{
  namespace EditorInput
  {
    [Flags]
    public enum class SelectionFlags
    {
      Normal          = 0x000,
      PickPoints      = 0x001,
      Duplicates      = 0x002,
      NestedEntities  = 0x004,
      SubEntities     = 0x008,
      SinglePick      = 0x010,
      PickfirstSet    = 0x020,
      PreviousSet     = 0x040,
      SubSelection    = 0x080,
      Undo            = 0x100,
      FailedPickAuto  = 0x200
    };

    public enum class SelectionMethod
    {
      Unavailable   = -1,
      NonGraphical  = 0,
      PickPoint     = 1,
      Window        = 2,
      Crossing      = 3,
      Fence         = 4,
      SubEntity     = 5
    };

    public enum class SelectionMode
    {
      Window    = 1,
      Crossing  = 2,
      Box       = 3,
      Last      = 4,
      Entity    = 5,
      All       = 6,
      FencePolyline   = 7,
      CrossingPolygon = 8,
      WindowPolygon   = 9,
      Pick      = 10,
      Every     = 11,
      Extents   = 12,
      Group     = 13,
      Previous  = 14
    };

    public ref class SelectionFilter sealed
    {
      //internal:
      //  SelectionFilter(const resbuf* buf);

      public:
        SelectionFilter( array< DatabaseServices::TypedValue >^ value );

        array< DatabaseServices::TypedValue >^ GetFilter();

      private:
        array< DatabaseServices::TypedValue >^ m_value;
    };

    public ref class SelectionDetails
    {
      //internal:
      //  SelectionDetails( resbuf** cur );
      public:
        array< DatabaseServices::ObjectId >^ GetContainers();

        virtual String^ ToString() override;

        String^ ToString( IFormatProvider^ provider );

        DECLARE_PROP_GET( Geometry::Matrix3d, Transform );

      private:
        array< DatabaseServices::ObjectId >^ m_containers;
        Geometry::Matrix3d m_mat;
    };

    public ref class SelectedSubObject
    {
      public:
        SelectedSubObject( DatabaseServices::FullSubentityPath path, SelectionMethod method, int gsMarker );

        virtual String^ ToString() override;

        String^ ToString( IFormatProvider^ provider );

        DECLARE_PROP_GET( DatabaseServices::FullSubentityPath^, FullSubentityPath );

        DECLARE_PROP_GET( int, GraphicsSystemMarker );

        DECLARE_PROP_GET( SelectionDetails^, OptionalDetails );

        DECLARE_PROP_GET( EditorInput::SelectionMethod, SelectionMethod );

      private:
        SelectionDetails^ m_details;

        int m_gsMarker;

        EditorInput::SelectionMethod m_method;

        DatabaseServices::FullSubentityPath m_path;
    };

    public ref class SelectedObject 
    {
      public:
        SelectedObject( DatabaseServices::ObjectId id, array< SelectedSubObject^ >^ subSelections );

        SelectedObject( DatabaseServices::ObjectId id, SelectionMethod method, int gsMarker );

        array< SelectedSubObject^ >^ GetSubentities();

        virtual String^ ToString() override;

        String^ ToString( IFormatProvider^ provider );

        DECLARE_PROP_GET( int, GraphicsSystemMarker );

        DECLARE_PROP_GET( DatabaseServices::ObjectId, ObjectId );

        DECLARE_PROP_GET( SelectionDetails^, OptionalDetails );

        DECLARE_PROP_GET( EditorInput::SelectionMethod, SelectionMethod );

      private:
        SelectionDetails^ m_details;

        int m_gsMarker;

        DatabaseServices::ObjectId m_id;

        EditorInput::SelectionMethod m_method;

        array< SelectedSubObject^ >^ m_subentities;
    };

    [DefaultMember( "Item" )]
    public ref class SelectionSet abstract : public ICollection
    {
      public:
        void CopyTo( array< SelectedObject^ >^ array, int index );

        static SelectionSet^ FromObjectIds( array< DatabaseServices::ObjectId >^ ids );

        virtual IEnumerator^ GetEnumerator() abstract;

        virtual array< DatabaseServices::ObjectId >^ GetObjectIds() abstract;

        virtual String^ ToString() override;

        String^ ToString( IFormatProvider^ provider );

        virtual property int Count { int get() abstract; }

        virtual property SelectedObject^ Item[int] { SelectedObject^ get(int index) abstract; }

        virtual property bool IsSynchronized
        { bool get(){ return false; } }

        virtual property Object^ SyncRoot
        { Object^ get() { return nullptr; } }

      private:
        virtual void CopyTo( Array^ array, int index ) sealed = ICollection::CopyTo;
    };

    [DefaultMember( "Item" )]
    ref class SelectionSetDelayMarshalled : public SelectionSet
    {
      public:
        SelectionSetDelayMarshalled( OdDbSelectionSetPtr pSSet );

        !SelectionSetDelayMarshalled();

        ~SelectionSetDelayMarshalled();

        virtual IEnumerator^ GetEnumerator() sealed override;

        virtual array< DatabaseServices::ObjectId >^ GetObjectIds() sealed override;

        virtual property int Count
        { int get() sealed override; }

        virtual property SelectedObject^ Item[int]
        { SelectedObject^ get(int index) sealed override; }

      internal:
        OdDbSelectionSet* m_pImpl;
    };

    [DefaultMember("Item")]
    private ref class SelectionSetFullyMarshalled sealed : public SelectionSet
    {
      public:
        SelectionSetFullyMarshalled( array< DatabaseServices::ObjectId >^ ids );

        virtual IEnumerator^ GetEnumerator() sealed override;

        virtual array< DatabaseServices::ObjectId >^ GetObjectIds() sealed override;

        virtual property int Count
        { int get() sealed override; }

        virtual property SelectedObject^ Item[int]
        { SelectedObject^ get(int index) sealed override; }

      private:
        array< SelectedObject^ >^ m_imp;
    };

  }
}

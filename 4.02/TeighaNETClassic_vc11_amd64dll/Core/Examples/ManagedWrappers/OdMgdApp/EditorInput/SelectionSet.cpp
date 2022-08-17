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

#include "SelectionSet.h"

namespace Teigha
{
  namespace EditorInput
  {
    SelectionFilter::SelectionFilter( array< DatabaseServices::TypedValue >^ value )
    {
      m_value = value;
    }

    array< DatabaseServices::TypedValue >^ SelectionFilter::GetFilter()
    {
      return m_value;
    }

    array< DatabaseServices::ObjectId >^ SelectionDetails::GetContainers()
    {
      return m_containers;
    }

    String^ SelectionDetails::ToString()
    {
      return ToString( nullptr );
    }

    String^ SelectionDetails::ToString( IFormatProvider^ provider )
    {
      Text::StringBuilder^ builder = gcnew Text::StringBuilder();
      builder->Append("(");
      builder->Append( m_mat.ToString( provider ) );

      for each ( DatabaseServices::ObjectId id in m_containers )
        builder->Append( id.ToString() );

      builder->Append(")");
      return builder->ToString();
    }

    Geometry::Matrix3d SelectionDetails::Transform::get()
    {
      return m_mat;
    }

    void SelectionSet::CopyTo( Array^ arr, int index )
    {
      for each ( SelectedObject^ obj in this )
        arr->SetValue( obj, index++ );
    }

    void SelectionSet::CopyTo( array< SelectedObject^ >^ arr, int index )
    {
      CopyTo( (Array^) arr, index );
    }

    SelectionSet^ SelectionSet::FromObjectIds( array< DatabaseServices::ObjectId >^ ids )
    {
      if ( ids == nullptr )
        return nullptr;

      return gcnew SelectionSetFullyMarshalled(ids) ;
    }

    String^ SelectionSet::ToString()
    {
      return ToString( nullptr );
    }

    String^ SelectionSet::ToString( IFormatProvider^ provider )
    {
      Text::StringBuilder^ builder = gcnew Text::StringBuilder();
      builder->Append("(");

      for each ( SelectedObject^ obj in this )
        builder->Append( obj->ToString( provider ) );

      builder->Append(")");
      return builder->ToString();
    }

    SelectedSubObject::SelectedSubObject( DatabaseServices::FullSubentityPath path, EditorInput::SelectionMethod method, int gsMarker )
    {
      m_path = path;
      m_method = method;
      m_gsMarker = gsMarker;
    }

    String^ SelectedSubObject::ToString()
    {
      return ToString( nullptr );
    }

    String^ SelectedSubObject::ToString( IFormatProvider^ provider )
    {
      array< Object^ >^ args = gcnew array< Object^ > { m_path, m_method, m_gsMarker, m_details };

      return String::Format(provider, "({0},{1},{2},{3})", args);
    }

    DatabaseServices::FullSubentityPath^ SelectedSubObject::FullSubentityPath::get()
    {
      return m_path;
    }

    int SelectedSubObject::GraphicsSystemMarker::get()
    {
      return m_gsMarker;
    }

    SelectionDetails^ SelectedSubObject::OptionalDetails::get()
    {
      return m_details;
    }

    EditorInput::SelectionMethod SelectedSubObject::SelectionMethod::get()
    {
      return m_method;
    }

    SelectedObject::SelectedObject( DatabaseServices::ObjectId id, array< SelectedSubObject^ >^ subSelections )
    {
      m_id = id;
      m_method = EditorInput::SelectionMethod::SubEntity;
      m_subentities = subSelections;
    }

    SelectedObject::SelectedObject( DatabaseServices::ObjectId id, EditorInput::SelectionMethod method, int gsMarker )
    {
      m_id = id;
      m_method = method;
      m_gsMarker = gsMarker;
    }

    array< SelectedSubObject^ >^ SelectedObject::GetSubentities()
    {
      return m_subentities;
    }

    String^ SelectedObject::ToString()
    {
      return ToString( nullptr );
    }

    String^ SelectedObject::ToString( IFormatProvider^ provider )
    {
      array< Object^ >^ args = gcnew array< Object^ > { m_id, m_method, m_gsMarker, m_details };

      Text::StringBuilder^ builder = gcnew Text::StringBuilder();
      builder->Append( String::Format(provider, "({0},{1},{2},{3}", args ) );

      for each( SelectedSubObject^ obj in m_subentities )
      {
        builder->Append( "," );
        builder->Append( obj->ToString( provider ) );
      }
      builder->Append(")");
      return builder->ToString();
    }

    int SelectedObject::GraphicsSystemMarker::get()
    {
      return m_gsMarker;
    }

    DatabaseServices::ObjectId SelectedObject::ObjectId::get()
    {
      return m_id;
    }

    SelectionDetails^ SelectedObject::OptionalDetails::get()
    {
      return m_details;
    }

    EditorInput::SelectionMethod SelectedObject::SelectionMethod::get()
    {
      return m_method;
    }

    private ref class SelectedObjectEnumerator : public IEnumerator
    {
      public:
        SelectedObjectEnumerator( SelectionSetDelayMarshalled^ pSSet )
        {
          m_pSSet = pSSet;
        }

        ~SelectedObjectEnumerator()
        {
          TD_START_WRAP_EXCEPTIONS;
          if ( nullptr != m_pIter )
            m_pIter->release();
          TD_END_WRAP_EXCEPTIONS;
        }

        !SelectedObjectEnumerator()
        {
          TD_START_WRAP_EXCEPTIONS;
          if ( nullptr != m_pIter )
            m_pIter->release();
          TD_END_WRAP_EXCEPTIONS;
        }

        virtual bool MoveNext()
        {
          TD_START_WRAP_EXCEPTIONS;
          if ( nullptr == m_pIter )
            m_pIter = m_pSSet->m_pImpl->newIterator().detach();
          else
            m_pIter->next();

          return !m_pIter->done();
          TD_END_WRAP_EXCEPTIONS;
        }

        virtual void Reset()
        {
          TD_START_WRAP_EXCEPTIONS;
          if ( nullptr != m_pIter )
            m_pIter->release();

          m_pIter = nullptr;
          TD_END_WRAP_EXCEPTIONS;
        }

        virtual property Object^ Current
        {
          Object^ get()
          {
            TD_START_WRAP_EXCEPTIONS;
            if ( m_pIter->done() )
              throw gcnew InvalidOperationException();

            DatabaseServices::ObjectId idObject( IntPtr( (OdDbStub*) m_pIter->objectId() ) );

            SelectionMethod sm = SelectionMethod::Unavailable;
            OdDbVisualSelectionPtr pMethod = OdDbVisualSelection::cast( m_pIter->method() );

            if ( !pMethod.isNull() ) switch ( pMethod->mode() ) // TODO:
            {
              case OdDbVisualSelection::kPoint:
                sm = SelectionMethod::PickPoint;
                break;
              case OdDbVisualSelection::kWindow:
                sm = SelectionMethod::Window;
                break;
              case OdDbVisualSelection::kCrossing:
                sm = SelectionMethod::Crossing;
                break;
              case OdDbVisualSelection::kFence:
                sm = SelectionMethod::Fence;
                break;
            }

            int iCount = m_pIter->subentCount();
            if ( 1 == iCount )
            {
              OdDbFullSubentPath path;
              m_pIter->getSubentity( 0, path );
              return gcnew SelectedObject( idObject, sm, path.subentId().index() );
            }

            array< SelectedSubObject^ >^ aSubObj = gcnew array< SelectedSubObject^ >( iCount );
            for ( int i = 0; i < iCount; i++ )
            {
              OdDbFullSubentPath path;
              m_pIter->getSubentity( i, path );

              int iSize = path.objectIds().size();
              array< DatabaseServices::ObjectId >^ ids = gcnew array< DatabaseServices::ObjectId >( iSize );
              for ( i = 0; i < iSize; i++ )
                ids[ i ] = DatabaseServices::ObjectId( IntPtr( (OdDbStub*) path.objectIds()[ i ] ) );

              OdDbSubentId subEntId = path.subentId();
              DatabaseServices::SubentityType eType = (DatabaseServices::SubentityType) subEntId.type();
              DatabaseServices::SubentityId subId( eType, subEntId.index() );
              DatabaseServices::FullSubentityPath resPath( ids, subId );

              aSubObj[ i ] = gcnew SelectedSubObject( resPath, sm, subEntId.index() );
            }
            return gcnew SelectedObject( idObject, aSubObj );
            TD_END_WRAP_EXCEPTIONS;
          }
        }

      private:
        SelectionSetDelayMarshalled^ m_pSSet;
        OdDbSelectionSetIterator* m_pIter;
    };

    SelectionSetDelayMarshalled::SelectionSetDelayMarshalled( OdDbSelectionSetPtr pSSet )
    {
      m_pImpl = pSSet.get();
      m_pImpl->addRef();
    }

    SelectionSetDelayMarshalled::!SelectionSetDelayMarshalled()
    {
      m_pImpl->release();
    }

    SelectionSetDelayMarshalled::~SelectionSetDelayMarshalled()
    {
      m_pImpl->release();
    }

    IEnumerator^ SelectionSetDelayMarshalled::GetEnumerator()
    {
      return gcnew SelectedObjectEnumerator( this );
    }

    array< DatabaseServices::ObjectId >^ SelectionSetDelayMarshalled::GetObjectIds()
    {
      array< DatabaseServices::ObjectId >^ aRes = gcnew array< DatabaseServices::ObjectId >( Count );
      
      int index = 0;
      for each ( SelectedObject^ obj in this )
        aRes[ index++ ] = obj->ObjectId;

      return aRes;
    }

    int SelectionSetDelayMarshalled::Count::get()
    {
      return m_pImpl->numEntities();
    }

    SelectedObject^ SelectionSetDelayMarshalled::Item::get( int index )
    {
      int i = 0;
      for each ( SelectedObject^ obj in this )
        if ( index == i++ )
          return obj;
      throw gcnew IndexOutOfRangeException();
    }

    SelectionSetFullyMarshalled::SelectionSetFullyMarshalled( array< DatabaseServices::ObjectId >^ ids )
    {
      int i = 0, iLen = ids->Length;
      m_imp = gcnew array< SelectedObject^ >( iLen );
      for each ( DatabaseServices::ObjectId id in ids )
        m_imp[ i++ ] = gcnew SelectedObject( id, SelectionMethod::Unavailable, 0 );
    }

    IEnumerator^ SelectionSetFullyMarshalled::GetEnumerator()
    {
      return m_imp->GetEnumerator();
    }

    array< DatabaseServices::ObjectId >^ SelectionSetFullyMarshalled::GetObjectIds()
    {
      array< DatabaseServices::ObjectId >^ aRes = gcnew array< DatabaseServices::ObjectId >( Count );
      int i = 0;
      for each ( SelectedObject^ so in this )
        aRes[ i++ ] = so->ObjectId;

      return aRes;
    }

    int SelectionSetFullyMarshalled::Count::get()
    {
      return m_imp->Length;
    }

    SelectedObject^ SelectionSetFullyMarshalled::Item::get( int index )
    {
      return m_imp[ index ];
    }
  }
}

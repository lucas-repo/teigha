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

#include "Editor.h"

#include "../ApplicationServices/Application.h"
#include "../ApplicationServices/Document.h"
#include "../ApplicationServices/DocumentCollection.h"
#include "../EditorInput/PromptOptions.h"
#include "../EditorInput/PromptResults.h"
#include "../EditorInput/SelectionSet.h"
#include "../EditorInput/Keyword.h"

#include "DbViewportTable.h"
#include "Ge/GeMatrix3d.h"
#include "Gs/GsModel.h"

namespace Teigha
{
  namespace EditorInput
  {
    template< class T = OdEdInputTracker >
    class OdEdInputTrackerImp : public T
    {
      public:
        virtual int addDrawables(OdGsView* ){ return 0; }

        virtual void removeDrawables(OdGsView* ){}
    };

    template< class T = OdEdSSetTracker >
    class OdEdSSetTrackerImp : public OdEdInputTrackerImp< T >
    {
      public:
        virtual bool append(const OdDbObjectId& , const OdDbSelectionMethod* ){ return true; }

        virtual bool remove(const OdDbObjectId& , const OdDbSelectionMethod* ){ return true; }
    };

    class OdMgdSSetEditorTracker : public OdEdSSetTrackerImp<>
    {
      public:
        virtual bool append( const OdDbObjectId& entId, const OdDbSelectionMethod* pMethod )
        {
          Editor::g_CurrentEditor->SelectionAdded( Editor::g_CurrentEditor, gcnew SelectionAddedEventArgs() );  // TODO:
          return __super::append( entId, pMethod );
        }

        virtual bool remove( const OdDbObjectId& entId, const OdDbSelectionMethod* pMethod )
        {
          Editor::g_CurrentEditor->SelectionRemoved( Editor::g_CurrentEditor, gcnew SelectionRemovedEventArgs() );  // TODO:
          return __super::append( entId, pMethod );
        }
    };

    Editor::Editor( ApplicationServices::Document^ doc )
    {
      m_doc = doc;
    }

    void Editor::WriteMessage( String^ message )
    {
      TD_START_WRAP_EXCEPTIONS;
      pin_ptr< const wchar_t > pMsg = PtrToStringChars( message );
      m_doc->GetImpObj()->cmdIO()->putString( pMsg );
      TD_END_WRAP_EXCEPTIONS;
    }

    void Editor::WriteMessage( String^ message, ...array< Object^ >^ parameter )
    {
      WriteMessage( String::Format( System::Threading::Thread::CurrentThread->CurrentCulture, message, parameter ) );
    }

    PromptResult^ Editor::DoPrompt( PromptOptions^ opt )
    {
      g_CurrentEditor = this;
      PromptResult^ res = opt->DoIt();
      g_CurrentEditor = nullptr;
      return res;
    }

    PromptDoubleResult^ Editor::GetDistance( PromptDistanceOptions^ options )
    {
      return ( PromptDoubleResult^ ) DoPrompt( options );
    }

    PromptDoubleResult^ Editor::GetDistance( String^ message )
    {
      return GetDistance( gcnew PromptDistanceOptions( message ) );
    }

    PromptDoubleResult^ Editor::GetDouble( PromptDoubleOptions^ options )
    {
      return (PromptDoubleResult^) DoPrompt( options );
    }

    PromptDoubleResult^ Editor::GetDouble( String^ message )
    {
      return GetDouble( gcnew PromptDoubleOptions( message ) );
    }

    PromptDoubleResult^ Editor::GetAngle( PromptAngleOptions^ options)
    {
      return ( PromptDoubleResult^ ) DoPrompt( options );
    }

    PromptDoubleResult^ Editor::GetAngle( String^ message )
    {
      return GetAngle( gcnew PromptAngleOptions( message ) );
    }

    PromptIntegerResult^ Editor::GetInteger( PromptIntegerOptions^ options)
    {
      return ( PromptIntegerResult^ ) DoPrompt( options );
    }

    PromptIntegerResult^ Editor::GetInteger( String^ message )
    {
      return GetInteger( gcnew PromptIntegerOptions( message ) );
    }

    PromptResult^ Editor::GetKeywords( PromptKeywordOptions^ options )
    {
      return ( PromptResult^ ) DoPrompt( options );
    }

    PromptResult^ Editor::GetKeywords( String^ message, ...array<String^>^ globalKeywords )
    {
      PromptKeywordOptions^ options = gcnew PromptKeywordOptions( message );
      for each ( String^ globalName in globalKeywords )
        options->Keywords->Add( globalName, globalName, globalName, true, true );

      return GetKeywords( options );
    }

    PromptResult^ Editor::GetString( PromptStringOptions^ options )
    {
      return ( PromptResult^ ) DoPrompt( options );
    }

    PromptResult^ Editor::GetString( String^ message )
    {
      return GetString( gcnew PromptStringOptions( message ) );
    }

    PromptFileNameResult^ Editor::GetFileNameForOpen( PromptOpenFileOptions^ options)
    {
      return options->DoIt();
    }

    PromptFileNameResult^ Editor::GetFileNameForOpen( String^ message )
    {
      return GetFileNameForOpen( gcnew PromptOpenFileOptions( message ) );
    }

    PromptFileNameResult^ Editor::GetFileNameForSave( PromptSaveFileOptions^ options)
    {
      return options->DoIt();
    }

    PromptFileNameResult^ Editor::GetFileNameForSave( String^ message )
    {
      return GetFileNameForSave( gcnew PromptSaveFileOptions( message ) );
    }

    PromptPointResult^ Editor::GetCorner( PromptCornerOptions^ options )
    {
      return (PromptPointResult^) DoPrompt( options );
    }

    PromptPointResult^ Editor::GetCorner( String^ message, Geometry::Point3d basePoint )
    {
      return GetCorner( gcnew PromptCornerOptions( message, basePoint ) );
    }

    PromptPointResult^ Editor::GetPoint( PromptPointOptions^ options )
    {
      return (PromptPointResult^) DoPrompt( options );
    }

    PromptPointResult^ Editor::GetPoint( String^ message )
    {
      return GetPoint( gcnew PromptPointOptions( message ) );
    }

    PromptEntityResult^ Editor::GetEntity( PromptEntityOptions^ options )
    {
      return (PromptEntityResult^) DoPrompt( options );
    }

    PromptEntityResult^ Editor::GetEntity( String^ message )
    {
      return GetEntity( gcnew PromptEntityOptions( message ) );
    }

    PromptSelectionResult^ Editor::SelectAll()
    {
      return SelectAll( nullptr );
    }

    OdRxObjectPtr getFilter( SelectionFilter^ filter )
    {
      OdRxObjectPtr pFilter;
      if ( filter != nullptr )
      {
        auto_handle< ResultBuffer > pBuffer = gcnew ResultBuffer( filter->GetFilter() );
        pFilter = ( OdRxObject* ) pBuffer->UnmanagedObject.ToPointer();
      }
      return pFilter;
    }

    PromptSelectionResult^ Editor::GetSelection( PromptSelectionOptions^ options )
    {
      return GetSelection( options, nullptr );
    }

    PromptSelectionResult^ Editor::GetSelection( PromptSelectionOptions^ options, SelectionFilter^ filter )
    {
      return options->Select( filter );
    }

    PromptSelectionResult^ Editor::GetSelection()
    {
      return GetSelection( (SelectionFilter^)nullptr );
    }

    PromptSelectionResult^ Editor::GetSelection( SelectionFilter^ /*filter*/ )
    {
      g_CurrentEditor = this;
      try
      {
        OdDbCommandContextPtr pCmdCtx = ApplicationServices::Application::DocumentManager->MdiActiveDocument->GetImpObj()->cmdCtx();
        OdDbUserIO* pOut = pCmdCtx->dbUserIO();

        OdDbSelectionSetPtr pSSet = pOut->pickfirst();
        return gcnew PromptSelectionResult( PromptStatus::OK, pSSet );
      }
      catch(...)
      {
        return gcnew PromptSelectionResult( PromptStatus::Error, OdDbSelectionSet::createObject() );
      }
      g_CurrentEditor = nullptr;
    }

    PromptSelectionResult^ Editor::SelectAll( SelectionFilter^ filter )
    {
      g_CurrentEditor = this;
      try
      {
        OdDbDatabasePtr pDb = m_doc->GetImpObj()->database();

        OdDbSelectionSetPtr pSSet = OdDbSelectionSet::select( pDb, getFilter( filter ) );

        return gcnew PromptSelectionResult( PromptStatus::OK, pSSet );
      }
      catch (...)
      {
        return gcnew PromptSelectionResult( PromptStatus::Error, OdDbSelectionSet::createObject() );
      }
      g_CurrentEditor = nullptr;
    }

    PromptSelectionResult^ Editor::SelectCrossingPolygon( Geometry::Point3dCollection^ polygon )
    {
      return SelectCrossingPolygon( polygon, nullptr );
    }

    PromptSelectionResult^ Editor::SelectCrossingPolygon( Geometry::Point3dCollection^ polygon, SelectionFilter^ filter )
    {
      g_CurrentEditor = this;
      try
      {
        OdDbDatabasePtr pDb = m_doc->GetImpObj()->database();
        OdDbObjectId vpId = pDb->activeViewportId();

        std::vector< OdGePoint3d > aPts;
        aPts.reserve( polygon->Count );
        for each ( Geometry::Point3d pt in polygon )
          aPts.push_back( *( OdGePoint3d* ) &pt );

        OdDbSelectionSetPtr pSSet = OdDbSelectionSet::select( vpId, aPts.size(), &aPts[0],
                                                              OdDbVisualSelection::kCPoly, OdDbVisualSelection::kDisableSubents,
                                                              getFilter( filter ).get() );

        return gcnew PromptSelectionResult( PromptStatus::OK, pSSet );
      }
      catch (...)
      {
        return gcnew PromptSelectionResult( PromptStatus::Error, OdDbSelectionSet::createObject() );
      }
      g_CurrentEditor = nullptr;
    }

    PromptSelectionResult^ Editor::SelectCrossingWindow( Geometry::Point3d pt1, Geometry::Point3d pt2 )
    {
      return SelectCrossingWindow( pt1, pt2, nullptr );
    }

    PromptSelectionResult^ Editor::SelectCrossingWindow( Geometry::Point3d pt1, Geometry::Point3d pt2, SelectionFilter^ filter )
    {
      g_CurrentEditor = this;
      try
      {
        OdDbDatabasePtr pDb = m_doc->GetImpObj()->database();
        OdDbObjectId vpId = pDb->activeViewportId();

        std::vector< OdGePoint3d > aPts(2);
        aPts[ 0 ] = *( OdGePoint3d* ) &pt1;
        aPts[ 1 ] = *( OdGePoint3d* ) &pt2;

        OdDbSelectionSetPtr pSSet = OdDbSelectionSet::select( vpId, aPts.size(), &aPts[0],
                                                              OdDbVisualSelection::kCrossing, OdDbVisualSelection::kDisableSubents,
                                                              getFilter( filter ).get() );

        return gcnew PromptSelectionResult( PromptStatus::OK, pSSet );
      }
      catch (...)
      {
        return gcnew PromptSelectionResult( PromptStatus::Error, OdDbSelectionSet::createObject() );
      }
      g_CurrentEditor = nullptr;
    }

    PromptSelectionResult^ Editor::SelectFence( Geometry::Point3dCollection^ fence )
    {
      return SelectFence( fence, nullptr );
    }

    PromptSelectionResult^ Editor::SelectFence( Geometry::Point3dCollection^ fence, SelectionFilter^ filter )
    {
      g_CurrentEditor = this;
      try
      {
        OdDbDatabasePtr pDb = m_doc->GetImpObj()->database();
        OdDbObjectId vpId = pDb->activeViewportId();

        std::vector< OdGePoint3d > aPts;
        aPts.reserve( fence->Count );
        for each ( Geometry::Point3d pt in fence )
          aPts.push_back( *( OdGePoint3d* ) &pt );

        OdDbSelectionSetPtr pSSet = OdDbSelectionSet::select( vpId, aPts.size(), &aPts[0],
                                                              OdDbVisualSelection::kFence, OdDbVisualSelection::kDisableSubents,
                                                              getFilter( filter ).get() );

        return gcnew PromptSelectionResult( PromptStatus::OK, pSSet );
      }
      catch (...)
      {
        return gcnew PromptSelectionResult( PromptStatus::Error, OdDbSelectionSet::createObject() );
      }
      g_CurrentEditor = nullptr;
    }

    PromptSelectionResult^ Editor::SelectWindow( Geometry::Point3d pt1, Geometry::Point3d pt2 )
    {
      return SelectWindow( pt1, pt2, nullptr );
    }

    PromptSelectionResult^ Editor::SelectWindow( Geometry::Point3d pt1, Geometry::Point3d pt2, SelectionFilter^ filter )
    {
      g_CurrentEditor = this;
      try
      {
        OdDbDatabasePtr pDb = m_doc->GetImpObj()->database();
        OdDbObjectId vpId = pDb->activeViewportId();

        std::vector< OdGePoint3d > aPts(2);
        aPts[ 0 ] = *( OdGePoint3d* ) &pt1;
        aPts[ 1 ] = *( OdGePoint3d* ) &pt2;

        OdDbSelectionSetPtr pSSet = OdDbSelectionSet::select( vpId, aPts.size(), &aPts[0],
                                                              OdDbVisualSelection::kWindow, OdDbVisualSelection::kDisableSubents,
                                                              getFilter( filter ).get() );

        return gcnew PromptSelectionResult( PromptStatus::OK, pSSet );
      }
      catch (...)
      {
        return gcnew PromptSelectionResult( PromptStatus::Error, OdDbSelectionSet::createObject() );
      }
      g_CurrentEditor = nullptr;
    }

    PromptSelectionResult^ Editor::SelectWindowPolygon( Geometry::Point3dCollection^ polygon )
    {
      return SelectWindowPolygon( polygon, nullptr );
    }

    PromptSelectionResult^ Editor::SelectWindowPolygon( Geometry::Point3dCollection^ polygon, SelectionFilter^ filter )
    {
      g_CurrentEditor = this;
      try
      {
        OdDbDatabasePtr pDb = m_doc->GetImpObj()->database();
        OdDbObjectId vpId = pDb->activeViewportId();

        std::vector< OdGePoint3d > aPts;
        aPts.reserve( polygon->Count );
        for each ( Geometry::Point3d pt in polygon )
          aPts.push_back( *( OdGePoint3d* ) &pt );

        OdDbSelectionSetPtr pSSet = OdDbSelectionSet::select( vpId, aPts.size(), &aPts[0],
                                                              OdDbVisualSelection::kWPoly, OdDbVisualSelection::kDisableSubents,
                                                              getFilter( filter ).get() );

        return gcnew PromptSelectionResult( PromptStatus::OK, pSSet );
      }
      catch (...)
      {
        return gcnew PromptSelectionResult( PromptStatus::Error, OdDbSelectionSet::createObject() );
      }
      g_CurrentEditor = nullptr;
    }

    PromptSelectionResult^ Editor::SelectLast()
    {
      throw gcnew NotSupportedException();
    }

    PromptSelectionResult^ Editor::SelectPrevious()
    {
      throw gcnew NotSupportedException();
    }

    PromptSelectionResult^ Editor::SelectImplied()
    {
      g_CurrentEditor = this;
      TD_START_WRAP_EXCEPTIONS;
      OdDbSelectionSetPtr pSSet = m_doc->GetImpObj()->cmdCtx()->dbUserIO()->pickfirst();
      return gcnew PromptSelectionResult( PromptStatus::OK, pSSet );
      TD_END_WRAP_EXCEPTIONS;
      g_CurrentEditor = nullptr;
    }

    void Editor::SetImpliedSelection( SelectionSet^ selectionSet )
    {
      SetImpliedSelection( selectionSet->GetObjectIds() );
    }

    void Editor::SetImpliedSelection( array< DatabaseServices::ObjectId >^ selectedObjects )
    {
      g_CurrentEditor = this;
      TD_START_WRAP_EXCEPTIONS;
      OdDbSelectionSetPtr pSSet = OdDbSelectionSet::createObject();
      for each ( DatabaseServices::ObjectId id in selectedObjects )
      {
        OdDbObjectId idEnt = *(OdDbStub**) &id;
        pSSet->append( idEnt );
      }
      m_doc->GetImpObj()->cmdCtx()->dbUserIO()->setPickfirst( pSSet );
      TD_END_WRAP_EXCEPTIONS;
      g_CurrentEditor = nullptr;
    }

    void Editor::SwitchToModelSpace()
    {
      TD_START_WRAP_EXCEPTIONS;
      m_doc->GetImpObj()->database()->setTILEMODE( true );
      TD_END_WRAP_EXCEPTIONS;
    }

    void Editor::SwitchToPaperSpace()
    {
      TD_START_WRAP_EXCEPTIONS;
      m_doc->GetImpObj()->database()->setTILEMODE( false );
      TD_END_WRAP_EXCEPTIONS;
    }

    DatabaseServices::ViewTableRecord^ Editor::GetCurrentView()
    {
      TD_START_WRAP_EXCEPTIONS;
      OdDbDatabasePtr pDb = m_doc->GetImpObj()->database();
      
      OdDbViewportTablePtr pVpTab = pDb->getViewportTableId().openObject();
      return (DatabaseServices::ViewTableRecord^) Runtime::RXObject::Create( IntPtr( pVpTab->getActiveViewportId().openObject().get() ), true );
      TD_END_WRAP_EXCEPTIONS;
    }

    void Editor::SetCurrentView( DatabaseServices::ViewTableRecord^ value )
    {
      TD_START_WRAP_EXCEPTIONS;
      OdDbDatabasePtr pDb = m_doc->GetImpObj()->database();
      OdDbViewportTablePtr pVpTab = pDb->getViewportTableId().openObject( OdDb::kForWrite );
      if ( !pVpTab.isNull() )
      {
        DatabaseServices::ObjectId id = value->ObjectId;
        pVpTab->SetActiveViewport( *( OdDbStub** ) &id );
      }
      TD_END_WRAP_EXCEPTIONS;
    }

    int Editor::GetViewportNumber( Point point )
    {
      TD_START_WRAP_EXCEPTIONS;
      OdGsLayoutHelperPtr pDevice = odGetDocDevice( m_doc->GetImpObj()->cDoc() );
      int i, iSize = pDevice->numViews();
      for ( i = 0; i < iSize; i++ )
      {
        OdGsView* pView = pDevice->viewAt( i );
        if ( pView->pointInViewport( OdGePoint2d( point.X, point.Y ) ) )
          return i;
      }
      throw gcnew ArgumentException();
      TD_END_WRAP_EXCEPTIONS;
    }

    DatabaseServices::ObjectId Editor::ViewportIdFromNumber( int viewportNumber )
    {
      TD_START_WRAP_EXCEPTIONS;
      OdGsLayoutHelperPtr pDevice = odGetDocDevice( m_doc->GetImpObj()->cDoc() );
      OdGsView* pView = pDevice->viewAt( viewportNumber );
      if ( nullptr == pView )
        throw gcnew IndexOutOfRangeException();

      OdGsClientViewInfo cInfo;
      pView->clientViewInfo( cInfo );
      return DatabaseServices::ObjectId( IntPtr( cInfo.viewportObjectId ) );
      TD_END_WRAP_EXCEPTIONS;
    }

    Point Editor::PointToScreen( Geometry::Point3d pt, int viewportNumber )
    {
      TD_START_WRAP_EXCEPTIONS;
      OdGsLayoutHelperPtr pDevice = odGetDocDevice( m_doc->GetImpObj()->cDoc() );
      OdGsView* pView = pDevice->viewAt( viewportNumber );
      OdGeMatrix3d mTr = pView->screenMatrix();
      
      OdGePoint3d ptRes( pt.X, pt.Y, pt.Z );
      ptRes.transformBy( mTr );
      return Point( ptRes.x, ptRes.y );
      TD_END_WRAP_EXCEPTIONS;
    }

    Geometry::Point3d Editor::PointToWorld( Point pt )
    {
      TD_START_WRAP_EXCEPTIONS;
      //OdGsLayoutHelperPtr pDevice = odGetDocDevice( m_doc->GetImpObj()->cDoc() );
      //OdGsView* pView = pDevice->activeView();
      OdGeMatrix3d mTr = OdGeMatrix3d::kIdentity;//pView->screenMatrix(); // TODO
      OdGePoint3d ptRes( pt.X, pt.Y, 0 );
      ptRes.transformBy( mTr );
      return Geometry::Point3d( ptRes.x, ptRes.y, ptRes.z );
      TD_END_WRAP_EXCEPTIONS;
    }

    Geometry::Point3d Editor::PointToWorld( Point pt, int /*viewportNumber*/ )
    {
      TD_START_WRAP_EXCEPTIONS;
      //OdGsLayoutHelperPtr pDevice = odGetDocDevice( m_doc->GetImpObj()->cDoc() );
      //OdGsView* pView = pDevice->viewAt( viewportNumber );
      OdGeMatrix3d mTr = OdGeMatrix3d::kIdentity;//pView->screenMatrix(); // TODO
      OdGePoint3d ptRes( pt.X, pt.Y, 0 );
      ptRes.transformBy( mTr );
      return Geometry::Point3d( ptRes.x, ptRes.y, ptRes.z );
      TD_END_WRAP_EXCEPTIONS;
    }

    int Editor::GetCommandVersion()
    {
      return 0;
    }

    int Editor::InitCommandVersion( int /*nVersion*/ )
    {
      return 0;
    }

    void Editor::Regen()
    {
      TD_START_WRAP_EXCEPTIONS;
      OdGsLayoutHelperPtr pDevice = odGetDocDevice( m_doc->GetImpObj()->cDoc() );
      pDevice->invalidate();
      if( pDevice->gsModel() )
        pDevice->gsModel()->invalidate( OdGsModel::kInvalidateAll );

      UpdateScreen();
      TD_END_WRAP_EXCEPTIONS;
    }

    void Editor::UpdateScreen()
    {
      TD_START_WRAP_EXCEPTIONS;
      OdGsLayoutHelperPtr pDevice = odGetDocDevice( m_doc->GetImpObj()->cDoc() );
      OdGsClientViewInfo cInfo;
      pDevice->activeView()->clientViewInfo( cInfo );
      ::PostMessage( ( HWND )cInfo.acadWindowId, WM_PAINT, 0, 0 );
      TD_END_WRAP_EXCEPTIONS;
    }

    void Editor::UpdateTiledViewportsFromDatabase()
    {
      // TODO:
    }

    void Editor::UpdateTiledViewportsInDatabase()
    {
      // TODO:
    }

    Geometry::Point3d Editor::Snap( String^ /*snapMode*/, Geometry::Point3d input )
    {
      odGetDocOsnapPoint( m_doc->GetImpObj()->cDoc(), *(OdGePoint3d*) &input );
      return input;
    }

    DatabaseServices::ObjectId Editor::ActiveViewportId::get()
    {
      TD_START_WRAP_EXCEPTIONS;
      OdDbDatabasePtr pDb = m_doc->GetImpObj()->database();
      return DatabaseServices::ObjectId( IntPtr( (OdDbStub*) pDb->activeViewportId() ) );
      TD_END_WRAP_EXCEPTIONS;
    }

    Geometry::Matrix3d Editor::CurrentUserCoordinateSystem::get()
    {
      TD_START_WRAP_EXCEPTIONS;
      OdDbDatabasePtr pDb = m_doc->GetImpObj()->database();
      DatabaseServices::UcsTableRecord^ ucs = (DatabaseServices::UcsTableRecord^)
        Runtime::RXObject::Create( IntPtr( pDb->getUCSTableId().openObject().get() ), true );
      
      return Geometry::Matrix3d::AlignCoordinateSystem( Geometry::Point3d::Origin,
                                                        Geometry::Vector3d::XAxis,
                                                        Geometry::Vector3d::YAxis,
                                                        Geometry::Vector3d::ZAxis,
                                                        ucs->Origin,
                                                        ucs->XAxis,
                                                        ucs->YAxis,
                                                        ucs->XAxis.CrossProduct( ucs->YAxis ) );
      TD_END_WRAP_EXCEPTIONS;
    }

    DatabaseServices::ObjectId Editor::CurrentViewportObjectId::get()
    {
      return ActiveViewportId; // TODO:
    }

    ApplicationServices::Document^ Editor::Document::get()
    {
      return m_doc;
    }
    
    bool Editor::IsDragging::get()
    {
      return false; // TODO:
    }
    
    bool Editor::IsQuiescent::get()
    {
      return false; // TODO:
    }
    
    bool Editor::IsQuiescentForTransparentCommand::get()
    {
      return false; // TODO:
    }
    
    bool Editor::MouseHasMoved::get()
    {
      return false; // TODO:
    }
    
    bool Editor::UseCommandLineInterface::get()
    {
      return false; // TODO:
    }
  }
}

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

#include "DocumentCollection.h"
#include "Document.h"

namespace Teigha
{
  namespace ApplicationServices
  {
    class OdMgdDocManagerReactor : public OdApplicationReactor
    {
      public:
        static OdSmartPtr< OdMgdDocManagerReactor > createObject( DocumentCollection^ owner )
        {
          TD_START_WRAP_EXCEPTIONS;
          OdSmartPtr< OdMgdDocManagerReactor > pRes = OdRxObjectImpl< OdMgdDocManagerReactor >::createObject();
          pRes->m_Owner = gcnew WeakReference( owner );
          return pRes;
          TD_END_WRAP_EXCEPTIONS;
        }

        virtual void documentCreateStarted( CDocument* pDocCreating )
        {
          if ( m_Owner->IsAlive )
            ((DocumentCollection^)m_Owner->Target)->DocumentCreateStarted( m_Owner->Target, gcnew DocumentCollectionEventArgs( Document::Create( pDocCreating ) ) );
        }

        virtual void documentCreated( CDocument* pDocCreating )
        {
          if ( m_Owner->IsAlive )
            ((DocumentCollection^)m_Owner->Target)->DocumentCreated( m_Owner->Target, gcnew DocumentCollectionEventArgs( Document::Create( pDocCreating ) ) );
        }

        virtual void documentToBeDestroyed( CDocument* pDocToDestroy )
        {
          if ( m_Owner->IsAlive )
            ((DocumentCollection^)m_Owner->Target)->DocumentToBeDestroyed( m_Owner->Target, gcnew DocumentCollectionEventArgs( Document::Create( pDocToDestroy ) ) );
        }

        virtual void documentDestroyed( const OdString& fileName )
        {
          if ( m_Owner->IsAlive )
            ((DocumentCollection^)m_Owner->Target)->DocumentDestroyed( m_Owner->Target, gcnew DocumentDestroyedEventArgs( gcnew String( (const wchar_t*) fileName ) ) );
        }

        virtual void documentCreateCanceled( CDocument* pDocCreateCancelled )
        {
          if ( m_Owner->IsAlive )
            ((DocumentCollection^)m_Owner->Target)->DocumentCreationCanceled( m_Owner->Target, gcnew DocumentCollectionEventArgs( Document::Create( pDocCreateCancelled ) ) );
        }

        virtual void documentBecameCurrent( CDocument* pDoc )
        {
          if ( m_Owner->IsAlive )
            ((DocumentCollection^)m_Owner->Target)->DocumentBecameCurrent( m_Owner->Target, gcnew DocumentCollectionEventArgs( Document::Create( pDoc ) ) );
        }

        virtual void documentToBeActivated( CDocument* pActivatingDoc )
        {
          if ( m_Owner->IsAlive )
            ((DocumentCollection^)m_Owner->Target)->DocumentToBeActivated( m_Owner->Target, gcnew DocumentCollectionEventArgs( Document::Create( pActivatingDoc ) ) );
        }

        virtual void documentToBeDeactivated( CDocument* pDeActivatedDoc )
        {
          if ( m_Owner->IsAlive )
            ((DocumentCollection^)m_Owner->Target)->DocumentToBeDeactivated( m_Owner->Target, gcnew DocumentCollectionEventArgs( Document::Create( pDeActivatedDoc ) ) );
        }

        virtual void DocumentActivated( CDocument* pActivatedDoc )
        {
          if ( m_Owner->IsAlive )
            ((DocumentCollection^)m_Owner->Target)->DocumentCreated( m_Owner->Target, gcnew DocumentCollectionEventArgs( Document::Create( pActivatedDoc ) ) );
        }

      private:
        gcroot< WeakReference^ > m_Owner;
    };

    DocumentCollection::DocumentCollection()
    {
      TD_START_WRAP_EXCEPTIONS;
      odAddAppReactor( OdMgdDocManagerReactor::createObject( this ).get() );
      TD_END_WRAP_EXCEPTIONS;
    }

    CDocTemplate* getDocTemplate()
    {
      CWinApp* pApp = AfxGetApp();

      POSITION pos = pApp->GetFirstDocTemplatePosition();
      CDocTemplate* pDwgTemplate = pApp->GetNextDocTemplate( pos );
      return pDwgTemplate;
    }

    void DocumentCollection::CloseAll()
    {
      CWinApp* pApp = AfxGetApp();
      pApp->CloseAllDocuments( false );
    }

    Document^ DocumentCollection::GetDocument( Database^ db )
    {
      TD_START_WRAP_EXCEPTIONS;
      for each ( Document^ doc in this )
      {
        if ( IntPtr( doc->GetImpObj()->database().get() ) == db->UnmanagedObject )
          return doc;
      }
      return nullptr;
      TD_END_WRAP_EXCEPTIONS;
    }

    private ref class DocumentIterator : public Runtime::DisposableWrapper, public Collections::IEnumerator
    {
      internal:
        DECLARE_IMP( CDocTemplate );

        DocumentIterator()
          : Runtime::DisposableWrapper( IntPtr( getDocTemplate() ), false )
        {
          m_pos = 0;
        }

      public:
        virtual bool MoveNext()
        {
          if ( m_pos == 0 )
          {
            m_pos = GetImpObj()->GetFirstDocPosition();
          }
          else
          {
            POSITION pos = m_pos;
            GetImpObj()->GetNextDoc( pos );
            m_pos = pos;
          }

          return m_pos != 0;
        }

        virtual void Reset()
        {
          m_pos = 0;
        }

        property virtual Object^ Current
        {
          Object^ get()
          {
            if ( m_pos == 0 )
              throw gcnew InvalidOperationException();

            POSITION pos = m_pos;
            CDocument* pDoc = GetImpObj()->GetNextDoc( pos );
            return Document::Create( pDoc );
          }
        }

      protected:
        virtual void DeleteUnmanagedObject() override
        {
        }

      private:
        POSITION m_pos;
    };

    Collections::IEnumerator^ DocumentCollection::GetEnumerator()
    {
      return gcnew DocumentIterator();
    }

    Document^ DocumentCollection::Open( String^ fileName )
    {
      return Open( fileName, true, nullptr );
    }

    Document^ DocumentCollection::Open( String^ fileName, bool forReadOnly )
    {
      return Open( fileName, forReadOnly, nullptr );
    }

    Document^ DocumentCollection::Open( String^ fileName, bool /*forReadOnly*/, String^ /*password*/ )
    {
      pin_ptr< const wchar_t > pFileName = PtrToStringChars( fileName );
      CWinApp* pApp = AfxGetApp();
      return Document::Create( pApp->OpenDocumentFile( pFileName ) );
    }

    Document^ DocumentCollection::MdiActiveDocument::get()
    {
      CMDIFrameWnd* pMainFrame = (CMDIFrameWnd*)AfxGetMainWnd();
      CFrameWnd* pFrame = pMainFrame->GetActiveFrame();
      if ( pFrame != nullptr )
      {
        CDocument* pDoc = pFrame->GetActiveDocument();
        if ( pDoc != nullptr )
          return Document::Create( pDoc );
      }
      return nullptr;
    }

    void DocumentCollection::MdiActiveDocument::set( Document^ doc )
    {
      CDocument* pDoc = doc->GetImpObj()->cDoc();

      POSITION pos = pDoc->GetFirstViewPosition();
      CView* pView = pDoc->GetNextView( pos );
      CFrameWnd* pFrame = pView->GetParentFrame();
      pFrame->ActivateFrame();
    }

    void DocumentCollection::CopyTo( array<Document^>^ arr, int index )
    {
      CopyTo( (Array^) arr, index );
    }

    void DocumentCollection::CopyTo( Array^ arr, int index )
    {
      for each ( Object^ doc in this )
      {
        arr->SetValue( doc, index++ );
      }
    }

    int DocumentCollection::Count::get()
    {
      int iSize = 0;
      for each ( Object^ o in this )
      {
        o;iSize++;
      }
      return iSize;
    }
  }
}

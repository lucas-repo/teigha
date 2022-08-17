Imports System
Imports Teigha.Core
Imports Teigha.TD


Public Class DisplayControl
    Public m_pDevice As OdGsDevice
    Public CurPalette() As UInteger
    Public Context As OdGiDefaultContext
    Public TDDatabase As OdDbDatabase

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        m_pDevice = Nothing
    End Sub

    Public Sub SetViewportBorderProperties(ByVal pDevice As OdGsDevice, ByVal bModel As Boolean)
        Dim n As Integer = pDevice.numViews()

        If n > 1 Then
            Dim i As Integer = 1
            If bModel Then
                i = 0
            End If
            While i < n
                Dim pView As OdGsView = pDevice.viewAt(i)
                pView.setViewportBorderProperties(CurPalette(7), 1)
                i = i + 1
            End While
        End If
    End Sub


    Public Sub ResetDevice()
        Dim rc As Rectangle = Bounds
        Dim pGs As OdGsModule = Globals.odrxDynamicLinker().loadModule("WinGDI.txv")
        m_pDevice = pGs.createDevice()
        Dim Properties As OdRxDictionary = m_pDevice.properties()
        Properties.putAt("WindowHWND", New OdRxVariantValue(Handle.ToInt32()))
        ';;;;;;;;;;;;;;;;;;;;;;;;;;;
        Dim gsRext As OdGsDCRect = New OdGsDCRect(rc.Left, rc.Right, rc.Bottom, rc.Top)

        CurPalette = Teigha.Core.AllPalettes.getLightPalette()
        m_pDevice.setBackgroundColor(CurPalette(0))
        m_pDevice.setLogicalPalette(CurPalette, 256)
        Dim Ctx1 As OdGiContextForDbDatabase = OdGiContextForDbDatabase.createObject()
        Ctx1.enableGsModel(True)
        Ctx1.setDatabase(TDDatabase)
        m_pDevice.setUserGiContext(Ctx1)
        m_pDevice = TD_Db.setupActiveLayoutViews(m_pDevice, Ctx1)
        Dim bModelSpace As Boolean = TDDatabase.getTILEMODE()
        SetViewportBorderProperties(m_pDevice, Not bModelSpace)

        Dim pView As OdGsView = m_pDevice.viewAt(0)
        Dim pViewPE As OdAbstractViewPE = OdAbstractViewPE.cast(pView)
        pViewPE.zoomExtents(pView)

        OnResize(EventArgs.Empty)
        ';;;;;;;;;;;;;;;;;;;;;;;;;;;
    End Sub
    Public Sub DeleteContext()
        If m_pDevice Is Nothing Then
            m_pDevice.Dispose()
            m_pDevice = Nothing
        End If
    End Sub
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        If m_pDevice IsNot Nothing Then
            Dim r As Rectangle = Bounds
            r.Offset(-Location.X, -Location.Y)
            m_pDevice.onSize(New OdGsDCRect(r.Left, r.Right, r.Bottom, r.Top))
        End If
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If m_pDevice IsNot Nothing Then
            m_pDevice.update()
        End If
        If m_pDevice Is Nothing Then
            MyBase.OnPaint(e)
        End If
    End Sub

    Protected Overrides Sub OnMouseWheel(ByVal e As MouseEventArgs)
        Dim view As OdGsView = m_pDevice.viewAt(0)
        Dim currentPosition As OdGePoint3d = view.position()
        Dim mousePosition As OdGePoint3d = New OdGePoint3d(e.X, e.Y, 0)
        Dim moveVector As OdGeVector3d = currentPosition - mousePosition
        moveVector *= -1
        moveVector = moveVector.transformBy((view.screenMatrix() * view.projectionMatrix()).inverse())
        view.dolly(moveVector)
        currentPosition = view.Position()
        Dim pVpPE As OdAbstractViewPE = OdAbstractViewPE.cast(view)
        Dim eyeToWorldMatrix As OdGeMatrix3d = pVpPE.eyeToWOrld(view)
        Dim wcsPt As OdGePoint3d = currentPosition.transformBy(eyeToWOrldMatrix)
        If e.Delta > 0 Then
            view.zoom(1.0 / 0.90000000000000002)
        End If
        If e.Delta <= 0 Then
            view.zoom(0.90000000000000002)
        End If
        moveVector *= -1
        view.dolly(moveVector)
        Invalidate()
    End Sub
    '''''''''''''''
End Class

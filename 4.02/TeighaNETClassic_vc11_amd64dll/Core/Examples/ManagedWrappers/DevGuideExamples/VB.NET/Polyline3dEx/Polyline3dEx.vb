'/////////////////////////////////////////////////////////////////////////////// 
'// Copyright (C) 2002-2016, Open Design Alliance (the "Alliance"). 
'// All rights reserved. 
'// 
'// This software and its documentation and related materials are owned by 
'// the Alliance. The software may only be incorporated into application 
'// programs owned by members of the Alliance, subject to a signed 
'// Membership Agreement and Supplemental Software License Agreement with the
'// Alliance. The structure and organization of this software are the valuable  
'// trade secrets of the Alliance and its suppliers. The software is also 
'// protected by copyright law and international treaty provisions. Application  
'// programs incorporating this software must include the following statement 
'// with their copyright notices:
'//   
'//   This application incorporates Teigha(R) software pursuant to a license 
'//   agreement with Open Design Alliance.
'//   Teigha(R) Copyright (C) 2002-2016 by Open Design Alliance. 
'//   All rights reserved.
'//
'// By use of this software, its documentation or related materials, you 
'// acknowledge and accept the above terms.
'///////////////////////////////////////////////////////////////////////////////
Public Class Polyline3dEx
  Public Sub New(ByVal path As String)
    Using db As Database = New Database(True, True)
      Dim tm As TransactionManager
      tm = db.TransactionManager

      Using ta As Transaction = tm.StartTransaction()
        Using btr As BlockTableRecord = CType(db.CurrentSpaceId.GetObject(Teigha.DatabaseServices.OpenMode.ForWrite), BlockTableRecord)
          REM Creates Polyline3d entity with specified parameters
          Dim pLine1 As Polyline3d = New Polyline3d(Poly3dType.SimplePoly, createCollectionForPolyline(1), False)
          btr.AppendEntity(pLine1)
          printPlolylineParams(pLine1)

          REM Creates empty Polyline3d entity then append vertices and set properties
          Dim pLine2 As Polyline3d = New Polyline3d()
          btr.AppendEntity(pLine2)
          pLine2.Closed = False
          For Each pt As Point3d In createCollectionForPolyline(2)
            pLine2.AppendVertex(New PolylineVertex3d(pt))
          Next
          REM Converts created SimplePoly polyline to QuadSplinePoly type
          pLine2.ConvertToPolyType(Poly3dType.QuadSplinePoly)
          printPlolylineParams(pLine2)

          REM Creates Polyline3d entity with specified parameters
          Dim pLine3 As Polyline3d = New Polyline3d(Poly3dType.SimplePoly, createCollectionForPolyline(3), False)
          btr.AppendEntity(pLine3)
          pLine3.Closed = True
          REM Converts created SimplePoly polyline to QuadSplinePoly type
          pLine3.ConvertToPolyType(Poly3dType.QuadSplinePoly)
          REM Straightens QuadSplinePoly polyline
          pLine3.Straighten()
          REM Converts created SimplePoly polyline to CubicSplinePoly type
          pLine3.ConvertToPolyType(Poly3dType.CubicSplinePoly)
          printPlolylineParams(pLine3)

          REM Creates Polyline3d entity with specified parameters
          Dim pLine4 As Polyline3d = New Polyline3d(Poly3dType.SimplePoly, createCollectionForPolyline(4), False)
          btr.AppendEntity(pLine4)

          REM Vertices to be inserted to SimplePoly polyline
          Dim vrtx1 As PolylineVertex3d = New PolylineVertex3d(New Point3d(45, -1, -3))
          Dim vrtx2 As PolylineVertex3d = New PolylineVertex3d(New Point3d(45, 12, -3))

          Dim verticesID(12) As ObjectId
          Dim j As Short = 0
          For Each obj As ObjectId In pLine4
            Using dbObj As DBObject = CType(tm.GetObject(obj, Teigha.DatabaseServices.OpenMode.ForRead), DBObject)
              If (TypeOf dbObj Is PolylineVertex3d) Then
                REM Gets all vertices IDs
                verticesID(j) = obj
                j += 1
              End If
            End Using
          Next

          REM Insrets vertices
          pLine4.InsertVertexAt(ObjectId.Null, vrtx1)
          pLine4.InsertVertexAt(verticesID(11), vrtx2)

          REM Creates spline fitted polyline of CubicSplinePoly type and segments number of 2
          pLine4.SplineFit(Poly3dType.CubicSplinePoly, 2)
          printPlolylineParams(pLine4)
        End Using
            ta.Commit()
        End Using
        db.SaveAs(path + "Polyline3dEx.dwg", DwgVersion.Current)
      End Using
  End Sub
  REM Creates collection of Point3ds as a polyline vertices
  Function createCollectionForPolyline(ByVal i As Integer) As Point3dCollection

    Dim p3d(12) As Point3d

    p3d(0) = New Point3d(6 + 10 * i, 0, 0)
    p3d(1) = New Point3d(1 + 10 * i, 1, 0)
    p3d(2) = New Point3d(0 + 10 * i, 2, -3)
    p3d(3) = New Point3d(5 + 10 * i, 3, -3)

    p3d(4) = New Point3d(6 + 10 * i, 4, 0)
    p3d(5) = New Point3d(1 + 10 * i, 5, 0)
    p3d(6) = New Point3d(0 + 10 * i, 6, -3)
    p3d(7) = New Point3d(5 + 10 * i, 7, -3)

    p3d(8) = New Point3d(6 + 10 * i, 8, 0)
    p3d(9) = New Point3d(1 + 10 * i, 9, 0)
    p3d(10) = New Point3d(0 + 10 * i, 10, -3)
    p3d(11) = New Point3d(5 + 10 * i, 11, -3)

    Return New Point3dCollection(p3d)
  End Function


  REM Prints polyline parameters to the console
  Sub printPlolylineParams(ByVal pLine As Polyline3d)
    Console.WriteLine("Polytype is " + pLine.PolyType.ToString())
    Console.WriteLine("Closed is " + pLine.Closed.ToString())
    Dim i As Integer = 0

    REM Gets vertices of a polyline and prints parameters of control and simple vertices 
    For Each objId As ObjectId In pLine
      Using obj As DBObject = CType(objId.GetObject(Teigha.DatabaseServices.OpenMode.ForRead), DBObject)
        If (TypeOf obj Is PolylineVertex3d) Then
          Dim pt As PolylineVertex3d = CType(obj, PolylineVertex3d)
          If ((pt.VertexType = Vertex3dType.ControlVertex) Or (pt.VertexType = Vertex3dType.SimpleVertex)) Then
            Console.WriteLine("Vertex #" + i.ToString() + ": " + pt.Position.ToString())
            i += 1
          End If
        End If
      End Using
    Next
    Console.WriteLine("Length is " + pLine.Length.ToString() + System.Environment.NewLine)
  End Sub

End Class

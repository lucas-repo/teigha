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

using System;
using Teigha.Core;

namespace OdBrExSwigMgd
{
  class Traverse
  {
    bool doLoopToCoEdge = false;
    string entityIdString(OdGe.EntityId type)
    {
      switch (type)
      {
        case OdGe.EntityId.kEllipArc3d:
          return "EllipArc3d";
        case OdGe.EntityId.kNurbCurve3d:
          return "NurbCurve3d";
        case OdGe.EntityId.kLineSeg3d:
          return "LineSeg3d";
        case OdGe.EntityId.kCone:
          return "GeCone";
        case OdGe.EntityId.kEllipCone:
          return "EllipCone";
        case OdGe.EntityId.kCylinder:
          return "Cylinder";
        case OdGe.EntityId.kEllipCylinder:
          return "EllipCylinder";
        case OdGe.EntityId.kNurbSurface:
          return "NurbSurface";
        case OdGe.EntityId.kPlane:
          return "Plane";
        case OdGe.EntityId.kSphere:
          return "Sphere";
        case OdGe.EntityId.kTorus:
          return "Torus";
        default:
          break;
      };

      return "Unknown";
    }

    bool findEdge(OdBrBrep aBrep, int edgeNum, out OdBrEdge edge)
    {
      OdBrBrepEdgeTraverser bet = new OdBrBrepEdgeTraverser();
      bet.setBrep(aBrep);
      int num = 0;
      while (!bet.done())
      {
        edge = bet.getEdge();
        if (num == edgeNum)
          return true;
        bet.next(); ++num;
      }
      edge = new OdBrEdge();
      return false;
    }

    void processBrep(OdBrBrep aBrep)
    {
      OdBrBrepComplexTraverser bct = new OdBrBrepComplexTraverser();
      bct.setBrep(aBrep);
      int complexNum = 0;
      Console.WriteLine("Start at which Complex (0 to start with first): ");
      int firstID = -1;
      firstID = Process.getConsoleInt();
      if (firstID >= 0)
      {
        bool foundIT = false;
        while (!bct.done())
        {
          OdBrComplex comp = bct.getComplex();
          if (complexNum == firstID)
          {
            bct.setComplex(comp);
            foundIT = true;
            break;
          }
          bct.next(); ++complexNum;
        }
        if (!foundIT)
        {
          Console.WriteLine("complex " + firstID + " is not found.");
          bct.restart();
        }
      }

      complexNum = 0;
      while (!bct.done())
      {
        OdBrComplex cur = bct.getComplex();
        Console.Write("\n\tComplex " + complexNum);

        processComplex(cur);

        complexNum++;
        bct.next();
      }
    }

    void processBrepToEdgeOnly(OdBrBrep aBrep)
    {
      OdBrBrepEdgeTraverser bet = new OdBrBrepEdgeTraverser();
      bet.setBrep(aBrep);
      int edgeNum = 0;
      Console.WriteLine("Start at which Edge (0 to start with first): ");
      int firstID;
      firstID = Process.getConsoleInt();
      OdBrEdge edg = new OdBrEdge();

      if (firstID >= 0)
      {
        if (findEdge(aBrep, firstID, out edg))
          bet.setEdge(edg);
        else
          Console.WriteLine(firstID + " is not found.\n");
      }

      edgeNum = 0;
      while (!bet.done())
      {
        OdBrEdge cur = bet.getEdge();
        Console.Write("\n\tEdge " + edgeNum + "\t");

        OdBrVertex v1 = new OdBrVertex();
        OdBrVertex v2 = new OdBrVertex();
        cur.getVertex1(v1);
        cur.getVertex2(v2);

        OdGe.EntityId eID = cur.getCurveType();
        Console.Write(" on curve type " + entityIdString(eID));

        edgeNum++;
        bet.next();
      }
    }

    void processBrepToFaceOnly(OdBrBrep aBrep)
    {
      OdBrBrepFaceTraverser bft = new OdBrBrepFaceTraverser();
      bft.setBrep(aBrep);
      int faceNum = 0;

      Console.WriteLine("Start at which Face (satfile ID; 0 to start with first): ");
      int firstID;
      firstID = Process.getConsoleInt();
      if (firstID >= 0)
      {
        bool foundIT = false;
        while (!bft.done())
        {
          OdBrFace face = bft.getFace();
          if (faceNum == firstID)
          {
            bft.setFace(face);
            foundIT = true;
            break;
          }
          bft.next(); ++faceNum;
        }
        if (!foundIT)
        {
          Console.WriteLine(firstID + " is not found.\n");
          bft.restart();
        }
      }

      faceNum = 0;
      while (!bft.done())
      {
        OdBrFace cur = bft.getFace();
        Console.Write("\n\tFace " + faceNum);

        OdGe.EntityId eID = new OdGe.EntityId();
        //cur.getSurfaceType(eID); //access error
        eID = cur.getSurface().type();
        if (eID == OdGe.EntityId.kExternalBoundedSurface)
        {
          OdGeExternalBoundedSurface exSurf = new OdGeExternalBoundedSurface();
          exSurf.Assign(cur.getSurface());
          OdGeSurface theSurface = exSurf.getBaseSurfaceEx();
          eID = theSurface.type();
        }

        Console.Write(" on surface type " + entityIdString(eID));

        faceNum++;
        bft.next();
      }
    }

    void processBrepToShellOnly(OdBrBrep aBrep)
    {
      OdBrBrepShellTraverser bst = new OdBrBrepShellTraverser();
      bst.setBrep(aBrep);
      int shellNum = 0;
      Console.WriteLine("Start at which Shell (satfile ID; 0 to start with first): ");
      int firstID;
      firstID = Process.getConsoleInt();
      if (firstID >= 0)
      {
        bool foundIT = false;
        while (!bst.done())
        {
          OdBrShell shell = bst.getShell();
          if (shellNum == firstID)
          {
            bst.setShell(shell);
            foundIT = true;
            break;
          }
          bst.next(); ++shellNum;
        }
        if (!foundIT)
        {
          Console.WriteLine(firstID + " is not found.\n");
          bst.restart();
        }
      }

      shellNum = 0;
      while (!bst.done())
      {
        OdBrShell cur = bst.getShell();
        Console.Write("\n\tShell " + shellNum);

        bst.next(); ++shellNum;
      }
    }

    void processBrepToVertexOnly(OdBrBrep aBrep)
    {
      OdBrBrepVertexTraverser bvt = new OdBrBrepVertexTraverser();
      bvt.setBrep(aBrep);
      int vertexNum = 0;
      Console.WriteLine("Start at which Vertex (0 to start with first): ");
      int firstID;
      firstID = Process.getConsoleInt();
      if (firstID >= 0)
      {
        bool foundIT = false;
        while (!bvt.done())
        {
          OdBrVertex vtx = bvt.getVertex();
          if (vertexNum == firstID)
          {
            bvt.setVertex(vtx);
            foundIT = true;
            break;
          }
          bvt.next(); ++vertexNum;
        }
        if (!foundIT)
        {
          Console.WriteLine(firstID + " is not found.\n");
          bvt.restart();
        }
      }
      vertexNum = 0;
      while (!bvt.done())
      {
        OdBrVertex cur = bvt.getVertex();
        Console.Write("\n\tVertex " + vertexNum);
        OdGePoint3d pnt = cur.getPoint();
        Console.Write(" at (" + Process.numToString(pnt.x) + ", "
          + Process.numToString(pnt.y) + ", "
          + Process.numToString(pnt.z) + ')');

        vertexNum++;
        bvt.next();
      }
    }

    void processLoopEdge(OdBrLoopEdgeTraverser elt)
    {
    }

    void processEdgeLoop(OdBrLoopEdgeTraverser elt)
    {
    }

    void processComplex(OdBrComplex aComp)
    {
      OdBrComplexShellTraverser cst = new OdBrComplexShellTraverser();
      cst.setComplex(aComp);
      int shellNum = 0;
      while (!cst.done())
      {
        OdBrShell cur = cst.getShell();
        Console.Write("\n\t\tShell: " + shellNum);

        processShell(cur);

        ++shellNum;
        cst.next();
      }
    }

    void processFace(OdBrFace f)
    {
      OdBrFaceLoopTraverser flt = new OdBrFaceLoopTraverser();
      flt.setFace(f);
      int loopNum = 0;
      while (!flt.done())
      {
        OdBrLoop cur = flt.getLoop();
        Console.Write("\n\t\t\t\tLoop: " + loopNum);

        if (doLoopToCoEdge)
          processLoopToEdge(cur);
        else
          processLoopToVertex(cur);

        loopNum++;
        flt.next();
      }
    }

    void processLoopToEdge(OdBrLoop l)
    {
      OdBrLoopEdgeTraverser lct = new OdBrLoopEdgeTraverser();
      OdBrErrorStatus err = lct.setLoop(l);
      if (err == OdBrErrorStatus.odbrDegenerateTopology)
      {
        Console.WriteLine("\t\t\t\t singularities(need OdBrLoopVertexTraverser)\n");
        return;
      }

      int edgeNum = 0;
      while (!lct.done())
      {
        OdBrEdge cur = lct.getEdge();
        Console.Write("\n\t\t\t\t\tEdge: " + edgeNum);

        processLoopEdge(lct);

        ++edgeNum;
        lct.next();
      }
    }

    void processLoopToVertex(OdBrLoop l)
    {
      OdBrLoopVertexTraverser lvt = new OdBrLoopVertexTraverser();
      lvt.setLoop(l);
      int vertexNum = 0;
      while (!lvt.done())
      {
        OdBrVertex cur = lvt.getVertex();
        Console.Write("\n\t\t\t\t\tVertex: " + vertexNum);
        processVertex(cur);

        ++vertexNum;
        lvt.next();
      }
    }

    void processShell(OdBrShell aShell)
    {
      OdBrShellFaceTraverser sft = new OdBrShellFaceTraverser();
      sft.setShell(aShell);
      int faceNum = 0;
      while (!sft.done())
      {
        OdBrFace cur = sft.getFace();
        Console.Write("\n\t\t\tFace: " + faceNum);

        processFace(cur);

        faceNum++;
        sft.next();
      }
    }

    void processVertex(OdBrVertex vtx)
    {
      //Console.WriteLine(/* coords and then */ '\n');
    }

    public void traverseBrep(OdBrBrep aBrep, int how)
    {
      Console.WriteLine("Brep");

      if ((how == 1) || (how == 2))
      {
        doLoopToCoEdge = (how == 1);
        processBrep(aBrep);
      }
      else if (how == 3)
        processBrepToShellOnly(aBrep);
      else if (how == 4)
        processBrepToFaceOnly(aBrep);
      else if (how == 5)
        processBrepToEdgeOnly(aBrep);
      else if (how == 6)
        processBrepToVertexOnly(aBrep);
    }

  }
}

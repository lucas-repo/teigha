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
  class Search
  {
    void dumpCone(OdGeCone theCone)
    {
      Console.Write("\n\tCone apex: ");
      dumpPoint(theCone.apex());

      Console.Write("\n\tCone axis: ");
      dumpVector(theCone.axisOfSymmetry());

      Console.Write("\n\tCone base center: ");
      dumpPoint(theCone.baseCenter());

      Console.Write("\n\tCone base radius: " + Process.numToString(theCone.baseRadius()));
      Console.Write("\n\tCone half angle: " + theCone.halfAngle() + "\n");
    }

    void dumpEllipCone(OdGeEllipCone theEllipCone)
    {
      Console.Write("\n\tElliptical cone apex: ");
      dumpPoint(theEllipCone.apex());

      Console.Write("\n\tElliptical cone axis: ");
      dumpVector(theEllipCone.axisOfSymmetry());

      Console.Write("\n\tElliptical cone base center: ");
      dumpPoint(theEllipCone.baseCenter());

      Console.Write("\n\tElliptical cone minor radius: " + Process.numToString(theEllipCone.minorRadius()));
      Console.Write("\n\tElliptical cone major radius: " + Process.numToString(theEllipCone.majorRadius()));
      Console.Write("\n\tElliptical cone half angle: " + theEllipCone.halfAngle() + "\n");
    }

    void dumpCylinder(OdGeCylinder theCyl)
    {
      Console.Write("\n\tCylinder base: ");
      dumpPoint(theCyl.origin());
      Console.Write("\n\tCylinder axis: ");
      dumpVector(theCyl.axisOfSymmetry());
      Console.Write("\n\tCylinder radius: " + Process.numToString(theCyl.radius()) + "\n");
    }

    void dumpEllipCylinder(OdGeEllipCylinder theCyl)
    {
      if (theCyl == null)
      {
        Console.WriteLine("NULL elliptical cylinder!");
        return;
      }
      Console.Write("\n\tElliptical cylinder base: ");
      dumpPoint(theCyl.origin());
      Console.Write("\n\tElliptical cylinder axis: ");
      dumpVector(theCyl.axisOfSymmetry());
      Console.Write("\n\tElliptical cylinder major radius: " + Process.numToString(theCyl.majorRadius()));
      Console.Write("\n\tElliptical cylinder minor radius: " + Process.numToString(theCyl.minorRadius()) + "\n");
    }

    void dumpEllipse(OdGeEllipArc3d ell)
    {
      if (ell == null)
      {
        Console.WriteLine("NULL ellipse!");
        return;
      }
      Console.Write("(C,u,v,R,r) = (");
      dumpPoint(ell.center());
      dumpVector(ell.majorAxis());
      dumpVector(ell.minorAxis());
      Console.Write(Process.numToString(ell.majorRadius()) + ", " + Process.numToString(ell.minorRadius()) + ")\n");
    }

    void dumpLine(OdGeLineSeg3d lin)
    {
      if (lin == null)
      {
        Console.WriteLine("NULL line!");
        return;
      }
      Console.Write("\n\tLine base point: ");
      dumpPoint(lin.pointOnLine());
      Console.Write("\n\tLine direction: ");
      dumpVector(lin.direction());
    }

    void dumpNurbCurve(OdGeNurbCurve3d nc)
    {
      if (nc == null)
      {
        Console.WriteLine("NULL nurbs curve!");
        return;
      }
      int degree;
      bool rational;
      bool periodic;
      OdGeKnotVector knots = new OdGeKnotVector();
      OdGeDoubleArray wts = new OdGeDoubleArray();
      OdGePoint3dArray cPts = new OdGePoint3dArray();
      nc.getDefinitionData(out degree, out rational, out periodic, knots, cPts, wts);
      Console.Write("Degree = " + degree + ", rational = " + rational
           + ", periodic = " + periodic + '\n'
           + "\t#knots = " + knots.length() + "\n\tknots: (");
      for (int i = 0; i < knots.length(); i++)
      {
        Console.Write(Process.numToString(knots[i]));
        if (i != (knots.length() - 1))
          Console.Write(", ");
      }
      Console.Write(")\n\t#cpts = " + cPts.Count + "\n\tControl points  weights: \n\t(");
      for (int j = 0; j < cPts.Count; j++)
      {
        dumpPoint(cPts[j]);
        Console.Write(", " + Process.numToString((wts.Count == 0 ? 1.0 : wts[j])) + ")");
        if (j != cPts.Count - 1)
          Console.Write(",\n\t");
      }
      Console.Write(")\n");
    }

    void dumpNurbSurface(OdGeNurbSurface theNurb)
    {
      int degreeU, degreeV, propsU, propsV, nCptsU, nCptsV;
      OdGePoint3dArray cPts = new OdGePoint3dArray();
      OdGeDoubleArray weights = new OdGeDoubleArray();
      OdGeKnotVector uKnots = new OdGeKnotVector();
      OdGeKnotVector vKnots = new OdGeKnotVector();

      theNurb.getDefinition(out degreeU, out degreeV, out propsU, out propsV, out nCptsU, out nCptsV, cPts, weights, uKnots, vKnots);

      Console.WriteLine("degreeU = " + degreeU + ", degreeV = " + degreeV
           + ", propsU = " + propsU + ", propsV = " + propsV
           + ", nCptsU = " + nCptsU + ", nCptsV = " + nCptsV + "\n\t");

      Console.Write("uKnots: (");
      int i = 0;
      for (; i < uKnots.length(); i++)
      {
        Console.Write(String.Format("{0:0.###}", uKnots[i]));
        if (i != uKnots.length() - 1)
          Console.Write(", ");
      }
      Console.Write(")\n");

      Console.Write("vKnots: (");
      for (i = 0; i < vKnots.length(); i++)
      {
        Console.Write(String.Format("{0:0.###}", vKnots[i]));
        if (i != vKnots.length() - 1)
          Console.Write(", ");
      }
      Console.Write(")\n");

      Console.Write("\nControl points  weights:\n\t(");
      for (i = 0; i < (int)cPts.Count; i++)
      {
        Console.Write("(");
        dumpPoint(cPts[i]);
        Console.Write(", " + Process.numToString((weights.Count == 0 ? 1.0 : weights[i])) + ")");
        if (i != (int)cPts.Count - 1)
          Console.Write(",\n\t");
      }
      Console.Write(")\n");
    }

    void dumpPlane(OdGePlane thePlane)
    {
      Console.Write("\n\tPlane base: ");
      dumpPoint(thePlane.pointOnPlane());
      Console.Write("\n\tPlane normal: ");
      dumpVector(thePlane.normal());
    }

    void dumpPoint(OdGePoint3d p)
    {
      Console.Write("(" + Process.numToString(p.x) + ", "
        + Process.numToString(p.y) + ", "
        + Process.numToString(p.z) + ")");
    }

    void dumpSphere(OdGeSphere theSphere)
    {
      Console.Write("\n\tSphere center: ");
      dumpPoint(theSphere.center());
      Console.Write("\n\tSphere radius: " + Process.numToString(theSphere.radius()) + "\n");
    }

    void dumpTorus(OdGeTorus theTorus)
    {
      Console.Write("\n\tTorus center: ");
      dumpPoint(theTorus.center());
      Console.Write("\n\tTorus axis: ");
      dumpVector(theTorus.axisOfSymmetry());
      Console.Write("\n\tTorus major radius: " + Process.numToString(theTorus.majorRadius())
           + ", \n\tTorus minor radius: " + Process.numToString(theTorus.minorRadius()) + "\n");
    }

    void dumpVector(OdGeVector3d w)
    {
      Console.Write("(" + Process.numToString(w.x) + ", "
        + Process.numToString(w.y) + ", "
        + Process.numToString(w.z) + ")");
    }

    public void edgeSearch(OdBrBrep aBrep, int edgeNum)
    {
      Console.WriteLine("Searching for edge " + edgeNum);

      int num = 0;
      OdBrBrepEdgeTraverser bet = new OdBrBrepEdgeTraverser();
      bet.setBrep(aBrep);
      while (!bet.done())
      {
        OdBrEdge cur = bet.getEdge();
        if (num == edgeNum)
        {
          reportEdgeAdjacencies(cur);
          return;
        }
        bet.next(); ++num;
      }

      Console.WriteLine("Edge not found.\n");
    }

    public void faceSearch(OdBrBrep aBrep, int faceNum)
    {
      Console.WriteLine("Searching for face " + faceNum);

      int num = 0;
      OdBrBrepFaceTraverser bft = new OdBrBrepFaceTraverser();
      bft.setBrep(aBrep);
      while (!bft.done())
      {
        OdBrFace cur = bft.getFace();
        if (num == faceNum)
        {
          reportFaceGeometry(cur);
          return;
        }
        bft.next(); ++num;
      }

      Console.WriteLine("Face not found.");
    }

    void reportEdgeAdjacencies(OdBrEdge edg)
    {
      OdBrEdgeLoopTraverser elt = new OdBrEdgeLoopTraverser();
      elt.setEdge(edg);
      Console.Write("Number of adjacent loops : ");
      int num = 0;
      while (!elt.done())
      {
        OdBrLoop curLoop = elt.getLoop();
        elt.next(); ++num;
      }

      Console.Write("\t " + num + "\n");

      OdGeCurve3d theCurve = edg.getCurve();
      if (theCurve == null)
        Console.WriteLine("Could not get curve!\n");

      OdGe.EntityId type = edg.getCurveType();
      switch (type)
      {
        case OdGe.EntityId.kEllipArc3d:
          Console.Write("Edge lies on an ellipse:\t");
          OdGeEllipArc3d ellArc = new OdGeEllipArc3d();
          ellArc.Assign(theCurve);
          dumpEllipse(ellArc);
          break;
        case OdGe.EntityId.kNurbCurve3d:
          Console.Write("Edge lies on a nurb curve:\t");
          OdGeNurbCurve3d nurbCurve = new OdGeNurbCurve3d();
          nurbCurve.Assign(theCurve);
          dumpNurbCurve(nurbCurve);
          break;
        case OdGe.EntityId.kLineSeg3d:
          Console.Write("Edge lies on a straight line:\t");
          OdGeLineSeg3d lineSeg = new OdGeLineSeg3d();
          lineSeg.Assign(theCurve);
          dumpLine(lineSeg);
          break;
        default:
          Console.WriteLine("Unknown curve type: " + type + "\n");
          Console.WriteLine("Returned as a NURBS curve:\t");
          OdGeNurbCurve3d nc = new OdGeNurbCurve3d();
          if (edg.getCurveAsNurb(nc))
            dumpNurbCurve(nc);
          else
            Console.WriteLine("Could not retrieve the curve as a Nurb curve!\n");
          break;
      }
    }


    void reportFaceGeometry(OdBrFace f)
    {
      if (f.isNull())
      {
        Console.WriteLine("No face!\n");
        return;
      }
      //disposing problems
      OdGeExternalBoundedSurface pExtSurface = new OdGeExternalBoundedSurface();

      pExtSurface.Assign(f.getSurface());
      if (pExtSurface == null)
        Console.WriteLine("Could not get surface!\n");

      OdGeSurface theSurface = pExtSurface.getBaseSurfaceEx();
      OdGe.EntityId surfType = theSurface.type();

      OdGe.EntityId type = pExtSurface.type();
      OdBrErrorStatus err;
      if (type == surfType)
        Console.WriteLine("Could not get surface type!");
      switch (surfType)
      {
        case OdGe.EntityId.kCone:
          Console.Write("Face lies on a cone:\t");
          OdGeCone cone = new OdGeCone();
          cone.Assign(theSurface);
          dumpCone(cone);
          break;
        case OdGe.EntityId.kEllipCone:
          Console.Write("Face lies on a elliptical cone:\t");
          OdGeEllipCone elCone = new OdGeEllipCone();
          elCone.Assign(theSurface);
          dumpEllipCone(elCone);
          break;
        case OdGe.EntityId.kCylinder:
          Console.Write("Face lies on a cylinder:\t");
          OdGeCylinder cylinder = new OdGeCylinder();
          cylinder.Assign(theSurface);
          dumpCylinder(cylinder);
          break;
        case OdGe.EntityId.kEllipCylinder:
          Console.Write("Face lies on a elliptical cylinder:\t");
          OdGeEllipCylinder elCylinder = new OdGeEllipCylinder();
          elCylinder.Assign(theSurface);
          dumpEllipCylinder(elCylinder);
          break;
        case OdGe.EntityId.kNurbSurface:
          Console.Write("Face lies on a NURBS surface:\t");
          OdGeNurbSurface nurbSurface = new OdGeNurbSurface();
          nurbSurface.Assign(theSurface);
          dumpNurbSurface(nurbSurface);
          break;
        case OdGe.EntityId.kPlane:
          Console.Write("Face lies on a plane:\t");
          OdGePlane plane = new OdGePlane();
          plane.Assign(theSurface);
          dumpPlane(plane);
          break;
        case OdGe.EntityId.kSphere:
          Console.Write("Face lies on a sphere:\t");
          OdGeSphere sphere = new OdGeSphere();
          sphere.Assign(theSurface);
          dumpSphere(sphere);
          break;
        case OdGe.EntityId.kTorus:
          Console.Write("Face lies on a torus:\t");
          OdGeTorus torus = new OdGeTorus();
          torus.Assign(theSurface);
          dumpTorus(torus);
          break;
        default:
          Console.Write("Unknown surface type: " + surfType + '\n');
          Console.WriteLine("\nReturned as a NURBS surface:\t");
          OdGeNurbSurface ns = new OdGeNurbSurface();
          err = f.getSurfaceAsNurb(ns);
          if (err == OdBrErrorStatus.odbrOK)
            dumpNurbSurface(ns);
          else
            Console.WriteLine("Could not retrieve the surface as a Nurb surface!\n");
          break;
      }
    }

    void reportVertexAdjacencies(OdBrVertex vtx)
    {
      OdBrVertexLoopTraverser vlt = new OdBrVertexLoopTraverser();
      vlt.setVertex(vtx);
      Console.Write("Number of adjacent loops:");
      int num = 0;
      while (!vlt.done())
      {
        OdBrLoop curLoop = vlt.getLoop();
        vlt.next(); ++num;
      }
      Console.Write("\t" + num + '\n');

      OdBrVertexEdgeTraverser vet = new OdBrVertexEdgeTraverser();
      vet.setVertex(vtx);
      Console.Write("Number of adjacent edges:");
      num = 0;
      while (!vet.done())
      {
        OdBrEdge curEdge = vet.getEdge();
        vet.next(); ++num;
      }
      Console.Write("\t" + num + '\n');
    }

    public void vertexSearch(OdBrBrep aBrep, int vertexNum)
    {
      Console.WriteLine("Searching for vertex " + vertexNum);

      OdBrBrepVertexTraverser bvt = new OdBrBrepVertexTraverser();
      bvt.setBrep(aBrep);
      int num = 0;
      while (!bvt.done())
      {
        OdBrVertex cur = bvt.getVertex();
        if (num == vertexNum)
        {
          reportVertexAdjacencies(cur);
          return;
        }
        bvt.next(); ++num;
      }

      Console.WriteLine("Vertex not found.\n");
    }
  }
}

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
using Teigha.TD;

namespace OdBrExSwigMgd
{
  class Process
  {
    Traverse traverse = new Traverse();
    Search search = new Search();
    public int menu()
    {
      Console.WriteLine("\n");
      Console.WriteLine("   1 ==> Complete top-down traversal of Brep, ending with loop.edge");
      Console.WriteLine("   2 ==> Complete top-down traversal of Brep, ending with loop.vertex");
      Console.WriteLine("   3 ==> Brep-shell traverser only");
      Console.WriteLine("   4 ==> Brep-face traverser only");
      Console.WriteLine("   5 ==> Brep-edge traverser only");
      Console.WriteLine("   6 ==> Brep-vertex traverser only");
      Console.WriteLine("   7 ==> Query for a face number, report surface geometry");
      Console.WriteLine("   8 ==> Query for an edge number, ");
      Console.WriteLine("         report adjacent Edges, Loops, and curve geometry");
      Console.WriteLine("   9 ==> Query for an SAT file vertex number;");
      Console.WriteLine("         report adjacent Edges and Loops");
      //Console.WriteLine("  10 ==> OdBrMesh");  //No wrappers yet
      Console.WriteLine("   0 ==> exit");
      Console.WriteLine("\n Choice: ");
      return Process.getConsoleInt();
    }

    public void processOption(OdBrBrep br, OdDbEntity pEnt, int testToRun)
    {
      if (testToRun < 7)
      {
        traverse.traverseBrep(br, testToRun);
      }
      else if (testToRun == 7)
      {
        int faceSeqNum;
        Console.WriteLine("\nEnter a face number (-1 to quit): ");
        faceSeqNum = Process.getConsoleInt();
        while (faceSeqNum >= 0)
        {
          search.faceSearch(br, faceSeqNum);
          Console.WriteLine("\nEnter a face number (-1 to quit): ");
          faceSeqNum = Process.getConsoleInt();
        }
      }
      else if (testToRun == 8)
      {
        int edgeSeqNum;
        Console.WriteLine("\nEnter an edge number (-1 to quit): ");
        edgeSeqNum = Process.getConsoleInt();
        while (edgeSeqNum >= 0)
        {
          search.edgeSearch(br, edgeSeqNum);
          Console.WriteLine("\nEnter an edge number (-1 to quit): ");
          edgeSeqNum = Process.getConsoleInt();
        }
      }
      else if (testToRun == 9)
      {
        int vertexSeqNum;
        Console.WriteLine("Enter a vertex number (-1 to quit): ");
        vertexSeqNum = Process.getConsoleInt();
        while (vertexSeqNum >= 0)
        {
          search.vertexSearch(br, vertexSeqNum);
          Console.WriteLine("Enter a vertex number (-1 to quit): ");
          vertexSeqNum = Process.getConsoleInt();
        }
      }
    }

    public static string numToString(double inNumber)
    {
      string retString = "";
      retString = String.Format("{0:0.####}", inNumber);
      return retString;
    }

    public static int getConsoleInt()
    {
      int choice = -2;
      string line = "";
      while (line == "" && choice != -1)
      {
        line = Console.ReadLine();
        try
        {
          choice = int.Parse(line);
        }
        catch (FormatException)
        {
          Console.WriteLine("Incorrect input value\n \tPlease try again");
        }
      }
      return choice;
    }
  }
}

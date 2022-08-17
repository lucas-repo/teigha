oda_net_sources (OdWriteExSwigMgd
  DbFiller.cs
  DbSubDMeshData.cs
  EntityBoxes.cs
  OdWinNTCrypt.cs
  OleWrappers.cs
  PageControllers.cs
  Program.cs
  ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
oda_net_csharp_executable(OdWriteExSwigMgd)
oda_net_references ( OdWriteExSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TD_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" OdWriteExSwigMgd.exe.manifest )
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\extrudedsurf.sat\"" "\"$(TargetDir)extrudedsurf.sat\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\loftedsurf.sat\"" "\"$(TargetDir)loftedsurf.sat\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\OdWriteEx.sat\"" "\"$(TargetDir)OdWriteEx.sat\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\revolvedsurf.sat\"" "\"$(TargetDir)revolvedsurf.sat\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\sweptsurf.sat\"" "\"$(TargetDir)sweptsurf.sat\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\OdWriteEx.xls.ole\"" "\"$(TargetDir)OdWriteEx.xls.ole\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\OdWriteEx.txt\"" "\"$(TargetDir)OdWriteEx.txt\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\odwriteex.dgn\"" "\"$(TargetDir)odwriteex.dgn\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\OdWriteEx.dwf\"" "\"$(TargetDir)OdWriteEx.dwf\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\OdWriteEx XRef.dwg\"" "\"$(TargetDir)OdWriteEx XRef.dwg\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\OdWriteEx.jpg\"" "\"$(TargetDir)OdWriteEx.jpg\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\odwriteex.pdf\"" "\"$(TargetDir)odwriteex.pdf\"")
add_custom_command ( TARGET OdWriteExSwigMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Teigha.NET\\Examples\\OdWriteExSwigMgd\\OdWriteEx.xls\"" "\"$(TargetDir)OdWriteEx.xls\"")

tn_project_group(OdWriteExSwigMgd "Examples")

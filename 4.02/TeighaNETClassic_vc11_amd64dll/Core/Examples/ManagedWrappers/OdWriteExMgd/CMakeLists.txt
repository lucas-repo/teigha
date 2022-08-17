if(MSVC_VERSION GREATER 1310)

oda_net_sources (OdWriteExMgd
    DbFiller.cs
    DbFiller2.cs
    EntityBoxes.cs
    Program.cs
    ../GeneralAssemblyInfo.cs
    ../OdaMgdMViewApp/HostAppServ.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Drawing,System.Xml )
set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})
oda_net_csharp_executable(OdWriteExMgd)
if( MSVC80 OR MSVC90 )
add_custom_command ( TARGET OdWriteExMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)\\mgd.manifest\"" OdWriteExMgd.exe.manifest )
endif( MSVC80 OR MSVC90 )
add_custom_command ( TARGET OdWriteExMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Core\\Examples\\ManagedWrappers\\OdWriteExMgd\\OdWriteEx.dwf\"" "\"$(TargetDir)OdWriteEx.dwf\"")
add_custom_command ( TARGET OdWriteExMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Core\\Examples\\ManagedWrappers\\OdWriteExMgd\\OdWriteEx XRef.dwg\"" "\"$(TargetDir)OdWriteEx XRef.dwg\"")
add_custom_command ( TARGET OdWriteExMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Core\\Examples\\ManagedWrappers\\OdWriteExMgd\\OdWriteEx.sat\"" "\"$(TargetDir)OdWriteEx.sat\"")
add_custom_command ( TARGET OdWriteExMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Core\\Examples\\ManagedWrappers\\OdWriteExMgd\\OdWriteEx.jpg\"" "\"$(TargetDir)OdWriteEx.jpg\"")
add_custom_command ( TARGET OdWriteExMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Core\\Examples\\ManagedWrappers\\OdWriteExMgd\\OdWriteEx.xls\"" "\"$(TargetDir)OdWriteEx.xls\"")
add_custom_command ( TARGET OdWriteExMgd POST_BUILD COMMAND copy /b /y "\"$(TargetDir)..\\..\\Core\\Examples\\ManagedWrappers\\OdWriteExMgd\\OdWriteEx.pdf\"" "\"$(TargetDir)OdWriteEx.pdf\"")
td_project_group(OdWriteExMgd "NET")
endif(MSVC_VERSION GREATER 1310)

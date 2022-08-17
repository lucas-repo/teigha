if(MSVC_VERSION GREATER 1310)

oda_net_sources (OdReadExMgd
  OdReadExMgd.cs
   ../GeneralAssemblyInfo.cs
   ../OdaMgdMViewApp/HostAppServ.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})
oda_net_csharp_executable(OdReadExMgd)

if( MSVC80 OR MSVC90 )
add_custom_command ( TARGET OdReadExMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\mgd.manifest\"" OdReadExMgd.exe.manifest )
endif( MSVC80 OR MSVC90 )

td_project_group(OdReadExMgd "NET")
endif(MSVC_VERSION GREATER 1310)
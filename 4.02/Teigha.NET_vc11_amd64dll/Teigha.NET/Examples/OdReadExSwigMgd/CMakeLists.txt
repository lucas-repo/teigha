oda_net_sources (OdReadExSwigMgd
  DbDumper.cs
  Program.cs
  ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
oda_net_csharp_executable(OdReadExSwigMgd)
oda_net_references ( OdReadExSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TD_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET OdReadExSwigMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" OdReadExSwigMgd.exe.manifest )

tn_project_group(OdReadExSwigMgd "Examples")

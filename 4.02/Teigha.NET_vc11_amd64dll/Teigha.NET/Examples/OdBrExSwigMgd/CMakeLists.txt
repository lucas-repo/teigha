oda_net_sources (OdBrExSwigMgd
  Program.cs
  Search.cs
  Process.cs
  Traverse.cs
  ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
oda_net_csharp_executable(OdBrExSwigMgd)
oda_net_references ( OdBrExSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TD_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET OdBrExSwigMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" OdBrExSwigMgd.exe.manifest )

tn_project_group(OdBrExSwigMgd "Examples")

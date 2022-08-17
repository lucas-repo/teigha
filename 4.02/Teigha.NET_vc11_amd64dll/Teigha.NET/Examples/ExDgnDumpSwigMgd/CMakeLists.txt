oda_net_sources (ExDgnDumpSwigMgd
  ExDgnDumpElements.cs
  Program.cs
  ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
oda_net_csharp_executable(ExDgnDumpSwigMgd)
oda_net_references(ExDgnDumpSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TG_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET ExDgnDumpSwigMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" ExDgnDumpSwigMgd.exe.manifest )

tn_project_group(ExDgnDumpSwigMgd "Examples")

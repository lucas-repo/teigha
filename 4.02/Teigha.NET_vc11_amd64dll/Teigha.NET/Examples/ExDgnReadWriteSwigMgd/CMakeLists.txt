oda_net_sources (ExDgnReadWriteSwigMgd
  Program.cs
  ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
oda_net_csharp_executable(ExDgnReadWriteSwigMgd)
oda_net_references(ExDgnReadWriteSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TG_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET ExDgnReadWriteSwigMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" ExDgnReadWriteSwigMgd.exe.manifest )

tn_project_group(ExDgnReadWriteSwigMgd "Examples")

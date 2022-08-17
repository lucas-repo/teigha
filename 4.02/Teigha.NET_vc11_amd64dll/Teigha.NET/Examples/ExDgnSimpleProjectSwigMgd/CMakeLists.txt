oda_net_sources (ExDgnSimpleProjectSwigMgd
  Program.cs
  ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
oda_net_csharp_executable(ExDgnSimpleProjectSwigMgd)
oda_net_references(ExDgnSimpleProjectSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TG_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET ExDgnSimpleProjectSwigMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" ExDgnSimpleProjectSwigMgd.exe.manifest )

tn_project_group(ExDgnSimpleProjectSwigMgd "Examples")

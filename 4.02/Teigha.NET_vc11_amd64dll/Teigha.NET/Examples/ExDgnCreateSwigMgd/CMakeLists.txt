oda_net_sources (ExDgnCreateSwigMgd
  EntityBoxes.cs
  ExDgnFiller.cs
  Program.cs
  ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
oda_net_csharp_executable(ExDgnCreateSwigMgd)
oda_net_references(ExDgnCreateSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TG_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET ExDgnCreateSwigMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" ExDgnCreateSwigMgd.exe.manifest )

tn_project_group(ExDgnCreateSwigMgd "Examples")

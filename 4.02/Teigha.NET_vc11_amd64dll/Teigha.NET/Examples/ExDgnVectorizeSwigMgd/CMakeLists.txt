oda_net_sources (ExDgnVectorizeSwigMgd
  ExGsSimpleDevice.cs
  GiDumper.cs
  OdGiConveyorGeometryDumper.cs
  Program.cs
  ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
oda_net_csharp_executable(ExDgnVectorizeSwigMgd)
oda_net_references(ExDgnVectorizeSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TG_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET ExDgnVectorizeSwigMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" ExDgnVectorizeSwigMgd.exe.manifest )

tn_project_group(ExDgnVectorizeSwigMgd "Examples")
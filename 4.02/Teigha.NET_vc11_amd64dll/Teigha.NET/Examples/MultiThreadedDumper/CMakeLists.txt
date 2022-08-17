oda_net_sources (MultiThreadedDumper
  DbDumper.cs
  Program.cs
  ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
oda_net_csharp_executable(MultiThreadedDumper)
oda_net_references ( MultiThreadedDumper ${TD_SWIG_CORE_MGD_LIB} ${TD_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET MultiThreadedDumper POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" MultiThreadedDumper.exe.manifest )

tn_project_group(MultiThreadedDumper "Examples")

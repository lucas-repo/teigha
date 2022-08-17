oda_net_sources (EvaluationExample
  Program.cs
  ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml)
oda_net_csharp_executable(EvaluationExample)
oda_net_references ( EvaluationExample ${TD_SWIG_CORE_MGD_LIB} ${TD_SWIG_DB_MGD_LIB})
set_target_properties(EvaluationExample PROPERTIES PROJECT_NATIVE_REFERENCES "OdaX.SxS.manifest=${ODA_BIN_DIR}")
add_custom_command ( TARGET EvaluationExample POST_BUILD COMMAND xcopy "$(TargetDir)..\\..\\Teigha.NET\\Examples\\EvaluationExample\\Properties\\FieldTest.dwg" "$(TargetDir)" /Y /I )

tn_project_group(EvaluationExample "Examples")

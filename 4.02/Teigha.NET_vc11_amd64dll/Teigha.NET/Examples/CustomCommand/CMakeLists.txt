oda_net_sources (CustomCommand
    Class1.cs
	../../GeneralAssemblyInfo.cs
   )
set ( CMAKE_NET_REFERENCES System,System.Data,System.Drawing,System.Windows.Forms )
oda_net_csharp_library(CustomCommand)
oda_net_references(CustomCommand ${TD_SWIG_CORE_MGD_LIB} ${TG_SWIG_DB_MGD_LIB} ${TD_SWIG_DB_MGD_LIB})
tn_project_group(CustomCommand "Examples")

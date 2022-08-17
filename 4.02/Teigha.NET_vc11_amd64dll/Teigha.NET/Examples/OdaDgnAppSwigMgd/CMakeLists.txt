oda_net_sources (OdaDgnAppSwigMgd
    DgnTreeFiller.cs
	ExDgnElementDumperPE.cs
    Program.cs
	../../GeneralAssemblyInfo.cs
   )
oda_net_csharp_form (OdaDgnAppSwigMgd Export2PdfForm.cs)
oda_net_csharp_form (OdaDgnAppSwigMgd Export2STLDlg.cs)
oda_net_csharp_form (OdaDgnAppSwigMgd FindObjectDlg.cs)
oda_net_csharp_form (OdaDgnAppSwigMgd Form1.cs)
oda_net_csharp_form (OdaDgnAppSwigMgd VectorizeForm.cs)
oda_net_csharp_form (OdaDgnAppSwigMgd Tree.cs)
set ( CMAKE_NET_REFERENCES System,System.Data,System.Drawing,System.Windows.Forms )
oda_net_csharp_executable(OdaDgnAppSwigMgd)
oda_net_references ( OdaDgnAppSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TG_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET OdaDgnAppSwigMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" OdaDgnAppSwigMgd.exe.manifest )

set_target_properties(OdaDgnAppSwigMgd PROPERTIES WIN32_EXECUTABLE 1)
tn_project_group(OdaDgnAppSwigMgd "Examples")

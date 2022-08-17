oda_net_sources (OdaDwgAppSwigMgd
    CmdReactor.cs
    DwgTreeFiller.cs
    ExDwgElementDumperPE.cs
    Program.cs
    ../../GeneralAssemblyInfo.cs
    Print/Print.cs
   )
oda_net_csharp_form (OdaDwgAppSwigMgd Export2PdfForm.cs)
oda_net_csharp_form (OdaDwgAppSwigMgd FindObjectDlg.cs)
oda_net_csharp_form (OdaDwgAppSwigMgd Form1.cs)
oda_net_csharp_form (OdaDwgAppSwigMgd VectorizeForm.cs)
oda_net_csharp_form (OdaDwgAppSwigMgd Tree.cs)
#oda_net_csharp_form (OdaDwgAppSwigMgd Print/PageSetup.cs )
set ( CMAKE_NET_REFERENCES System,System.Data,System.Drawing,System.Windows.Forms,System.Core )
oda_net_csharp_executable(OdaDwgAppSwigMgd)
oda_net_references ( OdaDwgAppSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TD_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET OdaDwgAppSwigMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" OdaDwgAppSwigMgd.exe.manifest )

set_target_properties(OdaDwgAppSwigMgd PROPERTIES WIN32_EXECUTABLE 1)
tn_project_group(OdaDwgAppSwigMgd "Examples")

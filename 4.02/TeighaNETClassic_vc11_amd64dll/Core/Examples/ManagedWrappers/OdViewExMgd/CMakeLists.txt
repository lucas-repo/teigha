if(MSVC_VERSION GREATER 1310)

oda_net_sources (OdViewExMgd
	AppSettings.cs
	AuxCls.cs
	HostAppServ.cs
	Orbit.cs
	SelectionReactor.cs
	Program.cs
	../GeneralAssemblyInfo.cs
	GripPionts/ExGripData.cs
	GripPionts/ExGripDrag.cs
	GripPionts/GripManager.cs
	GripPionts/Overruling/ImageOverrule.cs
	GripPionts/Overruling/LineOverrule.cs
	Export_Import/ImExport.cs
	Print/Print.cs
)


oda_net_csharp_form ( OdViewExMgd BlockInsert.cs )
oda_net_csharp_form ( OdViewExMgd File_Dependency.cs )
oda_net_csharp_form ( OdViewExMgd Export_Import/BMPExport.cs )
oda_net_csharp_form ( OdViewExMgd Form1.cs )
oda_net_csharp_form ( OdViewExMgd Export_Import/PDFExport.cs )
oda_net_csharp_form ( OdViewExMgd Export_Import/PublishDrawingSheets.cs )
oda_net_csharp_form ( OdViewExMgd Export_Import/publishSetting.cs )
oda_net_csharp_form ( OdViewExMgd PasswordDlg.cs )
oda_net_csharp_form ( OdViewExMgd Print/PageSetup.cs )
oda_net_csharp_form ( OdViewExMgd SelectLayouts.cs )
oda_net_csharp_form ( OdViewExMgd Layers.cs )
set ( CMAKE_NET_REFERENCES System,System.Data,System.Deployment,System.Drawing,System.Windows.Forms,System.Xml )
set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})

oda_net_csharp_executable(OdViewExMgd)
if( MSVC80 OR MSVC90 )
add_custom_command ( TARGET OdViewExMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\mgd.manifest\"" OdViewExMgd.exe.manifest )
endif( MSVC80 OR MSVC90 )

set_target_properties(OdViewExMgd PROPERTIES WIN32_EXECUTABLE 1)
td_project_group(OdViewExMgd "NET")
endif(MSVC_VERSION GREATER 1310)


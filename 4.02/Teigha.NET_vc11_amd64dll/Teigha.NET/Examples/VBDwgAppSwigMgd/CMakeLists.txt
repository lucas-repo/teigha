oda_net_sources (VBViewEx
	 ../../GeneralAssemblyInfo.vb
   )

oda_net_vb_form(VBDwgAppSwigMgd Form1.vb)
oda_net_vb_form(VBDwgAppSwigMgd CustomPDFProperties.vb)
oda_net_vb_form(VBDwgAppSwigMgd DisplayControl.vb)
oda_net_vb_form(VBDwgAppSwigMgd PDFParams.vb)

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml,System.Windows.Forms,System.Drawing,System.Collections.Generic)
#set ( CMAKE_PROJECT_REFERENCES ${TD_SWIG_CORE_MGD_LIB},${TD_SWIG_DB_MGD_LIB})

oda_net_vb_executable(VBDwgAppSwigMgd VBDwgAppSwigMgd.Form1)
oda_net_imports(VBDwgAppSwigMgd System,System.Windows.Forms,System.Drawing,System.Collections.Generic,Teigha.Core,Teigha.TD)
oda_net_references ( VBDwgAppSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TD_SWIG_DB_MGD_LIB})

set_target_properties(VBDwgAppSwigMgd PROPERTIES WIN32_EXECUTABLE 1)
tn_project_group(VBDwgAppSwigMgd "Examples")

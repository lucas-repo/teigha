#
#  ManagedTestCommand library
#
set (tb_managedtestcommand_source
  ManagedTestCommand.cpp
  AssemblyInfo.cpp
  ../../GeneralAssemblyInfo.cpp
)
SOURCE_GROUP ("Source Files" FILES ${tb_managedtestcommand_source})

set (tb_managedtestcommand_include
  ManagedTestCommand.h
)
SOURCE_GROUP ("Header Files" FILES ${tb_managedtestcommand_include})



  td_sources(ManagedTestCommand
			 ${tb_managedtestcommand_source}
			 ${tb_managedtestcommand_include})
			 
td_sources(ManagedTestCommand ManagedTestCommand.rc )

include_directories( ${TD_ROOT} )

add_definitions( "/wd4996 /wd4793" )

td_tx(ManagedTestCommand)

SET (CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})

set_target_properties (ManagedTestCommand PROPERTIES COMPILE_FLAGS_REMOVE "/RTC1 /RTCsu /Zc:wchar_t- /Gm" )
set_target_properties(ManagedTestCommand PROPERTIES COMPILE_FLAGS "/W3 /EHa")
set(CMAKE_MANIFEST 1) 
set(CMAKE_CLR_FLAG 1)

oda_set_cmake_net_framework(CMAKE_NET_FRAMEWORK)

if( MSVC80 OR MSVC90 )
set(CMAKE_NET_REFERENCES System.dll,System.Data.dll,System.XML.dll,${TD_MGD_LIB})
else( MSVC80 OR MSVC90 )
set(CMAKE_NET_REFERENCES System.dll,System.Data.dll,System.XML.dll)
endif( MSVC80 OR MSVC90 )

set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})
td_project_group(ManagedTestCommand "NET/ManagedCommands")

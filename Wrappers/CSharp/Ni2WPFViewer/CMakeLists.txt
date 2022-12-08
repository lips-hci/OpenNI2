cmake_minimum_required (VERSION 3.1)

include(CSharpUtilities)

project(Ni2WPFViewer LANGUAGES CSharp)

set(EXE_NAME Ni2WPFViewer)

set(CMAKE_CSharp_FLAGS "/platform:x64")

add_executable(${EXE_NAME}
    App.config
    App.xaml
    App.xaml.cs
    MainWindow.xaml
    MainWindow.xaml.cs
    Properties/AssemblyInfo.cs
    Properties/Resources.Designer.cs
    Properties/Resources.resx
    Properties/Settings.Designer.cs
    Properties/Settings.settings
)

# designer files
#csharp_set_designer_cs_properties(
#    Properties/AssemblyInfo.cs
#    Properties/Resources.Designer.cs
#    Properties/Resources.resx
#    Properties/Settings.Designer.cs
#    Properties/Settings.settings
#)

# entry point
#csharp_set_xaml_cs_properties(
#    App.xaml
#    App.xaml.cs
#    MainWindow.xaml
#    MainWindow.xaml.cs
#)

# Set the source file properties for Windows Forms use.
csharp_set_windows_forms_properties(
    App.xaml.cs
    MainWindow.xaml.cs
    Properties/AssemblyInfo.cs
    Properties/Resources.Designer.cs
    Properties/Resources.resx
    Properties/Settings.Designer.cs
    Properties/Settings.settings
)

set_property(SOURCE App.xaml PROPERTY VS_XAML_TYPE "ApplicationDefinition")

# cs project flags
set_property(TARGET ${EXE_NAME} PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.0")
set_property(TARGET ${EXE_NAME} PROPERTY WIN32_EXECUTABLE TRUE)

# references
LIST(APPEND vs_dotnet_references "Microsoft.CSharp")
LIST(APPEND vs_dotnet_references "PresentationCore")
LIST(APPEND vs_dotnet_references "PresentationFramework")
LIST(APPEND vs_dotnet_references "WindowsBase")
LIST(APPEND vs_dotnet_references "System")
LIST(APPEND vs_dotnet_references "System.Data")
LIST(APPEND vs_dotnet_references "System.Drawing")
LIST(APPEND vs_dotnet_references "System.Xml")
LIST(APPEND vs_dotnet_references "System.Core")
LIST(APPEND vs_dotnet_references "System.Xaml")
LIST(APPEND vs_dotnet_references "System.Xml.Linq")
LIST(APPEND vs_dotnet_references "System.Data.DataSetExtensions")
LIST(APPEND vs_dotnet_references "System.Net.Http")
LIST(APPEND vs_dotnet_references "System.Windows")

set_property(TARGET ${EXE_NAME} PROPERTY VS_DOTNET_REFERENCES "${vs_dotnet_references}")

target_link_libraries(${EXE_NAME} PRIVATE NiWrapper.Net)

set_property(TARGET ${EXE_NAME} PROPERTY FOLDER "CSharp_samples")

## Installation
install(TARGETS ${EXE_NAME} NiWrapper.Net NiWrapper DESTINATION .)

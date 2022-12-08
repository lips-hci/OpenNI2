cmake_minimum_required (VERSION 3.1)

include(CSharpUtilities)

project(SimpleViewer.Net LANGUAGES CSharp)

set(EXE_NAME SimpleViewer.Net)

set(CMAKE_CSharp_FLAGS "/platform:x64 /unsafe")

add_executable(${EXE_NAME}
    App.config
    Program.cs
    MainWindow.cs
    MainWindow.Designer.cs
    Properties/AssemblyInfo.cs
)

# designer files
#csharp_set_designer_cs_properties(
#    Properties/AssemblyInfo.cs
#)

# Set the source file properties for Windows Forms use.
csharp_set_windows_forms_properties(
    Program.cs
    MainWindow.cs
    MainWindow.Designer.cs
    Properties/AssemblyInfo.cs
)

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
LIST(APPEND vs_dotnet_references "System.Xml.Linq")
LIST(APPEND vs_dotnet_references "System.Windows")
LIST(APPEND vs_dotnet_references "System.Windows.Forms")

set_property(TARGET ${EXE_NAME} PROPERTY VS_DOTNET_REFERENCES "${vs_dotnet_references}")

target_link_libraries(${EXE_NAME} PRIVATE NiWrapper.Net)

set_property(TARGET ${EXE_NAME} PROPERTY FOLDER "CSharp_samples")

## Installation
install(TARGETS ${EXE_NAME} NiWrapper.Net NiWrapper DESTINATION .)

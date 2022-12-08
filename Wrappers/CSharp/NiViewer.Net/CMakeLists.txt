cmake_minimum_required (VERSION 3.1)

include(CSharpUtilities)

project(NiViewer.Net LANGUAGES CSharp)

set(EXE_NAME NiViewer.Net)

set(CMAKE_CSharp_FLAGS "/platform:x64")

add_executable(${EXE_NAME}
    App.config
    frm_Main.cs
    frm_Main.Designer.cs
    frm_Main.resx
    Program.cs
    Packages.config
    Settings.StyleCop
    Properties/AssemblyInfo.cs
    Properties/Resources.Designer.cs
    Properties/Resources.resx
    Properties/Settings.Designer.cs
    Properties/Settings.settings
)

# Set the source file properties for Windows Forms use.
csharp_set_windows_forms_properties(
    frm_Main.cs
    frm_Main.Designer.cs
    Program.cs
    Properties/AssemblyInfo.cs
    Properties/Resources.Designer.cs
    Properties/Resources.resx
    Properties/Settings.Designer.cs
    Properties/Settings.settings
)

#set_property(SOURCE App.xaml PROPERTY VS_XAML_TYPE "ApplicationDefinition")

# cs project flags
set_property(TARGET ${EXE_NAME} PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.0")
set_property(TARGET ${EXE_NAME} PROPERTY WIN32_EXECUTABLE TRUE)

# references
LIST(APPEND vs_dotnet_references "Microsoft.CSharp")
LIST(APPEND vs_dotnet_references "WindowsBase")
LIST(APPEND vs_dotnet_references "System")
LIST(APPEND vs_dotnet_references "System.Data")
LIST(APPEND vs_dotnet_references "System.Drawing")
LIST(APPEND vs_dotnet_references "System.Drawing.Common")
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

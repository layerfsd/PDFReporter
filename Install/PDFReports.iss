[Setup]
AppName                     =AxiomCoders PDF Reports
AppVerName                  =AxiomCoders PDF Reports 2.1
AppCopyright                =AxiomCoders © 2010
AppID                       =AxiomCoders PDF Reports
AppMutex                    =AxiomCodersPDFReports
AppPublisher                =AxiomCoders
AppPublisherURL             =www.axiomcoders.com
AppSupportURL               =support@axiomcoders.com
AppUpdatesURL               =www.axiomcoders.com
AppVersion                  =2.1
Compression	                =lzma/max
DefaultDirName              ={pf}\AxiomCoders\PDF Reports
DefaultGroupName            =AxiomCoders\PDF Reports
DisableStartupPrompt        =true
LicenseFile                 =Files\EULA.rtf
OutputBaseFilename          =AxiomCoders.PDFReports
SolidCompression            =yes
UninstallDisplayIcon        ={app}\PDFTemplateEditor.exe
VersionInfoVersion          =2.1
WizardImageFile             =Files\PDFReports.bmp
WizardSmallImageFile        =Files\PDFReportsSmall.bmp
WizardImageStretch          =no
WizardImageBackColor        =clWhite
Uninstallable               =yes
CreateUninstallRegKey       =yes
UpdateUninstallLogAppName   =yes
PrivilegesRequired          =admin
ChangesAssociations         =no
UserInfoPage                =no


[Types]
Name: Full; Description: Full Install
Name: PDFTemplateEditor; Description: Install PDF Template Editor
Name: PDFReports; Description: Install PDF Reports .NET Component
Name: PDFFactory; Description: Install PDF Reports Win32/Win64 DLL Component

[Components]
Name: PDFTemplateEditor; Description: AxiomCoders PDF Reports Template Editor; Types: Full PDFTemplateEditor
Name: PDFReports; Description: AxiomCoders PDF Reports .NET Component; Types: Full PDFReports
Name: PDFFactory; Description: AxiomCoders PDF Reports Win32/Win64 DLL Component; Types: Full PDFFactory


[Files]

; Common web links
Source: Files\AxiomCoders.url; DestDir: {app}; Flags: ignoreversion overwritereadonly
Source: Files\AxiomCoders PDF Reports.url; DestDir: {app}; Flags: ignoreversion overwritereadonly
Source: Files\AxiomCoders Support.url; DestDir: {app}; Flags: ignoreversion overwritereadonly

; PDF Reports .NET Component
Components: PDFReports; Source: ..\PdfReports\bin\Release_Secure\AxiomCoders.PdfReports.dll; DestDir: {app}; StrongAssemblyName: ..\PdfReports\PdfReports\AxiomCoders.PDFReports.snk; Flags: ignoreversion overwritereadonly gacinstall
Components: PDFReports; Source: ..\PdfReports\Depending\PDFReports_ReadMe.txt; DestDir: {app}; Flags: ignoreversion overwritereadonly
Components: PDFReports; Source: ..\PdfReports\Depending\Demo\PDFReportsTest.prtp; DestDir: {app}; Flags: ignoreversion overwritereadonly
Components: PDFReports; Source: ..\PdfReports\Depending\Demo\PDFReportsTest.csproj; DestDir: {app}; Flags: ignoreversion overwritereadonly
Components: PDFReports; Source: ..\PdfReports\Depending\Demo\Program.cs; DestDir: {app}; Flags: ignoreversion overwritereadonly
Components: PDFReports; Source: ..\PdfReports\Depending\Demo\AssemblyInfo.cs; DestDir: {app}; Flags: ignoreversion overwritereadonly

; Native Component Sample
Components: PDFFactory; Source: ..\PdfFactory\Sample\DemoMain.cpp; DestDir: {app}; Flags: ignoreversion overwritereadonly
Components: PDFFactory; Source: ..\PdfFactory\Sample\PdfFactory.h; DestDir: {app}; Flags: ignoreversion overwritereadonly
Components: PDFFactory; Source: ..\PdfFactory\Sample\PdfFactoryDemo.vcproj; DestDir: {app}; Flags: ignoreversion overwritereadonly
Components: PDFFactory; Source: ..\PdfFactory\Sample\PdfReportsTest.prtp; DestDir: {app}; Flags: ignoreversion overwritereadonly

; PDF Factory Component
Components: PDFFactory; Source: ..\PdfFactory\build\release\win32\PDFFactory.dll; DestDir: {app}; Flags: ignoreversion overwritereadonly
Components: PDFFactory; Source: ..\PdfFactory\build\release\win64\PDFFactoryx64.dll; DestDir: {app}; Flags: ignoreversion overwritereadonly
Components: PDFFactory; Source: ..\PdfFactory\build\release\win32\PDFFactory.lib; DestDir: {app}; Flags: ignoreversion overwritereadonly
Components: PDFFactory; Source: ..\PdfFactory\build\release\win64\PDFFactoryx64.lib; DestDir: {app}; Flags: ignoreversion overwritereadonly


; PDF Reports Template Editor
Components: PDFTemplateEditor; Source: ..\PdfTemplateEditor\bin\Release_Secure\PDFTemplateEditor.exe; DestDir: {app}; Flags: ignoreversion overwritereadonly
Components: PDFTemplateEditor; Source: ..\PdfTemplateEditor\Depending\PDFTemplateEditor_Readme.txt; DestDir: {app}; Flags: ignoreversion overwritereadonly


[Registry]
; Associate project file extension
Root: HKLM; SubKey: "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\AxiomCoders PDF Reports"; ValueType: string; ValueName: ""; ValueData: "{app}"; Flags: uninsdeletekey

[Icons]

; Common online shortcuts
Name: {group}\AxiomCoders Home Page; Filename: {app}\AxiomCoders.url; WorkingDir: {app}
Name: {group}\AxiomCoders PDF Reports Product Page; Filename: {app}\AxiomCoders PDF Reports.url; WorkingDir: {app}
Name: {group}\AxiomCoders Support; Filename: {app}\AxiomCoders Support.url; WorkingDir: {app}

; App shortcuts
Name: {group}\PDF Reports Template Editor; Filename: {app}\PDFTemplateEditor.exe; WorkingDir: {app}
Name: {commondesktop}\PDF Reports Template Editor; Filename: {app}\PDFTemplateEditor.exe; WorkingDir: {app}; Tasks: desktopicon

[Tasks]
; Manager tasks
Name: desktopicon; Description: "Create a &desktop icon"; GroupDescription: "Additional shortcuts:"; Components: PDFTemplateEditor

[Run]


[Code]
// Check for .Net 2.0 framework and offer to download if not already installed
function InitializeSetup(): Boolean;
var
  ErrorCode: Integer;
  NetFrameWorkInstalled : Boolean;
  Result1 : Boolean;

begin
	NetFrameWorkInstalled := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\.NETFramework\policy\v2.0');
	if NetFrameWorkInstalled = true then
	  begin
		  Result := true;
	  end;

	if NetFrameWorkInstalled = false then
    begin
		  NetFrameWorkInstalled := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\.NETFramework\policy\v2.0');
		  if NetFrameWorkInstalled = true then
		    begin
			    Result := true;
		    end;

		if NetFrameWorkInstalled = false then
			begin
 				Result1 := MsgBox('Microsoft .NET 2.0 framework was not found and it is required for running this application. Do you want to download the framework now?',
          mbConfirmation, MB_YESNO) = idYes;
				if Result1 = false then
				  begin
					  Result:=false;
				  end
				else
				  begin
					  Result:=false;
					  ShellExec('open',
					  'http://www.microsoft.com/downloads/details.aspx?familyid=0856eacb-4362-4b0d-8edd-aab15c5e04f5',
					  '', '', SW_SHOWNORMAL, ewNoWait, ErrorCode);
          end;
      end;
	  end;
end;




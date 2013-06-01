
"C:\Program Files\Inno Setup 5\ISCC.exe" PdfReports.iss


del Input\AxiomCoders.PdfReports.dll
ren Input\AxiomCoders.PdfReports_trial.dll AxiomCoders.PdfReports.dll
"C:\Program Files\Inno Setup 5\ISCC.exe" /FPdfReportsTrial PdfReports.iss



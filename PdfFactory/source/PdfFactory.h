#ifndef _PDFFACTORY_
#define _PDFFACTORY_

// Modify the following defines if you have to target a platform prior to the ones specified below.
// Refer to MSDN for the latest info on corresponding values for different platforms.
#define EPSILON 0.0001f

// ignore warnings related to security
#pragma warning(disable : 4996)

#ifndef BUILD_TESTING
	#define DLLEXPORT_TEST_FUNCTION
#endif

#ifndef LINUX_VERSION

	#ifndef WINVER				// Allow use of features specific to Windows XP or later.
	#define WINVER 0x0501		// Change this to the appropriate value to target other versions of Windows.
	#endif

	#ifndef _WIN32_WINNT		// Allow use of features specific to Windows XP or later.                   
	#define _WIN32_WINNT 0x0501	// Change this to the appropriate value to target other versions of Windows.
	#endif						

	#ifndef _WIN32_WINDOWS		// Allow use of features specific to Windows 98 or later.
	#define _WIN32_WINDOWS 0x0410 // Change this to the appropriate value to target Windows Me or later.
	#endif

	#ifndef _WIN32_IE			// Allow use of features specific to IE 6.0 or later.
	#define _WIN32_IE 0x0600	// Change this to the appropriate value to target other versions of IE.
	#endif

	#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
	// Windows Header Files:
	#include <windows.h>
	#include <memory.h>


#ifdef PDFFACTORY_EXPORTS
	#define DLLEXPORT _declspec (dllexport)
    #ifdef BUILD_TESTING
	    #define DLLEXPORT_TEST_FUNCTION _declspec(dllexport)
    #endif
#else
	#define DLLEXPORT _declspec (dllimport)
	#ifdef BUILD_TESTING
	   #define DLLEXPORT_TEST_FUNCTION _declspec(dllimport)
    #endif
#endif

	BOOL WINAPI DllMain(HINSTANCE hinstDLL,DWORD,LPVOID);

#else
// for linux version
	#define DLLEXPORT
	#define TRUE 1
	#define FALSE 0


#endif

#endif

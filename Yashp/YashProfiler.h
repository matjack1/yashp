// YashProfiler.h : Declaration of the CYashProfiler

#pragma once
#include "resource.h"       // main symbols

#include "Yashp_i.h"

#include "CorProfilerCallback.h"
#include "resource.h"
#include "FunctionInfo.h"
#include <map>

#define NAME_BUFFER_SIZE 1024

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif



// CYashProfiler

class ATL_NO_VTABLE CYashProfiler :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CYashProfiler, &CLSID_YashProfiler>,
	public IDispatchImpl<IYashProfiler, &IID_IYashProfiler, &LIBID_YashpLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
	public CCorProfilerCallback
{
public:
CYashProfiler();

	DECLARE_REGISTRY_RESOURCEID(IDR_YASHPROFILER)
	BEGIN_COM_MAP(CYashProfiler)
		COM_INTERFACE_ENTRY(ICorProfilerCallback)
		COM_INTERFACE_ENTRY(ICorProfilerCallback2)
	END_COM_MAP()
	DECLARE_PROTECT_FINAL_CONSTRUCT()

	// overridden implementations of FinalConstruct and FinalRelease
	HRESULT FinalConstruct();
	void FinalRelease();

    // STARTUP/SHUTDOWN EVENTS
    STDMETHOD(Initialize)(IUnknown *pICorProfilerInfoUnk);
    STDMETHOD(Shutdown)();

	// callback functions
	void Enter(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo);
	void Leave(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_RANGE *argumentRange);
	void Tailcall(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo);

	// mapping functions
	static UINT_PTR _stdcall FunctionMapper(FunctionID functionId, BOOL *pbHookFunction);
	void MapFunction(FunctionID);

	// logging function
    void LogString(char* pszFmtString, ... );

private:
    // container for ICorProfilerInfo reference
	CComQIPtr<ICorProfilerInfo> m_pICorProfilerInfo;
    // container for ICorProfilerInfo2 reference
	CComQIPtr<ICorProfilerInfo2> m_pICorProfilerInfo2;
	// STL map for our hashed functions
	std::map<FunctionID, CFunctionInfo*> m_functionMap;
	// the number of levels deep we are in the call stack
	int m_callStackSize;
	// handle and filename of log file
	HANDLE m_hLogFile;
	TCHAR m_logFileName[_MAX_PATH]; 

	// gets the full method name given a function ID
	HRESULT GetFullMethodName(FunctionID functionId, LPWSTR wszMethod, int cMethod );
	// function to set up our event mask
	HRESULT SetEventMask();
	// creates the log file
	void CreateLogFile();
	// closes the log file ;)
	void CloseLogFile();
};

OBJECT_ENTRY_AUTO(__uuidof(YashProfiler), CYashProfiler)

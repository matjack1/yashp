// YashProfiler.cpp : Implementation of CYashProfiler

#include "stdafx.h"
#include "YashProfiler.h"

using namespace std;

#pragma warning (disable: 4996) 

#define ARRAY_SIZE(s) (sizeof(s) / sizeof(s[0]))

// global reference to the profiler object (this) used by the static functions
CYashProfiler* g_pICorProfilerCallback = NULL;

// CYashProfiler
CYashProfiler::CYashProfiler() 
{
	m_hLogFile = INVALID_HANDLE_VALUE;
	m_callStackSize = 0;
}

HRESULT CYashProfiler::FinalConstruct()
{
	// create the log file
	CreateLogFile();

	// log that we have reached FinalConstruct
	LogString("Entering FinalConstruct\r\n\r\n");

	return S_OK;
}

void CYashProfiler::FinalRelease()
{
	// log that we have reached FinalRelease
	LogString("\r\n\r\nEntering FinalRelease\r\n\r\n");

	// close the log file
	CloseLogFile();
}

// ----  CALLBACK FUNCTIONS ------------------

// this function simply forwards the FunctionEnter call the global profiler object
void __stdcall FunctionEnterGlobal(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_INFO *argInfo)
{
	// make sure the global reference to our profiler is valid
    if (g_pICorProfilerCallback != NULL)
        g_pICorProfilerCallback->Enter(functionID, clientData, frameInfo, argInfo);
}

// this function is called by the CLR when a function has been entered
void _declspec(naked) FunctionEnterNaked(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo)
{
    __asm
    {
        push    ebp                 // Create a standard frame
        mov     ebp,esp
        pushad                      // Preserve all registers

        mov     eax,[ebp+0x14]      // argumentInfo
        push    eax
        mov     ecx,[ebp+0x10]      // func
        push    ecx
        mov     edx,[ebp+0x0C]      // clientData
        push    edx
        mov     eax,[ebp+0x08]      // functionID
        push    eax
        call    FunctionEnterGlobal

        popad                       // Restore all registers
        pop     ebp                 // Restore EBP
        ret     16
    }
}

// this function simply forwards the FunctionLeave call the global profiler object
void __stdcall FunctionLeaveGlobal(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange)
{
	// make sure the global reference to our profiler is valid
    if (g_pICorProfilerCallback != NULL)
        g_pICorProfilerCallback->Leave(functionID,clientData,frameInfo,retvalRange);
}

// this function is called by the CLR when a function is exiting
void _declspec(naked) FunctionLeaveNaked(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange)
{
    __asm
    {
        push    ebp                 // Create a standard frame
        mov     ebp,esp
        pushad                      // Preserve all registers

        mov     eax,[ebp+0x14]      // argumentInfo
        push    eax
        mov     ecx,[ebp+0x10]      // func
        push    ecx
        mov     edx,[ebp+0x0C]      // clientData
        push    edx
        mov     eax,[ebp+0x08]      // functionID
        push    eax
        call    FunctionLeaveGlobal

        popad                       // Restore all registers
        pop     ebp                 // Restore EBP
        ret     16
    }
}

// this function simply forwards the FunctionLeave call the global profiler object
void __stdcall FunctionTailcallGlobal(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo)
{
    if (g_pICorProfilerCallback != NULL)
        g_pICorProfilerCallback->Tailcall(functionID,clientData,frameInfo);
}

// this function is called by the CLR when a tailcall occurs.  A tailcall occurs when the 
// last action of a method is a call to another method.
void _declspec(naked) FunctionTailcallNaked(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func)
{
    __asm
    {
        push    ebp                 // Create a standard frame
        mov     ebp,esp
        pushad                      // Preserve all registers

        mov     eax,[ebp+0x14]      // argumentInfo
        push    eax
        mov     ecx,[ebp+0x10]      // func
        push    ecx
        mov     edx,[ebp+0x0C]      // clientData
        push    edx
        mov     eax,[ebp+0x08]      // functionID
        push    eax
        call    FunctionTailcallGlobal

        popad                       // Restore all registers
        pop     ebp                 // Restore EBP
        ret     16
    }
}

// ----  MAPPING FUNCTIONS ------------------

// this function is called by the CLR when a function has been mapped to an ID
UINT_PTR CYashProfiler::FunctionMapper(FunctionID functionID, BOOL *pbHookFunction)
{
	// make sure the global reference to our profiler is valid.  Forward this
	// call to our profiler object
    if (g_pICorProfilerCallback != NULL)
        g_pICorProfilerCallback->MapFunction(functionID);

	// we must return the function ID passed as a parameter
	return (UINT_PTR)functionID;
}

// the static function called by .Net when a function has been mapped to an ID
void CYashProfiler::MapFunction(FunctionID functionID)
{
	// see if this function is in the map
	CFunctionInfo* functionInfo = NULL;
	std::map<FunctionID, CFunctionInfo*>::iterator iter = m_functionMap.find(functionID);
	if (iter == m_functionMap.end())
	{
		// add it to the map
		functionInfo = new CFunctionInfo(functionID);
		m_functionMap.insert(std::pair<FunctionID, CFunctionInfo*>(functionID, functionInfo));
	}
}

// ----  CALLBACK HANDLER FUNCTIONS ------------------

// our real handler for FunctionEnter notification
void CYashProfiler::Enter(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo)
{

	// see if this function is in the map.  It should be since we are using the funciton mapper
	CFunctionInfo* functionInfo = NULL;
	std::map<FunctionID, CFunctionInfo*>::iterator iter = m_functionMap.find(functionID);
	if (iter != m_functionMap.end())
	{
		// get it from the map and update it
		functionInfo = (iter->second);
		// increment the call count
		functionInfo->incrementCallCount();
		// create the padding based on the call stack size
		int padCharCount = m_callStackSize * 2;
		if (padCharCount > 0)
		{
			char* padding = new char[(padCharCount) + 1];
			memset(padding, 0, padCharCount + 1);
			memset(padding, ' ', padCharCount);
			// log the function call
			if (functionInfo->isFiltered(frameInfo))
				LogString("%s %s %s%s(%s), id=%d, call count = %d\r\n", padding, functionInfo->getReturnType().c_str(), functionInfo->getClassName().c_str(), functionInfo->getFunctionName().c_str(), functionInfo->getParameters().c_str(), functionInfo->getFunctionID(), functionInfo->getCallCount());
			delete padding;
		}
		else
		{
			// log the function call
				LogString("%s %s%s(%s), id=%d, call count = %d\r\n", functionInfo->getReturnType().c_str(), functionInfo->getClassName().c_str(), functionInfo->getFunctionName().c_str(), functionInfo->getParameters().c_str(), functionInfo->getFunctionID(), functionInfo->getCallCount());
		}
	}
	else
	{
		// log an error (this shouldn't happen because we're caching the functions
		// in the function mapping callback
		LogString("Error finding function ID %d in the map.\r\n", (int)functionID);
	}
	// increment the call stack size (we must do this even if we don't find the 
	// function in the map
	m_callStackSize++;
}

// our real handler for FunctionLeave notification
void CYashProfiler::Leave(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_RANGE *argumentRange)
{
	// decrement the call stack size
	if (m_callStackSize > 0)
		m_callStackSize--;
}

// our real handler for the FunctionTailcall notification
void CYashProfiler::Tailcall(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo)
{
	// decrement the call stack size
	if (m_callStackSize > 0)
		m_callStackSize--;
}

// ----  ICorProfilerCallback IMPLEMENTATION ------------------

STDMETHODIMP CYashProfiler::RuntimeThreadSuspended(ThreadID threadID)
{
	LogString("Thread %d Suspended...\r\n\r\n", threadID);
    return S_OK;
}

STDMETHODIMP CYashProfiler::RuntimeThreadResumed(ThreadID threadID)
{
	LogString("Thread %d resumed...\r\n\r\n", threadID);
    return S_OK;
}

STDMETHODIMP CYashProfiler::ThreadCreated(ThreadID threadID)
{
	LogString("Thread %d Created...\r\n\r\n", threadID);
    return S_OK;
}

STDMETHODIMP CYashProfiler::ThreadDestroyed(ThreadID threadID)
{
	LogString("Thread %d Destroyed...\r\n\r\n", threadID);
    return S_OK;
}

STDMETHODIMP CYashProfiler::ThreadAssignedToOSThread(ThreadID managedThreadID, DWORD osThreadID) 
{
	LogString("Thread %d Assigned to OS Thread...\r\n\r\n", managedThreadID);
    return S_OK;
}

// called when the profiling object is created by the CLR
STDMETHODIMP CYashProfiler::Initialize(IUnknown *pICorProfilerInfoUnk)
{
	// set up our global access pointer
	g_pICorProfilerCallback = this;

	// log that we are initializing
	LogString("Initializing...\r\n\r\n");

	// get the ICorProfilerInfo interface
    HRESULT hr = pICorProfilerInfoUnk->QueryInterface(IID_ICorProfilerInfo, (LPVOID*)&m_pICorProfilerInfo);
    if (FAILED(hr))
        return E_FAIL;

	// determine if this object implements ICorProfilerInfo2
    hr = pICorProfilerInfoUnk->QueryInterface(IID_ICorProfilerInfo2, (LPVOID*)&m_pICorProfilerInfo2);
    if (FAILED(hr))
	{
		// we still want to work if this call fails, might be an older .NET version
		m_pICorProfilerInfo2.p = NULL;
	}

	// Indicate which events we're interested in.
	hr = SetEventMask();
    if (FAILED(hr))
        LogString("Error setting the event mask\r\n\r\n");

	// set the enter, leave and tailcall hooks
	if (m_pICorProfilerInfo2.p == NULL)
	{
		// note that we are casting our functions to the definitions for the callbacks
		hr = m_pICorProfilerInfo->SetEnterLeaveFunctionHooks((FunctionEnter*)&FunctionEnterNaked, (FunctionLeave*)&FunctionLeaveNaked, (FunctionTailcall*)&FunctionTailcallNaked);
		if (SUCCEEDED(hr))
			hr = m_pICorProfilerInfo->SetFunctionIDMapper((FunctionIDMapper*)&FunctionMapper);
	}
	else
	{
		hr = m_pICorProfilerInfo2->SetEnterLeaveFunctionHooks2(FunctionEnterNaked, FunctionLeaveNaked, FunctionTailcallNaked);
		if (SUCCEEDED(hr))
			hr = m_pICorProfilerInfo2->SetFunctionIDMapper(FunctionMapper);
	}
	// report our success or failure to the log file
    if (FAILED(hr))
        LogString("Error setting the enter, leave and tailcall hooks\r\n\r\n");
	else
		LogString("Successfully initialized profiling\r\n\r\n" );

	CFunctionInfo::setProfilerInfo(m_pICorProfilerInfo2);

    return S_OK;
}

// called when the profiler is being terminated by the CLR
STDMETHODIMP CYashProfiler::Shutdown()
{
	// log the we're shutting down
	LogString("\r\n\r\nShutdown... writing function list\r\n\r\n" );

	// write the function names and call counts to the output file
	std::map<FunctionID, CFunctionInfo*>::iterator iter;
	for (iter = m_functionMap.begin(); iter != m_functionMap.end(); iter++)
	{
		// log the function's info
		CFunctionInfo* functionInfo = iter->second;
		if (strstr(functionInfo->getFunctionName().c_str(), "System") == NULL)
			LogString("%s : call count = %d\r\n", functionInfo->getFunctionName().c_str(), functionInfo->getCallCount());
		// free the memory for the object
		delete iter->second;
	}
	// clear the map
	m_functionMap.clear();

	// tear down our global access pointers
	g_pICorProfilerCallback = NULL;

    return S_OK;
}

// Creates the log file.  It uses the LOG_FILENAME environment variable if it 
// exists, otherwise it creates the file "ICorProfilerCallback Log.log" in the 
// executing directory.  This function doesn't report success or not because 
// LogString always checks for a valid file handle whenever the file is written
// to.
void CYashProfiler::CreateLogFile()
{
	// get the log filename
	memset(m_logFileName, 0, sizeof(m_logFileName));
	// get the log file name (stored in an environment var)
	if (GetEnvironmentVariable(_T("LOG_FILENAME"), m_logFileName, _MAX_PATH) == 0)
	{
		// just write to "ICorProfilerCallback Log.log"
		_tcscpy(m_logFileName, _T("ICorProfilerCallback Log.log"));
	}
	// delete any existing log file
	::DeleteFile(m_logFileName);
	// set up log file in the current working directory
	m_hLogFile = CreateFile(m_logFileName, GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
}

// Closes the log file
void CYashProfiler::CloseLogFile()
{
	// close the log file
	if (m_hLogFile != INVALID_HANDLE_VALUE)
	{
		CloseHandle(m_hLogFile);
		m_hLogFile = INVALID_HANDLE_VALUE;
	}
}

// Writes a string to the log file.  Uses the same calling convention as printf.
void CYashProfiler::LogString(char *pszFmtString, ...)
{
	CHAR szBuffer[4096]; DWORD dwWritten = 0;

	if(m_hLogFile != INVALID_HANDLE_VALUE)
	{
		va_list args;
		va_start( args, pszFmtString );
		vsprintf(szBuffer, pszFmtString, args );
		va_end( args );

		// write out to the file if the file is open
		WriteFile(m_hLogFile, szBuffer, (DWORD)strlen(szBuffer), &dwWritten, NULL);
	}
}

HRESULT CYashProfiler::SetEventMask()
{
	// set the event mask 
	DWORD eventMask = (DWORD)(COR_PRF_MONITOR_ENTERLEAVE | COR_PRF_MONITOR_THREADS | COR_PRF_MONITOR_EXCEPTIONS | COR_PRF_DISABLE_INLINING | COR_PRF_ENABLE_FUNCTION_ARGS | COR_PRF_ENABLE_FUNCTION_RETVAL );
	return m_pICorProfilerInfo->SetEventMask(eventMask);
}

// CYashProfiler


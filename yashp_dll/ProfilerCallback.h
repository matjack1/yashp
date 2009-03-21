// ProfilerCallback.h : Declaration of the CProfilerCallback

#pragma once
#include "resource.h"       // main symbols

#include "yashp_i.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif



// CProfilerCallback

class ATL_NO_VTABLE CProfilerCallback :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CProfilerCallback, &CLSID_ProfilerCallback>,
	public IDispatchImpl<IProfilerCallback, &IID_IProfilerCallback, &LIBID_yashpLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
	public ICorProfilerCallback2
{
public:
	CProfilerCallback()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_PROFILERCALLBACK)

DECLARE_NOT_AGGREGATABLE(CProfilerCallback)

BEGIN_COM_MAP(CProfilerCallback)
	COM_INTERFACE_ENTRY(IProfilerCallback)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:
	STDMETHOD(Initialize)(LPUNKNOWN pICorProfilerInfoUnk)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(Shutdown)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(AppDomainCreationStarted)(AppDomainID appDomainId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(AppDomainCreationFinished)(AppDomainID appDomainId, HRESULT hrStatus)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(AppDomainShutdownStarted)(AppDomainID appDomainId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(AppDomainShutdownFinished)(AppDomainID appDomainId, HRESULT hrStatus)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(AssemblyLoadStarted)(AssemblyID assemblyId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(AssemblyLoadFinished)(AssemblyID assemblyId, HRESULT hrStatus)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(AssemblyUnloadStarted)(AssemblyID assemblyId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(AssemblyUnloadFinished)(AssemblyID assemblyId, HRESULT hrStatus)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ModuleLoadStarted)(ModuleID moduleId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ModuleLoadFinished)(ModuleID moduleId, HRESULT hrStatus)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ModuleUnloadStarted)(ModuleID moduleId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ModuleUnloadFinished)(ModuleID moduleId, HRESULT hrStatus)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ModuleAttachedToAssembly)(ModuleID moduleId, AssemblyID assemblyId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ClassLoadStarted)(ClassID classId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ClassLoadFinished)(ClassID classId, HRESULT hrStatus)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ClassUnloadStarted)(ClassID classId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ClassUnloadFinished)(ClassID classId, HRESULT hrStatus)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(FunctionUnloadStarted)(FunctionID functionId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(JITCompilationStarted)(FunctionID functionId, BOOL fIsSafeToBlock)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(JITCompilationFinished)(FunctionID functionId, HRESULT hrStatus, BOOL fIsSafeToBlock)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(JITCachedFunctionSearchStarted)(FunctionID functionId, BOOL* pbUseCachedFunction)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(JITCachedFunctionSearchFinished)(FunctionID functionId, COR_PRF_JIT_CACHE result)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(JITFunctionPitched)(FunctionID functionId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(JITInlining)(FunctionID callerId, FunctionID calleeId, BOOL* pfShouldInline)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ThreadCreated)(ThreadID threadId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ThreadDestroyed)(ThreadID threadId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ThreadAssignedToOSThread)(ThreadID managedThreadId, DWORD osThreadId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RemotingClientInvocationStarted)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RemotingClientSendingMessage)(GUID * pCookie, BOOL fIsAsync)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RemotingClientReceivingReply)(GUID * pCookie, BOOL fIsAsync)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RemotingClientInvocationFinished)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RemotingServerReceivingMessage)(GUID * pCookie, BOOL fIsAsync)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RemotingServerInvocationStarted)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RemotingServerInvocationReturned)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RemotingServerSendingReply)(GUID * pCookie, BOOL fIsAsync)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(UnmanagedToManagedTransition)(FunctionID functionId, COR_PRF_TRANSITION_REASON reason)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ManagedToUnmanagedTransition)(FunctionID functionId, COR_PRF_TRANSITION_REASON reason)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RuntimeSuspendStarted)(COR_PRF_SUSPEND_REASON suspendReason)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RuntimeSuspendFinished)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RuntimeSuspendAborted)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RuntimeResumeStarted)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RuntimeResumeFinished)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RuntimeThreadSuspended)(ThreadID threadId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RuntimeThreadResumed)(ThreadID threadId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(MovedReferences)(unsigned long cMovedObjectIDRanges, ObjectID* oldObjectIDRangeStart, ObjectID* newObjectIDRangeStart, unsigned long * cObjectIDRangeLength)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ObjectAllocated)(ObjectID objectId, ClassID classId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ObjectsAllocatedByClass)(unsigned long cClassCount, ClassID* classIds, unsigned long* cObjects)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ObjectReferences)(ObjectID objectId, ClassID classId, unsigned long cObjectRefs, ObjectID* objectRefIds)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RootReferences)(unsigned long cRootRefs, ObjectID* rootRefIds)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionThrown)(ThreadID thrownObjectId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionSearchFunctionEnter)(FunctionID functionId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionSearchFunctionLeave)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionSearchFilterEnter)(FunctionID functionId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionSearchFilterLeave)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionSearchCatcherFound)(FunctionID functionId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionOSHandlerEnter)(UINT_PTR __unused)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionOSHandlerLeave)(UINT_PTR __unused)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionUnwindFunctionEnter)(FunctionID functionId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionUnwindFunctionLeave)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionUnwindFinallyEnter)(FunctionID functionId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionUnwindFinallyLeave)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionCatcherEnter)(FunctionID functionId, ObjectID objectId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionCatcherLeave)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(COMClassicVTableCreated)(ClassID wrappedClassId, const GUID& implementedIID, void * pVTable, unsigned long cSlots)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(COMClassicVTableDestroyed)(ClassID wrappedClassId, const GUID& implementedIID, void * pVTable)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionCLRCatcherFound)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ExceptionCLRCatcherExecute)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(ThreadNameChanged)(ThreadID threadId, ULONG cchName, WCHAR name[])
	{
		return E_NOTIMPL;
	}
	STDMETHOD(GarbageCollectionStarted)(int cGenerations, BOOL generationCollected[], COR_PRF_GC_REASON reason)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(SurvivingReferences) (ULONG cSurvivingObjectIDRanges,ObjectID objectIDRangeStart[],ULONG cObjectIDRangeLength[])
	{
		return E_NOTIMPL;
	}
	STDMETHOD(GarbageCollectionFinished)()
	{
		return E_NOTIMPL;
	}
	STDMETHOD(FinalizeableObjectQueued)(DWORD finalizerFlags,ObjectID objectID)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(RootReferences2)(ULONG cRootRefs, ObjectID rootRefIds[], COR_PRF_GC_ROOT_KIND rootKinds[],
			  COR_PRF_GC_ROOT_FLAGS rootFlags[], UINT_PTR rootIds[])
	{
		return E_NOTIMPL;
	}
	STDMETHOD(HandleCreated)(GCHandleID handleId,ObjectID initialObjectId)
	{
		return E_NOTIMPL;
	}
	STDMETHOD(HandleDestroyed)(GCHandleID handleId)
	{
		return E_NOTIMPL;
	}
};

OBJECT_ENTRY_AUTO(__uuidof(ProfilerCallback), CProfilerCallback)

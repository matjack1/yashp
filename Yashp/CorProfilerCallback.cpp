#include "stdafx.h"
#include "CorProfilerCallback.h"

CCorProfilerCallback::CCorProfilerCallback() {}

STDMETHODIMP CCorProfilerCallback::Initialize(IUnknown *pICorProfilerInfoUnk)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::Shutdown()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::AppDomainCreationStarted(AppDomainID appDomainID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::AppDomainCreationFinished(AppDomainID appDomainID, HRESULT hrStatus)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::AppDomainShutdownStarted(AppDomainID appDomainID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::AppDomainShutdownFinished(AppDomainID appDomainID, HRESULT hrStatus)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::AssemblyLoadStarted(AssemblyID assemblyID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::AssemblyLoadFinished(AssemblyID assemblyID, HRESULT hrStatus)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::AssemblyUnloadStarted(AssemblyID assemblyID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::AssemblyUnloadFinished(AssemblyID assemblyID, HRESULT hrStatus)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ModuleLoadStarted(ModuleID moduleID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ModuleLoadFinished(ModuleID moduleID, HRESULT hrStatus)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ModuleUnloadStarted(ModuleID moduleID)
{
    return S_OK;
}
	  
STDMETHODIMP CCorProfilerCallback::ModuleUnloadFinished(ModuleID moduleID, HRESULT hrStatus)
{
	return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ModuleAttachedToAssembly(ModuleID moduleID, AssemblyID assemblyID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ClassLoadStarted(ClassID classID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ClassLoadFinished(ClassID classID, HRESULT hrStatus)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ClassUnloadStarted(ClassID classID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ClassUnloadFinished(ClassID classID, HRESULT hrStatus)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::FunctionUnloadStarted(FunctionID functionID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::JITCompilationStarted(FunctionID functionID, BOOL fIsSafeToBlock)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::JITCompilationFinished(FunctionID functionID, HRESULT hrStatus, BOOL fIsSafeToBlock)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::JITCachedFunctionSearchStarted(FunctionID functionID, BOOL *pbUseCachedFunction)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::JITCachedFunctionSearchFinished(FunctionID functionID, COR_PRF_JIT_CACHE result)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::JITFunctionPitched(FunctionID functionID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::JITInlining(FunctionID callerID, FunctionID calleeID, BOOL *pfShouldInline)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::UnmanagedToManagedTransition(FunctionID functionID, COR_PRF_TRANSITION_REASON reason)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ManagedToUnmanagedTransition(FunctionID functionID, COR_PRF_TRANSITION_REASON reason)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ThreadCreated(ThreadID threadID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ThreadDestroyed(ThreadID threadID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ThreadAssignedToOSThread(ThreadID managedThreadID, DWORD osThreadID) 
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RemotingClientInvocationStarted()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RemotingClientSendingMessage(GUID *pCookie, BOOL fIsAsync)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RemotingClientReceivingReply(GUID *pCookie, BOOL fIsAsync)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RemotingClientInvocationFinished()
{
	return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RemotingServerReceivingMessage(GUID *pCookie, BOOL fIsAsync)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RemotingServerInvocationStarted()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RemotingServerInvocationReturned()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RemotingServerSendingReply(GUID *pCookie, BOOL fIsAsync)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RuntimeSuspendStarted(COR_PRF_SUSPEND_REASON suspendReason)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RuntimeSuspendFinished()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RuntimeSuspendAborted()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RuntimeResumeStarted()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RuntimeResumeFinished()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RuntimeThreadSuspended(ThreadID threadID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RuntimeThreadResumed(ThreadID threadID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::MovedReferences(ULONG cmovedObjectIDRanges, ObjectID oldObjectIDRangeStart[], ObjectID newObjectIDRangeStart[], ULONG cObjectIDRangeLength[])
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ObjectAllocated(ObjectID objectID, ClassID classID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ObjectsAllocatedByClass(ULONG classCount, ClassID classIDs[], ULONG objects[])
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ObjectReferences(ObjectID objectID, ClassID classID, ULONG objectRefs, ObjectID objectRefIDs[])
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RootReferences(ULONG rootRefs, ObjectID rootRefIDs[])
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionThrown(ObjectID thrownObjectID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionUnwindFunctionEnter(FunctionID functionID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionUnwindFunctionLeave()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionSearchFunctionEnter(FunctionID functionID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionSearchFunctionLeave()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionSearchFilterEnter(FunctionID functionID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionSearchFilterLeave()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionSearchCatcherFound(FunctionID functionID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionCLRCatcherFound()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionCLRCatcherExecute()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionOSHandlerEnter(FunctionID functionID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionOSHandlerLeave(FunctionID functionID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionUnwindFinallyEnter(FunctionID functionID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionUnwindFinallyLeave()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionCatcherEnter(FunctionID functionID,
    											 ObjectID objectID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ExceptionCatcherLeave()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::COMClassicVTableCreated(ClassID wrappedClassID, REFGUID implementedIID, void *pVTable, ULONG cSlots)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::COMClassicVTableDestroyed(ClassID wrappedClassID, REFGUID implementedIID, void *pVTable)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::ThreadNameChanged(ThreadID threadID, ULONG cchName, WCHAR name[])
{
	return S_OK;
}

STDMETHODIMP CCorProfilerCallback::GarbageCollectionStarted(int cGenerations, BOOL generationCollected[], COR_PRF_GC_REASON reason)
{
	return S_OK;
}

STDMETHODIMP CCorProfilerCallback::SurvivingReferences(ULONG cSurvivingObjectIDRanges, ObjectID objectIDRangeStart[], ULONG cObjectIDRangeLength[])
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::GarbageCollectionFinished()
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::FinalizeableObjectQueued(DWORD finalizerFlags, ObjectID objectID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::RootReferences2(ULONG cRootRefs, ObjectID rootRefIDs[], COR_PRF_GC_ROOT_KIND rootKinds[], COR_PRF_GC_ROOT_FLAGS rootFlags[], UINT_PTR rootIDs[])
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::HandleCreated(GCHandleID handleID, ObjectID initialObjectID)
{
    return S_OK;
}

STDMETHODIMP CCorProfilerCallback::HandleDestroyed(GCHandleID handleID)
{
    return S_OK;
}




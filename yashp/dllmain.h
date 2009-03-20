
// This class represents the module for the DLL

class CProfilerServerModule : public CAtlDllModuleT< CProfilerServerModule > {
public :

	// Provides a way for ATL to obtain the libid of the type library.
	DECLARE_LIBID(LIBID_ProfilerServerLib)

	// Specifies the information required to automatically register the appid.
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_PROFILERSERVER, "{21EC66D5-D8CF-4A3A-B5BA-6772AECD3154}")

};

extern class CProfilerServerModule _AtlModule;

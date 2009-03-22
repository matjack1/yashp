// dllmain.h : Declaration of module class.

class CYashpModule : public CAtlDllModuleT< CYashpModule >
{
public :
	DECLARE_LIBID(LIBID_YashpLib)
};

extern class CYashpModule _AtlModule;

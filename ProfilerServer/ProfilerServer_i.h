

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0499 */
/* at Fri Mar 20 22:24:06 2009
 */
/* Compiler settings for .\ProfilerServer.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __ProfilerServer_i_h__
#define __ProfilerServer_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IProfilerCallback_FWD_DEFINED__
#define __IProfilerCallback_FWD_DEFINED__
typedef interface IProfilerCallback IProfilerCallback;
#endif 	/* __IProfilerCallback_FWD_DEFINED__ */


#ifndef __ProfilerCallback_FWD_DEFINED__
#define __ProfilerCallback_FWD_DEFINED__

#ifdef __cplusplus
typedef class ProfilerCallback ProfilerCallback;
#else
typedef struct ProfilerCallback ProfilerCallback;
#endif /* __cplusplus */

#endif 	/* __ProfilerCallback_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __IProfilerCallback_INTERFACE_DEFINED__
#define __IProfilerCallback_INTERFACE_DEFINED__

/* interface IProfilerCallback */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IProfilerCallback;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("80ECA64F-77B2-4D95-9DF1-005196D62E1A")
    IProfilerCallback : public IDispatch
    {
    public:
    };
    
#else 	/* C style interface */

    typedef struct IProfilerCallbackVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IProfilerCallback * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IProfilerCallback * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IProfilerCallback * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IProfilerCallback * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IProfilerCallback * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IProfilerCallback * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IProfilerCallback * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } IProfilerCallbackVtbl;

    interface IProfilerCallback
    {
        CONST_VTBL struct IProfilerCallbackVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IProfilerCallback_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IProfilerCallback_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IProfilerCallback_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IProfilerCallback_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IProfilerCallback_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IProfilerCallback_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IProfilerCallback_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IProfilerCallback_INTERFACE_DEFINED__ */



#ifndef __ProfilerServerLib_LIBRARY_DEFINED__
#define __ProfilerServerLib_LIBRARY_DEFINED__

/* library ProfilerServerLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_ProfilerServerLib;

EXTERN_C const CLSID CLSID_ProfilerCallback;

#ifdef __cplusplus

class DECLSPEC_UUID("5553A90C-3E0E-4D55-833A-CB5128BD7258")
ProfilerCallback;
#endif
#endif /* __ProfilerServerLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif



/*****************************************************************************
 * DotNetProfiler
 * 
 * Copyright (c) 2006 Scott Hackett
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author be held liable for any damages arising from the
 * use of this software. Permission to use, copy, modify, distribute and sell
 * this software for any purpose is hereby granted without fee, provided that
 * the above copyright notice appear in all copies and that both that
 * copyright notice and this permission notice appear in supporting
 * documentation.
 * 
 * Scott Hackett (code@scotthackett.com)
 *****************************************************************************/

#include "StdAfx.h"
#include "FunctionInfo.h"
#include <sstream>

using namespace std;
const int MAX_FUNCTION_LENGTH=2048;

// STL includes
#pragma warning ( disable : 4530 )

ICorProfilerInfo2* CFunctionInfo::m_profilerInfo = NULL;

void CFunctionInfo::setProfilerInfo(ICorProfilerInfo2* info)
{
	m_profilerInfo = info;
}

CFunctionInfo::CFunctionInfo(FunctionID functionID)
{
	m_functionID = functionID;
	m_callCount = 0;

	fillSignature();
}

CFunctionInfo::~CFunctionInfo()
{
}

FunctionID CFunctionInfo::getFunctionID()
{
	return m_functionID;
}

long CFunctionInfo::getCallCount()
{
	return m_callCount;
}

void CFunctionInfo::incrementCallCount()
{
	m_callCount++;
}

void CFunctionInfo::fillSignature() {

	if (!m_profilerInfo) return;

	ULONG args;
	WCHAR returnTypeString[ MAX_FUNCTION_LENGTH ];
	WCHAR parametersString[ MAX_FUNCTION_LENGTH ];
	WCHAR functionNameString[ MAX_FUNCTION_LENGTH ];
	WCHAR classNameString[ MAX_FUNCTION_LENGTH ];

	fillSignatureImpl( &m_iMethodAttributes, &args, returnTypeString, parametersString, classNameString, functionNameString );

	m_sReturnType = CW2A( returnTypeString );
	m_sParameters = CW2A( parametersString );
	m_sClassName = CW2A( classNameString );
	m_sFunctionName = CW2A( functionNameString );
}

bool CFunctionInfo::isFiltered(COR_PRF_FRAME_INFO frameInfo, wchar_t *appPath) {

	ClassID classId = 0;
	ModuleID moduleId = 0;
	mdToken token = 0;
	ULONG32 cTypeArgs = 10;
	ULONG32 retTypeArgs = 0;
	ClassID typeArgs[10];
    HRESULT hr = S_OK;

	hr = m_profilerInfo->GetFunctionInfo2(
		m_functionID,
		frameInfo,
		&classId,
		&moduleId,
		&token,
		cTypeArgs,
		&retTypeArgs,
		typeArgs
	);

	if(SUCCEEDED(hr)) {
		LPCBYTE baseLoadAddress;
		WCHAR szName[2048];
		AssemblyID assemblyID;
		hr = m_profilerInfo->GetModuleInfo(
			moduleId,
			&baseLoadAddress,
			1024,
			NULL,
			szName,
			&assemblyID
		);

		if(SUCCEEDED(hr)) {
			// if this didn't originate from our module, we don't care
			// so we don't see all of the CLR calls going on (and there 
			// are a ton of them which is why we are doing this!)
			if(wcsicmp(szName, appPath) == 0 ){			
				return true;
			}
		}
	}
	return false;
}

HRESULT CFunctionInfo::fillSignatureImpl(
	UINT32* methodAttributes,
	ULONG *argCount,
	WCHAR *returnType, 
	WCHAR *parameters,
	WCHAR *className,
	WCHAR *funName
)
{
	HRESULT hr = E_FAIL; // assume success

	// init return values
	*argCount = 0;
	returnType[0] = NULL; 
	parameters[0] = NULL;
	funName[0] = NULL;
	className[0] = NULL;

	if ( m_functionID == NULL ) {
		swprintf( funName, L"UNMANAGED FRAME" );
		return S_OK;
	}

	mdToken	token;
	ClassID classID;
	ModuleID moduleID;
	IMetaDataImport *metaDataImport = NULL;	
	mdToken moduleToken;
    
	// Get the classID 
	try {
		hr = m_profilerInfo->GetFunctionInfo(m_functionID, &classID, &moduleID, &moduleToken );
	} catch ( ... ) {
		hr = E_FAIL;
	}

	if (FAILED(hr)) {
		swprintf( funName, L"FAILED" );
		return S_OK;
	}
	
	std::stringstream out;
	out << moduleID;
	m_classID = out.str();

	// Get the MetadataImport interface and the metadata token 
	hr = m_profilerInfo->GetTokenAndMetaDataFromFunction( m_functionID, 
       								 				IID_IMetaDataImport, 
													(IUnknown **)&metaDataImport,
													&token );
	if (FAILED(hr)) {
		swprintf( funName, L"FAILED" );
		return S_OK;
	}

	hr = metaDataImport->GetMethodProps( token,
									NULL,
									funName,
									MAX_FUNCTION_LENGTH,
									0,
									0,
									NULL,
									NULL,
									NULL, 
									NULL );
	if (FAILED(hr)) {
		swprintf( funName, L"FAILED" );
		return S_OK;
	}

	mdTypeDef classToken = NULL;

	hr = m_profilerInfo->GetClassIDInfo( classID, 
        						   NULL,  
								   &classToken );
	
	if (FAILED(hr)) {
		swprintf( funName, L"FAILED" );
		return S_OK;
	}

	if ( classToken != mdTypeDefNil ){
		hr = metaDataImport->GetTypeDefProps( classToken, 
										 className, 
										 MAX_FUNCTION_LENGTH,
										 NULL, 
										 NULL, 
										 NULL ); 
	}

	DWORD methodAttr = 0;
	PCCOR_SIGNATURE sigBlob = NULL;

	hr = metaDataImport->GetMethodProps( (mdMethodDef) token,
									NULL,
									NULL,
									0,
									NULL,
									&methodAttr,
									&sigBlob,
									NULL,
									NULL,
									NULL );
	if (FAILED(hr)) {
		swprintf( funName, L"FAILED" );
		return S_OK;
	}


	ULONG callConv;

	// Is the method static ?
	*methodAttributes = methodAttr;

	// Make sure we have a method signature.
	char buffer[2 * MAX_FUNCTION_LENGTH];
	sigBlob += CorSigUncompressData( sigBlob, &callConv );
	if ( callConv != IMAGE_CEE_CS_CALLCONV_FIELD ) {
		static WCHAR* callConvNames[8] = 
		{	
			L"", 
			L"unmanaged cdecl ", 
			L"unmanaged stdcall ",	
			L"unmanaged thiscall ",	
			L"unmanaged fastcall ",	
			L"vararg ",	 
			L"<error> "	 
			L"<error> "	 
		};	
		buffer[0] = '\0';
		if ( (callConv & 7) != 0 )
			sprintf( buffer, "%s ", callConvNames[callConv & 7]);	
		
		// Grab the argument count
		sigBlob += CorSigUncompressData( sigBlob, argCount );

		m_argCount = (int) (*argCount);

		// Get the return type
		sigBlob = parseElementType( metaDataImport, sigBlob, buffer );

		// if the return typ returned back empty, write void
		if ( buffer[0] == '\0' )
			sprintf( buffer, "void" );

		swprintf( returnType, L"%S",buffer );
		
		// Get the parameters						
		for ( ULONG i = 0; 
			  (SUCCEEDED( hr ) && (sigBlob != NULL) && (i < (*argCount))); 
			  i++ ) 
		{
			buffer[0] = '\0';

			sigBlob = parseElementType( metaDataImport, sigBlob, buffer );									
			if ( i == 0 )
				swprintf( parameters, L"%S", buffer );

			else if ( sigBlob != NULL )
				swprintf( parameters, L"%s, %S", parameters, buffer );
			
			else
				hr = E_FAIL;
		}								    								
	} else {
		// Get the return type
		buffer[0] = '\0';
		sigBlob = parseElementType( metaDataImport, sigBlob, buffer );
		swprintf( returnType, L"%s %S",returnType, buffer );
	}

	metaDataImport->Release();

	return hr;
}

PCCOR_SIGNATURE CFunctionInfo::parseElementType( IMetaDataImport *metaDataImport, PCCOR_SIGNATURE signature, char *buffer )
{
	switch ( *signature++ ) 
	{	
		case ELEMENT_TYPE_VOID:
			strcat( buffer, "void" );	
			break;					
		case ELEMENT_TYPE_BOOLEAN:	
			strcat( buffer, "bool" );	
			break;	
		case ELEMENT_TYPE_CHAR:
			strcat( buffer, "wchar" );	
			break;		
		case ELEMENT_TYPE_I1:
			strcat( buffer, "int8" );	
			break;		
		case ELEMENT_TYPE_U1:
			strcat( buffer, "unsigned int8" );	
			break;		
		case ELEMENT_TYPE_I2:
			strcat( buffer, "int16" );	
			break;		
		case ELEMENT_TYPE_U2:
			strcat( buffer, "unsigned int16" );	
			break;			
		case ELEMENT_TYPE_I4:
			strcat( buffer, "int32" );	
			break;
		case ELEMENT_TYPE_U4:
			strcat( buffer, "unsigned int32" );	
			break;		
		case ELEMENT_TYPE_I8:
			strcat( buffer, "int64" );	
			break;		
		case ELEMENT_TYPE_U8:
			strcat( buffer, "unsigned int64" );	
			break;		
		case ELEMENT_TYPE_R4:
			strcat( buffer, "float32" );	
			break;			
		case ELEMENT_TYPE_R8:
			strcat( buffer, "float64" );	
			break;		
		case ELEMENT_TYPE_U:
			strcat( buffer, "unsigned int" );	
			break;		 
		case ELEMENT_TYPE_I:
			strcat( buffer, "int" );	
			break;			  
		case ELEMENT_TYPE_OBJECT:
			strcat( buffer, "Object" );	
			break;		 
		case ELEMENT_TYPE_STRING:
			strcat( buffer, "String" );	
			break;		 
		case ELEMENT_TYPE_TYPEDBYREF:
			strcat( buffer, "refany" );	
			break;				       
		case ELEMENT_TYPE_CLASS:	
		case ELEMENT_TYPE_VALUETYPE:
		case ELEMENT_TYPE_CMOD_REQD:
		case ELEMENT_TYPE_CMOD_OPT:
			{	
				mdToken	token;	
				char classname[MAX_FUNCTION_LENGTH];

				classname[0] = '\0';
   				signature += CorSigUncompressToken( signature, &token ); 
				if ( TypeFromToken( token ) != mdtTypeRef )
				{
    				HRESULT	hr;
					WCHAR zName[MAX_FUNCTION_LENGTH];
					
					hr = metaDataImport->GetTypeDefProps( token, 
													 zName,
													 MAX_FUNCTION_LENGTH,
													 NULL,
													 NULL,
													 NULL );
					if ( SUCCEEDED( hr ) )
						wcstombs( classname, zName, MAX_FUNCTION_LENGTH );
				}
				strcat( buffer, classname );		
			}
			break;	
		case ELEMENT_TYPE_SZARRAY:	 
			signature = parseElementType( metaDataImport, signature, buffer ); 
			strcat( buffer, "[]" );
			break;		
		case ELEMENT_TYPE_ARRAY:	
			{	
				ULONG rank;
				signature = parseElementType( metaDataImport, signature, buffer );                 
				rank = CorSigUncompressData( signature );													
				if ( rank == 0 ) 
					strcat( buffer, "[?]" );

				else 
				{
					ULONG *lower;	
					ULONG *sizes; 	
					ULONG numsizes; 
					ULONG arraysize = (sizeof ( ULONG ) * 2 * rank);
	                                     
					lower = (ULONG *)_alloca( arraysize );                                                        
					memset( lower, 0, arraysize ); 
					sizes = &lower[rank];

					numsizes = CorSigUncompressData( signature );	
					if ( numsizes <= rank )
					{
        				ULONG numlower;
	                    
						for ( ULONG i = 0; i < numsizes; i++ )	
							sizes[i] = CorSigUncompressData( signature );	
	                    
						numlower = CorSigUncompressData( signature );	
						if ( numlower <= rank )
						{
							for (ULONG i = 0; i < numlower; i++)	
								lower[i] = CorSigUncompressData( signature ); 
	                        
							strcat( buffer, "[" );	
							for (ULONG i = 0; i < rank; i++ )	
							{	
								if ( (sizes[i] != 0) && (lower[i] != 0) )	
								{	
									if ( lower[i] == 0 )	
										sprintf ( buffer, "%d", sizes[i] );	

									else	
									{	
										sprintf( buffer, "%d", lower[i] );	
										strcat( buffer, "..." );	
										
										if ( sizes[i] != 0 )	
											sprintf( buffer, "%d", (lower[i] + sizes[i] + 1) );	
									}	
								}
	                            	
								if ( i < (rank - 1) ) 
									strcat( buffer, "," );	
							}	
	                        
							strcat( buffer, "]" );  
						}						
					}
				}
			} 
			break;	
		case ELEMENT_TYPE_PINNED:
			signature = parseElementType( metaDataImport, signature, buffer ); 
			strcat( buffer, "pinned" );	
			break;	
		case ELEMENT_TYPE_PTR:   
			signature = parseElementType( metaDataImport, signature, buffer ); 
			strcat( buffer, "*" );	
			break;   
		case ELEMENT_TYPE_BYREF:   
			signature = parseElementType( metaDataImport, signature, buffer ); 
			strcat( buffer, "&" );	
			break;  		    
		default:	
		case ELEMENT_TYPE_END:	
		case ELEMENT_TYPE_SENTINEL:	
			strcat( buffer, "<UNKNOWN>" );	
			break;				                      				            
	} // switch	
	
	return signature;
}
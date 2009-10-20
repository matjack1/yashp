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

#pragma once


#include "CorProfilerCallback.h"
#include <string>
#include <iostream>

class CFunctionInfo
{
public:
	CFunctionInfo(FunctionID functionID);
	virtual ~CFunctionInfo();

	inline std::string getFunctionName() { return m_sFunctionName; }
	inline std::string getClassName() { return m_sClassName; }
	inline std::string getParameters() { return m_sParameters; }
	inline std::string getReturnType() { return m_sReturnType; }
	inline std::string getClassID() { return m_classID; }
	inline std::string getStaticMethod() { return m_staticMethod; }
	inline int getArgCount() { return m_argCount; }
	inline long int getInstanceAddr() { return m_instanceAddr; }

	inline void setStaticMethod(std::string isStatic) { m_staticMethod = isStatic; }
	inline void setInstanceAddr(long int in_addr) { m_instanceAddr = in_addr; }

	bool isFiltered(COR_PRF_FRAME_INFO frameInfo, wchar_t *appPath);

	FunctionID getFunctionID();
	long getCallCount();

	void incrementCallCount();

	static void setProfilerInfo(ICorProfilerInfo2* profilerInfo);

private:
	FunctionID m_functionID;
	
	long m_callCount;

	void fillSignature();
	HRESULT fillSignatureImpl(UINT32* methodAttributes, ULONG *argCount, WCHAR *returnType, WCHAR *parameters, WCHAR *className, WCHAR *funName);
	PCCOR_SIGNATURE parseElementType( IMetaDataImport *metaDataImport, PCCOR_SIGNATURE signature, char *buffer );
	static ICorProfilerInfo2* m_profilerInfo;

	std::string m_sReturnType;
	std::string m_sParameters;
	std::string m_sClassName;
	std::string m_sFunctionName;
	std::string m_classID;
	std::string m_staticMethod;
	int m_argCount;
	long int m_instanceAddr;
	UINT32 m_iMethodAttributes;
};

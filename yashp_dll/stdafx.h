#pragma once

#ifndef STRICT
	#define STRICT
#endif


#include "resource.h"
#include "targetver.h"


#define _ATL_APARTMENT_THREADED
#define _ATL_NO_AUTOMATIC_NAMESPACE
#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// some CString constructors will be explicit
#include <atlbase.h>
#include <atlcom.h>
#include <atlctl.h>
using namespace ATL;


#include <iostream>
#include <map>
#include <string>
#include <stack>
#include <utility>
#include <memory>
#include <sstream>
#include <fstream>
#include <vector>
using namespace std;


#include "windows.h"
#include "winnt.h"


#include "cor.h"
#include "corhdr.h"
#include "corhlpr.h"
#include "corerror.h"
#include "corpub.h"
#include "corprof.h"


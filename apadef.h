// APADEF.H
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

#ifndef __APADEF_H__
#define __APADEF_H__

#if !defined(__E32BASE_H__)
#include <e32base.h>
#endif

// comand line tokens

/**
@internalComponent
*/
const TUint KApaCommandLetterOpen='O';

/**
@internalComponent
*/
const TUint KApaCommandLetterCreate='C';

/**
@internalComponent
*/
const TUint KApaCommandLetterRun='R';

/**
@internalComponent
*/
const TUint KApaCommandLetterBackground='B';

/**
@internalComponent
*/
const TUint KApaCommandLetterViewActivate='V';

/**
@internalComponent
*/
const TUint KApaCommandLetterRunWithoutViews='W';

/**
@internalComponent
*/
const TUint KApaCommandLetterBackgroundAndWithoutViews='A';

/** Defines the command codes used to indicate the way an application is to be 
launched.

@publishedAll 
@released
@see CApaCommandLine */
enum TApaCommand
	{
	/** The application is to open the document specified on the command line. */
	EApaCommandOpen,
	/** The application is to create the document specified on the command line. */
	EApaCommandCreate,
	/** The application is to run and open the last used document. */
	EApaCommandRun,
	/** The application is to open the last used document and to run in the background. */
	EApaCommandBackground,
	/** Reserved for future use. */
	EApaCommandViewActivate,
	/** The application is to run without views.
	
	This should NOT be used for view based applications. */
	EApaCommandRunWithoutViews,
	/** The application is to run in the background and viewless mode */
	EApaCommandBackgroundAndWithoutViews
	};

/**
@publishedAll 
@released
*/
const TInt KApaMaxAppCaption=0x100;	// Default name is a file name, so the same limit is used

// TApaAppCaption is the current language name of the app (for task lists, dialogs etc)

/** Defines a modifiable buffer descriptor to contain the caption or the short caption 
for an application. 

@publishedAll 
@released */
typedef TBuf<KApaMaxAppCaption> TApaAppCaption; 

/**
@internalComponent
*/
const TInt KApaMaxCommandLine=0x100;	

/**
@internalComponent
*/
typedef TBuf<KApaMaxCommandLine> TApaCommandLine;

/**
@internalComponent
*/
const TInt KApaMaxAppFileName=0x10;	// Length of App's filename without path or extension (not it's caption)

/**
@internalComponent
*/
typedef TBuf<KApaMaxAppFileName> TApaAppFileName;

/** Maximum length of an application group name.

@publishedAll 
@released
@see TApaAppGroupName */
const TInt KApaMaxAppGroupName=0x10; // Length of App's logical groupname

/** An application group name.

This is a name that allows applications to be categorized, for instance "Games" 
or "Utilities". 

@publishedAll 
@released */
typedef TBuf<KApaMaxAppGroupName> TApaAppGroupName;

/** The hexadecimal value of the 2nd UID that identifies a DLL as being an ASCII UI 
application. In .mmp files, the hexadecimal number is explicitly inserted 
as the first value following the UID keyword. 

@publishedAll 
@deprecated */
const TInt KAppUidValue8 = 0x1000006c;

/** The 2nd UID that identifies a DLL as being an ASCII UI application. 

@publishedAll 
@deprecated */
const TUid KUidApp8={KAppUidValue8};

/** The hexadecimal value of the 2nd UID that defines a DLL as being a Unicode 
UI application. In .mmp files, the hexadecimal number is explicitly inserted 
as the first value following the UID keyword.

@publishedAll 
@released
@see KAppUidValue */
const TInt KAppUidValue16 = 0x100039CE;

/** The 2nd UID that defines a DLL as being a Unicode UI application.

@publishedAll 
@released
@see KUidApp */
const TUid KUidApp16={KAppUidValue16};

//
// 2nd Uid for app doc files
/**
@publishedAll 
@deprecated
*/
const TUid KUidAppDllDoc8={268435565}; 

/**
@publishedAll 
@released
*/
const TUid KUidAppDllDoc16={0x10003A12};

/** The UID encoded in a TPictureHeader that identifies a picture as a door (for 
ASCII builds). 

@publishedAll 
@deprecated
*/
const TUid KUidPictureTypeDoor8={268435537};

/** The UID encoded in a TPictureHeader that identifies a picture as a door (for 
Unicode builds).

@publishedAll 
@released
@see KUidPictureTypeDoor */
const TUid KUidPictureTypeDoor16={0x10003A33};

//
// Uid's for streams in stream dictionaries
/**
@publishedAll 
@deprecated
*/
const TUid KUidSecurityStream8={268435661};

/**
@publishedAll 
@released
*/
const TUid KUidSecurityStream16={0x10003A40};

/**
@publishedAll 
@deprecated
*/
const TUid KUidAppIdentifierStream8={268435593}; // stream containing a TApaAppIdentifier

/**
@publishedAll 
@released
*/
const TUid KUidAppIdentifierStream16={0x10003A34};

#ifdef _UNICODE
/** The type-independent 2nd UID that identifies a DLL as being a UI application.

@publishedAll 
@released
@see KUidApp16
@see KUidApp8 */
#define KUidApp KUidApp16
/** The type-independent hexadecimal value of the 2nd UID that identifies a DLL as 
being a UI application.

@publishedAll 
@released
@see KAppUidValue16
@see KAppUidValue8 */
#define KAppUidValue KAppUidValue16
/**
@publishedAll 
@released
*/
#define KUidAppDllDoc KUidAppDllDoc16
/** The type independent UID encoded in a TPictureHeader that identifies a picture 
as a door.

@publishedAll 
@released
@see KUidPictureTypeDoor16
@see KUidPictureTypeDoor8
@see TPictureHeader
@see TApaModelDoorFactory::NewPictureL() */
#define KUidPictureTypeDoor KUidPictureTypeDoor16
/**
@publishedAll 
@released
*/
#define KUidAppIdentifierStream KUidAppIdentifierStream16
/**
@publishedAll 
@released
*/
#define KUidSecurityStream KUidSecurityStream16
#else
/** The type independent 2nd UID that defines a DLL as being a UI application.

@see KUidApp16
@see KUidApp8 */
#define KUidApp KUidApp8
/** The type independent hexadecimal value of the 2nd UID that defines a DLL as 
being a UI application.

@see KAppUidValue16
@see KAppUidValue8 */
#define KAppUidValue KAppUidValue8
#define KUidAppDllDoc KUidAppDllDoc8
/** The type independent UID encoded in a TPictureHeader that identifies a picture 
as a door.

@see KUidPictureTypeDoor16
@see KUidPictureTypeDoor8
@see TPictureHeader
@see TApaModelDoorFactory::NewPictureL() */
#define KUidPictureTypeDoor KUidPictureTypeDoor8
#define KUidAppIdentifierStream KUidAppIdentifierStream8
#define KUidSecurityStream KUidSecurityStream8
#endif

const TUid KUidFileEmbeddedApplicationInterfaceUid={0x101f8c96};

#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Utility;


namespace SISX.Fields
{  

    public class SISExpression : SISField
    {
        public UInt32 operatore;                // TOperator
        public Int32 intValue;                  // Presente se EPrimTypeOption, EPrimTypeVariable, EPrimTypeNumber
        public SISString strValue;              // Presente se EPrimTypeString, EFuncExists
        public SISExpression leftExpression;    // Presente se e' Binary, Logical, EFuncAppProperties, EFuncDevProperties
        public SISExpression rightExpression;   // Presente se NON e' EPrimTypeOption, EPrimTypeVariable, EPrimTypeNumber, EPrimTypeString, EFuncExists


        public SISExpression(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            operatore = br.ReadUInt32();
            intValue = br.ReadInt32();
            // In base al valore di operatore decide cos'altro leggere...
            TOperator oper = (TOperator)Enum.Parse( typeof( TOperator ), operatore.ToString() );
            if (oper == TOperator.EPrimTypeString || oper == TOperator.EFuncExists)
            {
                strValue = (SISString)SISField.Factory( br );
            }

            if (oper == TOperator.EBinOpEqual ||
                oper == TOperator.EBinOpGreaterOrEqual ||
                oper == TOperator.EBinOpGreaterThan ||
                oper == TOperator.EBinOpLessOrEqual ||
                oper == TOperator.EBinOpLessThan ||
                oper == TOperator.EBinOpNotEqual ||
                oper == TOperator.ELogOpAnd ||
                oper == TOperator.ELogOpOr ||
                oper == TOperator.EFuncAppProperties ||
                oper == TOperator.EFuncDevProperties)
            {
                leftExpression = (SISExpression)SISField.Factory( br );
            }

            // NON CONGRUENTE CON IL PDF... AGGIUNTO EFuncDevProperties
            if ((leftExpression != null || oper == TOperator.EUnaryOpNot) && oper != TOperator.EFuncDevProperties)
            {
                rightExpression = (SISExpression)SISField.Factory( br );
            }
        }

        public override string ToString()
        {
            TOperator oper = (TOperator)Enum.Parse( typeof( TOperator ), operatore.ToString() );
            string ris = "";
            switch (oper)
            {
                case TOperator.EBinOpEqual:
                    {
                        ris = leftExpression.ToString() + " == " + rightExpression.ToString();
                        break;
                    }
                case TOperator.EBinOpGreaterOrEqual:
                    {
                        ris = leftExpression.ToString() + " >= " + rightExpression.ToString();
                        break;
                    }
                case TOperator.EBinOpGreaterThan:
                    {
                        ris = leftExpression.ToString() + " > " + rightExpression.ToString();
                        break;
                    }
                case TOperator.EBinOpLessOrEqual:
                    {
                        ris = leftExpression.ToString() + " <= " + rightExpression.ToString();
                        break;
                    }
                case TOperator.EBinOpLessThan:
                    {
                        ris = leftExpression.ToString() + " < " + rightExpression.ToString();
                        break;
                    }
                case TOperator.EBinOpNotEqual:
                    {
                        ris = leftExpression.ToString() + " != " + rightExpression.ToString();
                        break;
                    }
                case TOperator.EFuncAppProperties:
                    {
                        ris = "APPPROP( " + leftExpression.ToString() + "," + rightExpression.ToString() + " )";
                        break;
                    }
                case TOperator.EFuncDevProperties:
                    {
                        ris = "PACKAGE( " + leftExpression.ToString() +" )";
                        break;
                    }
                case TOperator.EFuncExists:
                    {
                        ris = "EXISTS( " + strValue + " )";
                        break;
                    }
                case TOperator.ELogOpAnd:
                    {
                        ris = "( "+leftExpression.ToString() +" ) AND ( " + rightExpression.ToString() + " )";
                        break;
                    }
                case TOperator.ELogOpOr:
                    {
                        ris = "( " + leftExpression.ToString() + " ) OR ( " + rightExpression.ToString() + " )";
                        break;
                    }
                case TOperator.EPrimTypeNumber:
                    {
                        ris = "0x" + String.Format( "{0:X8}", intValue );
                        break;
                    }
                case TOperator.EPrimTypeOption:
                    {
                        ris = "option"+intValue;
                        break;
                    }
                case TOperator.EPrimTypeString:
                    {
                        ris = strValue.ToString();
                        break;
                    }
                case TOperator.EPrimTypeVariable:
                    {
                        ris = Bits.GetStringFromEnum<TAttribute>( (uint)intValue );
                        if (ris == "")
                        {
                            ris = Bits.GetStringFromEnum<TVariableIndex>( (uint)intValue );
                            if (ris == "")
                                ris = "NULL";/**/
                        }
                        break;
                    }
                case TOperator.EUnaryOpNot:
                    {
                        ris = "NOT( " + rightExpression.ToString() + " )";
                        break;
                    }
                default:
                    throw new Exception("Operation Error: "+operatore);
            }

            // Gestione Language
            if (leftExpression != null && rightExpression != null)
            {
                if (leftExpression.ToString() == "EVarLanguage")
                {
                    string lang = Bits.GetStringFromEnum<TLanguage>((uint)rightExpression.intValue);
                    ris = ris.Replace( rightExpression.ToString(), lang );
                }
            }
//            string s = "true";
            return ris;
        }
    }


    enum TVariableIndex
    {
        // 0-0x1000 Reserved for HALData values see Appendix B        
        EVarLanguage = 0x1000,              // Errata Corrige: Il documento di specifica e' errato
        EVarRemoteInstall = 0x1001          // Errata Corrige: Il documento di specifica e' errato
    };


    enum TOperator
    {
        // Binary
        EBinOpEqual = 1,
        EBinOpNotEqual,
        EBinOpGreaterThan,
        EBinOpLessThan,
        EBinOpGreaterOrEqual,
        EBinOpLessOrEqual,

        // Logical
        ELogOpAnd,
        ELogOpOr,

        // Unary
        EUnaryOpNot,

        // Functions
        EFuncExists,            // EXISTS( string1 ) // Verifica esistenza del file string1...
        EFuncAppProperties,     // APPPROP( expr1, expr2 ) // expr1 e' UID mentre expr2 e' la key di cui rilevare il valore, leggere leftExpression e rightExpression
        EFuncDevProperties,     // PACKAGE( expr1 )  // expr1 e' UID da cercare, leggere leftExpression

        // Primitives
        EPrimTypeString,  
        EPrimTypeOption,  
        EPrimTypeVariable,
        EPrimTypeNumber   
    };


    enum TAttribute
    {
        /**
        Identifies the manufacturer of a device.
        If this is not enumerated in TManufacturer, then the manufacturer must
        obtain a value from the Symbian registry.
        
        @see HALData::TManufacturer
        */
        Manufacturer,


        /**
        The device specific hardware version number, as defined by
        the device manufacturer.
        */
        ManufacturerHardwareRev,


        /**
        The device specific version number, as defined by
        the device manufacturer.
        */
        ManufacturerSoftwareRev,


        /**
        The device specific software version number, as defined by
        the device manufacturer.
        */
        ManufacturerSoftwareBuild,


        /**
        This is device specific model number, as defined by
        the device manufacturer.
        */
        Model,


        /**
        This is the device specific UID, It is unique to the class /model
        of device. A value must be obtained from Symbian's UID registry for
        this attribute.
        */
        MachineUid,


        /**
        The Symbian OS specified device family identifier.
        If the device family is not one of those enumerated by TDeviceFamily,
        then the licensee must obtain a UID from Symbian for this attribute.
		
        @see HALData::TDeviceFamily
        */
        DeviceFamily,


        /**
        The Symbian OS specified device family version
        */
        DeviceFamilyRev,


        /**
        The CPU architecture used by this device. The values are enumerated
        by TCPU.
		
        @see HALData::TCPU
        */
        CPU,


        /**
        A revision number for the CPU architecture.
        */
        CPUArch,


        /**
        This is the default ABI used by CPU for user applications.
        The values are enumerated by HALData::TCPUABI.
        */
        CPUABI,


        /**
        The processor speed in KHz.
        */
        CPUSpeed,


        /**
        The reason for most recent system boot.
        This is dynamic and readonly; the values are enumerated by
        TSystemStartupReason.

        @see HALData::TSystemStartupReason
        */
        ESystemStartupReason,


        /**
        This is the last exception code, in the case of system reboot.
        This is dynamic and readonly.
        */
        ESystemException,


        /**
        The time between system ticks, in microseconds.
        */
        SystemTickPeriod,


        /** 
        The total system RAM, in bytes.
        */
        MemoryRAM,


        /**
        The currently free system RAM.
		
        This is dynamic and readonly.
        */
        MemoryRAMFree,


        /**
        The total System ROM, in bytes.
        */
        MemoryROM,


        /**
        The MMU page size in bytes.
        */
        MemoryPageSize,


        /**
        Indicates the state of the power supply.
        
        It has the values:
        1 = Power is good (i.e. external power is available,
        or the 'power' battery is >= low);
        0 = otherwise.
        
        This is dynamic and readonly.
        */
        EPowerGood,


        /**
        The System (or 'Main') battery power level.
        The allowable values are enumerated by TPowerBatteryStatus

        This is dynamic and readonly,
		
        @see HALData::TPowerBatteryStatus
        */
        EPowerBatteryStatus,


        /**
        Indicates whether a backup power supply is available.
        It has the values:
        0 = the device does not support (or need) a backup battery source;
        1 = a backup batter source is present.
        This is dynamic and readonly
        */
        PowerBackup,


        /**
        The power level for backup power
        
        It has the values enumerated by TPowerBackupStatus.

        This is dynamic and readonly.
		
        @see HALData::TPowerBackupStatus
        */
        EPowerBackupStatus,


        /**
        Indicates the state of the external power.

        It has the values:
        0 = external power is not in use;
        1 = external power is in use.
		        
        This is dynamic and readonly.
        */
        EPowerExternal,


        /**
        A bitmask that describes the available keyboard types (it may support
        more than one).

        @see HALData::TKeyboard
        */
        Keyboard,


        /**
        */
        KeyboardDeviceKeys,


        /**
        */
        KeyboardAppKeys,


        /**
        Indicates whether the device can produce a click sound for
        each keypress.
		
        It has the values:
        0 = the device cannot produce a click sound for each keypress;
        1 = the device can produce a click sound.
        */
        KeyboardClick,


        /**
        The state of keyboard clicking.

        It has the values:
        0 = key click disabled;
        1 = key click enabled.
        
        This is dynamic and writeable.

        @capability WriteDeviceData needed to Set this attribute
        */
        EKeyboardClickState,


        /**
        The keyboard click volume level.

        It can take a value in the range 0 to EkeyboardClickVolumeMax.
        
        This is dynamic and writeable.
        
        @see HALData::EkeyboardClickVolumeMax

        @capability WriteDeviceData needed to Set this attribute
        */
        EKeyboardClickVolume,


        /**
        The maximum value for EKeyboardClickVolume.
		
        @see HALData::EkeyboardClickVolume
        */
        KeyboardClickVolumeMax,


        /**
        The screen horizontal dimension in pixels.
        */
        DisplayXPixels,


        /**
        The screen vertical dimension in pixels.
        */
        DisplayYPixels,


        /**
        The screen horizontal dimension in twips.
        */
        DisplayXTwips,


        /**
        The screen vertical dimension in twips.
        */
        DisplayYTwips,


        /**
        The number of hues (colors or shades of grey) displayable on
        the screen.
        */
        DisplayColors,


        /**
        The state of the display.
		
        It has the values:
        0 = screen is turned off;
        1 = screen is on.
		
        This is dynamic and writeable.

        @capability PowerMgmt needed to Set this attribute
        */
        EDisplayState,


        /**
        The screen contrast level.
        It can take a value in the range 0 to EDisplayContrastMax.
        
        This is dynamic and writeable

        @see HALData::EDisplayContrastMax

        @capability WriteDeviceData needed to Set this attribute
        */
        EDisplayContrast,


        /**
        The maximum value for EDisplayContrast
		
        @see HALData::EDisplayContrast
        */
        DisplayContrastMax,


        /**
        Indicates whether there is a backlight on the device.

        It has the values:
        0 = there is no screen backlight;
        1 = a screen backlight is present.
        */
        Backlight,


        /**
        The current status of the backlight.

        It has the values:
        0 = off;
        1 = on.
        
        This is dynamic and writeable.

        @capability WriteDeviceData needed to Set this attribute
        */
        EBacklightState,


        /**
        Indicates whether a pen or digitizer is available for input.

        It has the values:
        0 = a pen/digitizer is not available for input;
        1 = a pen/digitizeris present.
        */
        Pen,


        /**
        The pen/digitizer horizontal resolution, in pixels.
        */
        PenX,


        /**
        The is the pen/digitizer vertical resolution, in pixels.
        */
        PenY,


        /**
        Indicates whether a pen tap will turn the display on.

        It has the values:
        0 = a pen tap has no effect;
        1 = a pent tap or press enables the display.
        
        The is dynamic and writeable

        @capability WriteDeviceData needed to Set this attribute
        */
        PenDisplayOn,


        /**
        Indicates whether the device can produce a click sound for
        each pen tap.

        It has the values:
        0 = the device cannot produce a click sound
        1 = production of a click sound is supported by the device.
        */
        PenClick,


        /**
        The state of pen clicking.
		
        It has the values:
        0 = pen clicking is disabled;
        1 = pen clicking is enabled.
		
        This is dynamic and writable.

        @capability WriteDeviceData needed to Set this attribute
        */
        EPenClickState,


        /**
        This pen click volume level.
        It can take a value in the range 0 to EPenClickVolumeMax.
        
        This value is dynamic and writable.
        
        @see HALData::EPenClickVolumeMax

        @capability WriteDeviceData needed to Set this attribute
        */
        EPenClickVolume,


        /**
        The maximum value for EPenClickVolume.
		
        @see HALData::EPenClickVolume
        */
        PenClickVolumeMax,


        /**
        Indicates whether a mouse is available for input.
		
        It has the values:
        0 = there is no mouse availablea pen/digitizeris present;
        1 = a mouse is available for input.
        */
        Mouse,


        /**
        The mouse horizontal resolution, in pixels.
        */
        MouseX,


        /**
        The mouse vertical resolution, in pixels.
        */
        MouseY,


        /**
        Describes the mouse cursor visibility.

        The value is enumerated by TMouseState.
        
        This is dynamic and writable.
        @see HALData::TMouseState
		
        @capability MultimediaDD needed to Set this attribute
        */
        EMouseState,


        /**
        Reserved for future use.
        @capability MultimediaDD needed to Set this attribute
        */
        EMouseSpeed,


        /**
        Reserved for future use.
        @capability MultimediaDD needed to Set this attribute
        */
        EMouseAcceleration,


        /**
        The number of buttons on the mouse.
        */
        MouseButtons,


        /**
        A bitmask defining the state of each button .

        For each bit, it has values:
        0 = up;
        1 = down.
        
        This is dynamic and read only.
        */
        EMouseButtonState,


        /**
        Defines the state of the case.
		
        It has the values:
        0 = case closed;
        1 = case opened.
        
        This is dynamic and read only.
        */
        ECaseState,


        /**
        Indicates whether the device has a case switch, that actions when
        the case opens and closes.
		
        It has values:
        0 = no;
        1 = yes.
        */
        CaseSwitch,


        /**
        Indicates whether the device is to switch on when case opens.
		
        It has the values:
        0 = disable device switchon when the case opens;
        1 = enable device  switchon when the case opens.
        
        This is dynamic and writeable.

        @capability WriteDeviceData needed to Set this attribute
        */
        ECaseSwitchDisplayOn,


        /**
        Indicates whether the device is to switch off when case close.

        It has the values:
        0 = disable device switchoff when the case closes;
        1 = enable device switchoff when the case closes.
        
        This is dynamic and writeable.

        @capability WriteDeviceData needed to Set this attribute
        */
        ECaseSwitchDisplayOff,


        /**
        The number of LEDs on the device.
        */
        LEDs,


        /**
        A bitmask defining the state of each LED.

        For each bit, it has values:
        0 = off;
        1 = on.
		
        This is dynamic and writeable.
        */
        ELEDmask,


        /**
        Indicates how the phone hardware is connected.
		
        It has the values:
        0 = phone hardware is not permanently connected;
        1 = phone hardware is permanently connected.
        */
        IntegratedPhone,


        /**
        @capability WriteDeviceData needed to Set this attribute
        */
        DisplayBrightness,


        /**
        */
        DisplayBrightnessMax,


        /**
        Inidcates the state of the keyboard backlight.
        
        It has the values:
        0 = keyboard backlight is off;
        1 = keyboard backlight is on.

        This is dynamic and writeable.

        @capability PowerMgmt needed to Set this attribute
        */
        KeyboardBacklightState,


        /**
        Power supply to an accessory port.

        It has the values:
        0 = turn off power to an accessory port on the device;
        1 = turn on power.
        
        This is dynamic and writeable.

        @capability PowerMgmt needed to Set this attribute
        */
        AccessoryPower,


        /**
        A 2 decimal digit language index. 
		
        It is used as the two digit language number that is the suffix of
        language resource DLLs, e.g ELOCL.01.

        The locale with this language index is loaded the next time that
        the device boots.

        This is dynamic and writeable.

        @see TLanguage

        @capability WriteDeviceData needed to Set this attribute
        */
        ELanguageIndex,


        /**
        A 2 decimal digit (decimal) language keyboard index.
        It is used as the two digit language number that is the suffix of
        language resource DLLs, e.g. EKDATA.01.
		
        @see TLanguage

        @capability WriteDeviceData needed to Set this attribute
        */
        EKeyboardIndex,


        /**
        The maximum allowable size of RAM drive, in bytes.
        */
        EMaxRAMDriveSize,


        /**
        Indicates the state of the keyboard.
		
        It has the values:
        0 = keyboard is disabled;
        1 = Keyboard is enabled.
        
        This is dynamic and writeable.

        @capability PowerMgmt needed to Set this attribute
        */
        EKeyboardState,


        /**
        Defines the system drive.
		
        This has a value in the range 0 to 25.
        This will be cast to a TDriveNumber, and the values represent
        the range EDriveA to EdriveZ.
        
        @see TDriveNumber

        @capability WriteDeviceData needed to Set this attribute
        */
        ESystemDrive,


        /**
        Indicates the state of the pen or digitiser.

        It has the values:
        1 = pen/digitiser is enabled;
        0 = pen/digitiser is disabled.
		
        This is dynamic and writeable.

        @capability PowerMgmt needed to Set this attribute
        */
        EPenState,


        /**
        On input: aInOut contains the mode number.
        On output: aInOut contains: 0 = display is colour;
                                    1 = display is black & white.
        
        aInOut is the 3rd parameter passed to accessor functions
        for derived attributes.
        */
        EDisplayIsMono,


        /**
        On input: aInOut contains the mode number;
        On output, aInOut contains: 0 = display is not palettised;
                                    1 = display is palettised.
        
        aInOut is the 3rd parameter passed to accessor functions
        for derived attributes.
        */
        EDisplayIsPalettized,


        /**
        The display bits per pixel.
		
        On input, aInOut contains the mode number.
        On output, aInOut contains the bits per pixel for that mode.

        aInOut is the 3rd parameter passed to accessor functions
        for derived attributes.
        
        It is read only data.
        */
        EDisplayBitsPerPixel,


        /**
        The number of display modes available.
        */
        EDisplayNumModes,


        /**
        The address of the display memory.
        */
        EDisplayMemoryAddress,


        /**
        The offset, in bytes, to the pixel area of the screen from the start of screen memory.
		
        This is used to account for the fact that the palette is sometimes at
        the beginning of the display memory.
		
        On input, aInOut contains the mode number.
        On output, aInOut contains the offset to the first pixel for that mode.

        aInOut is the 3rd parameter passed to accessor functions
        for derived attributes.
        */
        EDisplayOffsetToFirstPixel,


        /**
        The separation, in bytes, of successive lines of display in memory.
        
        On input, aInOut contains the mode number.
        On output, aInOut contains the display offset between lines.

        aInOut is the 3rd parameter passed to accessor functions
        for derived attributes.
        */
        EDisplayOffsetBetweenLines,


        /**
        @capability MultimediaDD needed to Set this attribute
        */
        EDisplayPaletteEntry,


        /**
        It has the values:
        1 = order of pixels in display is RGB;
        0 = otherwise.
        */
        EDisplayIsPixelOrderRGB,


        /**
        It has the values:
        1 = pixel order is landscape;
        0 = pixel order is portrait.
        */
        EDisplayIsPixelOrderLandscape,


        /**
        This indicates or sets the current display mode where
        EDisplayNumModes-1 is the maximum value for the display mode.
        The properties of a particular display mode are entirely defined by
        the base port software associated with the hardware upon which the OS
        is running.

        @capability MultimediaDD needed to Set this attribute
        */
        EDisplayMode,


        /**
        If the target hardware upon which Symbian OS is running has switches
        which can be read by the base port software, this interface allows
        the current status of those switches to be read. 
        */
        ESwitches,


        /**
        The port number of the debug port.
        */
        EDebugPort,


        /**
        The language code of the Locale which was loaded at device boot time.

        This is dynamic and writeable.

        @see ELanguageIndex

        @capability WriteSystemData needed to Set this attribute
        */
        ELocaleLoaded,


        /**
        The drive number to use for storage of Clipboard data.
        0 = Drive A, 1 = Drive B, etc...
        */
        EClipboardDrive,

        /**
        Custom restart
        @capability PowerMgmt
        */
        ECustomRestart,

        /**
        Custom restart reason
        */
        ECustomRestartReason,

        /**
        The number of screens.
        */
        EDisplayNumberOfScreens,

        /**
        The time between nanokernel ticks, in microseconds.
        */
        ENanoTickPeriod,

        /**
        The frequecncy of the fast counter.
        */
        EFastCounterFrequency,

        /**
        Indicates the whether the fast counter counts up or down.
        */
        EFastCounterCountsUp,

        /**
        @prototype

        Indicates whether a 3 dimensional pointing device is available for input.

        It has the values:
        0 = a 3D pointer is not available for input;
        1 = a 3D pointer is present.
        */
        EPointer3D,

        /**
        @prototype

        The 3D pointing device detection range, in units of distance above the screen.

        This is dynamic and writeable.
        */
        EPointer3DZ,

        /**
        @prototype

        Indicates whether a 3 dimensional pointing device supports Theta polar angle reading.

        It has the values:
        0 = a 3D pointer does not support Theta polar angle reading;
        1 = a 3D pointer supports Theta polar angle reading.
        */
        EPointer3DThetaSupported,

        /**
        @prototype

        Indicates whether a 3 dimensional pointing device supports Phi polar angle reading.

        It has the values:
        0 = a 3D pointer does not support Phi polar angle reading;
        1 = a 3D pointer supports Phi polar angle reading.
        */
        EPointer3DPhiSupported,

        /**
        @prototype

        Indicates whether a 3 dimensional pointing device supports rotation angle along its main axis reading.

        It has the values:
        0 = a 3D pointer does not support alpha (rotation) reading;
        1 = a 3D pointer supports alpha (rotation) reading.
        */
        EPointer3DRotationSupported,

        /**
        @prototype

        Indicates whether a 3 dimensional pointing device supports readings of pressure applied on screen.

        It has the values:
        0 = a 3D pointer does not support pressure reading;
        1 = a 3D pointer supports pressure reading.
        */
        EPointer3DPressureSupported,

        /**
        Indicates whether hardware floating point is available, and what type.
		
        If no hardware floating point is available, reading this attribute will return KErrNotSupported.
        If hardware floating point is available, reading this attribute will return KErrNone and the type
        available. These types are specified in TFloatingPointType.
        */
        FPHardware,

        /**
        The number of HAL attributes.
		
        It is simply defined by its position in the enumeration.
        */
        NumHalAttributes

    };


}

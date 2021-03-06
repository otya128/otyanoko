﻿using System;
using System.Text;
using System.Runtime.InteropServices;

// HISTORY
///
// Version 0.1: wherin PInvoke.net snippets are copied in, and missing functions coded and submitted to PInvoke.net
// 11/21/2012 Correcting signature for GetConsoleScreenBufferInfoEx and cleaned up CONSOLE_SCREEN_BUFFER_INFO_EX.ColorTable

namespace ConsoleClassLibrary
{
    /// <summary>
    /// 
    /// --- begin MSDN ---
    /// http://msdn.microsoft.com/en-us/library/ms682073(VS.85).aspx
    /// Console Functions
    /// The following functions are used to access a console.
    /// --- end MSDN ---
    /// 
    /// </summary>
    class ConsoleFunctions
    {
    // http://pinvoke.net/default.aspx/kernel32/AddConsoleAlias.html
    [DllImport("kernel32", SetLastError = true)]
    static extern public bool AddConsoleAlias(
        string Source,
        string Target,
        string ExeName
        );

    // http://pinvoke.net/default.aspx/kernel32/AllocConsole.html
    [DllImport("kernel32", SetLastError = true)]
    static extern public bool AllocConsole();

    // http://pinvoke.net/default.aspx/kernel32/AttachConsole.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool AttachConsole(
        uint dwProcessId
        );

    // http://pinvoke.net/default.aspx/kernel32/CreateConsoleScreenBuffer.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public IntPtr CreateConsoleScreenBuffer(
        uint dwDesiredAccess,
        uint dwShareMode, 
        IntPtr lpSecurityAttributes, 
        uint dwFlags,
        IntPtr lpScreenBufferData
        );

    // http://pinvoke.net/default.aspx/kernel32/FillConsoleOutputAttribute.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool FillConsoleOutputAttribute(
        IntPtr hConsoleOutput,
        ushort wAttribute, 
        uint nLength, 
        COORD dwWriteCoord, 
        out uint lpNumberOfAttrsWritten
        );

    // http://pinvoke.net/default.aspx/kernel32/FillConsoleOutputCharacter.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool FillConsoleOutputCharacter(
        IntPtr hConsoleOutput,
        char cCharacter,
        uint nLength,
        COORD dwWriteCoord,
        out uint lpNumberOfCharsWritten
        );

    // http://pinvoke.net/default.aspx/kernel32/FlushConsoleInputBuffer.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool FlushConsoleInputBuffer(
        IntPtr hConsoleInput
        );

    // http://pinvoke.net/default.aspx/kernel32/FreeConsole.html
    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    static extern public bool FreeConsole();

    // http://pinvoke.net/default.aspx/kernel32/GenerateConsoleCtrlEvent.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool GenerateConsoleCtrlEvent(
        uint dwCtrlEvent,
        uint dwProcessGroupId
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleAlias.html
    [DllImport("kernel32", SetLastError = true)]
    static extern public bool GetConsoleAlias(
        string Source,
        out StringBuilder TargetBuffer,
        uint TargetBufferLength,
        string ExeName
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliases.html
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint GetConsoleAliases(
        StringBuilder[] lpTargetBuffer,
        uint targetBufferLength,
        string lpExeName
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliasesLength.html
    [DllImport("kernel32", SetLastError = true)]
    static extern public uint GetConsoleAliasesLength(
        string ExeName
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliasExes.html
    [DllImport("kernel32", SetLastError = true)]
    static extern public uint GetConsoleAliasExes(
        out StringBuilder ExeNameBuffer,
        uint ExeNameBufferLength
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliasExesLength.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public uint GetConsoleAliasExesLength();

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleCP.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public uint GetConsoleCP();

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleCursorInfo.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool GetConsoleCursorInfo(
        IntPtr hConsoleOutput,
        out CONSOLE_CURSOR_INFO lpConsoleCursorInfo
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleDisplayMode.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool GetConsoleDisplayMode(
        out uint ModeFlags
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleFontSize.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public COORD GetConsoleFontSize(
        IntPtr hConsoleOutput,
        Int32 nFont
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleHistoryInfo.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool GetConsoleHistoryInfo(
        out CONSOLE_HISTORY_INFO ConsoleHistoryInfo
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleMode.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool GetConsoleMode(
        IntPtr hConsoleHandle,
        out uint lpMode
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleOriginalTitle.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public uint GetConsoleOriginalTitle(
        out StringBuilder ConsoleTitle,
        uint Size
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleOutputCP.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public uint GetConsoleOutputCP();

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleProcessList.html
    // TODO: Test - what's an out uint[] during interop? This probably isn't quite right, but provides a starting point:
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public uint GetConsoleProcessList(
        out uint[] ProcessList,
        uint ProcessCount
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleScreenBufferInfo.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool GetConsoleScreenBufferInfo(
        IntPtr hConsoleOutput,
        out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleScreenBufferInfoEx.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool GetConsoleScreenBufferInfoEx(
        IntPtr hConsoleOutput,
        ref CONSOLE_SCREEN_BUFFER_INFO_EX ConsoleScreenBufferInfo
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleSelectionInfo.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool GetConsoleSelectionInfo(
        out CONSOLE_SELECTION_INFO ConsoleSelectionInfo
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleTitle.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public uint GetConsoleTitle(
        [Out] StringBuilder lpConsoleTitle,
        uint nSize
        );

    // http://pinvoke.net/default.aspx/kernel32/GetConsoleWindow.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public IntPtr GetConsoleWindow();

    // http://pinvoke.net/default.aspx/kernel32/GetCurrentConsoleFont.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool GetCurrentConsoleFont(
        IntPtr hConsoleOutput,
        bool bMaximumWindow,
        out CONSOLE_FONT_INFO lpConsoleCurrentFont
        );

    // http://pinvoke.net/default.aspx/kernel32/GetCurrentConsoleFontEx.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool GetCurrentConsoleFontEx(
        IntPtr ConsoleOutput,
        bool MaximumWindow,
        out CONSOLE_FONT_INFO_EX ConsoleCurrentFont
        );

    // http://pinvoke.net/default.aspx/kernel32/GetLargestConsoleWindowSize.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public COORD GetLargestConsoleWindowSize(
        IntPtr hConsoleOutput
        );

    // http://pinvoke.net/default.aspx/kernel32/GetNumberOfConsoleInputEvents.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool GetNumberOfConsoleInputEvents(
        IntPtr hConsoleInput,
        out uint lpcNumberOfEvents
        );

    // http://pinvoke.net/default.aspx/kernel32/GetNumberOfConsoleMouseButtons.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static unsafe extern public bool GetNumberOfConsoleMouseButtons(
        ref MOUSE_EVENT_RECORD lpNumberOfMouseButtons
        );

    // http://pinvoke.net/default.aspx/kernel32/GetStdHandle.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public IntPtr GetStdHandle(
        int nStdHandle
        );

    // http://pinvoke.net/default.aspx/kernel32/HandlerRoutine.html
    // Delegate type to be used as the Handler Routine for SCCH
    delegate bool ConsoleCtrlDelegate(CtrlTypes CtrlType);

    // http://pinvoke.net/default.aspx/kernel32/PeekConsoleInput.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool PeekConsoleInput(
        IntPtr hConsoleInput,
        [Out] INPUT_RECORD[] lpBuffer,
        uint nLength,
        out uint lpNumberOfEventsRead
        );

    // http://pinvoke.net/default.aspx/kernel32/ReadConsole.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool ReadConsole(
        IntPtr hConsoleInput,
        [Out] StringBuilder lpBuffer,
        uint nNumberOfCharsToRead,
        out uint lpNumberOfCharsRead,
        IntPtr lpReserved
        );

    // http://pinvoke.net/default.aspx/kernel32/ReadConsoleInput.html
    [DllImport("kernel32.dll", EntryPoint = "ReadConsoleInputW", CharSet = CharSet.Unicode)]
    static extern public bool ReadConsoleInput(
        IntPtr hConsoleInput,
        [Out] INPUT_RECORD[,] lpBuffer,
        uint nLength,
        out uint lpNumberOfEventsRead
        );

    // http://pinvoke.net/default.aspx/kernel32/ReadConsoleOutput.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool ReadConsoleOutput(
        IntPtr hConsoleOutput,
        [Out] CHAR_INFO[] lpBuffer,
        COORD dwBufferSize,
        COORD dwBufferCoord,
        ref SMALL_RECT lpReadRegion
        );

    // http://pinvoke.net/default.aspx/kernel32/ReadConsoleOutputAttribute.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool ReadConsoleOutputAttribute(
        IntPtr hConsoleOutput,
        [Out] ushort[] lpAttribute,
        uint nLength,
        COORD dwReadCoord,
        out uint lpNumberOfAttrsRead
        );

    // http://pinvoke.net/default.aspx/kernel32/ReadConsoleOutputCharacter.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool ReadConsoleOutputCharacter(
        IntPtr hConsoleOutput,
        [Out] StringBuilder lpCharacter,
        uint nLength,
        COORD dwReadCoord,
        out uint lpNumberOfCharsRead
        );

    // http://pinvoke.net/default.aspx/kernel32/ScrollConsoleScreenBuffer.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool ScrollConsoleScreenBuffer(
        IntPtr hConsoleOutput,
       [In] ref SMALL_RECT lpScrollRectangle,
        IntPtr lpClipRectangle,
       COORD dwDestinationOrigin,
        [In] ref CHAR_INFO lpFill
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleActiveScreenBuffer.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleActiveScreenBuffer(
        IntPtr hConsoleOutput
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleCP.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleCP(
        uint wCodePageID
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleCtrlHandler.html
    /*[DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleCtrlHandler(
        ConsoleCtrlDelegate HandlerRoutine,
        bool Add
        );
        */
    // http://pinvoke.net/default.aspx/kernel32/SetConsoleCursorInfo.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleCursorInfo(
        IntPtr hConsoleOutput,
        [In] ref CONSOLE_CURSOR_INFO lpConsoleCursorInfo
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleCursorPosition.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleCursorPosition(
        IntPtr hConsoleOutput,
       COORD dwCursorPosition
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleDisplayMode.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleDisplayMode(
        IntPtr ConsoleOutput,
        uint Flags,
        out COORD NewScreenBufferDimensions
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleHistoryInfo.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleHistoryInfo(
        CONSOLE_HISTORY_INFO ConsoleHistoryInfo
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleMode.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleMode(
        IntPtr hConsoleHandle, 
        uint dwMode
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleOutputCP.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleOutputCP(
        uint wCodePageID
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleScreenBufferInfoEx.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleScreenBufferInfoEx(
        IntPtr ConsoleOutput,
        ref CONSOLE_SCREEN_BUFFER_INFO_EX ConsoleScreenBufferInfoEx
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleScreenBufferSize.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleScreenBufferSize(
        IntPtr hConsoleOutput,
        COORD dwSize
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleTextAttribute.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleTextAttribute(
        IntPtr hConsoleOutput,
       ushort wAttributes
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleTitle.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleTitle(
        string lpConsoleTitle
        );

    // http://pinvoke.net/default.aspx/kernel32/SetConsoleWindowInfo.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetConsoleWindowInfo(
        IntPtr hConsoleOutput,
        bool bAbsolute,
        [In] ref SMALL_RECT lpConsoleWindow
        );

    // http://pinvoke.net/default.aspx/kernel32/SetCurrentConsoleFontEx.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetCurrentConsoleFontEx(
        IntPtr ConsoleOutput, 
        bool MaximumWindow,
        ref CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx
        );

    // http://pinvoke.net/default.aspx/kernel32/SetStdHandle.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool SetStdHandle(
        uint nStdHandle,
        IntPtr hHandle
        );

    // http://pinvoke.net/default.aspx/kernel32/WriteConsole.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool WriteConsole(
        IntPtr hConsoleOutput,
        string lpBuffer,
        uint nNumberOfCharsToWrite,
        out uint lpNumberOfCharsWritten,
        IntPtr lpReserved
        );

    // http://pinvoke.net/default.aspx/kernel32/WriteConsoleInput.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool WriteConsoleInput(
        IntPtr hConsoleInput,
        INPUT_RECORD[] lpBuffer,
        uint nLength,
        out uint lpNumberOfEventsWritten
        );

    // http://pinvoke.net/default.aspx/kernel32/WriteConsoleOutput.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern unsafe public bool WriteConsoleOutput(
        IntPtr hConsoleOutput,
        /*CHAR_INFO[]*/ IntPtr lpBuffer,
        COORD dwBufferSize,
        COORD dwBufferCoord,
        ref SMALL_RECT lpWriteRegion
        );

    // http://pinvoke.net/default.aspx/kernel32/WriteConsoleOutputAttribute.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool WriteConsoleOutputAttribute(
        IntPtr hConsoleOutput,
        ushort[] lpAttribute,
        uint nLength,
        COORD dwWriteCoord,
        out uint lpNumberOfAttrsWritten
        );

    // http://pinvoke.net/default.aspx/kernel32/WriteConsoleOutputCharacter.html
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern public bool WriteConsoleOutputCharacter(
        IntPtr hConsoleOutput,
        string lpCharacter, 
        uint nLength, 
        COORD dwWriteCoord,
        out uint lpNumberOfCharsWritten
        );

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {

        public short X;
        public short Y;

    }

    public struct SMALL_RECT
    {

        public short Left;
        public short Top;
        public short Right;
        public short Bottom;

    }

    public struct CONSOLE_SCREEN_BUFFER_INFO
    {

        public COORD dwSize;
        public COORD dwCursorPosition;
        public ushort wAttributes;
        public SMALL_RECT srWindow;
        public COORD dwMaximumWindowSize;

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_SCREEN_BUFFER_INFO_EX
    {
        public uint cbSize;
        public COORD dwSize;
        public COORD dwCursorPosition;
        public ushort wAttributes;
        public SMALL_RECT srWindow;
        public COORD dwMaximumWindowSize;

        public ushort wPopupAttributes;
        public bool bFullscreenSupported;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public COLORREF[] ColorTable;

        public static CONSOLE_SCREEN_BUFFER_INFO_EX Create()
        {
        return new CONSOLE_SCREEN_BUFFER_INFO_EX { cbSize = 96 };
        }
    }

    //[StructLayout(LayoutKind.Sequential)]
    //struct COLORREF
    //{
    //    public byte R;
    //    public byte G;
    //    public byte B;
    //}

    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF
    {
        public uint ColorDWORD;

        public COLORREF(System.Drawing.Color color)
        {
        ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }

        public System.Drawing.Color GetColor()
        {
        return System.Drawing.Color.FromArgb((int)(0x000000FFU & ColorDWORD),
           (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
        }

        public void SetColor(System.Drawing.Color color)
        {
        ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_FONT_INFO
    {
        public int nFont;
        public COORD dwFontSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CONSOLE_FONT_INFO_EX 
    {
        public uint cbSize;
        public uint nFont;
        public COORD dwFontSize;
        public ushort FontFamily;
        public ushort FontWeight;
       // [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)LF_FACESIZE)]
       // public char[] FaceName;
        public fixed char FaceName[(int)LF_FACESIZE];

        public const uint LF_FACESIZE = 32;
    } 

    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT_RECORD
    {
        [FieldOffset(0)]
        public ushort EventType;
        [FieldOffset(4)]
        public KEY_EVENT_RECORD KeyEvent;
        [FieldOffset(4)]
        public MOUSE_EVENT_RECORD MouseEvent;
        [FieldOffset(4)]
        public WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;
        [FieldOffset(4)]
        public MENU_EVENT_RECORD MenuEvent;
        [FieldOffset(4)]
        public FOCUS_EVENT_RECORD FocusEvent;
    };

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public struct KEY_EVENT_RECORD
    {
        [FieldOffset(0), MarshalAs(UnmanagedType.Bool)]
        public bool bKeyDown;
        [FieldOffset(4), MarshalAs(UnmanagedType.U2)]
        public ushort wRepeatCount;
        [FieldOffset(6), MarshalAs(UnmanagedType.U2)]
        //public VirtualKeys wVirtualKeyCode;
        public ushort wVirtualKeyCode;
        [FieldOffset(8), MarshalAs(UnmanagedType.U2)]
        public ushort wVirtualScanCode;
        [FieldOffset(10)]
        public char UnicodeChar;
        [FieldOffset(12), MarshalAs(UnmanagedType.U4)]
        //public ControlKeyState dwControlKeyState;
        public uint dwControlKeyState;
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct MOUSE_EVENT_RECORD
    {
        [FieldOffset(0)]
        public COORD dwMousePosition;
        [FieldOffset(4), MarshalAs(UnmanagedType.U4)]
        public uint dwButtonState;
        [FieldOffset(8), MarshalAs(UnmanagedType.U4)]
        public uint dwControlKeyState;
        [FieldOffset(12), MarshalAs(UnmanagedType.U4)]
        public uint dwEventFlags;
    }

    /*[StructLayout(LayoutKind.Sequential)]
    public struct MOUSE_EVENT_RECORD
    {
        public COORD dwMousePosition;
        public uint dwButtonState;
        public uint dwControlKeyState;
        public uint dwEventFlags;
    }*/

    public struct WINDOW_BUFFER_SIZE_RECORD
    {
        public COORD dwSize;

        public WINDOW_BUFFER_SIZE_RECORD(short x, short y)
        {
        dwSize = new COORD();
        dwSize.X = x;
        dwSize.Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MENU_EVENT_RECORD
    {
        public uint dwCommandId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FOCUS_EVENT_RECORD
    {
        public uint bSetFocus;
    }

    //CHAR_INFO struct, which was a union in the old days
    // so we want to use LayoutKind.Explicit to mimic it as closely
    // as we can
    [StructLayout(LayoutKind.Explicit)]
    public struct CHAR_INFO
    {
        [FieldOffset(0)]
        public char UnicodeChar;
        [FieldOffset(0)]
        public byte AsciiChar;
        [FieldOffset(2)] //2 bytes seems to work properly
        public UInt16 Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_CURSOR_INFO
    {
      public uint Size;
      public bool Visible;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_HISTORY_INFO
    {
        ushort cbSize;
        ushort HistoryBufferSize;
        ushort NumberOfHistoryBuffers;
        uint dwFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_SELECTION_INFO 
    {
        public uint Flags;
        public COORD SelectionAnchor;
        public SMALL_RECT Selection;

        // Flags values:
        const uint CONSOLE_MOUSE_DOWN = 0x0008; // Mouse is down
        const uint CONSOLE_MOUSE_SELECTION = 0x0004; //Selecting with the mouse
        const uint CONSOLE_NO_SELECTION = 0x0000; //No selection
        const uint CONSOLE_SELECTION_IN_PROGRESS = 0x0001; //Selection has begun
        const uint CONSOLE_SELECTION_NOT_EMPTY = 0x0002; //Selection rectangle is not empty
    }

    // Enumerated type for the control messages sent to the handler routine
    enum CtrlTypes : uint
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT,
        CTRL_CLOSE_EVENT,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT
    }
    }
}
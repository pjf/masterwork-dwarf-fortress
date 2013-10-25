;Urist Vespasian, Arena Editor, v1.0
;by ManaUser

#SingleInstance Ignore
#NoTrayIcon
#NoEnv

TxtFile = arena.txt
BmpFile = arena.bmp

SetFormat IntegerFast, HEX

Header =
( LTrim Join
  424d46e202000000000036040000
  28000000900000001905000001000800
  0000000010de0200130b0000130b00000001000000010000
) ;Identifies this as a bitmap, sets dimentions, etc.

MapChars = .PRF+WwLl
VoidChar = #

Palette =
( LTrim Join
  000000 #
  404040 .
  808080 P
  C0C0C0 R
  0055AA F
  FFFF00 +
  FF0000 W
  FF8080 w
  0000FF L
  8080FF l
) ;Palette Data
While (StrLen(Palette) < 2040)
   Palette .= "00000000" ;Pad with black
Palette .= "FFFFFF00" ;Make last color white

VarSetCapacity(Divider, 288, Asc("F")) ;bunch of Fs, 144 white pixels

If InStr(FileExist(A_WorkingDir . "\data\init"), "D")
   SetWorkingDir %A_WorkingDir%\data\init

Gui Add, Button, x8 y8 w104 h24 gToBmp, TXT->&BMP
Gui Add, Button, x120 y8 w104 h24 gToTxt, BMP->&TXT
Gui Add, CheckBox, x232 y8 w80 h24 vAutoOpen, &Auto Open
Gui Add, Progress, x8 y40 w304 h24 Range0-1305 vMyProgress
Gui +LastFound
Gui Show,, Urist Vespasian 
Return

;*************************************************************
ToBmp:
FileSelectFile TxtFile, 1, %A_WorkingDir%, Source Arena Text File, Text Files (*.txt)
If ErrorLEvel
   Return
FileSelectFile BmpFile, 16, %A_WorkingDir%, Destination Arena Bitmap, Bitmap Filess (*.bmp)
If ErrorLEvel
   Return
While (SubStr(BmpFile, -3) != ".bmp")
{
   FileSelectFile BmpFile, 16, %BmpFile%.bmp, Destination Arena Bitmap, Bitmap Filess (*.bmp)
   If ErrorLEvel
      Return
}

WinSet Disable
Gui Submit, NoHide
FileDelete %BmpFile%

BinWrite(BmpFile, Header, 0, 0)
BinWrite(BmpFile, Palette, 0, 54)

Row := 0
Screen := 0
GuiControl,, MyProgress, 0
Loop Read, %TxtFile%
{
   If (Row = 0)
      BinWrite(BmpFile, Divider, 144, 188854 - (Row + Screen * 145) * 144)
   else
   {
      OutLine =
      Loop Parse, A_LoopReadLine  
         OutLine .= HexFix(InStr(MapChars, A_LoopField, 1))
      BinWrite(BmpFile, OutLine, 144, 188854 - (Row + Screen * 145) * 144)
   }
   Row++
   GuiControl,, MyProgress, +1
   If (Row == 145)
   {
      Row := 0
      Screen++
      If (Screen == 9)
         Break
   }
}
If (AutoOpen)
   Run %BmpFile%
Else
   MsgBox Done!
WinSet Enable
Return

;*************************************************************
ToTxt:
FileSelectFile BmpFile, 1, %A_WorkingDir%, Source Arena Bitmap, Bitmap Filess (*.bmp)
If ErrorLEvel
   Return
FileSelectFile TxtFile, 16, %A_WorkingDir%, Destination Arena Text File, Text Files (*.txt)
If ErrorLEvel
   Return
While (SubStr(TxtFile, -3) != ".txt")
{
   FileSelectFile TxtFile, 16, %TxtFile%.txt, Destination Arena Text File, Text Files (*.txt)
   If ErrorLEvel
      Return
}

WinSet Disable
Gui Submit, NoHide
FileDelete %TxtFile%

Row := 0
Screen := 0
GuiControl,, MyProgress, 0
Loop
{
   If (Row = 0)
   {
      SetFormat IntegerFast, DEC
      OutLine := "Z=" . Screen - 4
      SetFormat IntegerFast, HEX
      FileAppend %OutLine%`n, %TxtFile%
   }
   else
   {
      BinRead(BmpFile, InLine, 144, 188854 - (Row + Screen * 145) * 144)
      OutLine =
      Loop 144
      {
         GetChar := "0x" . SubStr(InLine, A_index * 2 - 1, 2)
         If (GetChar = 0 OR GetChar > StrLen(MapChars))
            GetChar := VoidChar
         else
            GetChar := SubStr(MapChars, GetChar, 1)
         OutLine .= GetChar
      }
      FileAppend %OutLine%`n, %TxtFile%
   }
   Row++
   GuiControl,, MyProgress, +1
   If (Row == 145)
   {
      Row := 0
      Screen++
      If (Screen == 9)
         Break
   }
}
If (AutoOpen)
   Run %TxtFile%
Else
   MsgBox Done!
WinSet Enable
Return
;*************************************************************

GuiClose:
ExitApp

;*************************************************************

HexFix(FixMe)
{
   StringReplace FixMe, FixMe, x,, A
   StringRight FixMe, FixMe, 2
   Return FixMe
}

;*************************************************************

;Courtesy of Laszlo
/* ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;; BinWrite ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
|  - Open binary file
|  - (Over)Write n bytes (n = 0: all)
|  - From offset (offset < 0: counted from end)
|  - Close file
|  data -> file[offset + 0..n-1], rest of file unchanged
|  Return #bytes actually written
*/ ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

BinWrite(file, data, n=0, offset=0)
{
   ; Open file for WRITE (0x40..), OPEN_ALWAYS (4): creates only if it does not exists
   h := DllCall("CreateFile","str",file,"Uint",0x40000000,"Uint",0,"UInt",0,"UInt",4,"Uint",0,"UInt",0)
   IfEqual h,-1, SetEnv, ErrorLevel, -1
   IfNotEqual ErrorLevel,0,Return,0 ; couldn't create the file

   m = 0                            ; seek to offset
   IfLess offset,0, SetEnv,m,2
   r := DllCall("SetFilePointerEx","Uint",h,"Int64",offset,"UInt *",p,"Int",m)
   IfEqual r,0, SetEnv, ErrorLevel, -3
   IfNotEqual ErrorLevel,0, {
      t = %ErrorLevel%              ; save ErrorLevel to be returned
      DllCall("CloseHandle", "Uint", h)
      ErrorLevel = %t%              ; return seek error
      Return 0
   }

   TotalWritten = 0
   m := Ceil(StrLen(data)/2)
   If (n <= 0 or n > m)
       n := m
   Loop %n%
   {
      StringLeft c, data, 2         ; extract next byte
      StringTrimLeft data, data, 2  ; remove  used byte
      c = 0x%c%                     ; make it number
      result := DllCall("WriteFile","UInt",h,"UChar *",c,"UInt",1,"UInt *",Written,"UInt",0)
      TotalWritten += Written       ; count written
      if (!result or Written < 1 or ErrorLevel)
         break
   }

   IfNotEqual ErrorLevel,0, SetEnv,t,%ErrorLevel%

   h := DllCall("CloseHandle", "Uint", h)
   IfEqual h,-1, SetEnv, ErrorLevel, -2
   IfNotEqual t,,SetEnv, ErrorLevel, %t%

   Return TotalWritten
}

;Courtesy of Laszlo
/* ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;; BinRead ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
|  - Open binary file
|  - Read n bytes (n = 0: all)
|  - From offset (offset < 0: counted from end)
|  - Close file
|  data (replaced) <- file[offset + 0..n-1]
|  Return #bytes actually read
*/ ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

BinRead(file, ByRef data, n=0, offset=0)
{
   h := DllCall("CreateFile","Str",file,"Uint",0x80000000,"Uint",3,"UInt",0,"UInt",3,"Uint",0,"UInt",0)
   IfEqual h,-1, SetEnv, ErrorLevel, -1
   IfNotEqual ErrorLevel,0,Return,0 ; couldn't open the file

   m = 0                            ; seek to offset
   IfLess offset,0, SetEnv,m,2
   r := DllCall("SetFilePointerEx","Uint",h,"Int64",offset,"UInt *",p,"Int",m)
   IfEqual r,0, SetEnv, ErrorLevel, -3
   IfNotEqual ErrorLevel,0, {
      t = %ErrorLevel%              ; save ErrorLevel to be returned
      DllCall("CloseHandle", "Uint", h)
      ErrorLevel = %t%              ; return seek error
      Return 0
   }

   TotalRead = 0
   data =
   IfEqual n,0, SetEnv n,0xffffffff ; almost infinite

   format = %A_FormatInteger%       ; save original integer format
   SetFormat Integer, Hex           ; for converting bytes to hex

   Loop %n%
   {
      result := DllCall("ReadFile","UInt",h,"UChar *",c,"UInt",1,"UInt *",Read,"UInt",0)
      if (!result or Read < 1 or ErrorLevel)
         break
      TotalRead += Read             ; count read
      c += 0                        ; convert to hex
      StringTrimLeft c, c, 2        ; remove 0x
      c = 0%c%                      ; pad left with 0
      StringRight c, c, 2           ; always 2 digits
      data = %data%%c%              ; append 2 hex digits
   }

   IfNotEqual ErrorLevel,0, SetEnv,t,%ErrorLevel%

   h := DllCall("CloseHandle", "Uint", h)
   IfEqual h,-1, SetEnv, ErrorLevel, -2
   IfNotEqual t,,SetEnv, ErrorLevel, %t%

   SetFormat Integer, %format%      ; restore original format
   Totalread += 0                   ; convert to original format
   Return TotalRead
}
Add-Type @"
using System;
using System.Runtime.InteropServices;
public class PInvoke {
    [DllImport("user32.dll")] public static extern IntPtr GetDC(IntPtr hwnd);
    [DllImport("gdi32.dll")] public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
}
"@

$number = 10
$i = 1
do{
     sleep 3
     write-host "loop number"
    $hdc = [PInvoke]::GetDC([IntPtr]::Zero)
    [PInvoke]::GetDeviceCaps($hdc, 118) # width
    [PInvoke]::GetDeviceCaps($hdc, 117) # height

    $i++
 } while ($i -le $number)

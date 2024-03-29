// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.</auto-generated>
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class HatchBrush : Brush
    {
        public HatchBrush()
        {
            SetNativePointer(NativeApi.HatchBrush_Create_());
        }
        
        public HatchBrush(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public void Initialize(BrushHatchStyle style, Alternet.Drawing.Color color)
        {
            CheckDisposed();
            NativeApi.HatchBrush_Initialize_(NativePointer, style, color);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr HatchBrush_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void HatchBrush_Initialize_(IntPtr obj, BrushHatchStyle style, NativeApiTypes.Color color);
            
        }
    }
}

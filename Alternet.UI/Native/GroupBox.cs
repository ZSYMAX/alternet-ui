// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.</auto-generated>
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class GroupBox : Control
    {
        public GroupBox()
        {
            SetNativePointer(NativeApi.GroupBox_Create_());
        }
        
        public GroupBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string? Title
        {
            get
            {
                CheckDisposed();
                return NativeApi.GroupBox_GetTitle_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.GroupBox_SetTitle_(NativePointer, value);
            }
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GroupBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string? GroupBox_GetTitle_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void GroupBox_SetTitle_(IntPtr obj, string? value);
            
        }
    }
}

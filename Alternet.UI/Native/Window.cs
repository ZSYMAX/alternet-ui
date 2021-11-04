// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.</auto-generated>
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Window : Control
    {
        public Window()
        {
            SetNativePointer(NativeApi.Window_Create_());
            SetEventCallback();
        }
        
        public Window(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string Title
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetTitle_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetTitle_(NativePointer, value);
            }
        }
        
        public WindowStartPosition WindowStartPosition
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetWindowStartPosition_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetWindowStartPosition_(NativePointer, value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.WindowEventCallbackType((obj, e, parameter) =>
                {
                    var w = NativeObject.GetFromNativePointer<Window>(obj, p => new Window(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.Window_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.WindowEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.WindowEvent.Closing:
                {
                    var cea = new CancelEventArgs();
                    Closing?.Invoke(this, cea);
                    return cea.Cancel ? new IntPtr(1) : IntPtr.Zero;
                }
                case NativeApi.WindowEvent.SizeChanged:
                SizeChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected WindowEvent value: " + e);
            }
        }
        
        public event EventHandler<CancelEventArgs>? Closing;
        public event EventHandler? SizeChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr WindowEventCallbackType(IntPtr obj, WindowEvent e, IntPtr param);
            
            public enum WindowEvent
            {
                Closing,
                SizeChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetEventCallback_(WindowEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Window_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string Window_GetTitle_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetTitle_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern WindowStartPosition Window_GetWindowStartPosition_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetWindowStartPosition_(IntPtr obj, WindowStartPosition value);
            
        }
    }
}
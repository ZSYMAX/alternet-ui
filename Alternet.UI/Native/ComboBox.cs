// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.</auto-generated>
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class ComboBox : Control
    {
        public ComboBox()
        {
            SetNativePointer(NativeApi.ComboBox_Create_());
            SetEventCallback();
        }
        
        public ComboBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int ItemsCount
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetItemsCount_(NativePointer);
            }
            
        }
        
        public bool IsEditable
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetIsEditable_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetIsEditable_(NativePointer, value);
            }
        }
        
        public int SelectedIndex
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetSelectedIndex_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetSelectedIndex_(NativePointer, value);
            }
        }
        
        public string Text
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetText_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetText_(NativePointer, value);
            }
        }
        
        public void InsertItem(int index, string value)
        {
            CheckDisposed();
            NativeApi.ComboBox_InsertItem_(NativePointer, index, value);
        }
        
        public void RemoveItemAt(int index)
        {
            CheckDisposed();
            NativeApi.ComboBox_RemoveItemAt_(NativePointer, index);
        }
        
        public void ClearItems()
        {
            CheckDisposed();
            NativeApi.ComboBox_ClearItems_(NativePointer);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ComboBoxEventCallbackType((obj, e, parameter) =>
                {
                    var w = NativeObject.GetFromNativePointer<ComboBox>(obj, p => new ComboBox(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.ComboBox_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.ComboBoxEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.ComboBoxEvent.SelectedItemChanged:
                SelectedItemChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                case NativeApi.ComboBoxEvent.TextChanged:
                TextChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected ComboBoxEvent value: " + e);
            }
        }
        
        public event EventHandler? SelectedItemChanged;
        public event EventHandler? TextChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ComboBoxEventCallbackType(IntPtr obj, ComboBoxEvent e, IntPtr param);
            
            public enum ComboBoxEvent
            {
                SelectedItemChanged,
                TextChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetEventCallback_(ComboBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ComboBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetItemsCount_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ComboBox_GetIsEditable_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetIsEditable_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetSelectedIndex_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetSelectedIndex_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string ComboBox_GetText_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetText_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_InsertItem_(IntPtr obj, int index, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_RemoveItemAt_(IntPtr obj, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_ClearItems_(IntPtr obj);
            
        }
    }
}

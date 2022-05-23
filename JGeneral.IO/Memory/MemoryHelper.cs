using System;
using System.Runtime.InteropServices;

namespace JGeneral.IO.Memory
{
    public static class MemoryHelper
    {
        public static unsafe T* GetPointer<T>(this ref T unmanagedVariable) where T : unmanaged
        {
            fixed (T* ptr = &unmanagedVariable)
            {
                return ptr;
            }
        }

        public static GCHandle GetHandle<T>(this T managedVariable) where T : class
        {
            var handle = GCHandle.Alloc(managedVariable, GCHandleType.WeakTrackResurrection);

            return handle;
        }

        public static IntPtr GetPointer(this GCHandle handle)
        {
            if (!handle.IsAllocated)
            {
                throw new AggregateException("GCHandle's value has not been allocated in the heap memory, cannot fetch it's pointer since it is non-existent!");
            }
            return GCHandle.ToIntPtr(handle);
        }
    }
}
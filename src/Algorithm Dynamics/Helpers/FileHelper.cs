using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Algorithm_Dynamics.Helpers
{
    internal static class FileHelper
    {
        internal async static Task<StorageFile> FileOpenPicker(string fileTypeFilter = "*")
        {
            // https://github.com/microsoft/WindowsAppSDK/issues/1188
            // https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-using-file-and-folder-pickers
            var filePicker = new FileOpenPicker();

            // Get the current window's HWND by passing in the Window object
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

            // Use file picker like normal!
            filePicker.FileTypeFilter.Add(fileTypeFilter);
            StorageFile file = await filePicker.PickSingleFileAsync();
            return file;
        }
    }
}

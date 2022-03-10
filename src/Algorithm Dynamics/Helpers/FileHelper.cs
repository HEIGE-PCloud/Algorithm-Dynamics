using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using WinRT.Interop;

namespace Algorithm_Dynamics.Helpers
{
    internal static class FileHelper
    {
        internal async static Task<IReadOnlyList<StorageFile>> FileOpenPicker(string fileTypeFilter = "*")
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
            IReadOnlyList<StorageFile> files = await filePicker.PickMultipleFilesAsync();
            return files;
        }

        internal async static Task<bool> FileSavePicker(string fileType, List<string> fileTypeChoices, string suggestedFileName, string content)
        {
            var savePicker = new FileSavePicker();
            IntPtr hwnd = WindowNative.GetWindowHandle(App.m_window);
            InitializeWithWindow.Initialize(savePicker, hwnd);
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add(fileType, fileTypeChoices);
            savePicker.SuggestedFileName = suggestedFileName;
            StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                await FileIO.WriteTextAsync(file, content);
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                return status == FileUpdateStatus.Complete;
            }
            return false;
        }
    }
}

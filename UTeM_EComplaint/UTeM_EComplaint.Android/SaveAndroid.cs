using System;
using Android.Content;
using Xamarin.Forms;
using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using AndroidX.Core.Content;
using AndroidX.Core.App;
using UTeM_EComplaint.Services;
using System.IO;
using Java.IO;

[assembly: Dependency(typeof(SaveAndroid))]

class SaveAndroid : ISave
{
    //Method to save document as a file in Android and view the saved document
    [Obsolete]
    public async Task SaveAndView(string fileName, String contentType, MemoryStream stream)
    {
        await Task.Delay(100);
        string exception = string.Empty;
        string root = null;

        if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions((Android.App.Activity)Forms.Context, new String[] { Manifest.Permission.WriteExternalStorage }, 1);
        }

        if (Android.OS.Environment.IsExternalStorageEmulated)
        {
            root = Android.OS.Environment.ExternalStorageDirectory.ToString();
        }
        else
            root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        Java.IO.File myDir = new Java.IO.File(root + "/E-Complaint");
        myDir.Mkdir();

        Java.IO.File file = new Java.IO.File(myDir, fileName);

        bool isExists = file.Exists();
        bool isCanRead = file.CanRead();
        if (file.Exists()) file.Delete();

        try
        {
            FileOutputStream outs = new FileOutputStream(file);
            outs.Write(stream.ToArray());

            outs.Flush();
            outs.Close();
        }
        catch (Exception)
        {
            throw;
        }
        if (file.Exists() && contentType != "application/html")
        {
            string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
            string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            Android.Net.Uri path = FileProvider.GetUriForFile(Forms.Context, Android.App.Application.Context.PackageName + ".provider", file);
            intent.SetDataAndType(path, mimeType);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
        }
    }
}

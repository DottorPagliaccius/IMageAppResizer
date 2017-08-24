using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppKit;
using CoreGraphics;
using Foundation;

namespace IMageAppResizer
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            AndroidXxxCheckBox.Activated += (sender, e) =>
            {
                if (AndroidXxxCheckBox.State == NSCellStateValue.Off)
                    return;

                IOS1xCheckBox.State = NSCellStateValue.Off;
                IOS2xCheckBox.State = NSCellStateValue.Off;
                IOS3xCheckBox.State = NSCellStateValue.Off;
            };

            AndroidXxCheckBox.Activated += (sender, e) =>
            {
                IOS3xCheckBox.State = AndroidXxCheckBox.State;
            };

            IOS3xCheckBox.Activated += (sender, e) =>
            {
                AndroidXxCheckBox.State = IOS3xCheckBox.State;
            };

            AndroidXCheckBox.Activated += (sender, e) =>
            {
                IOS2xCheckBox.State = AndroidXCheckBox.State;
            };

            IOS2xCheckBox.Activated += (sender, e) =>
            {
                AndroidXCheckBox.State = IOS2xCheckBox.State;
            };

            AndroidMCheckBox.Activated += (sender, e) =>
            {
                IOS1xCheckBox.State = AndroidMCheckBox.State;
            };

            IOS1xCheckBox.Activated += (sender, e) =>
            {
                AndroidMCheckBox.State = IOS1xCheckBox.State;
            };
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
            }
        }

        partial void ChooseButtonClick(NSObject sender)
        {
            var openDialog = NSOpenPanel.OpenPanel;
            openDialog.CanChooseFiles = false;
            openDialog.CanChooseDirectories = true;
            openDialog.AllowedFileTypes = new string[] { "jpg", "png" };
            openDialog.AllowsMultipleSelection = false;

            if (openDialog.RunModal() == 1)
            {
                var url = openDialog.Url;

                if (url != null)
                    FolderTextBox.StringValue = url.Path;
            }
        }

        partial void GenerateImages(NSObject sender)
        {
            if (!CheckDestinationFolder())
                return;

            var files = Directory.GetFiles(FolderTextBox.StringValue, "*.*", SearchOption.TopDirectoryOnly).Where((x) => x.EndsWith("png") || x.EndsWith("jpg"));

            if (!CheckFiles(files))
                return;

            var destinationFolder = GetDestinationFolder();

            if (string.IsNullOrEmpty(destinationFolder))
                return;

            var iosFormats = GetIosFormats();
            var androidFormats = GetAndroidFormats();

            if (!iosFormats.Any() && !androidFormats.Any())
                return;

            var exportFolder = $"{destinationFolder}/image_export_{DateTime.Now.ToString("yyyyMMdd")}";

            Directory.CreateDirectory(exportFolder);

            if (iosFormats.Any())
            {
                string iosFolder = $"{exportFolder}/iOS";

                Directory.CreateDirectory(iosFolder);

                GenerateIosImages(iosFolder, files, iosFormats);
            }

            if (iosFormats.Any())
            {
                string androidFolder = $"{exportFolder}/Android";

                Directory.CreateDirectory(androidFolder);


            }
        }

        private void GenerateIosImages(string iosFolder, IEnumerable<string> files, IList<double> formats)
        {
            var highestFormat = formats.Max();
            var formatDictionary = new Dictionary<double, string>(formats.Count);

            foreach (var format in formats)
            {
                var fileSuffix = string.Empty;

                if (format == 3)
                    fileSuffix = "@3x";

                if (format == 2)
                    fileSuffix = "@2x";

                formatDictionary.Add(format, fileSuffix);
            }

            foreach (var filePath in files)
            {
                var fileNameWithoutExtension = $"{Path.GetFileNameWithoutExtension(filePath)}";
                var fileNameExtension = $"{Path.GetExtension(filePath)}";

                var image = new NSImage(filePath);

                var factor1Size = new CGSize(image.Size.Width / highestFormat, image.Size.Height / highestFormat);

                foreach (var format in formatDictionary)
                {
                    var destinationFilePath = $"{iosFolder}/{fileNameWithoutExtension}{format.Value}{fileNameExtension}";

                    CreateImage(filePath, destinationFilePath, factor1Size.Width * format.Key, factor1Size.Height * format.Key);
                }
            }
        }

        private void GenerateAndroidImages(string iosFolder, IEnumerable<string> files, IList<double> formats)
        {
            var highestFormat = formats.Max();
            var formatDictionary = new Dictionary<double, string>(formats.Count);

            foreach (var format in formats)
            {
                var subDirectory = string.Empty;

                if (format == 4)
                    subDirectory = "@3x";

                if (format == 3)
                    subDirectory = "@3x";

                if (format == 2)
                    subDirectory = "@2x";

                formatDictionary.Add(format, subDirectory);
            }

            foreach (var filePath in files)
            {
                var fileNameWithoutExtension = $"{Path.GetFileNameWithoutExtension(filePath)}";
                var fileNameExtension = $"{Path.GetExtension(filePath)}";

                var image = new NSImage(filePath);

                var factor1Size = new CGSize(image.Size.Width / highestFormat, image.Size.Height / highestFormat);

                foreach (var format in formatDictionary)
                {
                    var destinationFilePath = $"{iosFolder}/{fileNameWithoutExtension}{format.Value}{fileNameExtension}";

                    CreateImage(filePath, destinationFilePath, factor1Size.Width * format.Key, factor1Size.Height * format.Key);
                }
            }
        }

        private void CreateImage(string filePath, string destinationFilePath, double width, double height)
        {
            var image = new NSImage(filePath);
            var newImage = Resize(image, width, height);

            var rep = new NSBitmapImageRep(newImage.AsTiff());
            var resizesImageData = rep.RepresentationUsingTypeProperties(Path.GetExtension(filePath) == "png" ? NSBitmapImageFileType.Png : NSBitmapImageFileType.Jpeg);

            resizesImageData.Save(destinationFilePath, true);
        }

        private NSImage Resize(NSImage image, double width, double height)
        {
            var newImage = new NSImage(new CGSize(width, height));

            newImage.LockFocus();

            image.DrawInRect(new CGRect(0, 0, width, height), new CGRect(0, 0, image.Size.Width, image.Size.Height), NSCompositingOperation.SourceOver, 1);

            newImage.UnlockFocus();

            return newImage;
        }

        private IList<double> GetIosFormats()
        {
            var formats = new List<double>();

            if (IOS3xCheckBox.State == NSCellStateValue.On)
                formats.Add(3);

            if (IOS2xCheckBox.State == NSCellStateValue.On)
                formats.Add(2);

            if (IOS2xCheckBox.State == NSCellStateValue.On)
                formats.Add(1);

            return formats;
        }

        private IList<double> GetAndroidFormats()
        {
            var formats = new List<double>();

            if (AndroidXxxCheckBox.State == NSCellStateValue.On)
                formats.Add(4);

            if (AndroidXxCheckBox.State == NSCellStateValue.On)
                formats.Add(3);

            if (AndroidXCheckBox.State == NSCellStateValue.On)
                formats.Add(2);

            if (AndroidHCheckBox.State == NSCellStateValue.On)
                formats.Add(1.5);

            if (AndroidMCheckBox.State == NSCellStateValue.On)
                formats.Add(1);

            if (AndroidLCheckBox.State == NSCellStateValue.On)
                formats.Add(0.75);

            return formats;
        }

        private string GetDestinationFolder()
        {
            var saveDialog = NSSavePanel.SavePanel;

            saveDialog.CanCreateDirectories = true;
            saveDialog.Title = "Choose destination folder";

            if (saveDialog.RunModal() == 1)
            {
                var url = saveDialog.Url;

                if (url != null)
                    return url.Path;
            }

            return string.Empty;
        }

        private bool CheckDestinationFolder()
        {
            if (string.IsNullOrEmpty(FolderTextBox.StringValue))
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Warning,
                    InformativeText = "Select a source folder",
                    MessageText = "Error",
                };

                alert.RunModal();

                return false;
            }

            return true;
        }

        private bool CheckFiles(IEnumerable<string> files)
        {
            if (!files.Any())
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Warning,
                    InformativeText = "Folder contains no valid images",
                    MessageText = "Error",
                };

                alert.RunModal();

                return false;
            }

            return true;
        }
    }
}

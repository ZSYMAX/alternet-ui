using System;
using Alternet.Drawing;
using System.IO;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes an image to be drawn on a <see cref="DrawingContext"/> or displayed in a UI control.
    /// </summary>
    public class Image : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class from the specified data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the image.</param>
        public Image(Stream stream) // todo: stream lifetime
        {
            NativeImage = new UI.Native.Image();
            using (var inputStream = new UI.Native.InputStream(stream))
                NativeImage.LoadFromStream(inputStream);
        }

        /// <summary>
        /// Gets the size of the image in device-independent units (1/96th inch per unit).
        /// </summary>
        public SizeF Size => NativeImage.Size;

        /// <summary>
        /// Gets the size of the image in pixels.
        /// </summary>
        public Size PixelSize => NativeImage.PixelSize;

        internal UI.Native.Image NativeImage { get; private set; }

        /// <summary>
        /// Releases all resources used by the <see cref="Image"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Image"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativeImage.Dispose();
                    NativeImage = null!;
                }

                isDisposed = true;
            }
        }
    }
}
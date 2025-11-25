using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VDEApp.Utils.Memory
{
    internal class ScopeGuard : IDisposable
    {
        private readonly Action _onDispose;
        private bool _isDisposed = false;

        public ScopeGuard(Action onDispose)
        {
            _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _onDispose();
                _isDisposed = true;
            }
        }
    }

    internal class MemoryOperator
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMemory(IntPtr pDst, IntPtr pSrc, int len);

        private static void CreateBitmap(out Bitmap bitmap, int width, int height, bool color)
        {
            bitmap = null;
            try
            {
                PixelFormat pixelFormat = PixelFormat.Format8bppIndexed;

                if (!color)
                {
                    pixelFormat = PixelFormat.Format8bppIndexed;
                }
                else
                {
                    pixelFormat = PixelFormat.Format24bppRgb;
                }

                bitmap = new Bitmap(width, height, pixelFormat);

                if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    ColorPalette palette = bitmap.Palette;
                    for (int i = 0; i != 256; i++)
                    {
                        ref Color reference = ref palette.Entries[i];
                        reference = Color.FromArgb(i, i, i);
                    }

                    bitmap.Palette = palette;
                }
            }
            catch (Exception ex)
            {
            }
        }



        public static Bitmap ToBitmap(IntPtr pRaw, int nLen, int nWidth, int nHeight, bool color)
        {
            if (pRaw == IntPtr.Zero)
            {
                return null;
            }

            Bitmap bitmap = null;
            try
            {
                CreateBitmap(out bitmap, nWidth, nHeight, color);
                if (bitmap == null)
                {
                    return null;
                }

                UpdateBitmap(bitmap, pRaw, nWidth, nHeight, color);
                return bitmap;

            }
            catch (Exception ex)
            {
                return bitmap;
            }
        }
        public static Bitmap ToBitmap(byte[] buffer, int nWidth, int nHeight, bool color)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return null;
            }

            Bitmap result = null;
            try
            {
                IntPtr intPtr = Marshal.AllocHGlobal(buffer.Length);

                Marshal.Copy(buffer, 0, intPtr, buffer.Length);

                result = ToBitmap(intPtr, buffer.Length, nWidth, nHeight, color);

                Marshal.FreeHGlobal(intPtr);

                return result;
            }
            catch (Exception exception)
            {
                return result;
            }
        }
        public static Bitmap ToBitmap(IntPtr pRaw, int nWidth, int nHeight)
        {
            if (pRaw == IntPtr.Zero)
            {
                return null;
            }

            Bitmap bitmap = null;
            try
            {
                bitmap = new Bitmap(nWidth, nHeight, PixelFormat.Format16bppGrayScale);

                if (bitmap == null)
                {
                    return null;
                }

                UpdateBitmap(bitmap, pRaw, nWidth, nHeight);

                return bitmap;

            }
            catch (Exception exception)
            {
                return bitmap;
            }
        }
        public static Bitmap ToBitmap(short[] buffer, int nWidth, int nHeight)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return null;
            }

            Bitmap bitmap = null;
            try
            {
                bitmap = new Bitmap(nWidth, nHeight, PixelFormat.Format16bppGrayScale);

                if (bitmap == null)
                {
                    return null;
                }

                UpdateBitmap(bitmap, buffer, nWidth, nHeight);

                return bitmap;

            }
            catch (Exception exception)
            {
                return bitmap;
            }
        }




        private static void UpdateBitmap(Bitmap bitmap, IntPtr buffer, int width, int height, bool color)
        {
            try
            {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                IntPtr scan = bitmapData.Scan0;

                int stride = 0;

                if (!color)
                {
                    stride = width;
                }
                else
                {
                    stride = width * 3;
                }


                if (stride == bitmapData.Stride)
                {
                    CopyMemory(scan, buffer, bitmapData.Stride * bitmap.Height);
                }
                else
                {
                    for (int i = 0; i < bitmap.Height; i++)
                    {
                        CopyMemory(new IntPtr(scan.ToInt64() + i * bitmapData.Stride), new IntPtr(buffer.ToInt64() + i * width), width);
                    }
                }

                bitmap.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
            }
        }
        private static void UpdateBitmap(Bitmap bitmap, byte[] buffer, int width, int height, bool color)
        {
            try
            {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                IntPtr scan = bitmapData.Scan0;

                int stride = 0;

                if (!color)
                {
                    stride = width;
                }
                else
                {
                    stride = width * 3;
                }

                if (stride == bitmapData.Stride)
                {
                    Marshal.Copy(buffer, 0, scan, bitmapData.Stride * bitmap.Height);
                }
                else
                {
                    for (int i = 0; i < bitmap.Height; i++)
                    {
                        Marshal.Copy(buffer, i * stride, new IntPtr(scan.ToInt64() + i * bitmapData.Stride), width);
                    }
                }

                bitmap.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
            }
        }
        private static void UpdateBitmap(Bitmap bitmap, IntPtr buffer, int width, int height)
        {
            try
            {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                IntPtr scan = bitmapData.Scan0;

                for (int i = 0; i < bitmap.Height; i++)
                {
                    CopyMemory(new IntPtr(scan.ToInt64() + i * bitmapData.Stride), new IntPtr(buffer.ToInt64() + i * width), width);
                }

                bitmap.UnlockBits(bitmapData);
            }
            catch (Exception exception)
            {
            }
        }
        private static void UpdateBitmap(Bitmap bitmap, short[] buffer, int width, int height)
        {
            try
            {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                IntPtr scan = bitmapData.Scan0;

                int stride = bitmapData.Stride;

                ushort[] data = new ushort[buffer.Length];

                for (int i = 0; i < data.Length; i++)
                {

                    short pixelValue = buffer[i]; //获取当前的像素点

                    data[i] = (ushort)(pixelValue - short.MinValue);//将short类型的像素值转换为Ushort类型并写入缓存
                }

                unsafe
                {
                    ushort* Pointer = (ushort*)scan.ToPointer();

                    for (int i = 0; i < height; ++i)
                    {
                        for (int j = 0; j < width; ++j)
                        {
                            ushort greyValue = data[i * width + j];

                            *Pointer = greyValue;

                            ++Pointer;
                        }

                        Pointer += (stride / 2 - width);
                    }
                }

                bitmap.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
            }
        }


    }
}

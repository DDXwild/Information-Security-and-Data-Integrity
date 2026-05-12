using System;
using System.IO;
using System.Text;

namespace LSBWatermark
{
    class Bitmap24
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public byte[,,] Pixels { get; private set; }

        byte[] _fileHeader;
        byte[] _dibHeader;

        public static Bitmap24 Load(string path)
        {
            var bmp = new Bitmap24();
            byte[] data = File.ReadAllBytes(path);

            if (data[0] != 'B' || data[1] != 'M')
                throw new Exception("Not a BMP file.");

            int pixelOffset = BitConverter.ToInt32(data, 10);
            int width = BitConverter.ToInt32(data, 18);
            int height = BitConverter.ToInt32(data, 22);
            short bpp = BitConverter.ToInt16(data, 28);

            if (bpp != 24)
                throw new Exception("Only 24-bit BMP supported.");

            bmp.Width = width;
            bmp.Height = Math.Abs(height);
            bmp._fileHeader = data[0..14];
            bmp._dibHeader = data[14..pixelOffset];
            bmp.Pixels = new byte[bmp.Height, bmp.Width, 3];

            int rowSize = ((width * 3 + 3) / 4) * 4;
            bool topDown = height < 0;

            for (int y = 0; y < bmp.Height; y++)
            {
                int srcY = topDown ? y : (bmp.Height - 1 - y);
                int rowStart = pixelOffset + srcY * rowSize;
                for (int x = 0; x < bmp.Width; x++)
                {
                    bmp.Pixels[y, x, 0] = data[rowStart + x * 3];     // B
                    bmp.Pixels[y, x, 1] = data[rowStart + x * 3 + 1]; // G
                    bmp.Pixels[y, x, 2] = data[rowStart + x * 3 + 2]; // R
                }
            }
            return bmp;
        }

        public void Save(string path)
        {
            int rowSize = ((Width * 3 + 3) / 4) * 4;
            int pixelDataSize = rowSize * Height;
            int pixelOffset = _fileHeader.Length + _dibHeader.Length;
            int fileSize = pixelOffset + pixelDataSize;

            byte[] outData = new byte[fileSize];
            Array.Copy(_fileHeader, outData, _fileHeader.Length);
            Array.Copy(_dibHeader, 0, outData, _fileHeader.Length, _dibHeader.Length);

            BitConverter.GetBytes(fileSize).CopyTo(outData, 2);

            for (int y = 0; y < Height; y++)
            {
                int dstY = Height - 1 - y;
                int rowStart = pixelOffset + dstY * rowSize;
                for (int x = 0; x < Width; x++)
                {
                    outData[rowStart + x * 3] = Pixels[y, x, 0];
                    outData[rowStart + x * 3 + 1] = Pixels[y, x, 1];
                    outData[rowStart + x * 3 + 2] = Pixels[y, x, 2];
                }
            }
            File.WriteAllBytes(path, outData);
        }
    }

    class Program
    {
        static void EmbedMessage(Bitmap24 bmp, string message)
        {
            byte[] msgBytes = Encoding.UTF8.GetBytes(message);
            int totalBits = (msgBytes.Length + 4) * 8;
            int capacity = bmp.Height * bmp.Width * 3;

            if (totalBits > capacity)
                throw new Exception($"Message too long. Capacity: {capacity / 8 - 4} bytes.");

            byte[] payload = new byte[4 + msgBytes.Length];
            BitConverter.GetBytes(msgBytes.Length).CopyTo(payload, 0);
            Array.Copy(msgBytes, 0, payload, 4, msgBytes.Length);

            int bitIndex = 0;
            for (int y = 0; y < bmp.Height && bitIndex < payload.Length * 8; y++)
                for (int x = 0; x < bmp.Width && bitIndex < payload.Length * 8; x++)
                    for (int c = 0; c < 3 && bitIndex < payload.Length * 8; c++)
                    {
                        int bytePos = bitIndex / 8;
                        int bitPos = 7 - (bitIndex % 8);
                        int bit = (payload[bytePos] >> bitPos) & 1;
                        bmp.Pixels[y, x, c] = (byte)((bmp.Pixels[y, x, c] & 0xFE) | bit);
                        bitIndex++;
                    }
        }

        static string ExtractMessage(Bitmap24 bmp)
        {
            int length = 0;
            int bitIndex = 0;
            for (int y = 0; y < bmp.Height && bitIndex < 32; y++)
                for (int x = 0; x < bmp.Width && bitIndex < 32; x++)
                    for (int c = 0; c < 3 && bitIndex < 32; c++)
                    {
                        length = (length << 1) | (bmp.Pixels[y, x, c] & 1);
                        bitIndex++;
                    }

            if (length <= 0 || length > bmp.Height * bmp.Width * 3 / 8)
                throw new Exception("No valid watermark found or image is corrupted.");

            byte[] msgBytes = new byte[length];
            int msgBit = 0;
            bool pastHeader = false;
            int headerBits = 32;
            int totalBits = headerBits + length * 8;
            bitIndex = 0;

            for (int y = 0; y < bmp.Height && bitIndex < totalBits; y++)
                for (int x = 0; x < bmp.Width && bitIndex < totalBits; x++)
                    for (int c = 0; c < 3 && bitIndex < totalBits; c++)
                    {
                        if (bitIndex >= headerBits)
                        {
                            int bytePos = msgBit / 8;
                            msgBytes[bytePos] = (byte)((msgBytes[bytePos] << 1) | (bmp.Pixels[y, x, c] & 1));
                            msgBit++;
                        }
                        bitIndex++;
                    }

            return Encoding.UTF8.GetString(msgBytes);
        }

        static Bitmap24 CreateTestBMP(int width, int height, string savePath)
        {
            int rowSize = ((width * 3 + 3) / 4) * 4;
            int pixelOffset = 54;
            int fileSize = pixelOffset + rowSize * height;

            byte[] data = new byte[fileSize];
            data[0] = (byte)'B'; data[1] = (byte)'M';
            BitConverter.GetBytes(fileSize).CopyTo(data, 2);
            BitConverter.GetBytes(pixelOffset).CopyTo(data, 10);
            BitConverter.GetBytes(40).CopyTo(data, 14);
            BitConverter.GetBytes(width).CopyTo(data, 18);
            BitConverter.GetBytes(-height).CopyTo(data, 22);
            data[26] = 1; data[27] = 0;
            data[28] = 24; data[29] = 0;

            var rng = new Random(42);
            for (int y = 0; y < height; y++)
            {
                int row = pixelOffset + y * rowSize;
                for (int x = 0; x < width; x++)
                {
                    data[row + x * 3] = (byte)(x % 256);
                    data[row + x * 3 + 1] = (byte)(y % 256);
                    data[row + x * 3 + 2] = (byte)((x + y) % 256);
                }
            }
            File.WriteAllBytes(savePath, data);
            return Bitmap24.Load(savePath);
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== LSB Digital Watermarking ===\n");

            string srcPath = "image_original.bmp";
            string dstPath = "image_watermarked.bmp";

            Bitmap24 bmp;
            if (File.Exists(srcPath))
            {
                Console.WriteLine($"Loaded existing image: {srcPath}");
                bmp = Bitmap24.Load(srcPath);
            }
            else
            {
                Console.WriteLine("No image_original.bmp found. Creating a 200x150 test image.");
                bmp = CreateTestBMP(200, 150, srcPath);
                Console.WriteLine($"Test image saved: {srcPath}");
            }

            Console.WriteLine($"Image size: {bmp.Width}x{bmp.Height} pixels");
            Console.WriteLine($"Max watermark capacity: {bmp.Width * bmp.Height * 3 / 8 - 4} bytes\n");

            Console.Write("Enter watermark message : ");
            string message = Console.ReadLine() ?? "watermark";

            EmbedMessage(bmp, message);
            bmp.Save(dstPath);
            Console.WriteLine($"Watermarked image saved : {dstPath}");

            Bitmap24 wm = Bitmap24.Load(dstPath);
            string extracted = ExtractMessage(wm);
            Console.WriteLine($"Extracted message       : {extracted}");
            Console.WriteLine($"Verification            : {(extracted == message ? "OK - messages match" : "FAIL - messages differ")}");

            int changed = 0;
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                    for (int c = 0; c < 3; c++)
                        if ((bmp.Pixels[y, x, c] & 1) != (wm.Pixels[y, x, c] & 1))
                            changed++;

            Console.WriteLine($"Pixels modified (LSB)   : {changed} of {bmp.Width * bmp.Height * 3} channels");
        }
    }
}
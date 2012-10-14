using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using Puzzlebox.Imaging.TextRenderer.Quantizer;

namespace Puzzlebox.Imaging.TextRenderer
{
	/// <summary>
	/// Render images with text.
	/// </summary>
	public class TextRender
	{
		public TextRender(Image image, Point textPosition)
		{
			SourceImage = image;
			TextPosition = textPosition;
			BackgroundColor = Color.White;
			ForegroundColor = Color.Black;
			Font = new Font("Arial", 10);
		}

		public TextRender(Size imageSize) : this(imageSize, new Point(0, 0))
		{
		}

		public TextRender(Size imageSize, Point textPosition)
			: this(new Bitmap(imageSize.Width, imageSize.Height), textPosition)
		{
		}

		private Image SourceImage { get; set; }

		public Font Font { get; set; }

		public string Text { get; set; }

		public Color BackgroundColor { get; set; }

		public Color ForegroundColor { get; set; }

		public Point TextPosition { get; set; }

		public void Save(Stream stream)
		{
			var myGraphics = GraphicsConstructer((Bitmap)SourceImage);
			var quantizer = new OctreeQuantizer(255, 8);
			var quantized = quantizer.Quantize(SourceImage);

			quantized.Save(stream, ImageFormat.Gif);

			// clean up...
			quantized.Dispose();
			myGraphics.Dispose();
		}

		public void Save(string pathToOutput)
		{
			Save(new FileStream(pathToOutput, FileMode.CreateNew));
		}

		private Graphics GraphicsConstructer(Bitmap myBitmap)
		{
			var graphics = Graphics.FromImage(myBitmap);

			// Set background colour
			graphics.FillRectangle(new SolidBrush(BackgroundColor), 0, 0, SourceImage.Width,
			                         SourceImage.Height);

			// Set smoothness of graphics rendered
			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			// AntiAlias the text
			graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

			// Specify the brush colour
			var brush = new SolidBrush(ForegroundColor);

			// draw the text
			graphics.DrawString(Text, Font, brush, TextPosition);
			return (graphics);
		}

	}
}
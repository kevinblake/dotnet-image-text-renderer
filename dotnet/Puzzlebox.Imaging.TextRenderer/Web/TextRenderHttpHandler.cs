using System.Drawing;
using System.Web;

namespace Puzzlebox.Imaging.TextRenderer.Web
{
	public class TextRenderHttpHandler: IHttpHandler
	{
		#region IHttpHandler Members

		public void ProcessRequest(HttpContext context)
		{
			var text = context.Request.QueryString.Get("text");

			var textImage = new TextRender(new Size(GetQueryStringValueOrDefault("width", 500), GetQueryStringValueOrDefault("height", 200)))
				{
					Text = text,
					TextPosition = new Point(GetQueryStringValueOrDefault("top", 0), GetQueryStringValueOrDefault("topleft", 0)),
					Font = new Font(GetQueryStringValueOrDefault("font", "verdana"),
					                GetQueryStringValueOrDefault("fontSize", 12f))
				};

			context.Response.ContentType = "image/gif";

			textImage.Save(context.Response.OutputStream);
		}

		public bool IsReusable { get; private set; }

		#endregion

		private float GetQueryStringValueOrDefault(string queryStringKey, float @default)
		{
			var value = @default;
			var queryStringValue = HttpContext.Current.Request.QueryString.Get(queryStringKey);

			if (!string.IsNullOrWhiteSpace(queryStringValue))
			{
				float.TryParse(queryStringValue, out value);
			}

			return value;
		}

		private int GetQueryStringValueOrDefault(string queryStringKey, int @default)
		{
			var value = @default;
			var queryStringValue = HttpContext.Current.Request.QueryString.Get(queryStringKey);

			if (!string.IsNullOrWhiteSpace(queryStringValue))
			{
				int.TryParse(queryStringValue, out value);
			}

			return value;
		}

		private string GetQueryStringValueOrDefault(string queryStringKey, string @default)
		{
			var queryStringValue = HttpContext.Current.Request.QueryString.Get(queryStringKey);
			return string.IsNullOrWhiteSpace(queryStringValue) ? @default : queryStringValue;
		}
	}
}

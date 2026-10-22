using Archipelago.Core.Util.Overlay;

namespace MedievilArchipelago.Helpers
{
    // wraps the game overlay so it can be switched on and off from the CLI. On by default.
    public class ToggleableOverlayService : IOverlayService
    {
        private readonly IOverlayService _inner;

        public bool Enabled { get; set; } = true;

        public ToggleableOverlayService(IOverlayService inner)
        {
            _inner = inner;
        }

        public bool AttachToWindow(IntPtr targetWindowHandle) => _inner.AttachToWindow(targetWindowHandle);

        public void AddTextPopup(string text)
        {
            if (Enabled) _inner.AddTextPopup(text);
        }

        public void AddRichTextPopup(List<ColoredTextSpan> spans)
        {
            if (Enabled) _inner.AddRichTextPopup(spans);
        }

        public void SetPosition(float x, float y) => _inner.SetPosition(x, y);

        public void SetSize(float width, float height) => _inner.SetSize(width, height);

        public void SetSizeAndPosition(float x, float y, float width, float height) => _inner.SetSizeAndPosition(x, y, width, height);

        public void Dispose() => _inner.Dispose();
    }
}

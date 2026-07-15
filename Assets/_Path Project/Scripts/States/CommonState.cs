

namespace TDGame
{
    public class DirtyState
    {
        public bool IsDirty { get; private set; } = false;
        public void MarkDirty() => IsDirty = true;
        public void Clearn() => IsDirty = false;
    }
}
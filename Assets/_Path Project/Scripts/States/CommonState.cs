

namespace TDGame
{
    public class DirtyState
    {
        public bool IsDirty { get; protected set; } = false;
        public void MarkDirty() => IsDirty = true;
        public void Clearn() => IsDirty = false;

        protected void SetValue<T>(ref T field, T value)
        {
            field = value;
            IsDirty = true;
        }

    }
}
using System.Text;

namespace JGeneral.IO
{
    public readonly ref struct ScreenPoint
    {
        public readonly int X;
        public readonly int Y;

        public ScreenPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("X: ");
            builder.Append(X);
            builder.Append("    ");
            builder.AppendLine();
            builder.Append("Y: ");
            builder.Append(Y);
            builder.Append("    ");

            return builder.ToString();
        }
    }
}
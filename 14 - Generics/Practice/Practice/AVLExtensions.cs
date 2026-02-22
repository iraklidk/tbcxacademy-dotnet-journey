namespace Practice
{
    public static class AVLExtension
    {
        public static int Height<T>(this Node<T> node) => node == null ? 0 : node.Height;

        public static int Balance<T>(this Node<T> node) => node == null ? 0 : node.Left.Height() - node.Right.Height();

        public static Node<T> RotateRight<T>(this Node<T> y)
        {
            Node<T> x = y.Left;
            Node<T> T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(y.Left.Height(), y.Right.Height()) + 1;
            x.Height = Math.Max(x.Left.Height(), x.Right.Height()) + 1;

            return x;
        }

        public static Node<T> RotateLeft<T>(this Node<T> x)
        {
            Node<T> y = x.Right;
            Node<T> T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(x.Left.Height(), x.Right.Height()) + 1;
            y.Height = Math.Max(y.Left.Height(), y.Right.Height()) + 1;

            return y;
        }

        public static Node<T> BalanceNode<T>(this Node<T> root)
        {
            root.Height = 1 + Math.Max(root.Left.Height(), root.Right.Height());
            int balance = root.Balance();

            if (balance > 1 && root.Left.Balance() >= 0) return root.RotateRight(); // left left

            if (balance < -1 && root.Right.Balance() <= 0) return root.RotateLeft(); // right right

            if (balance > 1 && root.Left.Balance() < 0) // left right
            {
                root.Left = root.Left.RotateLeft(); // makes LL case then rotate right
                return root.RotateRight();
            }
            
            if (balance < -1 && root.Right.Balance() > 0) // right left
            {
                root.Right = root.Right.RotateRight(); // makes RR case then rotate left
                return root.RotateLeft();
            }

            return root;
        }

    }
}